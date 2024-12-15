using TankLibrary.Objects.TankDecorator;

namespace TankLibrary._2DObjectsGraph
{
    /// <summary>
    /// Класс, представляющий фон игры, наследующий класс VisualizationObject.
    /// </summary>
    public class BackgroundGame : VisualizationObject
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса BackgroundGame с заданными параметрами.
        /// </summary>
        /// <param name="background">Фон.</param>
        /// <param name="sprite">Спрайт.</param>
        public BackgroundGame(Background background, Sprite sprite) : base(background, sprite)
        {
            _scale = 1;
        }
    }
}
