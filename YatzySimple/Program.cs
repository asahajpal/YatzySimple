using YatzySimple.Core;
using YatzySimple.States;

public class Program
{
    public static void Main(string[] args)
    {
        GameContext game = new GameContext();

        while (game.CurrentState.IsGameOn)
        {
            Console.WriteLine("Press any key to play the next turn, or type 's' to stop the game.");
            var input = Console.ReadKey(intercept: true).KeyChar;
            if (input == 's')
            {
                Console.WriteLine("Game stopped by user.");
                break;
            }

            game.PlayNextTurn();

            if (game.CurrentState is RollingDiceState)
            {
                Console.WriteLine("Rolling dice...");
                Console.WriteLine("Dice values: " + string.Join(", ", game.DiceValues));
            }
            else if (game.CurrentState is ScoringState)
            {
                Console.WriteLine("Scoring...");
                Console.WriteLine("Current Scores:");
                foreach (var score in game.Scores)
                {
                    Console.WriteLine($"{score.Key}: {score.Value}");
                }
                Console.WriteLine($"Total Score: {game.TotalScore}");
            }

            if (!game.CurrentState.IsGameOn)
            {
                Console.WriteLine("Game over");
                break;
            }
        }
    }
}