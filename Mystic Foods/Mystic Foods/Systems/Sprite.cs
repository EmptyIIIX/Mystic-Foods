using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Systems
{
    public class Sprite : IDraggable, ITargetable
    {
        protected readonly Texture2D texture;
        protected readonly Vector2 origin;
        public Vector2 Position { get; set; }

        public Rectangle Rectangle => new((int)(Position.X - origin.X),
                                          (int)(Position.Y - origin.Y),
                                          texture.Width,
                                          texture.Height);

        public Sprite(Texture2D tex, Vector2 pos)
        {
            texture = tex;
            Position = pos;
            origin = new(tex.Width / 2, tex.Height / 2);
        }

        public Rectangle GetRectangle(Vector2 cameraPos)
        {
            return new Rectangle(
                (int)(Position.X - origin.X - cameraPos.X),
                (int)(Position.Y - origin.Y - cameraPos.Y),
                texture.Width,
                texture.Height
            );
        }

        public void Draw(Vector2 cameraPos)
        {
            Globals.SpriteBatch.Draw(texture, Position - cameraPos, null, Color.White, 0, origin, 1, SpriteEffects.None, 1);
        }
    }

}
