using System;
using System.Collections.Generic;

namespace Mystic_Foods.Managers
{
    public class CustomerManager
    {
        public bool IsEventFinished { get; private set; } = false;
        public List<Customer> Customers { get; private set; }
        private int _currentIndex = 0;
        public List<Customer> Customers2 { get; private set; }
        private int _currentIndex2 = 0;

        public List<Customer> Customers3 { get; private set; }
        private int _currentIndex3 = 0;

        public List<Customer> EventCustomer { get; private set; }
        private int _currentEventIndex = 0;

        private Random _rng = new Random();

        public CustomerManager()
        {
            Customers = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Pink",
                    IdOrder = 110,
                    SpritePathHappy = "Customers/Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC1/npc1HumanAngry",
                    TalkDia = "",
                    Dia1 = "I’d like the soft white one that smells like flowers \nwith coconut.",
                    Dia2 = "Yeah, the jasmine dough… with that sweet coconut \nfilling.",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something is \nmissing.",
                    DiaHappy = "Thank you so much, it smells wonderful.",
                    DiaNormal = "Thank you, it looks nice.",
                    DiaAngry = "I waited a little while… but it does look delicious."
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    IdOrder = 110,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    TalkDia = "",
                    Dia1 = "I’d like the soft white one that smells like \nflowers with coconut.",
                    Dia2 = "Jasmine Moon Dough with Coconut Amber \nfilling",
                    DiaCurrect = "Nice.",
                    DiaWrong = "Wrong. Completely wrong.",
                    DiaHappy = "Nice.",
                    DiaNormal = "Hmph. Fine.",
                    DiaAngry = "Seriously? I could’ve made it myself \nfaster."
                },
                new Customer {
                    Id = 3,
                    Name = "Granny",
                    IdOrder = 310,
                    SpritePathHappy = "Customers/Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC3/npc3HumanAngry",
                    TalkDia = "",
                    Dia1 = "The jasmine dough, paired with lotus root.",
                    Dia2 = "Yes, Jasmine Moon Dough with Lotus Root \nSpirit Filling.",
                    DiaCurrect = "Well, that’s… acceptable, I suppose.",
                    DiaWrong = "G Order wrong",
                    DiaHappy = "Tch… quick hands. Don’t think that \nmakes you better than me.",
                    DiaNormal = "Well, that’s… acceptable, I suppose.",
                    DiaAngry = "Even a turtle could cook faster \nthan you."
                }
            };

            Customers2 = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Pink",
                    IdOrder = 410,
                    SpritePathHappy = "Customers/Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC1/npc1HumanAngry",
                    TalkDia = "",
                    Dia1 = "I’ll take the pink one… the one with the \nfragrant green cream.",
                    Dia2 = "Yes, Lotus Blossom with Pandan Taro cream.",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something is \nmissing.",
                    DiaHappy = "Thank you so much, it smells wonderful.",
                    DiaNormal = "Thank you, it looks nice.",
                    DiaAngry = "I waited a little while… but it does \nlook delicious."
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    IdOrder = 910,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    TalkDia = "",
                    Dia1 = "The golden one, with lotus root.",
                    Dia2 = "Golden Moon Dough with Lotus Root Spirit \nFilling.",
                    DiaCurrect = "Nice.",
                    DiaWrong = "Wrong. Completely wrong.",
                    DiaHappy = "Nice.",
                    DiaNormal = "Hmph. Fine.",
                    DiaAngry = "Seriously? I could’ve made it myself \nfaster."
                },
                new Customer {
                    Id = 3,
                    Name = "Banana fine shyt",
                    IdOrder = 210,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "",
                    Dia1 = "the pink pastry… the one made from lotus \npetals with original taste.",
                    Dia2 = "Lotus Blossom with Coconut Amber filling.",
                    DiaCurrect = "Swift and sharp… just the way I \nlike it.",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "Swift and sharp… just the way I \nlike it.",
                    DiaNormal = "Adequate. Not too slow, not too fast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster."
                },
                new Customer {
                    Id = 4,
                    Name = "Kid",
                    IdOrder = 310,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "",
                    Dia1 = "mmm… the white one again with that \nlotus filling.",
                    Dia2 = "Yes, Jasmine Moon Dough with Lotus \nRoot Spirit Filling.",
                    DiaCurrect = "Well, well… you actually nailed \nit this time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well… you actually nailed \nit this time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…"
                }
            };

            Customers3 = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Banana fine shyt",
                    IdOrder = 610,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    Dia1 = "Let me have the golden pastry… with \nthat green cream.",
                    Dia2 = "Golden Moon Dough with Pandan Taro \nCream.",
                    DiaCurrect = "Swift and sharp… just the way I \nlike it.",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "Swift and sharp… just the way \nI like it.",
                    DiaNormal = "Adequate. Not too slow, not too \nfast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster."
                },
                new Customer {
                    Id = 2,
                    Name = "Tall dude",
                    IdOrder = 610,
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    Dia1 = "Could I try the pink pastry, with that \nmystical lotus root inside?",
                    Dia2 = "Lotus Blossom with Lotus Root Spirit \nfilling.",
                    DiaCurrect = "Ah, finally! Just what I wanted.\n Perfect!",
                    DiaWrong = "Wait, seriously? That’s not even \nclose!",
                    DiaHappy = "Ah, finally! Just what I wanted. \nPerfect!",
                    DiaNormal = "Hmm… alright, not bad.",
                    DiaAngry = "Hey, I’m not getting any younger here!"
                },
                new Customer {
                    Id = 3,
                    Name = "Kid",
                    IdOrder = 210,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    Dia1 = "the pink pastry… the one made from lotus \npetals with original taste.",
                    Dia2 = "Lotus Blossom with Coconut Amber filling.",
                    DiaCurrect = "Well, well… you actually nailed it \nthis time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well… you actually nailed it \nthis time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…"
                }
            };

            EventCustomer = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Banana fine shyt",
                    IdOrder = 110,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "You waste too much banana leaves, I'm here to stop your nature destruction",
                    Dia1 = "Wait, that smell!! is that??",
                    Dia2 = "Give me Jasmine Moon Dough with Coconut Amber filling.",
                    DiaCurrect = " ",
                    DiaWrong = "seriously? all banana leaves waste to this?",
                    DiaHappy = "Hmmm this is so good",
                    DiaNormal = "Adequate. Not too slow, not too fast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster."
                },
                new Customer {
                    Id = 2,
                    Name = "Banana fine shyt",
                    IdOrder = 510,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "Ok, one more dish and I'll decide your fate",
                    Dia1 = "Now I'll try Lotus Blossom with Pandan Taro Cream",
                    Dia2 = "You heard that.",
                    DiaCurrect = " ",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "So sweettt~~~, I'll let you enjoy this a bit longer",
                    DiaNormal = "Hmm… acceptable.",
                    DiaAngry = "Delicious but better be faster next time."
                },
                new Customer {
                    Id = 3,
                    Name = "Tall dude",
                    IdOrder = 610,
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    TalkDia = "",
                    Dia1 = "Nah",
                    Dia2 = "Lotus Blossom with Lotus Root Spirit \nfilling.",
                    DiaCurrect = "Ah, finally! Just what I wanted.\n Perfect!",
                    DiaWrong = "Wait, seriously? That’s not even \nclose!",
                    DiaHappy = "Ah, finally! Just what I wanted. \nPerfect!",
                    DiaNormal = "Hmm… alright, not bad.",
                    DiaAngry = "Hey, I’m not getting any younger here!"
                },
                new Customer {
                    Id = 4,
                    Name = "Kid",
                    IdOrder = 210,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "",
                    Dia1 = "ok yapper",
                    Dia2 = "Lotus Blossom with Coconut Amber filling.",
                    DiaCurrect = "Well, well… you actually nailed it \nthis time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well… you actually nailed it \nthis time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…"
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
            //return GetNextFromList(Customers, ref _currentIndex);
            return GetRandomFromList(Customers);
        }

        public Customer GetNextCustomer2()
        {
            //return GetNextFromList(Customers2, ref _currentIndex2);
            return GetRandomFromList(Customers2);
        }

        public Customer GetNextCustomer3()
        {
            //return GetNextFromList(Customers3, ref _currentIndex3);
            return GetRandomFromList(Customers3);
        }

        public Customer GetEventCustomer()
        {
            // ถ้าไม่มี EventCustomer หรือครบแล้ว ให้ถือว่าจบ
            if (EventCustomer == null || EventCustomer.Count == 0 || _currentEventIndex >= EventCustomer.Count)
            {
                IsEventFinished = true;
                return null;
            }

            var customer = EventCustomer[_currentEventIndex];
            _currentEventIndex++;

            // ถ้าเรียกถึงคนสุดท้ายแล้ว → Event หมด
            if (_currentEventIndex >= EventCustomer.Count)
            {
                IsEventFinished = true;
            }

            return customer;
        }
        public void ResetQueue()
        {
            _currentIndex = 0;
        }
    }
}