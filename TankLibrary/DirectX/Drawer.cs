using SharpDX.Direct2D1;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.DirectX;
using TankLibrary._2DObjectsGraph;
using TankLibrary.Objects;

namespace TankLibrary.DirectX
{
    // <summary>
    /// Класс для рисования объектов с помощью DirectX2D.
    /// </summary>
    public class Drawer
    {
        private DirectX2D _dx2d;
        private Vector2 _translation;
        private Matrix3x2 _matrix;
        private Matrix3x2 _last;

        /// <summary>
        /// Создает новый экземпляр класса Drawer.
        /// </summary>
        /// <param name="dx2d">Объект DirectX2D для рисования.</param>
        public Drawer(DirectX2D dx2d)
        {
            _dx2d = dx2d;
        }

        /// <summary>
        /// Рисует объект визуализации.
        /// </summary>
        /// <param name="render">Объект визуализации, содержащий информацию о рисуемом объекте.</param>
        public void DrawObject(VisualizationObject render)
        {
            RectangleF rect = render.Rect;
            _dx2d.RenderTarget.DrawBitmap(_dx2d.Bitmap[render.Sprite.SpriteId], rect, 1, BitmapInterpolationMode.Linear);
        }

        /// <summary>
        /// Рисует текст в заданном прямоугольнике.
        /// </summary>
        /// <param name="rect">Прямоугольник, в котором будет отображаться текст.</param>
        /// <param name="count">Числовое значение, отображаемое в тексте.</param>
        public void DrawText(RectangleF rect, int count)
        {
            if (count == 0)
                _dx2d.RenderTarget.DrawText(count.ToString(), _dx2d.TextFormat, rect, _dx2d.BrushRed);
            else
                _dx2d.RenderTarget.DrawText(count.ToString(), _dx2d.TextFormat, rect, _dx2d.Brush);
        }

        /// <summary>
        /// Рисует уровень топлива игрока в заданной позиции.
        /// </summary>
        /// <param name="player">Игрок, чей уровень топлива будет отображаться.</param>
        /// <param name="pos">Позиция, в которой будет отображаться текст.</param>
        public void DrawGas(TankPlayer player, Vector2 pos)
        {
            _dx2d.RenderTarget.DrawText(player.Gaz.ToString(), _dx2d.TextFormat,
                 new SharpDX.Mathematics.Interop.RawRectangleF(pos.X, pos.Y, 600, 100), _dx2d.Brush);
        }

        /// <summary>
        /// Рисует уровень здоровья игрока в заданной позиции.
        /// </summary>
        /// <param name="player">Игрок, чей уровень здоровья будет отображаться.</param>
        /// <param name="pos">Позиция, в которой будет отображаться текст.</param>
        public void DrawXP(TankPlayer player, Vector2 pos)
        {
            _dx2d.RenderTarget.DrawText(player.Health.ToString(), _dx2d.TextFormat,
                 new SharpDX.Mathematics.Interop.RawRectangleF(pos.X, pos.Y, 600, 100), _dx2d.Brush);
        }

        /// <summary>
        /// Рисует броню игрока в заданной позиции.
        /// </summary>
        /// <param name="player">Игрок, чей уровень брони будет отображаться.</param>
        /// <param name="pos">Позиция, в которой будет отображаться текст.</param>
        public void DrawArmor(TankPlayer player, Vector2 pos)
        {
            _dx2d.RenderTarget.DrawText(player.Armor.ToString(), _dx2d.TextFormat,
                 new SharpDX.Mathematics.Interop.RawRectangleF(pos.X, pos.Y, 600, 100), _dx2d.Brush);
        }

        /// <summary>
        /// Рисует объект визуализации с учетом масштабирования, смещения и вращения.
        /// </summary>
        /// <param name="render">Объект визуализации, содержащий информацию о рисуемом объекте.</param>
        public void Draw(VisualizationObject render)
        {
            float scale = render.Scale;
            _translation.X = (render.ObjectGame.Position.X - render.Sprite.Center.X);
            _translation.Y = (render.ObjectGame.Position.Y - render.Sprite.Center.Y);

            _matrix = Matrix3x2.Rotation(render.Sprite.Rotation, render.Sprite.Center) * Matrix3x2.Scaling(scale, scale, Vector2.Zero) * Matrix3x2.Translation(_translation);

            WindowRenderTarget target = _dx2d.RenderTarget;
            _last = target.Transform;
            target.Transform = _matrix;

            Bitmap bitmap = _dx2d.Bitmap[render.Sprite.SpriteId];
            target.DrawBitmap(bitmap, 1, BitmapInterpolationMode.Linear);
            target.Transform = _last;
        }
    }
}
