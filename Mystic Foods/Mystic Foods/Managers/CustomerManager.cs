using System;
using System.Collections.Generic;

namespace Mystic_Foods.Managers
{
    public class CustomerManager
    {
        public List<Customer> Customers { get; private set; }
        private int _currentIndex = 0;

        public CustomerManager()
        {
            Customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Anna", SpritePath = "Human/pink_morning", Patience = 1f, Preference = "chop see dang", IsVIP = false, Mood = "Happy" },
                new Customer { Id = 2, Name = "Jek", SpritePath = "Human/man_sunset", Patience = 1f, Preference = "mai gin pak", IsVIP = true, Mood = "Neutral" },
                new Customer { Id = 3, Name = "Wo", SpritePath = "Human/pink_midnight", Patience = 1f, Preference = "mai bok :P", IsVIP = false, Mood = "Silly :3" }
            };
        }

        public Customer GetNextCustomer()
        {
            if (_currentIndex < Customers.Count)
            {
                return Customers[_currentIndex++];
            }
            else
            {
                // หากลูกค้าหมดคิว อาจจะวนใหม่หรือ return null
                // แบบวนลูป
                _currentIndex = 0;
                return Customers[_currentIndex++];
                // หรือถ้าไม่ต้องวนให้: return null;
            }
        }

        public void ResetQueue()
        {
            _currentIndex = 0;
        }
    }
}