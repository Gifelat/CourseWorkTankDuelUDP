using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.Objects.Enums;

namespace TankLibrary.Objects.BulletDecorator
{
    public class MediumBullet : BulletAbstract
    {
        public MediumBullet(Bullet bullet) : base(bullet)
        {
            _speed = 5.0f;
            _damage = 30;
            _type = BulletType.Meduim;
        }
    }
}
