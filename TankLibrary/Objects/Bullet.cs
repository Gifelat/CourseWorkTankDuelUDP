using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.Objects.Enums;

namespace TankLibrary.Objects
{
    /// <summary>
    /// Класс, представляющий пулю.
    /// </summary>
    public class Bullet : Objects.Object
    {
        protected float _speed;
        /// <summary>
        /// Скорость пули.
        /// </summary>
        public float Speed { get => _speed; }

        protected int _damage;
        /// <summary>
        /// Урон, наносимый пулей.
        /// </summary>
        public int Damage { get => _damage; }

        private MovementKeys _move;
        /// <summary>
        /// Направление движения пули.
        /// </summary>
        public MovementKeys Move { get => _move; set => _move = value; }

        protected BulletType _type;
        /// <summary>
        /// Тип пули.
        /// </summary>
        public BulletType Type { get => _type; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Bullet с указанной позицией.
        /// </summary>
        /// <param name="position">Позиция пули.</param>
        public Bullet(Vector2 position) : base(position)
        {
            // Конструктор класса Bullet.
            // Вы можете добавить здесь свою логику и инициализацию объекта.
        }

        /// <summary>
        /// Перемещает пулю в соответствии с заданным направлением движения.
        /// </summary>
        public void Fly()
        {
            switch (_move)
            {
                case MovementKeys.Right:
                    _position.X += _speed;
                    break;
                case MovementKeys.Left:
                    _position.X -= _speed;
                    break;
                case MovementKeys.Up:
                    _position.Y -= _speed;
                    break;
                case MovementKeys.Down:
                    _position.Y += _speed;
                    break;
            }
        }

        public void SetPosition(Vector2 position) => _position = position;
        public void SetDamage(int damage) => _damage = damage;
        public void SetSpeed(float speed) => _speed = speed;
    }
}
