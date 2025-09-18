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
                    IdOrder = 100,
                    SpritePathHappy = "Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Human/NPC1/npc1HumanAngry",
                    Patience = 0.68f,
                    Dia1 = "Cheeesssseeee Rice",
                    Dia2 = "Rice Dough with Cheese Fillings",
                    DiaHappy = "Pink happy",
                    DiaNormal = "Pink Normal",
                    DiaAngry = "Pink Angry",
                    Dough = "Rice",
                    Fillings = "Cheese"
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    IdOrder = 400,
                    SpritePathHappy = "Human/NPC2/npc2HumanHappy",
                    SpritePathNeutral = "Human/NPC2/npc2HumanNormal",
                    SpritePathGrumpy = "Human/NPC2/npc2HumanAngry",
                    Patience = 0.68f,
                    Dia1 = "Meat & Corn is good",
                    Dia2 = "Corn Dough with Meat Fillings",
                    DiaHappy = "Man happy",
                    DiaNormal = "Man Normal",
                    DiaAngry = "Man Angry",
                    Dough = "Corn",
                    Fillings = "Meat"
                },
                new Customer {
                    Id = 3,
                    Name = "Granny",
                    IdOrder = 900,
                    SpritePathHappy = "Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Human/NPC3/npc3HumanAngry",
                    Patience = 0.68f,
                    Dia1 = "Wheat Meat is good",
                    Dia2 = "Wheat Dough with Meat Fillings",
                    DiaHappy = "Granny happy",
                    DiaNormal = "Granny Normal",
                    DiaAngry = "Granny Angry",
                    Dough = "Wheat",
                    Fillings = "Meat"
                }
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