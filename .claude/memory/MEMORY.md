# Memory index

Committed project memory for **C# Mistakes Explained**. This index is
auto-loaded every session (imported from `CLAUDE.md`); the topic files are read
on demand.

- `halls.md` - canonical hall registry (the front-page generator reads it).
- `state.md` - current exhibit count, the exhibit table, next id.
- `backlog.md` - candidate exhibits to pick from.
- `rejected.md` - declined candidates + the curator's reasons. **Read before proposing.**
- `archetypes.md` - the 7 bug archetypes; the curation taxonomy.
- `todo.md` - remaining framework/infra work.

After an exhibit lands: update `state.md` and move the candidate out of
`backlog.md`. Memory is committed **separately** from exhibit commits.
