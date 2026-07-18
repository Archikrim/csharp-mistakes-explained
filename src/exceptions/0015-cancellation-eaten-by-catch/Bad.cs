// Exhibit #0015: a catch-all that swallows cancellation

using var cts = new CancellationTokenSource();

// The user closes the export dialog moments after starting it.
cts.CancelAfter(100);

int attempts = 0;
const int MaxRetries = 5;

for (int attempt = 1; attempt <= MaxRetries; attempt++)
{
    try
    {
        attempts++;
        await ExportReport(cts.Token);
        Console.WriteLine("Export finished.");
        break;
    }
    catch (Exception ex) // 💥 cancellation lands here too
    {
        Console.WriteLine($"[retry] attempt {attempt} failed: {ex.GetType().Name}");
    }
}

Console.WriteLine($"Attempts made:  {attempts}");
Console.WriteLine($"User cancelled: {cts.IsCancellationRequested}");

if (attempts > 1 && cts.IsCancellationRequested)
{
    throw new InvalidOperationException(
        $"the user said stop once - the worker tried {attempts} times anyway");
}

Console.WriteLine("Cancelled means stopped. The worker listened.");

async Task ExportReport(CancellationToken token)
{
    for (int page = 1; page <= 100; page++)
    {
        token.ThrowIfCancellationRequested();
        await Task.Delay(30, token); // fetching a page
    }
}
