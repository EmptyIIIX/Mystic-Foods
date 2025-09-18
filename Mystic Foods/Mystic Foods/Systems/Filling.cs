using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Systems
{
    public class Filling : Sprite, IDraggable
    {
        public enum FillingType 
        { 
            Cheese = 10, 
            Meat = 20, 
            Vegetable = 30 
        }
        public FillingType FillingKind { get; private set; }
        public string Type => $"Filling_{FillingKind}";

        public Filling(Texture2D tex, Vector2 pos, FillingType kind) : base(tex, pos)
        {
            FillingKind = kind;
            (this as IDraggable).RegisterDraggable();
        }
    }
}