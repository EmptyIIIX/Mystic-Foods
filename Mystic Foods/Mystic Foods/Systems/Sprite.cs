using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Systems
{
    public class Sprite : IDraggable, ITargetable
    {
        protected Texture2D texture;
        protected readonly Vector2 origin;
        public Vector2 Position { get; set; }
        public bool Visible { get; set; } = true;
        public Vector2 Size => new Vector2(texture.Width, texture.Height);

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
            Color drawColor = Visible ? Color.White : Color.Transparent;
            Globals.SpriteBatch.Draw(texture, Position - cameraPos, null, drawColor, 0f, origin, 1, SpriteEffects.None, 1);
        }
    }

}
