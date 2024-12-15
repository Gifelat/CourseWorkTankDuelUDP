using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpDX;
using System;
using TankLibrary._2DObjectsGraph;
using TankLibrary.Objects;
using TankLibrary;

namespace TankTest
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void DamageTank_ArmorNotZero_ArmorReduced()
        {
            // Arrange
            TankGame tank = new TankGame(new TankPlayer(new SharpDX.Vector2(100, 100), 1, 20, 10), new Sprite(0, 10, 10, 0), null, 0);
            int damage = 10;
            int expectedArmor = tank.Player.Armor - damage;
            int expectedHealth = tank.Player.Health;

            // Act
            tank.Player.DamageTank(damage);

            // Assert
            Assert.AreEqual(expectedArmor, tank.Player.Armor);
            Assert.AreEqual(expectedHealth, tank.Player.Health);
        }


        [TestMethod]
        public void TestDamageTank()
        {
            // Arrange
            var tankPlayer = new TankPlayer(new Vector2(0, 0), 10, 50, 5);

            // Act
            tankPlayer.DamageTank(20);

            // Assert
            Assert.AreEqual(85, tankPlayer.Health);
            Assert.AreEqual(0, tankPlayer.Armor);

            // Act
            tankPlayer.DamageTank(10);

            // Assert
            Assert.AreEqual(75, tankPlayer.Health);
            Assert.AreEqual(0, tankPlayer.Armor);

            // Act
            tankPlayer.DamageTank(80);

            // Assert
            Assert.AreEqual(-5, tankPlayer.Health);
            Assert.AreEqual(0, tankPlayer.Armor);
        }

        [TestMethod]
        public void TestMovement()
        {
            // Arrange
            var tankPlayer = new TankPlayer(new Vector2(0, 0), 10, 50, 5);

            // Act
            tankPlayer.Up();
            tankPlayer.Left();

            // Assert
            Assert.AreEqual(-10, tankPlayer.Position.X);
            Assert.AreEqual(-10, tankPlayer.Position.Y);

            // Act
            tankPlayer.Right();
            tankPlayer.Down();

            // Assert
            Assert.AreEqual(0, tankPlayer.Position.X);
            Assert.AreEqual(0, tankPlayer.Position.Y);

            // Act
            tankPlayer.Speed = 20;
            tankPlayer.Up();

            // Assert
            Assert.AreEqual(-20, tankPlayer.Position.Y);
            Assert.AreEqual(48.75f, tankPlayer.Gaz);
        }
    }
}
