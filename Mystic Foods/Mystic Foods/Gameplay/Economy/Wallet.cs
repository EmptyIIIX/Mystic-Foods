namespace Mystic_Foods.Gameplay.Economy
{
    public sealed class Wallet
    {
        public float TotalMoney { get; private set; }
        public float Revenue { get; private set; }
        public float Cost { get; private set; }

        public Wallet(float startingMoney = 100f) => TotalMoney = startingMoney;

        public void AddRevenue(float amount) { Revenue += amount; TotalMoney += amount; }
        public void AddCost(float amount) { Cost += amount; TotalMoney -= amount; }
        public float Profit => Revenue - Cost;
        public bool CanAfford(float amount) => TotalMoney >= amount;
    }
}