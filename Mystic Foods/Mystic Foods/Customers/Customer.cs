using System;

namespace Mystic_Foods
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpritePathHappy { get; set; }
        public string SpritePathNeutral { get; set; }
        public string SpritePathGrumpy { get; set; }
        public float Patience { get; set; }
        public string Dia1 { get; set; }
        public string Dia2 { get; set; }
        public string Fillings { get; set; }
        public string Dough { get; set; }
    }
}