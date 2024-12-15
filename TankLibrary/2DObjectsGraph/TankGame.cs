using SharpDX;
using System.Collections.Generic;
using System.Text;
using TankLibrary.Objects;
using TankLibrary.Objects.BulletDecorator;
using TankLibrary.Objects.Enums;
using System.Globalization;
using System;

namespace TankLibrary._2DObjectsGraph
{
    /// <summary>
    /// Класс, представляющий игру с танком, наследующий класс VisualizationObject.
    /// </summary>
    public class TankGame : VisualizationObject
    {
        private TankPlayer _player;
        public TankPlayer Player { get => _player; }

        private Dictionary<int, int> _bullet;
        public Dictionary<int, int> Bullet { get => _bullet; }

        private float _rotation;
        private MovementKeys _move;
        public MovementKeys Move { get => _move; }

        /// <summary>
        /// Инициализирует новый экземпляр класса TankGame с заданными параметрами.
        /// </summary>
        /// <param name="player">Игрок-танк.</param>
        /// <param name="sprite">Спрайт.</param>
        /// <param name="bullets">Словарь с количеством пуль различного типа.</param>
        /// <param name="scale">Масштаб.</param>
        public TankGame(TankPlayer player, Sprite sprite, Dictionary<int, int> bullets, float scale) : base(player, sprite)
        {
            _bullet = bullets;
            _move = MovementKeys.Up;
            _player = player;
            _scale = scale;
        }

        /// <summary>
        /// Выстрел танка в зависимости от текущего направления движения.
        /// </summary>
        /// <returns>Снаряд, который был выпущен.</returns>
        public Bullet Shoot()
        {
            switch (_move)
            {
                case MovementKeys.Up:
                    {
                        if (_bullet[1] > 0)
                        {
                            _bullet[1]--;
                            return new SmallBullet(new Bullet(Rect.Center));
                        }

                        if (_bullet[2] > 0)
                        {
                            _bullet[2]--;
                            return new MediumBullet(new Bullet(Rect.Center));
                        }
                        if (_bullet[3] > 0)
                        {
                            _bullet[3]--;
                            return new LargeBullet(new Bullet(Rect.Center));
                        }
                    }
                    break;
                case MovementKeys.Left:
                    {
                        if (_bullet[1] > 0)
                        {
                            _bullet[1]--;
                            return new SmallBullet(new Bullet(Rect.Center));
                        }

                        if (_bullet[2] > 0)
                        {
                            _bullet[2]--;
                            return new MediumBullet(new Bullet(Rect.Center));
                        }
                        if (_bullet[3] > 0)
                        {
                            _bullet[3]--;
                            return new LargeBullet(new Bullet(Rect.Center));
                        }
                    }
                    break;
                case MovementKeys.Down:
                    {
                        if (_bullet[1] > 0)
                        {
                            _bullet[1]--;
                            return new SmallBullet(new Bullet(Rect.Center));
                        }

                        if (_bullet[2] > 0)
                        {
                            _bullet[2]--;
                            return new MediumBullet(new Bullet(Rect.Center));
                        }
                        if (_bullet[3] > 0)
                        {
                            _bullet[3]--;
                            return new LargeBullet(new Bullet(Rect.Center));
                        }
                    }
                    break;
                case MovementKeys.Right:
                    {
                        if (_bullet[1] > 0)
                        {
                            _bullet[1]--;
                            return new SmallBullet(new Bullet(Rect.Center));
                        }

                        if (_bullet[2] > 0)
                        {
                            _bullet[2]--;
                            return new MediumBullet(new Bullet(Rect.Center));
                        }
                        if (_bullet[3] > 0)
                        {
                            _bullet[3]--;
                            return new LargeBullet(new Bullet(Rect.Center));
                        }
                    }
                    break;
            }

            return new NullBullet(new Bullet(Rect.Center));
        }

        /// <summary>
        /// Выполняет движение танка в указанном направлении.
        /// </summary>
        /// <param name="key">Направление движения.</param>
        public void Movement(MovementKeys key)
        {
            switch (key)
            {
                case MovementKeys.Up:
                    _player.Up();
                    _move = MovementKeys.Up;
                    _rotation = 0;
                    break;
                case MovementKeys.Left:
                    _player.Left();
                    _move = MovementKeys.Left;
                    _rotation = -(float)(3.14 / 2);
                    break;
                case MovementKeys.Down:
                    _player.Down();
                    _move = MovementKeys.Down;
                    _rotation = (float)3.14;
                    break;
                case MovementKeys.Right:
                    _player.Right();
                    _move = MovementKeys.Right;
                    _rotation = (float)(3.14 / 2);
                    break;
            }

            _sprite.Rotation = _rotation;
        }

        /// <summary>
        /// Устанавливает центр вращения спрайта танка.
        /// </summary>
        public void RotationCenter()
        {
            Sprite.SetCenterRotation(Rect.Width);
        }


        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(CultureInfo.InvariantCulture,
                "{0},{1};{2};{3};{4}",
                _player.Position.X, _player.Position.Y, _rotation, _player.Gaz, _player.Health);

            return sb.ToString();
        }

        public void Deserialize(string str)
        {
            string[] data = str.Split(';');

            if (data.Length == 4)
            {
                string[] positionSplit = data[0].Split(',');

                if (positionSplit.Length == 2 &&
                    float.TryParse(positionSplit[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float posX) &&
                    float.TryParse(positionSplit[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float posY) &&
                    float.TryParse(data[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float rotation) &&
                    float.TryParse(data[2], NumberStyles.Float, CultureInfo.InvariantCulture, out float gaz) &&
                    int.TryParse(data[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out int health))
                {
                    Vector2 position = new Vector2(posX, posY);
                    _player.SetPosition(position);
                    _sprite.Rotation = rotation;
                    _player.SetGaz(gaz);
                    _player.SetHealth(health);
                }
                else
                {
                    throw new FormatException($"Invalid data format: {str}");
                }
            }
            else
            {
                throw new FormatException($"Incorrect number of elements in data: {str}");
            }
        }

    }
}
