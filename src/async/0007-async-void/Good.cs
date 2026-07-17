// Exhibit #0007: the fix

// Fire the welcome email, guard it with try/catch. Now actually bulletproof.
try
{
    await SendWelcomeEmail("new-user@example.com");
    Console.WriteLine("Welcome email queued. All good.");
}
catch (Exception ex)
{
    Console.WriteLine($"Caught and handled: {ex.Message}");
}

await Task.Delay(500);

Console.WriteLine("This line believes it will run. And it does.");

async Task SendWelcomeEmail(string address)
{
    await Task.Delay(100); // the SMTP handshake
    throw new InvalidOperationException($"SMTP rejected {address}");
}
