using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.Objects.Enums;

namespace TankLibrary.Objects.BulletDecorator
{
    public class NullBullet : BulletAbstract
    {
        public NullBullet(Bullet bullet) : base(bullet)
        {
            _speed = 5.0f;
            _damage = 0;
            _type = BulletType.Null;
        }
    }
}
