# TODO

Remaining work, current as of the framework migration (2026-07-19).

## Open

- **CI (GitHub Actions).** *Deferred by the curator (2026-07-19) - he will raise
  it again; do not start unprompted.* On push/PR: build/run every exhibit so they
  don't rot on SDK bumps; run `next-id.cs`, `check-links.cs`, and
  `gen-frontpage.cs` (fail if the front page is stale) - all three exit 1 on
  failure. Package exhibits (EF, DI, STJ) need restore; watch CI time.
- **Launch polish.** Badges (exhibit count), final proofread, LinkedIn poll copy
  (<=30 chars, 4 options).
- **Tags cross-index.** Once tags are consistent across exhibits, generate a
  tag/archetype index alongside the front page.

## Done

- Full framework migration to native Claude Code mechanisms: root `CLAUDE.md`,
  path-scoped `.claude/rules/`, the `add-exhibit` / `propose-exhibits` /
  `reject-exhibit` skills. Retired the homemade `conventions.md`,
  `exhibit-recipe.md`, `playbook.md`.
- Tools: `next-id.cs`, `check-links.cs`, `gen-frontpage.cs` (front page is now
  generated, list-style, no difficulty levels).
- Hall taxonomy expanded to ~30 in `halls.md`.
- Memory relocated from the freestyled `claude-calibration/` into `.claude/memory/`,
  indexed by `MEMORY.md` and auto-loaded via a `CLAUDE.md` import.
- Exhibits 0001-0023.
