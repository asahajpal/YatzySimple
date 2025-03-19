public class SimulatedPlayer : IPlayer
{
    private Random _random;
    private int[] _dice;
    private Strategy _strategy;

    public SimulatedPlayer()
    {
        _random = new Random();
        _dice = new int[5];
        _strategy = new Strategy();
    }

    public int[] Dice => _dice;

    public void RollDice()
    {
        for (int i = 0; i < 3; i++) // Roll dice up to 3 times
        {
            for (int j = 0; j < _dice.Length; j++)
            {
                _dice[j] = _random.Next(1, 7);
            }
        }
    }

    public void ScoreDice(GameContext context)
    {
        string category = _strategy.ChooseCategory(context.GetScores(), _dice);
        int newScore = CalculateScore(category);
        context.UpdateScore(category, newScore);
    }

    private int CalculateScore(string category)
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
        foreach (int die in _dice)
        {
            if (die == number)
            {
                score += number;
            }
        }
        return score;
    }
}
