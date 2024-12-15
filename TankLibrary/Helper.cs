using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary
{
    /// <summary>
    /// Класс, предоставляющий вспомогательные функции и информацию о времени и FPS (кадры в секунду).
    /// </summary>
    public class Helper
    {
        private Stopwatch _stopwatch;
        private int _frameCounter;
        private int _fps;
        private bool _game;

        /// <summary>
        /// Получает значение, указывающее, выполняется ли игра.
        /// </summary>
        public bool Game { get => _game; }

        /// <summary>
        /// Получает текущее значение FPS (кадры в секунду).
        /// </summary>
        public int FPS { get => _fps; }

        private long _previousFPSMeasurementTime;

        private float _time;

        /// <summary>
        /// Получает текущее значение времени в секундах.
        /// </summary>
        public float Time { get => _time; }

        /// <summary>
        /// Инициализирует новый экземпляр класса Helper.
        /// </summary>
        public Helper()
        {
            _stopwatch = new Stopwatch();
            Reset();
        }

        /// <summary>
        /// Сбрасывает все значения и счетчики в начальные состояния.
        /// </summary>
        public void Reset()
        {
            _stopwatch.Reset();
            _frameCounter = 0;
            _fps = 0;
            _game = true;
            _previousFPSMeasurementTime = _stopwatch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Обновляет значения времени и FPS (кадры в секунду).
        /// </summary>
        public void Update()
        {
            long ticks = _stopwatch.Elapsed.Ticks;
            _time = (float)ticks / TimeSpan.TicksPerSecond;

            _frameCounter++;
            if (_stopwatch.ElapsedMilliseconds - _previousFPSMeasurementTime >= 1000)
            {
                _fps = _frameCounter;
                _frameCounter = 0;
                _previousFPSMeasurementTime = _stopwatch.ElapsedMilliseconds;
            }
        }

        /// <summary>
        /// Останавливает секундомер.
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
        }
    }
}
