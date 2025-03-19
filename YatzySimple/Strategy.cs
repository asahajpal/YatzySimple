using System;
using System.Collections.Generic;

namespace YatzySimple.Core
{
    public class Strategy
    {
        public string ChooseCategory(Dictionary<string, int> scores, int[] dice)
        {
            string bestCategory = string.Empty;
            int maxScore = 0;

            foreach (var category in scores.Keys)
            {
                if (scores[category] == 0) // Choose only unscored categories
                {
                    int score = CalculatePotentialScore(category, dice);
                    if (score > maxScore)
                    {
                        maxScore = score;
                        bestCategory = category;
                    }
                }
            }

            return bestCategory;
        }

        private int CalculatePotentialScore(string category, int[] dice)
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
