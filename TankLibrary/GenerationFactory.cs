using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TankLibrary._2DObjectsGraph;
using TankLibrary.Objects.Enums;

namespace TankLibrary
{
    public abstract class Obstacle //абстрактный класс
    {
        protected float height; //позиционирование
        protected float width;

        protected int size; //размер стены (Т.к. квадрат, то height и width одинаковые)

        protected int index; //id спрайта, который мы загружаем

        public Obstacle(float height, float width, int size, int index)
        {
            this.height = height;
            this.width = width;
            this.size = size;
            this.index = index;
        }

        abstract public Creator Create();
    }

    public class ObstacleWeak : Obstacle //слабая стена (уничтожается за 1 раз)
    {
        public ObstacleWeak(float height, float width, int size, int index) : base(height, width, size, index)
        {

        }

        public override Creator Create()
        {
            return new ObstacleWeakCreate(height, width, size, index);
        }
    }

    public class ObstacleEndless : Obstacle //бесконечная стена (не уничтожается)
    {
        public ObstacleEndless(float height, float width, int size, int index) : base(height, width, size, index)
        {

        }

        public override Creator Create()
        {
            return new ObstacleEndlessCreate(height, width, size, index);
        }
    }

    public class ObstacleSwamp : Obstacle
    {
        public ObstacleSwamp(float height, float width, int size, int index) : base(height, width, size, index)
        {

        }

        public override Creator Create()
        {
            return new ObstacleSwampCreate(height, width, size, index);
        }
    }

    public class ObstacleIce : Obstacle
    {
        public ObstacleIce(float height, float width, int size, int index) : base(height, width, size, index)
        {

        }

        public override Creator Create()
        {
            return new ObstacleIceCreate(height, width, size, index);
        }
    }

    public abstract class Creator
    {
        protected ObstacleGame _wg;
        protected bool _destructibility; //разрушаемость
        protected int _health;
        protected int _speed;
        public abstract ObstacleGame CreateMap();
    }

    public class ObstacleWeakCreate : Creator
    {
        public ObstacleWeakCreate(float height, float width, int size, int index)
        {
            _destructibility = true;
            _health = 1;
            _speed = 100;
            _wg = new ObstacleGame(new Objects.Obstacle(new Vector2(height, width), _health, _destructibility, ObstacleType.Weak, _speed), new Sprite(index, size, size, 0));
        }

        public override ObstacleGame CreateMap()
        {
            return _wg;
        }
    }

    public class ObstacleEndlessCreate : Creator
    {
        public ObstacleEndlessCreate(float height, float width, int size, int index)
        {
            _destructibility = false;
            _health = 1;
            _speed = 100;
            _wg = new ObstacleGame(new Objects.Obstacle(new Vector2(height, width), _health, _destructibility, ObstacleType.Endless, _speed), new Sprite(index, size, size, 0));
        }

        public override ObstacleGame CreateMap()
        {
            return _wg;
        }
    }

    public class ObstacleSwampCreate : Creator
    {
        public ObstacleSwampCreate(float height, float width, int size, int index)
        {
            _destructibility = false;
            _health = -1;
            _speed = 50;
            _wg = new ObstacleGame(new Objects.Obstacle(new Vector2(height, width), _health, _destructibility, ObstacleType.Swamp, _speed), new Sprite(index, size, size, 0));
        }

        public override ObstacleGame CreateMap()
        {
            return _wg;
        }
    }

    public class ObstacleIceCreate : Creator
    {
        public ObstacleIceCreate(float height, float width, int size, int index)
        {
            _destructibility = false;
            _health = -1;
            _speed = 150;
            _wg = new ObstacleGame(new Objects.Obstacle(new Vector2(height, width), _health, _destructibility, ObstacleType.Ice, _speed), new Sprite(index, size, size, 0));
        }

        public override ObstacleGame CreateMap()
        {
            return _wg;
        }
    }
}
