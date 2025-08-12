using System;

namespace Mystic_Foods
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SpritePath { get; set; }
        public float Patience { get; set; }
        public string Preference { get; set; }
        public bool IsVIP { get; set; }
        public string Mood { get; set; }
    }
}