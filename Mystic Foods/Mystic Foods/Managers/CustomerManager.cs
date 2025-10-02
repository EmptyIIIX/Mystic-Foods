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

        public List<Customer> Customers3 { get; private set; }
        private int _currentIndex3 = 0;

        private Random _rng = new Random();

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
                    Dia1 = "I’d like the soft white one that smells like flowers \nwith coconut.",
                    Dia2 = "Yeah, the jasmine dough… with that sweet coconut \nfilling.",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something is \nmissing.",
                    DiaHappy = "Pink happy",
                    DiaNormal = "Pink Normal",
                    DiaAngry = "Pink Angry"
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    IdOrder = 410,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
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
                    SpritePathHappy = "Customers/Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC3/npc3HumanAngry",
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
                    Name = "Banana fine shyt",
                    IdOrder = 100,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
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
                    Name = "Tall dude",
                    IdOrder = 900,
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    Dia1 = "Test 2nd List",
                    Dia2 = "Wheat Dough with Meat Fillings",
                    DiaCurrect = "G2 Order correct",
                    DiaWrong = "G2 Order wrong",
                    DiaHappy = "Granny happy",
                    DiaNormal = "Granny Normal",
                    DiaAngry = "Granny Angry"
                },
                new Customer {
                    Id = 3,
                    Name = "Kid",
                    IdOrder = 900,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    Dia1 = "Test 2nd List",
                    Dia2 = "Wheat Dough with Meat Fillings",
                    DiaCurrect = "G2 Order correct",
                    DiaWrong = "G2 Order wrong",
                    DiaHappy = "Granny happy",
                    DiaNormal = "Granny Normal",
                    DiaAngry = "Granny Angry"
                },
                new Customer {
                    Id = 4,
                    Name = "Pink",
                    IdOrder = 110,
                    SpritePathHappy = "Customers/Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC1/npc1HumanAngry",
                    Dia1 = "Cheese Rice",
                    Dia2 = "Rice Dough with Cheese Fillings",
                    DiaCurrect = "P Order correct",
                    DiaWrong = "P Order wrong",
                    DiaHappy = "Pink happy",
                    DiaNormal = "Pink Normal",
                    DiaAngry = "Pink Angry"
                },
                new Customer {
                    Id = 5,
                    Name = "Man",
                    IdOrder = 410,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    Dia1 = "Meat & Corn is good",
                    Dia2 = "Corn Dough with Meat Fillings",
                    DiaCurrect = "M Order correct",
                    DiaWrong = "M Order wrong",
                    DiaHappy = "Man happy",
                    DiaNormal = "Man Normal",
                    DiaAngry = "Man Angry"
                },
                new Customer {
                    Id = 6,
                    Name = "Granny",
                    IdOrder = 910,
                    SpritePathHappy = "Customers/Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC3/npc3HumanAngry",
                    Dia1 = "Wheat Meat is good",
                    Dia2 = "Wheat Dough with Meat Fillings",
                    DiaCurrect = "G Order correct",
                    DiaWrong = "G Order wrong",
                    DiaHappy = "Granny happy",
                    DiaNormal = "Granny Normal",
                    DiaAngry = "Granny Angry"
                }
            };

            Customers3 = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Banana fine shyt",
                    IdOrder = 100,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
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
                    Name = "Tall dude",
                    IdOrder = 900,
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    Dia1 = "Test 2nd List",
                    Dia2 = "Wheat Dough with Meat Fillings",
                    DiaCurrect = "G2 Order correct",
                    DiaWrong = "G2 Order wrong",
                    DiaHappy = "Granny happy",
                    DiaNormal = "Granny Normal",
                    DiaAngry = "Granny Angry"
                },
                new Customer {
                    Id = 2,
                    Name = "Kid",
                    IdOrder = 900,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
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

        private Customer GetRandomFromList(List<Customer> list)
        {
            if (list == null || list.Count == 0)
                return null;

            int index = _rng.Next(list.Count); // สุ่มเลขตั้งแต่ 0 ถึง list.Count-1
            return list[index];
        }

        public Customer GetNextCustomer()
        {
            return GetNextFromList(Customers, ref _currentIndex);
            //return GetRandomFromList(Customers);
        }

        public Customer GetNextCustomer2()
        {
            return GetNextFromList(Customers2, ref _currentIndex2);
            //return GetRandomFromList(Customers2);
        }

        public Customer GetNextCustomer3()
        {
            return GetNextFromList(Customers3, ref _currentIndex3);
            //return GetRandomFromList(Customers2);
        }

        public void ResetQueue()
        {
            _currentIndex = 0;
        }
    }
}