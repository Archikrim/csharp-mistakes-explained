// Exhibit #0004: mutating an object that lives as a dictionary key

// Tournament leaderboard, keyed by player.
var scores = new Dictionary<Player, int>();

var player = new Player { Nickname = "xX_Slayer_Xx" };
scores[player] = 1500;

Console.WriteLine($"Players on the board: {scores.Count}");
Console.WriteLine($"{player.Nickname} has {scores[player]} points");

// Mid-season rebranding. What could a rename possibly break?
player.Nickname = "SeniorSlayer";

Console.WriteLine($"Players on the board: {scores.Count}");
foreach (var (p, points) in scores)
{
    Console.WriteLine($"The board plainly lists: {p.Nickname} with {points} points");
}
Console.WriteLine($"ContainsKey finds the player: {scores.ContainsKey(player)}");

// The entry is right there. We just watched foreach print it. Read it:
Console.WriteLine($"{player.Nickname} has {scores[player]} points"); // 💥

record Player
{
    public required string Nickname { get; set; }
}
