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
            Jasmine_Moon = 10, 
            Lotus_Blossom = 20, 
            Golden_Moon = 30 
        }
        public DoughType DoughKind { get; private set; }

        private Texture2D _normalTexture;
        private Texture2D _onPlateTexture;

        public Dough(Texture2D tex, Texture2D onPlateTexture, Vector2 pos, DoughType kind) : base(tex, pos)
        {
            _normalTexture = tex;
            _onPlateTexture = onPlateTexture;
            DoughKind = kind;
            (this as IDraggable).RegisterDraggable();
        }

        public void SetOnPlate(bool onPlate)
        {
            texture = onPlate ? _onPlateTexture : _normalTexture;
        }
    }
}