using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary._2DObjectsGraph;

namespace TankLibrary.Objects.TankDecorator
{
    public class Normal : TankAbstract
    {
        public Normal(TankGame tankPlayer) : base(tankPlayer)
        {
            tankPlayer.Player.Speed = 3;
        }
    }
}
