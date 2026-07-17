# C# Bad Practices

Ideal tutorials teach you SOLID. **Bad** code is remembered better.

> Museum stats: **3** exhibits in **3** halls, latest addition — **#0003**.

### 🗂 Collections

| # | Exhibit | Level | The pain |
|--:|---------|-------|----------|
| 0001 | [Modifying a collection while iterating](src/collections/0001-modify-while-enumerating/) | 🟢 | `foreach` + `Remove` on the same list — partial execution and a crash |

### 🔢 Numbers

| # | Exhibit | Level | The pain |
|--:|---------|-------|----------|
| 0002 | [Calculating money with double](src/numbers/0002-doubles-for-money/) | 🟢 | `0.1 + 0.2 != 0.3` — binary floats can't hold decimal cents |

### ⚡ Async & Threading

| # | Exhibit | Level | The pain |
|--:|---------|-------|----------|
| 0003 | [Incrementing a shared counter from parallel threads](src/async/0003-race-on-shared-counter/) | 🟢 | `counter++` from two threads — thousands of increments quietly vanish. |

# To Be Continued

More halls under construction: **ORM**, **datetime**,
**strings & memory**, **exceptions**, **DI lifetimes**, **security**.
