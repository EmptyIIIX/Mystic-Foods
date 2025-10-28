using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Systems
{
    public class Flowers : Sprite, IDraggable
    {
        public enum FlowersType
        {
            Mali = 10,
            Rose = 20,
            Lotus = 30
        }
        public FlowersType FlowerKind { get; private set; }

        public Flowers(Texture2D tex, Vector2 pos,  FlowersType kind) : base(tex, pos)
        {
            FlowerKind = kind;
            (this as IDraggable).RegisterDraggable();
        }
    }
}
