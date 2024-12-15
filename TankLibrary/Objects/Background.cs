using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary.Objects.TankDecorator
{
    /// <summary>
    /// Класс, представляющий фоновый объект.
    /// </summary>
    public class Background : Object
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса Background с указанной позицией.
        /// </summary>
        /// <param name="position">Позиция фонового объекта.</param>
        public Background(Vector2 position) : base(position)
        {
        }
    }
}
