using YatzySimple.Core;
using YatzySimple.Interfaces;

namespace YatzySimple.Players
{
    public class SimulatedPlayer : IPlayer
    {
        private Strategy _strategy;

        public SimulatedPlayer()
        {
            _strategy = new Strategy();
        }

        public void RollDice(GameContext context)
        {
            context.RollDice();
        }

        public void ScoreDice(GameContext context)
        {
            string category = _strategy.ChooseCategory(context.GetScores(), context.DiceValues);
            int newScore = CalculateScore(category, context.DiceValues);
            context.UpdateScore(category, newScore);
        }

        private int CalculateScore(string category, int[] dice)
        {
            int number = category switch
            {
                "Ones" => 1,
                "Twos" => 2,
                "Threes" => 3,
                "Fours" => 4,
                "Fives" => 5,
                "Sixes" => 6,
                _ => 0
            };

            int score = 0;
            foreach (int die in dice)
            {
                if (die == number)
                {
                    score += number;
                }
            }
            return score;
        }
    }
}