using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankLibrary
{
    public class Sprite
    {
        private float _rotation;
        public float Rotation { get => _rotation; set => _rotation = value; }

        private int _spriteId;
        public int SpriteId { get => _spriteId; }

        private float _width;
        public float Width { get => _width; }

        private float _height;
        public float Height { get => _height; }

        private Vector2 _center;

        public Vector2 Center { get => _center; }
        public Sprite(int spriteId, int width, int height, float rotation)
        {
            _spriteId = spriteId;
            _rotation = rotation;
            _width = width;
            _height = height;

            _center.X = _width / 2.0f;
            _center.Y = _height / 2.0f;
        }

        public void ReplaceSprite(int spriteId)
        {
            try
            {
                _spriteId = spriteId;
            }
            catch { }
        }

        public void SetCenterRotation(float size)
        {
            _center += size;
        }
    }
}
