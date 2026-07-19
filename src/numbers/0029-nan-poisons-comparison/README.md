---
id: "0029"
title: A NaN in the data the report aggregates
category: numbers
tags: [double, NaN, IEEE-754, comparison]
rule: "never divide by a value that can be **zero**"
---

# #0029 - A NaN in the Data the Report Aggregates

## 💥 Symptom

The analytics dashboard shows a best-performing page and, next to it, a worst
rate of `NaN`. Clicking through to see which page that is returns nothing -
the drill-down finds no row with that value, in the same table the value was
computed from. The page that caused it launched this morning and has zero
visits, which is not an error state anyone thought to handle.

## 🔍 The Offending Code

```csharp
var rates = pages.Select(p => (p.Url, Rate: (double)p.Conversions / p.Visits));

var worst     = rates.Min(r => r.Rate);               // NaN
var worstPage = rates.FirstOrDefault(r => r.Rate == worst);   // 💥 finds nothing
```

## 🧠 What's Actually Going On

Start with the boring part, because it is the part that gets skipped: **this is
a division by zero.** Everyone knows what that does - it throws. And that
knowledge is true for exactly the types where it doesn't matter:

```text
int     0 / 0      -> DivideByZeroException   (loud, stops here)
decimal 1 / 0      -> DivideByZeroException   (loud, stops here)
double  0.0 / 0.0  -> NaN                     (silent, keeps going)
double  1.0 / 0.0  -> Infinity                (silent, keeps going)
```

The one type where a zero denominator is most likely - the floating-point one
you reach for when computing rates and averages - is the one type that does not
stop you. There is no exception to catch, no log line, no crash to trace back.
The bad value is simply manufactured and handed downstream, and everything after
this point is that value spreading.

And `NaN` is a value that refuses to participate in comparison, which is where
.NET quietly applies **two different rule sets to it**:

| | operators (`==`, `<`, `>`) | framework (`Equals`, `Comparer`, `Sort`, `Contains`) |
|---|---|---|
| `NaN` vs `NaN` | not equal | equal |
| `NaN` vs a number | neither less nor greater | NaN sorts first, as the smallest |

The operators follow IEEE 754, where NaN is unordered and never equal to
anything, including itself. The framework needed a **total order** so that
sorting and dictionaries can work at all, so `Comparer<double>.Default` declares
NaN to be less than every number and `Equals` declares it equal to itself. Both
are correct; they are answering different questions - and NaN is the only value
where they disagree.

Every strange thing in the output follows from that split:

```csharp
rates.Max()                  // 0.8   - Max ignores the NaN
rates.Min()                  // NaN   - Min reports it as the minimum
list.Contains(NaN)           // true  - uses Equals
list.Any(x => x == NaN)      // false - uses ==
```

`Max` hides the problem and `Min` surfaces it, from the same list, in the same
line of report code. And `Min` handing back a value that `==` cannot then find
is not a contradiction in your code - it is two answers to "the same" question,
asked through two different doors.

## ✅ The Fix

**Guard the denominator.** Not "handle the NaN later" - never create it. Every
NaN in this exhibit exists because one division was allowed to run with a zero
on the bottom, and every strange behaviour after that is downstream of that one
unguarded line. A page with no visits has no conversion rate: that is an
absence, not a number, so it never belongs among the numbers.

```csharp
var rates = pages
    .Where(p => p.Visits > 0)
    .Select(p => (p.Url, Rate: (double)p.Conversions / p.Visits));
```

Full version in [Good.cs](Good.cs). The toolbox, in order of preference:

| Option | When it's the right call |
|---|---|
| Guard the denominator before dividing | Always first. The NaN has a cause you can name - no visits, no items, no responses - so handle *that*, at the division |
| Model absence as absence (`double?`, a separate bucket) | "No rate yet" is a real state the report should show rather than fake with a number |
| `double.IsFinite(x)` filter before aggregating | Last resort, for values that arrive from outside and whose source you cannot fix |

Treat any division whose denominator is a count, a total, a duration or a
`Length` as a guard site by default - those are exactly the values that are
legitimately zero on the first day, on an empty batch, or for a brand-new
record.

## 😈 The Even Worse Sibling

The report that only calls `Max`, `Sum` or `Average`. `Max` silently skips the
NaN, so nothing looks wrong - while `Sum` and `Average` return NaN for the whole
dataset, and a single unvisited page turns every headline figure into "not a
number". One aggregate lies by omission, the other collapses entirely, and which
one you get depends on which function the dashboard happened to call.

## 🎓 Advanced Nuance

The same split decides whether duplicates exist. `Distinct()` and `GroupBy` use
`Equals`, so two NaNs collapse into one group - while a hand-written
`x == y` dedupe keeps both, because those two NaNs are "not equal". The same
data, deduplicated two ways, yields two different row counts.

And `double.TryParse("NaN")` returns **true**. Every CSV import, JSON payload
and config value is one literal away from putting a NaN into your data on
purpose, and the parse gate that was supposed to stop bad input waves it
through - the same shape as trusting a parser to be a validator.

## 🔎 How to Find It in Your Codebase

- **Every division whose denominator can be zero** - rates, averages,
  percentages, "per unit" figures. That single line is where the NaN is
  manufactured; nothing downstream can undo it. Counts, totals, durations and
  `Length` are zero more often than the code assumes.
- `Min`, `Sum` and `Average` over computed `double` columns - check what happens
  when the source set is empty or the denominator is zero.
- A lookup by value (`First(x => x.Value == v)`) where `v` came from an
  aggregate over the same collection. If the value can be NaN, the lookup can
  fail on data that is definitely there.
