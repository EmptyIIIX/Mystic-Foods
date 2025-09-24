using System;
using System.Collections.Generic;

namespace Mystic_Foods.Managers
{
    public class CustomerManager
    {
        public List<Customer> Customers { get; private set; }
        private int _currentIndex = 0;
        public List<Customer> Customers2 { get; private set; }
        private int _currentIndex2 = 0;

        public CustomerManager()
        {
            Customers = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Pink",
                    IdOrder = 110,
                    SpritePathHappy = "Customers/Human/Dawn/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/Dawn/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/Dawn/NPC1/npc1HumanAngry",
                    Dia1 = "Cheese Rice",
                    Dia2 = "Rice Dough with Cheese Fillings",
                    DiaCurrect = "P Order correct",
                    DiaWrong = "P Order wrong",
                    DiaHappy = "Pink happy",
                    DiaNormal = "Pink Normal",
                    DiaAngry = "Pink Angry"
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    IdOrder = 410,
                    SpritePathHappy = "Customers/Human/Dawn/NPC2/npc2HumanHappy",
                    SpritePathNeutral = "Customers/Human/Dawn/NPC2/npc2HumanNormal",
                    SpritePathGrumpy = "Customers/Human/Dawn/NPC2/npc2HumanAngry",
                    Dia1 = "Meat & Corn is good",
                    Dia2 = "Corn Dough with Meat Fillings",
                    DiaCurrect = "M Order correct",
                    DiaWrong = "M Order wrong",
                    DiaHappy = "Man happy",
                    DiaNormal = "Man Normal",
                    DiaAngry = "Man Angry"
                },
                new Customer {
                    Id = 3,
                    Name = "Granny",
                    IdOrder = 910,
                    SpritePathHappy = "Customers/Human/Dawn/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/Dawn/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/Dawn/NPC3/npc3HumanAngry",
                    Dia1 = "Wheat Meat is good",
                    Dia2 = "Wheat Dough with Meat Fillings",
                    DiaCurrect = "G Order correct",
                    DiaWrong = "G Order wrong",
                    DiaHappy = "Granny happy",
                    DiaNormal = "Granny Normal",
                    DiaAngry = "Granny Angry"
                }
            };

            Customers2 = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Pink 2",
                    IdOrder = 100,
                    SpritePathHappy = "Customers/Human/Dawn/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/Dawn/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/Dawn/NPC1/npc1HumanAngry",
                    Dia1 = "Test the 2nd List",
                    Dia2 = "Rice Dough with Cheese Fillings",
                    DiaCurrect = "P2 Order correct",
                    DiaWrong = "P2 Order wrong",
                    DiaHappy = "Pink happy",
                    DiaNormal = "Pink Normal",
                    DiaAngry = "Pink Angry"
                },
                new Customer {
                    Id = 2,
                    Name = "Granny2",
                    IdOrder = 900,
                    SpritePathHappy = "Customers/Human/Dawn/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/Dawn/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/Dawn/NPC3/npc3HumanAngry",
                    Dia1 = "Test 2nd List",
                    Dia2 = "Wheat Dough with Meat Fillings",
                    DiaCurrect = "G2 Order correct",
                    DiaWrong = "G2 Order wrong",
                    DiaHappy = "Granny happy",
                    DiaNormal = "Granny Normal",
                    DiaAngry = "Granny Angry"
                }
            };
        }

        /*
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

        public Customer GetNextCustomer2()
        {
            if (_currentIndex2 < Customers2.Count)
            {
                return Customers2[_currentIndex2++];
            }
            else
            {
                _currentIndex2 = 0;
                return Customers2[_currentIndex2++];
            }
        }
        */

        private Customer GetNextFromList(List<Customer> list, ref int index)
        {
            if (list == null || list.Count == 0)
                return null;

            if (index < list.Count)
            {
                return list[index++];
            }
            else
            {
                index = 0;
                return list[index++];
            }
        }

        public Customer GetNextCustomer()
        {
            return GetNextFromList(Customers, ref _currentIndex);
        }

        public Customer GetNextCustomer2()
        {
            return GetNextFromList(Customers2, ref _currentIndex2);
        }

        public void ResetQueue()
        {
            _currentIndex = 0;
            _currentIndex2 = 0;
        }
    }
}