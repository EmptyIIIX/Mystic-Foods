using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Systems
{
    public class TrashBin : Sprite, ITargetable
    {
        public TrashBin(Texture2D tex, Vector2 pos) : base(tex, pos)
        {
            (this as ITargetable).RegisterTargetable();
        }
    }
}