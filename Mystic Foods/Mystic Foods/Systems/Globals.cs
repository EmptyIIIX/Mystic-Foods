using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Systems
{
    public static class Globals
    {
        public static float TotalSecond {  get; set; }
        public static ContentManager Content { get; set; }
        public static SpriteBatch SpriteBatch { get; set; }

        public static void Update(GameTime gt)
        {
            TotalSecond = (float)gt.ElapsedGameTime.TotalSeconds;
        }
    }
}
