using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary.Objects.BulletDecorator
{
    public abstract class BulletAbstract : Bullet
    {



        protected BulletAbstract(Bullet bullet) : base(bullet.Position)
        {
        }
    }
}
