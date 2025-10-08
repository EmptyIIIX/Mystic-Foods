using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Foods.Customers
{
    public class CustomerSprite
    {
        public Texture2D Texture;
        public Vector2 Position;
        public float Opacity = 0f;

        private Vector2 _target;
        private float _speed = 200f;
        private float _fadeSpeed = 1.5f;

        public bool IsEntering = false;
        public bool IsExiting = false;
        public bool IsActive => !IsEntering && !IsExiting;

        // เรียกตอนลูกค้าเริ่มเข้าร้าน
        public void Enter(Vector2 startPos, Vector2 targetPos)
        {
            Position = startPos;
            _target = targetPos;
            Opacity = 0f;
            IsEntering = true;
            IsExiting = false;
        }

        // เรียกตอนลูกค้าเริ่มออก
        public void Exit(Vector2 targetPos)
        {
            _target = targetPos;
            IsExiting = true;
            IsEntering = false;
            Opacity = 1f;
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsEntering)
            {
                MoveAndFade(dt, fadeIn: true);
            }
            else if (IsExiting)
            {
                MoveAndFade(dt, fadeIn: false);
            }
        }

        private void MoveAndFade(float dt, bool fadeIn)
        {
            // เคลื่อนที่เข้าหา target
            Position = Vector2.Lerp(Position, _target, dt * _speed / Vector2.Distance(Position, _target));

            // fade in / fade out
            if (fadeIn)
            {
                Opacity += _fadeSpeed * dt;
                if (Opacity >= 1f)
                {
                    Opacity = 1f;
                    IsEntering = false;
                }
            }
            else
            {
                Opacity -= _fadeSpeed * dt;
                if (Opacity <= 0f)
                {
                    Opacity = 0f;
                    IsExiting = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null && Opacity > 0f)
            {
                spriteBatch.Draw(Texture, Position, Color.White * Opacity);
            }
        }
    }
}
