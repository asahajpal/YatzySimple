using System;
using System.Collections.Generic;
using YatzySimple.Interfaces;
using YatzySimple.Players;
using YatzySimple.States;

namespace YatzySimple.Core
{
    public class GameContext
    {
        private IGameState _state;
        private readonly Random _random;
        public IPlayer Player { get; set; }
        private Dictionary<string, int> _scores;
        private int[] _dice;

        public GameContext(IGameState initialState, IPlayer player)
        {
            _state = initialState;
            Player = player;
            _random = new Random();
            _scores = new Dictionary<string, int>
            {
                { "Ones", 0 },
                { "Twos", 0 },
                { "Threes", 0 },
                { "Fours", 0 },
                { "Fives", 0 },
                { "Sixes", 0 }
            };
            _dice = new int[6];
        }

        public IGameState CurrentState => _state;

        public int[] DiceValues => _dice;

        public Dictionary<string, int> Scores => GetScores();

        public int TotalScore => CalculateTotalScore();

        private void SetState(IGameState state)
        {
            _state = state;
        }

        public void PlayNextTurn()
        {
            _state.NextTurn(this);
        }

        public bool AllCategoriesScored()
        {
            foreach (var score in _scores.Values)
            {
                if (score == 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateScore(string category, int score)
        {
            if (!string.IsNullOrEmpty(category))
            {
                _scores[category] = score;
            }
        }

        public Dictionary<string, int> GetScores()
        {
            return _scores;
        }

        public int CalculateTotalScore()
        {
            int totalScore = 0;
            foreach (var score in _scores.Values)
            {
                totalScore += score;
            }
            return totalScore;
        }

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

        // Example of a method to transition to a new state
        public void TransitionToScoringState()
        {
            SetState(new ScoringState());
        }

        public void TransitionToRollingDiceState()
        {
            SetState(new RollingDiceState());
        }

        internal void TransitionToGameOverState()
        {
           SetState(new GameOverState());
        }

        // Add other state transition methods as needed
    }
}