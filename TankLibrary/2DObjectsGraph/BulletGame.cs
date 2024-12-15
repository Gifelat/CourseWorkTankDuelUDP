using SharpDX;
using System;
using System.Globalization;
using System.Text;
using TankLibrary.Objects;

namespace TankLibrary._2DObjectsGraph
{
    /// <summary>
    /// Класс, представляющий игровой снаряд, наследующий класс VisualizationObject.
    /// </summary>
    public class BulletGame : VisualizationObject
    {
        private float _time;
        /// <summary>
        /// Время жизни снаряда.
        /// </summary>
        public float Time { get => _time; }

        /// <summary>
        /// Игровой снаряд.
        /// </summary>
        public Bullet Bullet { get; private set; }

        /// <summary>
        /// Инициализирует новый экземпляр класса BulletGame с заданными параметрами.
        /// </summary>
        /// <param name="bullet">Снаряд.</param>
        /// <param name="sprite">Спрайт.</param>
        /// <param name="time">Время жизни снаряда.</param>
        /// <param name="scale">Масштаб.</param>
        public BulletGame(Bullet bullet, Sprite sprite, float time, float scale) : base(bullet, sprite)
        {
            Bullet = bullet;
            _scale = scale;
            _time = time;
        }

        public string Serialize()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendFormat(CultureInfo.InvariantCulture,
                "{0};{1},{2};{3};{4};{5};{6};{7};{8};{9};{10}",
                _time, Bullet.Position.X, Bullet.Position.Y, _scale, Bullet.Damage,
                Sprite.SpriteId, Sprite.Width, Sprite.Height, Sprite.Rotation, Bullet.Speed,
                Bullet.Move.ToString()); 

            return sb.ToString();
        }


        public void Deserialize(string str)
        {
            string[] data = str.Split(';');

            if (data.Length == 9 || data.Length == 10) // Допускаем строки с 9 или 10 элементами
            {
                if (!float.TryParse(data[0], NumberStyles.Float, CultureInfo.InvariantCulture, out _time))
                    throw new FormatException($"Invalid time format: {data[0]}");

                string[] positionData = data[1].Split(',');

                if (positionData.Length != 2 ||
                    !float.TryParse(positionData[0], NumberStyles.Float, CultureInfo.InvariantCulture, out float posX) ||
                    !float.TryParse(positionData[1], NumberStyles.Float, CultureInfo.InvariantCulture, out float posY))
                    throw new FormatException($"Invalid position format: {data[1]}");

                Bullet.SetPosition(new Vector2(posX, posY));

                if (!float.TryParse(data[2], NumberStyles.Float, CultureInfo.InvariantCulture, out _scale))
                    throw new FormatException($"Invalid scale: {data[2]}");

                if (!int.TryParse(data[3], NumberStyles.Integer, CultureInfo.InvariantCulture, out int damage))
                    throw new FormatException($"Invalid damage format: {data[3]}");

                Bullet.SetDamage(damage);

                if (!int.TryParse(data[4], out int spriteId) ||
                    !int.TryParse(data[5], out int spriteWidth) ||
                    !int.TryParse(data[6], out int spriteHeight) ||
                    !float.TryParse(data[7], NumberStyles.Float, CultureInfo.InvariantCulture, out float spriteRotation))
                {
                    throw new FormatException($"Invalid sprite data: {data[4]}, {data[5]}, {data[6]}, {data[7]}");
                }

                _sprite = new Sprite(spriteId, spriteWidth, spriteHeight, spriteRotation);

                if (!float.TryParse(data[8], NumberStyles.Float, CultureInfo.InvariantCulture, out float speed))
                    throw new FormatException($"Invalid speed: {data[8]}");

                Bullet.SetSpeed(speed);

                if (data.Length == 10)
                {
                    switch (data[9])
                    {
                        case "Up":
                            Bullet.Move = Objects.Enums.MovementKeys.Up;
                            break;
                        case "Down":
                            Bullet.Move = Objects.Enums.MovementKeys.Down;
                            break;
                        case "Left":
                            Bullet.Move = Objects.Enums.MovementKeys.Left;
                            break;
                        case "Right":
                            Bullet.Move = Objects.Enums.MovementKeys.Right;
                            break;
                        default:
                            throw new FormatException($"Invalid movement key: {data[9]}");
                    }
                }
                else
                {
                    // Если поле движения отсутствует, устанавливаем значение по умолчанию
                    Bullet.Move = Objects.Enums.MovementKeys.Up;
                }
            }
            else
            {
                throw new FormatException($"Invalid data format: {str}");
            }
        }


    }
}
