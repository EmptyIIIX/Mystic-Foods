
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Systems
{
    public class Socket : Sprite, ITargetable
    {
        public Socket(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            (this as ITargetable).RegisterTargetable();
        }
    }
}
