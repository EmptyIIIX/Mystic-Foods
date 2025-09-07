
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Systems
{
    public class Food : Sprite, IDraggable
    {
        public Food(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            (this as IDraggable).RegisterDraggable();
        }
    }
}
