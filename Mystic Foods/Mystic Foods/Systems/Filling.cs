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
            Coconut_Amber = 10, 
            Pandan_Taro_Cream = 20, 
            Lotus_Root_Spirit = 30 
        }
        public FillingType FillingKind { get; private set; }

        public Filling(Texture2D tex, Vector2 pos, FillingType kind) : base(tex, pos)
        {
            FillingKind = kind;
            (this as IDraggable).RegisterDraggable();
        }
    }
}