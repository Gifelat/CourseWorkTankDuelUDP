using SharpDX.DirectInput;
using SharpDX.Windows;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary.DirectX
{
    /// <summary>
    /// Класс для работы с устройствами ввода, такими как клавиатура и мышь, с использованием DirectInput.
    /// </summary>
    public class DInput : IDisposable
    {
        private DirectInput _directInput;

        private Keyboard _keyboard;
        private KeyboardState _keyboardState;
        public KeyboardState KeyboardState { get => _keyboardState; }
        private bool _keyboardUpdated;
        public bool KeyboardUpdated { get => _keyboardUpdated; }
        private bool _keyboardConnect;

        private Mouse _mouse;
        private MouseState _mouseState;
        public MouseState MouseState { get => _mouseState; }
        private bool _mouseUpdated;
        public bool MouseUpdated { get => _mouseUpdated; }
        private bool _mouseAcquired;

        /// <summary>
        /// Создает новый экземпляр класса DInput.
        /// </summary>
        /// <param name="renderForm">Форма, с которой будет связаны DInput и получать ввод от устройств.</param>
        public DInput(RenderForm renderForm)
        {
            _directInput = new DirectInput();

            _keyboard = new Keyboard(_directInput);
            _keyboard.Properties.BufferSize = 16;
            _keyboard.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            ConnectionKeyboard();
            _keyboardState = new KeyboardState();

            _mouse = new Mouse(_directInput);
            _mouse.Properties.AxisMode = DeviceAxisMode.Relative;
            _mouse.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireMouse();
            _mouseState = new MouseState();
        }

        private void ConnectionKeyboard()
        {
            try
            {
                _keyboard.Acquire();
                _keyboardConnect = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _keyboardConnect = false;
            }
        }

        private void AcquireMouse()
        {
            try
            {
                _mouse.Acquire();
                _mouseAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _mouseAcquired = false;
            }
        }

        /// <summary>
        /// Обновляет состояние мыши.
        /// </summary>
        public void UpdateMouseState()
        {
            // Если доступ не был получен, пробуем здесь
            if (!_mouseAcquired) AcquireMouse();

            // Пробуем обновить состояние
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _mouse.GetCurrentState(ref _mouseState);
                // Успех
                _mouseUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                // Отказ
                _mouseUpdated = false;
            }

            // В большинстве случаев отказ из-за потери фокуса ввода
            // Устанавливаем соответствующий флаг, чтобы в следующем кадре попытаться получить доступ
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _mouseAcquired = false;
        }

        /// <summary>
        /// Обновляет состояние клавиатуры.
        /// </summary>
        public void UpdateKeyboard()
        {
            if (!_keyboardConnect) ConnectionKeyboard();

            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _keyboard.GetCurrentState(ref _keyboardState);
                _keyboardUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                _keyboardUpdated = false;
            }

            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _keyboardConnect = false;
        }

        /// <summary>
        /// Освобождает ресурсы, используемые объектом DInput.
        /// </summary>
        public void Dispose()
        {
            Utilities.Dispose(ref _mouse);
            Utilities.Dispose(ref _keyboard);
            Utilities.Dispose(ref _directInput);
        }
    }
}
