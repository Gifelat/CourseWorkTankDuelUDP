using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary._2DObjectsGraph;

namespace TankLibrary.Objects.TankDecorator
{
    public abstract class TankAbstract : TankGame
    {

        protected TankAbstract(TankGame tankPlayer) : base(tankPlayer.Player, tankPlayer.Sprite, tankPlayer.Bullet, tankPlayer.Scale)
        {

        }
    }
}
