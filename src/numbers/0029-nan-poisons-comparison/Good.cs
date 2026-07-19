// Exhibit #0029: the fix

var pages = new[]
{
    new PageStats("/home", Visits: 1200, Conversions: 960),
    new PageStats("/pricing", Visits: 400, Conversions: 120),
    new PageStats("/beta", Visits: 0, Conversions: 0), // launched this morning
    new PageStats("/docs", Visits: 800, Conversions: 400),
};

var rates = pages
    .Where(p => p.Visits > 0) // no visits is no rate, not a NaN rate
    .Select(p => (Page: p.Url, Rate: (double)p.Conversions / p.Visits))
    .ToList();

Console.WriteLine("Conversion rates:");
foreach (var r in rates)
    Console.WriteLine($"  {r.Page,-10} {r.Rate}");

var best = rates.Max(r => r.Rate);
var worst = rates.Min(r => r.Rate);

Console.WriteLine();
Console.WriteLine($"Best rate:  {best}");
Console.WriteLine($"Worst rate: {worst}");

// Which page performed worst? Look it up in the very list the value came from.
var worstPage = rates.FirstOrDefault(r => r.Rate == worst);

if (worstPage.Page is null)
{
    throw new InvalidOperationException(
        $"the worst rate is {worst} and no page in the list has it - the value came from this list");
}

Console.WriteLine($"Worst page: {worstPage.Page}");

record PageStats(string Url, int Visits, int Conversions);
