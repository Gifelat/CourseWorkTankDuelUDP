using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary.Objects
{
    /// <summary>
    /// Абстрактный класс, представляющий объект.
    /// </summary>
    public abstract class Object
    {
        protected Vector2 _position;

        /// <summary>
        /// Позиция объекта.
        /// </summary>
        public Vector2 Position { get => _position; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Object с указанной позицией.
        /// </summary>
        /// <param name="position">Позиция объекта.</param>
        public Object(Vector2 position)
        {
            _position = position;
        }
    }
}
