using SharpDX;
using System;
using TankLibrary.Objects;
using Object = TankLibrary.Objects.Object;

namespace TankLibrary._2DObjectsGraph
{
    /// <summary>
    /// Класс, представляющий визуализацию объекта игры.
    /// </summary>
    public class VisualizationObject
    {
        protected Object _objectGame;
        public Object ObjectGame { get => _objectGame; }

        protected Sprite _sprite;
        public Sprite Sprite { get => _sprite; }

        /// <summary>
        /// Возвращает ограничивающий прямоугольник объекта.
        /// </summary>
        public RectangleF Rect { get => GetRect(); }

        protected float _scale;
        public float Scale { get => _scale; }

        /// <summary>
        /// Инициализирует новый экземпляр класса VisualizationObject с заданными объектом игры и спрайтом.
        /// </summary>
        /// <param name="objectGame">Объект игры.</param>
        /// <param name="sprite">Спрайт.</param>
        public VisualizationObject(Objects.Object objectGame, Sprite sprite)
        {
            _objectGame = objectGame;
            _sprite = sprite;
        }

        private RectangleF GetRect()
        {
            return new RectangleF(_objectGame.Position.X - _sprite.Center.X, _objectGame.Position.Y - _sprite.Center.Y, _sprite.Width, _sprite.Height);
        }
    }
}
