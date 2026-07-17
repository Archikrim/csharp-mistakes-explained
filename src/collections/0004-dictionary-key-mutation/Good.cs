// Exhibit #0004: the fix

// Tournament leaderboard, keyed by player.
var scores = new Dictionary<Player, int>();

var player = new Player("xX_Slayer_Xx");
scores[player] = 1500;

Console.WriteLine($"Players on the board: {scores.Count}");
Console.WriteLine($"{player.Nickname} has {scores[player]} points");

// Mid-season rebranding: the key is immutable, so we replace it, not mutate it.
var renamed = player with { Nickname = "SeniorSlayer" };
scores[renamed] = scores[player];
scores.Remove(player);
player = renamed;

Console.WriteLine($"Players on the board: {scores.Count}");
foreach (var (p, points) in scores)
{
    Console.WriteLine($"The board plainly lists: {p.Nickname} with {points} points");
}
Console.WriteLine($"ContainsKey finds the player: {scores.ContainsKey(player)}");

Console.WriteLine($"{player.Nickname} has {scores[player]} points");
Console.WriteLine("Rename survived. The points did too.");

record Player(string Nickname);
