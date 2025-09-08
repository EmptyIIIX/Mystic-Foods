using System;
using System.Collections.Generic;

namespace Mystic_Foods.Managers
{
    public class CustomerManager
    {
        public List<Customer> Customers { get; private set; }
        private Random _random = new Random();

        public CustomerManager()
        {
            Customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Anna", SpritePath = "Human/pink_morning", Patience = 1.5f, Preference = "chop see dang", IsVIP = false, Mood = "Happy" },
                new Customer { Id = 2, Name = "Jek", SpritePath = "Human/man_sunset", Patience = 1f, Preference = "mai gin pak", IsVIP = true, Mood = "Neutral" },
                new Customer { Id = 3, Name = "Wo", SpritePath = "Human/pink_midnight", Patience = 0.5f, Preference = "mai bok :P", IsVIP = false, Mood = "Silly :3" }
            };
        }

        public Customer GetRandomCustomer()
        {
            int index = _random.Next(Customers.Count);
            return Customers[index];
        }
    }
}