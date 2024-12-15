using System;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX;
using SharpDX.WIC;
using SharpDX.Windows;
using System.Drawing;
using Color = SharpDX.Color;
using System.Collections.Generic;
using Brush = SharpDX.Direct2D1.Brush;

namespace Tank.DirectX
{
    /// <summary>
    /// Класс для работы с DirectX 2D.
    /// </summary>
    public class DirectX2D : IDisposable
    {
        private SharpDX.Direct2D1.Factory _factory;
        public SharpDX.Direct2D1.Factory Factory { get => _factory; }

        private ImagingFactory _imgFactory;
        public ImagingFactory ImgFactory { get => _imgFactory; }

        private SharpDX.DirectWrite.Factory _textFactory;
        /// <summary>
        /// Фабрика SharpDX.DirectWrite.Factory.
        /// </summary>
        public SharpDX.DirectWrite.Factory TextFactory { get => _textFactory; }


        private WindowRenderTarget _renderTarget;
        /// <summary>
        /// Оконная цель рендеринга (WindowRenderTarget).
        /// </summary>
        public WindowRenderTarget RenderTarget { get => _renderTarget; }

        private TextFormat _textFormat;
        /// <summary>
        /// Формат текста (TextFormat).
        /// </summary>
        public TextFormat TextFormat { get => _textFormat; }

        private Brush _brush;
        /// <summary>
        /// Кисть для рисования (Brush).
        /// </summary>
        public Brush Brush { get => _brush; }

        private Brush _brushRed;

        public Brush BrushRed { get => _brushRed; }

        private Brush _blackBrush;
        public Brush BlackBrush { get => _blackBrush; }

        private List<SharpDX.Direct2D1.Bitmap> _bitmap;
        /// <summary>
        /// Список загруженных битмапов (SharpDX.Direct2D1.Bitmap).
        /// </summary>
        public List<SharpDX.Direct2D1.Bitmap> Bitmap { get => _bitmap; }
        /// <summary>
        /// Создает новый экземпляр класса DirectX2D.
        /// </summary>
        /// <param name="form">Объект RenderForm для рендеринга.</param>
        public DirectX2D(RenderForm form)
        {
            _factory = new SharpDX.Direct2D1.Factory();
            _imgFactory = new ImagingFactory();
            _textFactory = new SharpDX.DirectWrite.Factory();

            RenderTargetProperties renderProp = new RenderTargetProperties();
            HwndRenderTargetProperties winProp = new HwndRenderTargetProperties()
            {
                Hwnd = form.Handle,
                PixelSize = new Size2(form.ClientSize.Width, form.ClientSize.Height),
                PresentOptions = PresentOptions.None

            };

            _renderTarget = new WindowRenderTarget(_factory, renderProp, winProp);

            _textFormat = new TextFormat(_textFactory, "Arial", 24);
            _textFormat.ParagraphAlignment = ParagraphAlignment.Center;
            _textFormat.TextAlignment = TextAlignment.Center;

            _brush = new SolidColorBrush(_renderTarget, Color.White);
            _brushRed = new SolidColorBrush(_renderTarget, Color.Red);
            _blackBrush = new SolidColorBrush(_renderTarget, Color.Black);
        }
        /// <summary>
        /// Загружает изображение и возвращает индекс в списке битмапов.
        /// </summary>
        /// <param name="path">Путь к изображению.</param>
        /// <returns>Индекс загруженного битмапа.</returns>
        public int ImageLoad(string path)
        {
            BitmapDecoder decoder = new BitmapDecoder(_imgFactory, path, DecodeOptions.CacheOnLoad);
            BitmapFrameDecode frame = decoder.GetFrame(0);
            FormatConverter converter = new FormatConverter(_imgFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            SharpDX.Direct2D1.Bitmap bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(_renderTarget, converter);
            Utilities.Dispose(ref converter);
            Utilities.Dispose(ref frame); // 
            Utilities.Dispose(ref decoder);
            if (_bitmap == null)
                _bitmap = new List<SharpDX.Direct2D1.Bitmap>(); //
            _bitmap.Add(bitmap);// 
            return _bitmap.Count - 1;

        }
        /// <summary>
        /// Освобождает ресурсы, используемые объектом DirectX2D.
        /// </summary>
        public void Dispose()
        {
            for (int i = _bitmap.Count - 1; i >= 0; i--)
            {
                SharpDX.Direct2D1.Bitmap bitmap = _bitmap[i];
                _bitmap.RemoveAt(i);
                Utilities.Dispose(ref bitmap);
            }

            Utilities.Dispose(ref _brush);
            Utilities.Dispose(ref _textFormat);
            Utilities.Dispose(ref _imgFactory);
            Utilities.Dispose(ref _renderTarget);
            Utilities.Dispose(ref _factory);
        }
    }
}
