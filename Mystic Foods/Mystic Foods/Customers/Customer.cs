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
    }
}