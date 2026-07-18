// Exhibit #0018: the fix

var customers = new[] { "Olena", "Ivan", "Petro", "Maria", "Bohdan" };

int delivered = 0;

// Send every invoice. Same lambda - but the tasks get awaited this time.
var sendTasks = customers.Select(async customer =>
{
    await Task.Delay(100); // the SMTP round-trip
    Interlocked.Increment(ref delivered);
    return customer;
});

var sent = await Task.WhenAll(sendTasks); // tasks become results, here and only here

Console.WriteLine($"Invoices sent:      {sent.Length}");
Console.WriteLine($"Invoices delivered: {delivered}");

if (delivered != customers.Length)
{
    throw new InvalidOperationException(
        $"the report claims {customers.Length} invoices, the SMTP server saw {delivered}");
}

Console.WriteLine("Report matches reality. All invoices delivered.");
