using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary._2DObjectsGraph
{
    /// <summary>
    /// Класс, представляющий игровое препятствие, наследующий класс VisualizationObject.
    /// </summary>
    public class ObstacleGame : VisualizationObject
    {
        private Objects.Obstacle _obstacle;

        /// <summary>
        /// Препятствие.
        /// </summary>
        public Objects.Obstacle Obstacle { get => _obstacle; }

        /// <summary>
        /// Инициализирует новый экземпляр класса ObstacleGame с заданными параметрами.
        /// </summary>
        /// <param name="obstacle">Препятствие.</param>
        /// <param name="sprite">Спрайт.</param>
        public ObstacleGame(Objects.Obstacle obstacle, Sprite sprite) : base(obstacle, sprite)
        {
            _obstacle = obstacle;
            _scale = 1;
        }
    }
}
