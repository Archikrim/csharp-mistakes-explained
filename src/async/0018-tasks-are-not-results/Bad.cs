// Exhibit #0018: tasks mistaken for results

var customers = new[] { "Olena", "Ivan", "Petro", "Maria", "Bohdan" };

int delivered = 0;

// Send every invoice. The await is right there - what could be missing?
var sent = customers.Select(async customer =>
{
    await Task.Delay(100); // the SMTP round-trip
    Interlocked.Increment(ref delivered);
    return customer;
});

Console.WriteLine($"Invoices sent:      {sent.Count()}"); // 💥 counting tasks, not deliveries
Console.WriteLine($"Invoices delivered: {delivered}");

if (delivered != customers.Length)
{
    throw new InvalidOperationException(
        $"the report claims {customers.Length} invoices, the SMTP server saw {delivered}");
}

Console.WriteLine("Report matches reality. All invoices delivered.");
