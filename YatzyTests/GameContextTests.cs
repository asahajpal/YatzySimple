using Xunit;
using YatzySimple.Core;
using YatzySimple.Players;
using YatzySimple.States;
using YatzySimple.Interfaces;
using Moq;

namespace YatzyTests
{
    public class GameContextTests
    {
        [Fact]
        public void GameContext_ShouldInitializeCorrectly()
        {
            // Arrange
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();

            // Act
            var gameContext = new GameContext(initialState, player);

            // Assert
            Assert.NotNull(gameContext.CurrentState);
            Assert.NotNull(gameContext.Player);
            Assert.Equal(initialState, gameContext.CurrentState);
            Assert.Equal(player, gameContext.Player);
        }

        [Fact]
        public void PlayNextTurn_ShouldTransitionToScoringState()
        {
            // Arrange
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();
            var gameContext = new GameContext(initialState, player);

            // Act
            gameContext.PlayNextTurn();

            // Assert
            Assert.IsType<ScoringState>(gameContext.CurrentState);
        }

        [Fact]
        public void AllCategoriesScored_ShouldReturnFalse_WhenNotAllScored()
        {
            // Arrange
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();
            var gameContext = new GameContext(initialState, player);

            // Act
            var result = gameContext.AllCategoriesScored();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void AllCategoriesScored_ShouldReturnTrue_WhenAllScored()
        {
            // Arrange
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();
            var gameContext = new GameContext(initialState, player);
            gameContext.UpdateScore("Ones", 1);
            gameContext.UpdateScore("Twos", 2);
            gameContext.UpdateScore("Threes", 3);
            gameContext.UpdateScore("Fours", 4);
            gameContext.UpdateScore("Fives", 5);
            gameContext.UpdateScore("Sixes", 6);

            // Act
            var result = gameContext.AllCategoriesScored();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void PlayNextTurn_ShouldReachGameOverState_WhenAllCategoriesAreScored()
        {
            // Arrange
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();
            var gameContext = new GameContext(initialState, player);
            
            // Set all categories to non-zero scores
            gameContext.UpdateScore("Ones", 1);
            gameContext.UpdateScore("Twos", 2);
            gameContext.UpdateScore("Threes", 3);
            gameContext.UpdateScore("Fours", 4);
            gameContext.UpdateScore("Fives", 5);
            gameContext.UpdateScore("Sixes", 6);
            gameContext.TransitionToScoringState();

            // Act
            gameContext.PlayNextTurn();

            // Assert
            Assert.IsType<GameOverState>(gameContext.CurrentState);
        }

        [Fact]
        public void PlayNextTurn_ShouldTransitionToScoringState_AfterRollingDice()
        {
            // Arrange
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();
            var gameContext = new GameContext(initialState, player);

            // Act
            gameContext.PlayNextTurn();

            // Assert
            Assert.IsType<ScoringState>(gameContext.CurrentState);
        }

        [Fact]
        public void PlayNextTurn_ShouldUpdateScores_WhenInScoringState()
        {
            // Arrange
            var mockPlayer = new Mock<IPlayer>();
            //mockPlayer.Setup(p => p.Dice).Returns(new int[] { 1, 1, 1, 1, 1 });
            //mockPlayer.Setup(p => p.RollDice());
            mockPlayer.Setup(p => p.ScoreDice(It.IsAny<GameContext>())).Callback<GameContext>(context =>
            {
                context.UpdateScore("Ones", 5);
            });
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();
            var gameContext = new GameContext(initialState, mockPlayer.Object);

            // Act
            gameContext.PlayNextTurn(); // RollingDiceState to ScoringState
            gameContext.PlayNextTurn(); // ScoringState to RollingDiceState or GameOverState

            // Assert
            Assert.Equal(5, gameContext.GetScores()["Ones"]);
        }

        [Fact]
        public void TotalScore_ShouldBeSumOfIndividualCategoryScores()
        {
            // Arrange
            var initialState = new RollingDiceState();
            var player = new SimulatedPlayer();
            var gameContext = new GameContext(initialState, player);

            // Update scores for different categories
            gameContext.UpdateScore("Ones", 1);
            gameContext.UpdateScore("Twos", 2);
            gameContext.UpdateScore("Threes", 3);
            gameContext.UpdateScore("Fours", 4);
            gameContext.UpdateScore("Fives", 5);
            gameContext.UpdateScore("Sixes", 6);

            // Act
            int totalScore = gameContext.CalculateTotalScore();

            // Assert
            Assert.Equal(21, totalScore); // 1 + 2 + 3 + 4 + 5 + 6 = 21
        }
    }
}
