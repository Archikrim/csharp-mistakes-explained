---
id: "0030"
title: Writing through a covariant array reference
category: collections
tags: [arrays, covariance, ArrayTypeMismatchException]
author: tygronia
rule: "never write through a **covariant array** reference"
---

# #0030 - Writing Through a Covariant Array Reference

## 💥 Symptom

A shared helper that stamps or blanks a cell in a row works everywhere in the
reporting layer except one report, where it throws `ArrayTypeMismatchException`
at runtime. The helper is unchanged, the row is a perfectly ordinary array, and
the exception points inside the helper - at an assignment that the compiler
accepted without a word. The call site that triggers it looks identical to every
call site that doesn't.

## 🔍 The Offending Code

```csharp
static void StampProcessed(object[] cells) => cells[0] = DateTime.UtcNow;

string[] row = ["SKU-1001", "SKU-2002", "SKU-3003"];
StampProcessed(row);   // 💥 ArrayTypeMismatchException
```

## 🧠 What's Actually Going On

C# arrays are **covariant**: a `string[]` is accepted anywhere an `object[]` is
expected, because a string *is* an object. That conversion is free and the
compiler allows it with no cast.

The catch is that covariance is only sound for **reading**. The moment you
*write* through the base-typed reference, the promise breaks: the reference
says `object[]`, so `cells[0] = DateTime.UtcNow` type-checks at compile time -
but the array is really a `string[]`, and a `DateTime` is not a string. The
compiler cannot catch it, because at the assignment it only knows the static
type `object[]`. So the runtime has to: **every** store into a reference-type
array carries a hidden per-element type check, and this one fails with
`ArrayTypeMismatchException`.

The hole was left in the language on purpose - Java has the same one - because
in the early days it let generic-like code exist before generics did. Today it
is pure legacy sharp edge: a covariant array write compiles clean and fails
only when that exact line runs, on that exact array type.

## ✅ The Fix

If the helper writes objects, give it an array that really is one:

```csharp
object[] row = ["SKU-1001", "SKU-2002", "SKU-3003"]; // a real object[], not a string[]
StampProcessed(row);
```

Full version in [Good.cs](Good.cs). The toolbox:

| Option | When it's the right call |
|---|---|
| Declare the array as the type you actually write | The collection genuinely holds mixed values - make that true, don't fake it with covariance |
| Make the helper generic: `Stamp<T>(T[] cells, T value)` | The type parameter forces the value to match the array, so a wrong write won't compile |
| Use `List<T>` | `List<T>` is invariant - a `List<string>` will not silently pass as a `List<object>`, so the write hole never opens |

## 😈 The Even Worse Sibling

The write that *doesn't* throw because the value happens to fit. Fill a
`string[]` through an `object[]` reference with another string, and it works -
until the day the value is null-checked, replaced with a sentinel object, or the
"filler" becomes a boxed struct. The bug was present the whole time; it only
became an exception when the data changed, so it surfaces in production long
after the covariant code shipped and passed every test.

## 🎓 Advanced Nuance

This is exactly why generic collections are *invariant*: `List<string>` is not a
`List<object>`, and the compiler refuses the conversion that arrays allow. The
inconsistency is deliberate - arrays kept covariance for backward compatibility,
generics chose safety. And when covariance is genuinely wanted and safe, C# has
a typed version of it: `IEnumerable<out T>` is covariant precisely because it is
**read-only**, so there is no write to go wrong. The array hole is the untyped,
unsafe ancestor of the same idea.

## 🔎 How to Find It in Your Codebase

- Methods that take `object[]` (or any base-typed array) and **write** into it,
  called anywhere with a derived-typed array like `string[]`.
- `ArrayTypeMismatchException` in a stack trace - it means exactly this, always.
- APIs shaped like `void GetValues(object[] values)` (ADO.NET has them): passing
  a `string[]` compiles and can throw on the first store.
