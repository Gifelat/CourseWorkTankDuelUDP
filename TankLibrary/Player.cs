using System.Collections.Generic;

namespace TankLibrary
{
    public class Player
    {
        public Dictionary<int, int> bulletCount { get; set; }
        public float gas { get; set; }
        public int armor { get; set; }

        public Player(Dictionary<int, int> bulletCount, float gas, int armor)
        {
            this.bulletCount = bulletCount;
            this.gas = gas;
            this.armor = armor;
        }
    }
}
