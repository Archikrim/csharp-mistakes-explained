# Add an exhibit

One evening, one exhibit, one merged pull request. If you have never opened a PR
before, this is a good first one - the work is small, self-contained, and you
end up with your username on the front page next to the rule you added.

## What you need

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Claude Code](https://claude.com/claude-code)
- A GitHub account

## How it goes

1. Fork the repo and clone your fork.
2. Run `claude` inside the folder.
3. Tell it: **"I want to add an exhibit."**

From there Claude does the guiding. It shows you a menu of C# mistakes that are
waiting to be built, helps you write the code and the explanation, runs
everything to prove the bug is real, and walks you to the pull request.

**Pick the one you like most and feel most sure about.** You will be explaining
it to other developers, so choose a mistake you actually understand or genuinely
want to understand - that comes through in the writing.

## Two things to know

- **Ask for your exhibit number.** Numbers are handed out by the maintainer -
  open an issue or just ask in your PR, and use the number you're given.
- **Your bug must actually run.** Every exhibit is a program that fails when you
  run it, next to a fixed version that doesn't. Claude will check this with you
  before you open the PR.

That's it. Questions go in the issue tracker.
