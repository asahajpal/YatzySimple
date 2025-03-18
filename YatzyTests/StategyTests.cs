using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YatzyTests
{

    //[TestClass]
    public class StrategyTests
    {
        [Fact]
        public void ChooseCategory_ShouldReturnCategory_None_WhenNoUnscoredCategoriesMatchDice()
        {
            // Arrange
            var strategy = new Strategy();
            var scores = new Dictionary<string, int>
        {
            { "Ones", 0 },
            { "Twos", 8 },
            { "Threes", 6 },
            { "Fours", 4 },
            { "Fives", 10 },
            { "Sixes", 6 }
        };
            int[] dice = { 2, 2, 3, 6, 2 }; // Dice values

            // Act
            var result = strategy.ChooseCategory(scores, dice);

            // Assert
            Assert.Equal(@"None", result);
        }
    }
}
