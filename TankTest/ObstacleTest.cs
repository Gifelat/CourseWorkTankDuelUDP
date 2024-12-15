using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary;
using TankLibrary.Objects;
using TankLibrary.Objects.Enums;
using Obstacle = TankLibrary.Objects.Obstacle;

namespace TankTest
{
    [TestClass]
    public class ObstacleTest
    {
        [TestMethod]
        public void TestDamageObstacle()
        {
            // Arrange
            var obstacle = new Obstacle(new Vector2(0, 0), 10, true, ObstacleType.Weak, 5);

            // Act
            obstacle.DamageObstacle();

            // Assert
            Assert.AreEqual(9, obstacle.Health);

            // Act
            obstacle.DamageObstacle();
            obstacle.DamageObstacle();

            // Assert
            Assert.AreEqual(7, obstacle.Health);
        }

        [TestMethod]
        public void TestGetLive()
        {
            // Arrange
            var obstacle1 = new Obstacle(new Vector2(0, 0), 5, true, ObstacleType.Weak, 3);
            var obstacle2 = new Obstacle(new Vector2(0, 0), 0, false, ObstacleType.Endless, 5);

            // Assert
            Assert.IsTrue(obstacle1.GetLive());
            Assert.IsFalse(obstacle2.GetLive());
        }

    }
}
