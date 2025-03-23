using YatzySimple.Core;
using YatzySimple.Interfaces;
using YatzySimple.Players;
using YatzySimple.States;

public class Program 
{
    public static void Main(string[] args)
    {
        // Initialize the initial state and player
        IGameState initialState = new RollingDiceState();
        IPlayer player = new SimulatedPlayer();

        // Create the GameContext with the initial state and player
        GameContext game = new GameContext(initialState, player);

        while (game.CurrentState.IsGameOn)
        {
            Console.WriteLine("Press any key to play the next turn, or type 's' to stop the game.");

            if(game.CurrentState is RollingDiceState)
            {           
                Console.WriteLine("Current Scores:");
                foreach (var score in game.Scores)
                {
                    Console.WriteLine($"{score.Key}: {score.Value}");
                }
                Console.WriteLine($"Total Score: {game.TotalScore}");

                Console.WriteLine("Rolling dice...");
                //Console.WriteLine("Dice values: " + string.Join(", ", game.DiceValues));
            }
            else if (game.CurrentState is ScoringState)
            {
                Console.WriteLine("Scoring...");
            }
            var input = Console.ReadKey(intercept: true).KeyChar;
            //var input = Console.Read();
            if (input == 's')
            {
                Console.WriteLine("Game stopped by user.");
                break;
            }

            game.PlayNextTurn();
            Console.WriteLine("Dice values: " + string.Join(", ", game.DiceValues));

            if (game.CurrentState is GameOverState)
            {
                Console.WriteLine("Current Scores:");
                foreach (var score in game.Scores)
                {
                    Console.WriteLine($"{score.Key}: {score.Value}");
                }
                Console.WriteLine($"Total Score: {game.TotalScore}");
                
                Console.WriteLine("All Categories scored...");
                Console.WriteLine("Game over");
                break;
            }
        }
    }
}