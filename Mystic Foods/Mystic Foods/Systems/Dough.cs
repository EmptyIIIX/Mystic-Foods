using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Systems
{
    public class Dough : Sprite, IDraggable
    {
        public enum DoughType 
        { 
            Wheat = 10, 
            Corn = 20, 
            Rice = 30 
        }
        public DoughType DoughKind { get; private set; }
        public string Type => $"Dough_{DoughKind}";

        public Dough(Texture2D tex, Vector2 pos, DoughType kind) : base(tex, pos)
        {
            DoughKind = kind;
            (this as IDraggable).RegisterDraggable();
        }
    }
}