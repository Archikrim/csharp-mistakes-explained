// Suggests the next free exhibit number and catches duplicates.
// Run from the repo root: dotnet run tools/next-id.cs

using System.Text.RegularExpressions;

if (!Directory.Exists("src"))
{
    Console.Error.WriteLine("Run from the repo root: dotnet run tools/next-id.cs");
    return 1;
}

// The source of truth is the exhibit folders themselves: src/<category>/NNNN-name/
var ids = Directory.GetDirectories("src", "*", SearchOption.AllDirectories)
    .Select(Path.GetFileName)
    .Where(name => name is not null && Regex.IsMatch(name, @"^\d{4}-"))
    .Select(name => int.Parse(name![..4]))
    .ToList();

if (ids.Count == 0)
{
    Console.WriteLine("No exhibits yet. Next number: 0001");
    return 0;
}

var duplicates = ids.GroupBy(id => id)
    .Where(g => g.Count() > 1)
    .Select(g => g.Key)
    .ToList();

if (duplicates.Count > 0)
{
    Console.Error.WriteLine($"❌ Duplicate exhibit numbers: {string.Join(", ", duplicates.Select(d => $"{d:D4}"))}");
    return 1;
}

Console.WriteLine($"Exhibits in the museum: {ids.Count}, latest: #{ids.Max():D4}");
Console.WriteLine($"Next free number: {ids.Max() + 1:D4}");
return 0;
