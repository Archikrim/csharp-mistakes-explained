// Exhibit #0030: writing through a covariant array reference

// A shared helper the reporting layer reuses to stamp a processed time
// into the first cell of any row it's given.
static void StampProcessed(object[] cells) => cells[0] = DateTime.UtcNow; // 💥

// One caller hands it a row of SKUs - a string[].
string[] row = ["SKU-1001", "SKU-2002", "SKU-3003"];

Console.WriteLine($"Row before: {string.Join(", ", row)}");

StampProcessed(row); // string[] is passed as object[] - and the write blows up

Console.WriteLine($"Row after:  {string.Join(", ", row)}");
Console.WriteLine("Stamped the row. On to the next one.");
