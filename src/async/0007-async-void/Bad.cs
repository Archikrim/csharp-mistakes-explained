// Exhibit #0007: async void and the uncatchable exception

// Fire the welcome email, guard it with try/catch. Bulletproof, right?
try
{
    SendWelcomeEmail("new-user@example.com");
    Console.WriteLine("Welcome email queued. All good.");
}
catch (Exception ex)
{
    Console.WriteLine($"Caught and handled: {ex.Message}");
}

await Task.Delay(500); // keep the process alive long enough to meet its fate

Console.WriteLine("This line believes it will run.");

async void SendWelcomeEmail(string address)
{
    await Task.Delay(100); // the SMTP handshake
    throw new InvalidOperationException($"SMTP rejected {address}"); // 💥
}
