using System;

namespace Mystic_Foods
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdOrder {  get; set; }
        public string SpritePathHappy { get; set; }
        public string SpritePathNeutral { get; set; }
        public string SpritePathGrumpy { get; set; }
        public string Dia1 { get; set; }
        public string Dia2 { get; set; }
        public string DiaCurrect { get; set; }
        public string DiaWrong { get; set; }
        public string DiaHappy { get; set; }
        public string DiaNormal { get; set; }
        public string DiaAngry { get; set; }
    }
}