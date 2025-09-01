using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Mystic_Foods
{
    public class Drag_Drop
    {
        public string Name { get; }
        public Texture2D Texture { get; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        
        public Drag_Drop(string name, Texture2D texture, Vector2 position)
        {
            Name = name;
            Texture = texture;
            Position = position;
        }

        //check mouse's position on sprite
        public bool Contains(Point mousePosition)
        {
            return Bounds.Contains(mousePosition);
        }
        //Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }

    public class Drag_DropManager
    {
        private List<Drag_Drop> _sprites;
        private Drag_Drop drag_Drop;
        private MouseState _previousMouseState;

        public Drag_DropManager()
        {
            _sprites = new List<Drag_Drop>();
            _previousMouseState = Mouse.GetState();
        }

        //add sprite
        public void AddSprite(Drag_Drop sprite)
        {
            _sprites.Add(sprite);
        }

        public void Update(GameTime gameTime)
        {
            MouseState ms = Mouse.GetState();
            
            if(ms.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released) //check Drag
            {
                for (int i = _sprites.Count - 1; i >= 0; i--)
                {
                    var sprite = _sprites[i];
                    if (sprite.Contains(ms.Position))
                    {
                        drag_Drop = sprite;
                        Mouse.SetCursor(MouseCursor.Hand);
                        break;
                    }
                }
            }

            if(drag_Drop != null && ms.LeftButton == ButtonState.Released) //check Drop
            {
                drag_Drop = null;
                Mouse.SetCursor(MouseCursor.Arrow);
            }

            //update position sprite (Drag state)
            if(drag_Drop != null)
            {
                drag_Drop.Position = new Vector2(ms.X - drag_Drop.Texture.Width / 2,
                                                 ms.Y - drag_Drop.Texture.Height / 2);
                Mouse.SetCursor(MouseCursor.Hand);
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);
            }

            _previousMouseState = ms;
        }

        //draw all sprite
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var sprite in _sprites)
            {
                sprite.Draw(spriteBatch);
            }
        }
    }
}
