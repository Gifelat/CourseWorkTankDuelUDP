using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using Tank.DirectX;
using TankLibrary._2DObjectsGraph;
using TankLibrary.DirectX;
using TankLibrary.Objects;
using TankLibrary.Objects.BulletDecorator;
using TankLibrary.Objects.Enums;
using TankLibrary.Objects.TankDecorator;
using UDPProtocol;

namespace TankLibrary
{
    public class App
    {
        private ISocket _socket;

        // Количество единиц на высоту
        private static float _unitsPerHeight = 20.0f;
        public static float UnitsPerHeight { get => _unitsPerHeight; }

        private RectangleF _clientRect;

        // Коэффициент масштабирования
        private float _scale;
        public float Scale { get => _scale; }

        private string _win;
        public string Win { get => _win; }

        public bool _fire;

        private int _mainSize = 68;  // 68 - 1920x1080 // 50 - 16:10
        private float _movementSpeed = 3;

        private Obstacle _obstacleFactory;
        private Creator _creator;

        private RenderForm _renderForm;
        public RenderForm RenderForm { get => _renderForm; }

        private DirectX2D _dx2d;
        public DirectX2D DX2D { get => _dx2d; }

        private DInput _dXInput;

        private WindowRenderTarget _target;
        public WindowRenderTarget Target { get => _target; }

        private Helper _helper;

        private int[,] _obstacleIndx;
        private BackgroundGame _background;
        private List<ObstacleGame> _obstacle;
        private int _obstacleBreaking;
        private int _obstacleIndestructible;
        private int _obstacleSwamp;
        private int _obstacleIce;

        private TankGame _playerOne;
        private TankGame _playerTwo;
        private int[] _playerOneSprite;
        private int[] _playerBullet;
        private int[] _playerTwoSprite;
        private bool[] _playerBulletSprite;

        private List<BulletGame> _bulletOne;
        private List<BulletGame> _bulletTwo;

        private Drawer _drawer;


        /// <summary>
        /// Конструктор класса App.
        /// </summary>
        /// <param name="bullet1">Словарь пуль первого игрока.</param>
        /// <param name="bullet2">Словарь пуль второго игрока.</param>
        /// <param name="gas1">Топливо первого игрока.</param>
        /// <param name="gas2">Топливо второго игрока.</param>
        /// <param name="armor1">Броня первого игрока.</param>
        /// <param name="armor2">Броня второго игрока.</param>
        public App(ISocket socket, Player player)
        {
            _socket = socket;

            _win = "Игра завершена!";
            _bulletOne = new List<BulletGame>();
            _bulletTwo = new List<BulletGame>();
            _renderForm = new RenderForm("TankBattle");
            _renderForm.WindowState = FormWindowState.Maximized;

            _dx2d = new DirectX2D(_renderForm);
            _dXInput = new DInput(_renderForm);
            _dx2d.RenderTarget.Resize(new Size2(1920, 1080)); // 1920, 1080 2560, 1600
            _target = _dx2d.RenderTarget;


            int background = _dx2d.ImageLoad(@"..\\..\\..\\img\\son.jpg");
            _background = new BackgroundGame(new Background(new Vector2(0, 0)), new Sprite(background, 0, 0, 0));

            _obstacleBreaking = _dx2d.ImageLoad(@"..\\..\\..\\img\\2.png");
            _obstacleIndestructible = _dx2d.ImageLoad(@"..\\..\\..\\img\\1.png");
            _obstacleSwamp = _dx2d.ImageLoad(@"..\\..\\..\\img\\swamp.jpg");
            _obstacleIce = _dx2d.ImageLoad(@"..\\..\\..\\img\\gol.jpg");
            _obstacle = new List<ObstacleGame>();
            ObstacleGeneration();

            _playerBullet = new int[3];

            _playerBullet[0] = _dx2d.ImageLoad(@"..\\..\\..\\img\\normalWeapon.bmp");
            _playerBullet[1] = _dx2d.ImageLoad(@"..\\..\\..\\img\\weakWeapon.bmp");
            _playerBullet[2] = _dx2d.ImageLoad(@"..\\..\\..\\img\\strongWeapon.bmp");

            _movementSpeed = _mainSize / 20;

            _playerOneSprite = new int[2];
            _playerTwoSprite = new int[2];

            if (_socket is Server server)
            {
                _playerOneSprite[0] = _dx2d.ImageLoad(@"..\\..\\..\\img\\tanksgreendown.png");
                _playerTwoSprite[0] = _dx2d.ImageLoad(@"..\\..\\..\\img\\tankred.png");

                _playerOne = new TankGame(new TankPlayer(new Vector2(_mainSize * 3, _mainSize + (float)(_mainSize * 2)), _movementSpeed, player.gas, player.armor), new Sprite(_playerOneSprite[0], _mainSize - 1, _mainSize - 1, 0), player.bulletCount, 0.3f);
                _playerOne.RotationCenter();

                _playerTwo = new TankGame(new TankPlayer(new Vector2(_mainSize * 24, _mainSize + (float)(_mainSize * 11)), _movementSpeed, player.gas, player.armor), new Sprite(_playerTwoSprite[0], _mainSize - 1, _mainSize - 1, 0), player.bulletCount, 0.35f);
                _playerTwo.RotationCenter();
            }
            else if (_socket is Client client)
            {
                _playerOneSprite[0] = _dx2d.ImageLoad(@"..\\..\\..\\img\\tankred.png");
                _playerTwoSprite[0] = _dx2d.ImageLoad(@"..\\..\\..\\img\\tanksgreendown.png");

                _playerOne = new TankGame(new TankPlayer(new Vector2(_mainSize * 24, _mainSize + (float)(_mainSize * 11)), _movementSpeed, player.gas, player.armor), new Sprite(_playerOneSprite[0], _mainSize - 1, _mainSize - 1, 0), player.bulletCount, 0.35f);
                _playerOne.RotationCenter();

                _playerTwo = new TankGame(new TankPlayer(new Vector2(_mainSize * 3, _mainSize + (float)(_mainSize * 2)), _movementSpeed, player.gas, player.armor), new Sprite(_playerTwoSprite[0], _mainSize - 1, _mainSize - 1, 0), player.bulletCount, 0.3f);
                _playerTwo.RotationCenter();
            }

            _playerBulletSprite = new bool[2];
            _playerBulletSprite[0] = false;
            _playerBulletSprite[1] = false;

            _drawer = new Drawer(_dx2d);
            _helper = new Helper();

            SendInfo();
            ReceiveInfo();
        }

        /// <summary>
        /// Отрисовка игры.
        /// </summary>
        private void Render()
        {

            _helper.Update();
            _dXInput.UpdateMouseState();
            _dXInput.UpdateKeyboard();
            _target.BeginDraw();
            _target.Clear(Color.Black);

            _drawer.Draw(_background);

            ObstacleDraw();

            BulletCollise();

            if (_bulletOne != null)
            {
                for (int i = 0; i < _bulletOne.Count; i++)
                {
                    _bulletOne[i].Bullet.Fly();
                    _drawer.Draw(_bulletOne[i]);
                }
            }


            if (_bulletTwo != null)
            {
                for (int i = 0; i < _bulletTwo.Count; i++)
                {
                    _bulletTwo[i].Bullet.Fly();
                    _drawer.Draw(_bulletTwo[i]);
                }
            }

            _drawer.Draw(_playerOne);
            _drawer.Draw(_playerTwo);

            if (_dXInput.KeyboardUpdated)
            {
                if (_playerOne.Player.Health > 0 && _playerOne.Player.Gaz > 0)
                {
                    if (!_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.F))
                    {
                        _fire = false;
                    }

                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.W) && !GetCollision(_playerOne, 0, -_movementSpeed)) _playerOne.Movement(MovementKeys.Up);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.A) && !GetCollision(_playerOne, -_movementSpeed, 0)) _playerOne.Movement(MovementKeys.Left);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.S) && !GetCollision(_playerOne, 0, _movementSpeed)) _playerOne.Movement(MovementKeys.Down);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.D) && !GetCollision(_playerOne, _movementSpeed, 0)) _playerOne.Movement(MovementKeys.Right);
                    if (_dXInput.KeyboardState.IsPressed(SharpDX.DirectInput.Key.F) && _fire != true)
                    {
                        Bullet bull = _playerOne.Shoot();
                        switch (bull.Type)
                        {
                            case BulletType.Small:
                                _bulletOne.Add(new BulletGame(bull, new Sprite(_playerBullet[0], 10, 10, 0), 1f, 0.5f));
                                break;
                            case BulletType.Meduim:
                                _bulletOne.Add(new BulletGame(bull, new Sprite(_playerBullet[1], 10, 10, 0), 1f, 0.5f));
                                break;
                            case BulletType.Large:
                                _bulletOne.Add(new BulletGame(bull, new Sprite(_playerBullet[2], 10, 10, 0), 1f, 0.5f));
                                break;
                            default:
                                break;
                        }

                        if (_bulletOne.Count > 0)
                        {
                            _bulletOne[_bulletOne.Count - 1].Bullet.Move = _playerOne.Move;
                            _fire = true;
                            _socket.SendMessageAsync($"BulletInfo {_bulletOne[_bulletOne.Count - 1].Serialize()}");
                        }
                    }
                }
            }

            _drawer.DrawGas(_playerOne.Player, new Vector2(200, 100));
            _drawer.DrawXP(_playerOne.Player, new Vector2(10, 100));
            _drawer.DrawArmor(_playerOne.Player, new Vector2(500, 100));

            _target.EndDraw();

            SendInfo();
            ReceiveInfo();

            if (_playerOne.Player.Health <= 0 || _playerOne.Player.Gaz <= 0)
            {
                MessageBox.Show("Танк первого игрока уничтожен");
                RenderForm.Close();
            }
            if (_playerTwo.Player.Health <= 0 || _playerTwo.Player.Gaz <= 0)
            {
                MessageBox.Show("Танк второго игрока уничтожен");
                RenderForm.Close();
            }
        }

        /// <summary>
        /// Обработка столкновений с пулями.
        /// </summary>
        public void BulletCollise()
        {
            foreach (var obstacle in _obstacle)
            {
                for (int i = 0; i < _bulletOne.Count; i++)
                {

                    if (_bulletOne[i].Rect.Intersects(obstacle.Rect) && obstacle.Obstacle.Health != 0 && obstacle.Obstacle.Type != ObstacleType.Swamp && obstacle.Obstacle.Type != ObstacleType.Ice)
                    {
                        _bulletOne.RemoveAt(i);

                        if (obstacle.Obstacle.Health != 0 && obstacle.Obstacle.Destructibility == true)
                        {
                            obstacle.Obstacle.DamageObstacle();
                        }
                    }

                }


                for (int i = 0; i < _bulletTwo.Count; i++)
                {

                    if (_bulletTwo[i].Rect.Intersects(obstacle.Rect) && obstacle.Obstacle.Health != 0 && obstacle.Obstacle.Type != ObstacleType.Swamp && obstacle.Obstacle.Type != ObstacleType.Ice)
                    {
                        _bulletTwo.RemoveAt(i);
                        if (obstacle.Obstacle.Health != 0 && obstacle.Obstacle.Destructibility == true)
                        {
                            obstacle.Obstacle.DamageObstacle();
                        }
                    }

                }
            }

            for (int i = 0; i < _bulletOne.Count; i++)
            {
                if (_bulletOne[i].Rect.Intersects(_playerTwo.Rect))
                {
                    _playerTwo.Player.DamageTank(_bulletOne[i].Bullet.Damage);
                    _bulletOne.RemoveAt(i);
                }
            }

            for (int i = 0; i < _bulletTwo.Count; i++)
            {
                if (_bulletTwo[i].Rect.Intersects(_playerOne.Rect))
                {
                    _playerOne.Player.DamageTank(_bulletTwo[i].Bullet.Damage);
                    _bulletTwo.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Отрисовка препятствий.
        /// </summary
        private void ObstacleDraw()
        {
            foreach (var obstacle in _obstacle)
            {
                if (obstacle.Obstacle.Health != 0)
                {
                    _drawer.DrawObject(obstacle);
                }
            }
        }

        /// <summary>
        /// Проверка столкновения объекта с препятствием.
        /// </summary>
        /// <param name="renderObject">Объект, для которого проверяется столкновение.</param>
        /// <param name="x">Смещение по оси X.</param>
        /// <param name="y">Смещение по оси Y.</param>
        /// <returns>True, если произошло столкновение; иначе - false.</returns>
        public bool GetCollision(TankGame renderObject, float x, float y)
        {
            renderObject.Player.Bonus = false;
            foreach (var obstacle in _obstacle)
            {
                RectangleF playerPoly = new RectangleF(renderObject.Rect.Center.X + x, renderObject.Rect.Center.Y + y, renderObject.Rect.Width, renderObject.Rect.Height);

                if (playerPoly.Intersects(new RectangleF(obstacle.Rect.Center.X, obstacle.Rect.Center.Y, obstacle.Rect.Width - 1, obstacle.Rect.Height - 1)) && obstacle.Obstacle.Health > 0)
                    return true;
                else
                if (playerPoly.Intersects(new RectangleF(obstacle.Rect.Center.X, obstacle.Rect.Center.Y, 5, 5)) && (obstacle.Obstacle.Type == ObstacleType.Ice || obstacle.Obstacle.Type == ObstacleType.Swamp))
                {
                    switch (obstacle.Obstacle.Type)
                    {
                        case ObstacleType.Ice:
                            renderObject = new Ice(renderObject);
                            break;
                        case ObstacleType.Swamp:
                            renderObject = new Swamp(renderObject);
                            break;
                    }

                    renderObject.Player.Bonus = true;
                }
            }

            if (renderObject.Player.Bonus == false)
            {
                renderObject = new Normal(renderObject);
            }

            return false;
        }

        /// <summary>
        /// Генерация препятствий на карте.
        /// </summary>
        private void ObstacleGeneration()
        {
            Map rm = new Map();
            _obstacleIndx = rm.GetMap();

            for (int y = 0; y < _obstacleIndx.GetLength(0); y++)
                for (int x = 0; x < _obstacleIndx.GetLength(1); x++)
                {
                    switch (_obstacleIndx[y, x])
                    {
                        case 0:
                            break;
                        case 1:
                            _obstacleFactory = new ObstacleWeak(x * _mainSize, y * _mainSize, _mainSize, _obstacleBreaking);
                            _creator = _obstacleFactory.Create();
                            _obstacle.Add(_creator.CreateMap());
                            break;
                        case 2:
                            _obstacleFactory = new ObstacleEndless(x * _mainSize, y * _mainSize, _mainSize, _obstacleIndestructible);
                            _creator = _obstacleFactory.Create();
                            _obstacle.Add(_creator.CreateMap());
                            break;
                        case 3:
                            _obstacleFactory = new ObstacleSwamp(x * _mainSize, y * _mainSize, _mainSize, _obstacleSwamp);
                            _creator = _obstacleFactory.Create();
                            _obstacle.Add(_creator.CreateMap());
                            break;
                        case 4:
                            _obstacleFactory = new ObstacleIce(x * _mainSize, y * _mainSize, _mainSize, _obstacleIce);
                            _creator = _obstacleFactory.Create();
                            _obstacle.Add(_creator.CreateMap());
                            break;
                        default:
                            break;
                    }
                }
        }

        private void SendInfo()
        {
            _socket.SendMessageAsync($"TankInfo {_playerOne.Serialize()}");
        }

        private void ReceiveInfo()
        {
            if(!string.IsNullOrEmpty(_socket.TankInfo))
            {
                _playerTwo.Deserialize(_socket.TankInfo);
            }

            if (!string.IsNullOrEmpty(_socket.BulletInfo))
            {
                BulletGame bullet = new BulletGame(new NullBullet(new Bullet(Vector2.Zero)), new Sprite(_playerBullet[0], 10, 10, 0), 1f, 0.5f);
                bullet.Deserialize(_socket.BulletInfo);
                _bulletTwo.Add(bullet);

                _socket.ResetBulletInfo();
            }
        }

        private void RenderForm_Resize(object sender, EventArgs e)
        {
            int width = _renderForm.ClientSize.Width;
            int height = _renderForm.ClientSize.Height;
            _dx2d.RenderTarget.Resize(new Size2(width, height));
            _clientRect.Width = _dx2d.RenderTarget.Size.Width;
            _clientRect.Height = _dx2d.RenderTarget.Size.Height;
            _scale = _clientRect.Height / _unitsPerHeight;
        }

        public void Run()
        {
            RenderLoop.Run(_renderForm, Render);
        }

        public void Dispose()
        {
            _dXInput.Dispose();
            _dx2d.Dispose();
            _renderForm.Dispose();
        }
    }
}