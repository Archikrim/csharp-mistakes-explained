// Exhibit #0030: the fix

// A shared helper the reporting layer reuses to stamp a processed time
// into the first cell of any row it's given.
static void StampProcessed(object[] cells) => cells[0] = DateTime.UtcNow;

// If the helper writes objects, the array has to really be an object[] -
// not a string[] wearing an object[] costume.
object[] row = ["SKU-1001", "SKU-2002", "SKU-3003"];

Console.WriteLine($"Row before: {string.Join(", ", row)}");

StampProcessed(row);

Console.WriteLine($"Row after:  {string.Join(", ", row)}");

if (row[0] is not DateTime)
{
    throw new InvalidOperationException(
        "the first cell was supposed to be stamped with a timestamp");
}

Console.WriteLine("Stamped the row. On to the next one.");
