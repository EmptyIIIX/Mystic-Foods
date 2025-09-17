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
                new Customer {
                    Id = 1,
                    Name = "Pink",
                    Order = 100,
                    SpritePathHappy = "Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Human/NPC1/npc1HumanAngry",
                    Patience = 0.694f,
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    Order = 200,
                    SpritePathHappy = "Human/NPC2/npc2HumanHappy",
                    SpritePathNeutral = "Human/NPC2/npc2HumanNormal",
                    SpritePathGrumpy = "Human/NPC2/npc2HumanAngry",
                    Patience = 1f,
                },
                new Customer {
                    Id = 3,
                    Name = "Granny",
                    Order = 300,
                    SpritePathHappy = "Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Human/NPC3/npc3HumanAngry",
                    Patience = 1f,
                },
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
                _currentIndex = 0;
                return Customers[_currentIndex++];
            }
        }

        public void ResetQueue()
        {
            _currentIndex = 0;
        }
    }
}