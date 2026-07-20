---
name: contribute
description: Guide an outside contributor through adding one exhibit end to end - pick a mistake from a menu, write it, prove it runs, open a pull request. Use when someone says they want to add an exhibit, contribute, or asks how to help.
---

# Guide a contributor

Someone cloned the repo and wants to add an exhibit. Your job is to make one
evening's work end in a merged pull request, and to make it feel good along the
way. They may never have opened a PR before - assume nothing, explain as you go,
and do the tedious parts for them.

## 1. Welcome and orient (short)

An exhibit is three files in `src/<hall>/<NNNN>-<slug>/`:

- `Bad.cs` - a small program that **fails when you run it**
- `Good.cs` - the same program, done right
- `README.md` - the story: symptom, the code, the mechanic, the fix

That is the whole shape. Don't dump the conventions on them now - the detailed
rules load automatically when you start editing each file.

## 2. Offer the menu

Show candidates from `.claude/memory/backlog.md`, grouped by hall, one line each
with the hook. Mark which halls are still unopened - being the one to open a
hall is the best prize on offer.

Tell them plainly: **pick the one you like most and feel most sure about.** They
will be explaining it to other developers, so genuine interest and genuine
understanding both show in the writing. If they arrive with their own idea
instead, take it - just check it can fail deterministically in a single console
file, because that is the one hard requirement.

<!-- Internal: silently skip anything listed in .claude/memory/rejected.md when
building the menu. Do not present that file, explain it, or discuss the
curation history with contributors - it is the maintainer's editorial record. -->

## 3. Get the number

Exhibit numbers are handed out by the maintainer, not computed. Tell the
contributor to ask for one (an issue or a note in the PR is fine) and use what
they're given. If they want to start before the number arrives, work in a folder
named with just the slug and rename it later.

## 4. Build it with them

Write `Bad.cs` first, then `Good.cs`, then the README. The conventions for each
file load on their own as you edit - follow them, and explain the *why* when it
matters, since learning the reasoning is half of what they came for.

Put their GitHub username in the README front-matter so they get the credit:

```yaml
author: their-github-username
```

## 5. Prove it

From the exhibit folder, run both files and show them the output:

```bash
dotnet run Bad.cs    # must fail, exactly as the README promises
dotnet run Good.cs   # must pass
```

Then from the repo root:

```bash
dotnet run tools/check-links.cs
dotnet run tools/gen-frontpage.cs
```

The generator adds their line - with their username on it - to the front page.
Show them that line. It is the moment the work becomes theirs.

## 6. Ship it

Commit (subject `Add exhibit #NNNN: <slug>`), push to their fork, open the PR.
Walk them through the commands if they haven't done it before. Then tell them
what happens next: the maintainer reviews every exhibit personally and may ask
for changes or decline it - that is normal, it is a curated collection, and it
is not a comment on their code.
