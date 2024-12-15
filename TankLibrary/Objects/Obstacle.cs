using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankLibrary.Objects.Enums;
using TankLibrary.Objects.Interfaces;

namespace TankLibrary.Objects
{
    /// <summary>
    /// Класс, представляющий препятствие.
    /// </summary>
    public class Obstacle : Object, IDamageWall
    {
        private int _health;

        /// <summary>
        /// Здоровье препятствия.
        /// </summary>
        public int Health { get => _health; }

        private int _speed;

        /// <summary>
        /// Скорость препятствия.
        /// </summary>
        public int Speed { get => _speed; }

        private bool _destructibility;

        /// <summary>
        /// Признак разрушаемости препятствия.
        /// </summary>
        public bool Destructibility { get => _destructibility; }

        private ObstacleType _type;

        /// <summary>
        /// Тип препятствия.
        /// </summary>
        public ObstacleType Type { get => _type; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Obstacle с указанной позицией, здоровьем, разрушаемостью, типом и скоростью.
        /// </summary>
        /// <param name="position">Позиция препятствия.</param>
        /// <param name="health">Здоровье препятствия.</param>
        /// <param name="destructibility">Признак разрушаемости препятствия.</param>
        /// <param name="type">Тип препятствия.</param>
        /// <param name="speed">Скорость препятствия.</param>
        public Obstacle(Vector2 position, int health, bool destructibility, ObstacleType type, int speed) : base(position)
        {
            // Конструктор класса Obstacle.
            // Вы можете добавить здесь свою логику и инициализацию препятствия.
            _health = health;
            _speed = speed;
            _destructibility = destructibility;
            _type = type;
        }

        /// <summary>
        /// Повреждает препятствие.
        /// </summary>
        public void DamageObstacle()
        {
            _health--;
        }

        /// <summary>
        /// Проверяет, живо ли препятствие.
        /// </summary>
        /// <returns>Значение true, если препятствие живо; в противном случае - false.</returns>
        public bool GetLive()
        {
            return _health > 0;
        }
    }
}
