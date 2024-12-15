using SharpDX;
using TankLibrary.Objects.Interfaces;

namespace TankLibrary.Objects
{
    /// <summary>
    /// Класс, представляющий игровой танк игрока.
    /// </summary>
    public class TankPlayer : Objects.Object, IMovement
    {
        protected float _speed;

        /// <summary>
        /// Скорость танка.
        /// </summary>
        public float Speed { get => _speed; set => _speed = value; }

        private int _health;

        /// <summary>
        /// Здоровье танка.
        /// </summary>
        public int Health { get => _health; }

        private float _gaz;

        /// <summary>
        /// Уровень топлива танка.
        /// </summary>
        public float Gaz { get => _gaz; }

        private int _armor;

        /// <summary>
        /// Броня танка.
        /// </summary>
        public int Armor { get => _armor; }

        /// <summary>
        /// Признак наличия бонуса.
        /// </summary>
        public bool Bonus;

        /// <summary>
        /// Инициализирует новый экземпляр класса TankPlayer с указанной позицией, скоростью, уровнем топлива и броней.
        /// </summary>
        /// <param name="position">Позиция танка.</param>
        /// <param name="speed">Скорость танка.</param>
        /// <param name="gaz">Уровень топлива танка.</param>
        /// <param name="armor">Броня танка.</param>
        public TankPlayer(Vector2 position, float speed, float gaz, int armor) : base(position)
        {
            // Конструктор класса TankPlayer.
            // Вы можете добавить здесь свою логику и инициализацию танка.
            _speed = speed;
            Bonus = false;
            _gaz = gaz;
            _armor = armor;
            _health = 100;
        }

        /// <summary>
        /// Наносит урон танку.
        /// </summary>
        /// <param name="damage">Количество урона.</param>
        public void DamageTank(int damage)
        {
            if (_armor != 0)
            {
                if (damage > _armor)
                {
                    damage -= _armor;
                    _armor = 0;
                    _health -= damage;
                }
                else
                {
                    _armor -= damage;
                }
            }
            else
            {
                _health -= damage;
            }
        }

        /// <summary>
        /// Двигает танк вниз.
        /// </summary>
        public void Down()
        {
            _position.Y += _speed;
            _gaz -= 0.25f;
        }

        /// <summary>
        /// Двигает танк влево.
        /// </summary>
        public void Left()
        {
            _position.X -= _speed;
            _gaz -= 0.25f;
        }

        /// <summary>
        /// Двигает танк вправо.
        /// </summary>
        public void Right()
        {
            _position.X += _speed;
            _gaz -= 0.25f;
        }

        /// <summary>
        /// Двигает танк вверх.
        /// </summary>
        public void Up()
        {
            _position.Y -= _speed;
            _gaz -= 0.25f;
        }

        public void SetPosition(Vector2 position)
        {
            _position = position;
        }

        public void SetGaz(float gaz)
        {
            _gaz = gaz;
        }

        public void SetHealth(int health)
        {
            _health = health;
        }
    }
}
