using Mystic_Foods.Systems;
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
                    TalkDia = "I long for a sweet as pale as the moon.\nSoft, warm, and carrying the scent of coconut\nkissed by fire.",
                    Dia1 = "I want a dough that is sweet, soft, warm,\nand has a coconut flavor.",
                    Dia2 = "I want the Jasmine Moon Coconut Filling\nwithout the decorations.",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something is \nmissing.",
                    DiaHappy = "Thank you so much, it smells wonderful.",
                    DiaNormal = "Thank you, it looks nice.",
                    DiaAngry = "I waited a little while… but it does look delicious.",
                    VoicePathTalk = "Female Laugh1",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Female Reject"
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    IdOrder = 320,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    TalkDia = "I want golden dough that is said to bring\ngood fortune in original coconut taste come with\nthe white petals.",
                    Dia1 = "the golden one with coconut taste and jasmine\npetals",
                    Dia2 = "I want the Golden Moon Coconut Filling with\nthe Jasmine Petal Charm decorations.",
                    DiaCurrect = "Nice.",
                    DiaWrong = "Wrong. Completely wrong.",
                    DiaHappy = "Nice.",
                    DiaNormal = "Hmph. Fine.",
                    DiaAngry = "Seriously? I could’ve made it myself \nfaster.",
                    VoicePathTalk = "Male Hi",
                    VoicePathMood = "Male Sigh",
                    VoicePathWrong = "Male Angry1"
                },
                new Customer {
                    Id = 3,
                    Name = "Granny",
                    IdOrder = 220,
                    SpritePathHappy = "Customers/Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC3/npc3HumanAngry",
                    TalkDia = "Um... could you make it soft and white\nlike moonlight? Maybe with a bit of green inside\nit And.. a few white flower petals.",
                    Dia1 = "The jasmine dough, paired with taro.\nMaybe you could add a few jasmine petals?\nOnly if it’s not too much trouble.",
                    Dia2 = "White jasmine dough, pandan taro cream\ninside... and a little jasmine petals on top.",
                    DiaCurrect = "Well, that is... acceptable, I suppose.",
                    DiaWrong = "G Order wrong",
                    DiaHappy = "Tch… quick hands. Don’t think that \nmakes you better than me.",
                    DiaNormal = "Well, that’s… acceptable, I suppose.",
                    DiaAngry = "Even a turtle could cook faster \nthan you.",
                    VoicePathTalk = "Female Augh",
                    VoicePathMood = "Female Augh2",
                    VoicePathWrong = "Female Disagree2"
                },
                new Customer {
                    Id = 4,
                    Name = "Pink",
                    IdOrder = 320,
                    SpritePathHappy = "Customers/Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC1/npc1HumanAngry",
                    TalkDia = "I want golden dough that is said to bring\ngood fortune in original coconut taste come with\nthe white petals.",
                    Dia1 = "the golden one with coconut taste and jasmine\npetals",
                    Dia2 = "I want the Golden Moon Coconut Filling with\nthe Jasmine Petal Charm decorations.",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something is \nmissing.",
                    DiaHappy = "Thank you so much, it smells wonderful.",
                    DiaNormal = "Thank you, it looks nice.",
                    DiaAngry = "I waited a little while… but it does look\ndelicious.",
                    VoicePathTalk = "Female Laugh1",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Female Reject"
                },
                new Customer {
                    Id = 5,
                    Name = "Man",
                    IdOrder = 130,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    TalkDia = "I long for a sweet as pale as the moon.\nSoft, warm, and carrying the scent of coconut\nkissed by fire with rose.",
                    Dia1 = "I want a white dough that is sweet, soft, warm,\nand has a coconut flavor decorate with rose patal.",
                    Dia2 = "I want the Jasmine Moon Coconut Filling\nwith the rose petal relic decorations.",
                    DiaCurrect = "Nice.",
                    DiaWrong = "Wrong. Completely wrong.",
                    DiaHappy = "Nice.",
                    DiaNormal = "Hmph. Fine.",
                    DiaAngry = "Seriously? I could’ve made it myself \nfaster.",
                    VoicePathTalk = "Male Hi",
                    VoicePathMood = "Male Sigh",
                    VoicePathWrong = "Male Angry1"
                },
                new Customer {
                    Id = 6,
                    Name = "Granny",
                    IdOrder = 610,
                    SpritePathHappy = "Customers/Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC3/npc3HumanAngry",
                    TalkDia = "I don't want anything too fancy. No flowers\nno shiny bits Just pink dough, with a\nlotus filling.",
                    Dia1 = "Don't you understand? The filling should be\nsmooth and rich lotus And make sure the dough\nis pink. Keep it simple.",
                    Dia2 = "Pink lotus blossom dough on the outside, Lotus \nroot spirit filling inside. No decoration",
                    DiaCurrect = "Well, that is.. acceptable, I suppose.",
                    DiaWrong = "G Order wrong",
                    DiaHappy = "Tch… quick hands. Don’t think that \nmakes you better than me.",
                    DiaNormal = "Well, that’s… acceptable, I suppose.",
                    DiaAngry = "Even a turtle could cook faster \nthan you.",
                    VoicePathTalk = "Female Augh",
                    VoicePathMood = "Female Augh2",
                    VoicePathWrong = "Female Disagree2"
                },
                new Customer {
                    Id = 7,
                    Name = "Pink",
                    IdOrder = 340,
                    SpritePathHappy = "Customers/Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC1/npc1HumanAngry",
                    TalkDia = "I want Something white and fragrant, like\nmoonlight with the taste of lotus root decorate\nwith green leaves",
                    Dia1 = "The outer layer should be clear and tender as\nmoon surrounded by emerald with lotus root\ninside",
                    Dia2 = "I want the Jasmine Moon dough Lotus root\nFilling with the Emerald lotus seal decorations.",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something\nis missing.",
                    DiaHappy = "Thank you so much, it smells wonderful.",
                    DiaNormal = "Thank you, it looks nice.",
                    DiaAngry = "I waited a little while… but it does\nlook delicious.",
                    VoicePathTalk = "Female Laugh1",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Female Reject"
                },
                new Customer {
                    Id = 8,
                    Name = "Man",
                    IdOrder = 210,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    TalkDia = "I long for a sweet pink as lotus blossom.\nSoft, warm, and carrying the scent of coconut\nkissed by fire.",
                    Dia1 = "I want a pink dough that is sweet, soft, warm\nand has a coconut flavor.",
                    Dia2 = "I want the Lotus Blossom dough Coconut Filling\nwithout the decorations.",
                    DiaCurrect = "Nice.",
                    DiaWrong = "Wrong. Completely wrong.",
                    DiaHappy = "Nice.",
                    DiaNormal = "Hmph. Fine.",
                    DiaAngry = "Seriously? I could’ve made it myself \nfaster.",
                    VoicePathTalk = "Male Hi",
                    VoicePathMood = "Male Sigh",
                    VoicePathWrong = "Male Angry1"
                },
                new Customer {
                    Id = 9,
                    Name = "Granny",
                    IdOrder = 410,
                    SpritePathHappy = "Customers/Human/NPC3/npc3HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC3/npc3HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC3/npc3HumanAngry",
                    TalkDia = "I long for a sweet pink as lotus blossom.\nSoft, warm, and carrying the scent of fresh green\nfilling",
                    Dia1 = "I want a pink dough that is sweet, soft, warm,\nand has a taro flavor.",
                    Dia2 = "Pink lotus blossom dough on the outside,\npandan taro cream inside. No decoration",
                    DiaCurrect = "Well, that's... acceptable, I suppose.",
                    DiaWrong = "G Order wrong",
                    DiaHappy = "Tch… quick hands. Don’t think that \nmakes you better than me.",
                    DiaNormal = "Well, that’s… acceptable, I suppose.",
                    DiaAngry = "Even a turtle could cook faster \nthan you.",
                    VoicePathTalk = "Female Augh",
                    VoicePathMood = "Female Augh2",
                    VoicePathWrong = "Female Disagree2"
                }
            };

            Customers2 = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Pink",
                    IdOrder = 430,
                    SpritePathHappy = "Customers/Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC1/npc1HumanAngry",
                    TalkDia = "I long for a sweet pink as lotus blossom.\nSoft, warm, and carrying the scent of fresh green\nfilling surrounded by red flower",
                    Dia1 = "I want a pink dough that is sweet, soft, warm,\nand has a taro flavor with rose petal.",
                    Dia2 = "Pink lotus blossom dough on the outside,\npandan taro cream inside. Rose petal decoration",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something is \nmissing.",
                    DiaHappy = "Thank you so much, it smells wonderful.",
                    DiaNormal = "Thank you, it looks nice.",
                    DiaAngry = "I waited a little while… but it does \nlook delicious.",
                    VoicePathTalk = "Female Laugh1",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Female Reject"
                },
                new Customer {
                    Id = 2,
                    Name = "Man",
                    IdOrder = 940,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    TalkDia = "The golden one the one tied to \nthe moon's blessing with lotus root and\ndecorate with lotus leaves.",
                    Dia1 = "I want the golden dough that said to\nbring good fortune fill with lotus root\nand decorate with lotus leaves",
                    Dia2 = "Golden moon dough on the outside,\nlotus root inside.and Emerald lotus seal\ndecoration.",
                    DiaCurrect = "Nice.",
                    DiaWrong = "Wrong. Completely wrong.",
                    DiaHappy = "Nice.",
                    DiaNormal = "Hmph. Fine.",
                    DiaAngry = "Seriously? I could’ve made it myself \nfaster.",
                    VoicePathTalk = "Male Hi",
                    VoicePathMood = "Male Sigh",
                    VoicePathWrong = "Male Angry1"
                },
                new Customer {
                    Id = 3,
                    Name = "Banana fine shyt",
                    IdOrder = 210,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "The pink pastry... the one made from \nlotus petals with coconut taste.",
                    Dia1 = "The pink blossom dough, with coconut filling.\nNo decoration",
                    Dia2 = "Lotus Blossom dough with Coconut Amber filling.\nNo more or less.",
                    DiaCurrect = "Swift and sharp… just the way I \nlike it.",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "Swift and sharp...\njust the way I like it.",
                    DiaNormal = "Adequate. Not too slow, not too fast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster.",
                    VoicePathTalk = "Female Hmm2",
                    VoicePathMood = "Female Ahem",
                    VoicePathWrong = "Female Disagree1"
                },
                new Customer {
                    Id = 4,
                    Name = "Kid",
                    IdOrder = 330,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "I want Something white and fragrant, like\nmoonlight with the taste of lotus root decorate\nwith red",
                    Dia1 = "The outer layer should be clear and tender as\nmoon surrounded by rose with lotus root\ninside",
                    Dia2 = "I want the Jasmine Moon dough Lotus root\nFilling with the Rose petal relic decorations.",
                    DiaCurrect = "Well, well... you actually nailed \nit this time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well… you actually nailed \nit this time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…",
                    VoicePathTalk = "Cartoon Talk",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Kid Angry"
                },
                new Customer {
                    Id = 5,
                    Name = "Pink",
                    IdOrder = 240,
                    SpritePathHappy = "Customers/Human/NPC1/npc1HumanHappy",
                    SpritePathNeutral = "Customers/Human/NPC1/npc1HumanNormal",
                    SpritePathGrumpy = "Customers/Human/NPC1/npc1HumanAngry",
                    TalkDia = "I long for a sweet as pale as the moon.\nSoft, warm, and carrying the scent of pandan\n with green leaves around it.",
                    Dia1 = "I want a white dough that is sweet, soft, warm,\nand has a pandan taro flavor decorate with\nlotus leaves.",
                    Dia2 = "I want the Jasmine Moon fill with pandan\ntaro cream decorate with emerald lotus seal",
                    DiaCurrect = "Thank you so much, it smells wonderful.",
                    DiaWrong = "Hmm… thank you, but I think something is \nmissing.",
                    DiaHappy = "Thank you so much, it smells wonderful.",
                    DiaNormal = "Thank you, it looks nice.",
                    DiaAngry = "I waited a little while… but it does \nlook delicious.",
                    VoicePathTalk = "Female Laugh1",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Female Reject"
                },
                new Customer {
                    Id = 6,
                    Name = "Man",
                    IdOrder = 430,
                    SpritePathHappy = "Customers/Human/NPC2/npc2HumanHappyVer2",
                    SpritePathNeutral = "Customers/Human/NPC2/npc2HumanNormalVer2",
                    SpritePathGrumpy = "Customers/Human/NPC2/npc2HumanAngryVer2",
                    TalkDia = "I long for a sweet pink as lotus blossom.\nSoft, warm, carying taste of taro and a few\nrose on top",
                    Dia1 = "I want a pink dough that is sweet, soft, warm,\nand has a panadan taro flavor and decorate\nwith rose petals.",
                    Dia2 = "I want the Lotus Blossom dough panadan taro\ncream with Rose Petal Relic around it.",
                    DiaCurrect = "Nice.",
                    DiaWrong = "Wrong. Completely wrong.",
                    DiaHappy = "Nice.",
                    DiaNormal = "Hmph. Fine.",
                    DiaAngry = "Seriously? I could’ve made it myself \nfaster.",
                    VoicePathTalk = "Male Hi",
                    VoicePathMood = "Male Sigh",
                    VoicePathWrong = "Male Angry1"
                },
                new Customer {
                    Id = 7,
                    Name = "Banana fine shyt",
                    IdOrder = 240,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "Um... could you make it soft and white\nlike moonlight? Maybe with a bit of green inside\nit And.. a few lotus petals.",
                    Dia1 = "The jasmine dough, paired with taro.\nMaybe you could add a few lotus petals?.",
                    Dia2 = "White jasmine dough, pandan taro cream\ninside... and a little Emerald lotus seal on top.",
                    DiaCurrect = "Swift and sharp… just the way I \nlike it.",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "Swift and sharp...\njust the way I like it.",
                    DiaNormal = "Adequate. Not too slow, not too fast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster.",
                    VoicePathTalk = "Female Hmm2",
                    VoicePathMood = "Female Ahem",
                    VoicePathWrong = "Female Disagree1"
                },
                new Customer {
                    Id = 8,
                    Name = "Kid",
                    IdOrder = 120,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "I long for a sweet as pale as the moon.\nwith scent of coconut and upon it, a few\nwhite petals, for beauty's sake.",
                    Dia1 = "I want a white dough that is sweet, soft, warm,\nand sweetness of roasted coconut with white\npetals.",
                    Dia2 = "I want the Jasmine Moon Coconut Filling\nwith the Jasmine Petal Charm decorations.",
                    DiaCurrect = "Well, well… you actually nailed \nit this time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well... you actually nailed \nit this time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…",
                    VoicePathTalk = "Cartoon Talk",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Kid Angry"
                },
            };

            Customers3 = new List<Customer>
            {
                new Customer {
                    Id = 1,
                    Name = "Banana fine shyt",
                    IdOrder = 630,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "I want golden dough that is said to bring\ngood fortune Maybe with a bit of green inside\nit And.. a few rose petals.",
                    Dia1 = "the golden one with pandan taro taste and\nrose petals",
                    Dia2 = "I want the Golden Moon dough pandan taro\ncream with the Rose Petal Relic decorations.",
                    DiaCurrect = "Swift and sharp...\njust the way I like it.",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "Swift and sharp… just the way \nI like it.",
                    DiaNormal = "Adequate. Not too slow, not too \nfast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster.",
                    VoicePathTalk = "Female Hmm2",
                    VoicePathMood = "Female Ahem",
                    VoicePathWrong = "Female Disagree1"
                },
                new Customer {
                    Id = 2,
                    Name = "Tall dude",
                    IdOrder = 620,
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    TalkDia = "I long for a sweet pink as lotus blossom.\nSoft, warm, carying taste of spirit root and a few\nwhite petals on top",
                    Dia1 = "I want a pink dough that is sweet, soft, warm,\nand has a lotus flavor and decorate\nwith jasmine petals.",
                    Dia2 = "I want the Lotus Blossom dough lotus spitit root\nfilling with Jasmine Petal charm around it.",
                    DiaCurrect = "Ah, finally! Just what I wanted.\n Perfect!",
                    DiaWrong = "Wait, seriously? That’s not even \nclose!",
                    DiaHappy = "Ah, finally! Just what I wanted. \nPerfect!",
                    DiaNormal = "Hmm… alright, not bad.",
                    DiaAngry = "Hey, I’m not getting any younger here!",
                    VoicePathTalk = "Giant Pleasure",
                    VoicePathMood = "Giant Mood",
                    VoicePathWrong = "Male Angry2"
                },
                new Customer {
                    Id = 3,
                    Name = "Kid",
                    IdOrder = 210,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "W-wait... that scent! Soft, white, like\njasmine under the moon... and something green...\npandan?",
                    Dia1 = "The sweetness taro cream, right? Smooth,\ngentle... But it needs something soft and\nfragrant like jasmine dough.",
                    Dia2 = "White jasmine moon outside, pandan taro\ncream inside and that's it.",
                    DiaCurrect = "Well, well… you actually nailed it \nthis time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well... you actually nailed it \nthis time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…",
                    VoicePathTalk = "Cartoon Talk",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Kid Angry"
                },
                new Customer {
                    Id = 4,
                    Name = "Banana fine shyt",
                    IdOrder = 340,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "I want Something white and fragrant, like\nmoonlight with the taste of lotus root decorate\nwith green leaves",
                    Dia1 = "The outer layer should be clear and tender as\nmoon surrounded by emerald with lotus root\ninside",
                    Dia2 = "I want the Jasmine Moon dough Lotus root\nFilling with the Emerald lotus seal decorations.",
                    DiaCurrect = "Swift and sharp...\njust the way I like it.",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "Swift and sharp… just the way \nI like it.",
                    DiaNormal = "Adequate. Not too slow, not too \nfast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster.",
                    VoicePathTalk = "Female Hmm2",
                    VoicePathMood = "Female Ahem",
                    VoicePathWrong = "Female Disagree1"
                },
                new Customer {
                    Id = 5,
                    Name = "Tall dude",
                    IdOrder = 630, //pink, blue, red
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    TalkDia = "I want Something Pink and fragrant, like\nLotus Blossom with the taste of lotus root\ndecorate with red petals",
                    Dia1 = "Lotus root inside of the pink dough\nsurrounded by crimson petals",
                    Dia2 = "Pink Lotus Blossom outside, Lotus root\nspirit inside, and those delicate Rose petals to\nfinish it yes, that's the taste I remember.",
                    DiaCurrect = "Ah, finally! Just what I wanted.\n Perfect!",
                    DiaWrong = "Wait, seriously? That’s not even \nclose!",
                    DiaHappy = "Ah, finally! Just what I wanted. \nPerfect!",
                    DiaNormal = "Hmm... alright, not bad.",
                    DiaAngry = "Hey, I’m not getting any younger here!",
                    VoicePathTalk = "Giant Pleasure",
                    VoicePathMood = "Giant Mood",
                    VoicePathWrong = "Male Angry2"
                },
                new Customer {
                    Id = 6,
                    Name = "Kid",
                    IdOrder = 410,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "I long for a sweet pink as lotus blossom.\nSoft, warm, and carrying the scent of fresh green\nfilling",
                    Dia1 = "I want a pink dough that is sweet, soft, warm,\nand has a taro flavor.",
                    Dia2 = "Pink lotus blossom dough on the outside,\npandan taro cream inside. No decoration",
                    DiaCurrect = "Well, well… you actually nailed it \nthis time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well... you actually nailed it \nthis time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…",
                    VoicePathTalk = "Cartoon Talk",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Kid Angry"
                },
                new Customer {
                    Id = 7,
                    Name = "Banana fine shyt",
                    IdOrder = 920,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "The golden one the one tied to \nthe moon's blessing with lotus root and\ndecorate with white petals.",
                    Dia1 = "I want the golden dough that said to\nbring good fortune fill with lotus root\nand decorate with Jamine petals",
                    Dia2 = "Golden moon dough on the outside,\nlotus root inside.and Jasmine Petal Charm\ndecoration.",
                    DiaCurrect = "Swift and sharp...\njust the way I like it.",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "Swift and sharp… just the way \nI like it.",
                    DiaNormal = "Adequate. Not too slow, not too \nfast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster.",
                    VoicePathTalk = "Female Hmm2",
                    VoicePathMood = "Female Ahem",
                    VoicePathWrong = "Female Disagree1"
                },
                new Customer {
                    Id = 8,
                    Name = "Tall dude",
                    IdOrder = 340,
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    TalkDia = "I want golden dough that is said to bring\ngood fortune in original coconut taste come with\nthe green leaves.",
                    Dia1 = "the golden one with coconut taste and lotus\nseal",
                    Dia2 = "I want the Golden Moon Coconut Filling with\nthe Emerald Lotus seal decorations.",
                    DiaCurrect = "Ah, finally! Just what I wanted.\n Perfect!",
                    DiaWrong = "Wait, seriously? That’s not even \nclose!",
                    DiaHappy = "Ah, finally! Just what I wanted. \nPerfect!",
                    DiaNormal = "Hmm… alright, not bad.",
                    DiaAngry = "Hey, I’m not getting any younger here!",
                    VoicePathTalk = "Giant Pleasure",
                    VoicePathMood = "Giant Mood",
                    VoicePathWrong = "Male Angry2"
                },
                new Customer {
                    Id = 9,
                    Name = "Kid",
                    IdOrder = 620,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "I want golden dough that is said to bring\ngood fortune Maybe with a bit of green inside\nit And.. a few white petals.",
                    Dia1 = "the golden one with pandan taro taste and\nJasmine petals",
                    Dia2 = "I want the Golden Moon dough pandan taro\ncream with the Jasmine Petal Charm decorations.",
                    DiaCurrect = "Well, well...\nyou actually nailed it this time!",
                    DiaWrong = "That’s not what I asked for... but \nI’ll take it.",
                    DiaHappy = "Well, well...\nyou actually nailed it this time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…",
                    VoicePathTalk = "Cartoon Talk",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Kid Angry"
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
                    TalkDia = "You waste too much banana leaves, \nI'm here to stop your nature destruction\n behavior.",
                    Dia1 = "Wait, that smell!! is that??",
                    Dia2 = "Give me Jasmine Moon Dough with \nCoconut Amber filling.",
                    DiaCurrect = " ",
                    DiaWrong = "seriously? all banana leaves waste to this?",
                    DiaHappy = "Hmmm this is so good",
                    DiaNormal = "Adequate. Not too slow, not too fast",
                    DiaAngry = "Took you ages… even the trees grow \nfaster.",
                    VoicePathTalk = "Female Hmm2",
                    VoicePathMood = "Female Ahem",
                    VoicePathWrong = "Female Disagree1"
                },
                new Customer {
                    Id = 2,
                    Name = "Banana fine shyt",
                    IdOrder = 510,
                    SpritePathHappy = "Customers/UnHuman/NPC1/npc1UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC1/npc1UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC1/npc1UnhumanAngry",
                    TalkDia = "Ok, one more dish and I'll decide your fate",
                    Dia1 = "Now I'll try Lotus Blossom with Pandan\nTaro Cream",
                    Dia2 = "You heard that.",
                    DiaCurrect = " ",
                    DiaWrong = "Wrong. Utterly wrong. How disappointing.",
                    DiaHappy = "So sweettt~~~, I'll let you enjoy this\na bit longer",
                    DiaNormal = "Hmm… acceptable.",
                    DiaAngry = "Delicious but better be faster next time.",
                    VoicePathTalk = "Female Hmm2",
                    VoicePathMood = "Female Ahem",
                    VoicePathWrong = "Female Disagree1"
                },
                new Customer {
                    Id = 3,
                    Name = "Tall dude",
                    IdOrder = 610,
                    SpritePathHappy = "Customers/UnHuman/NPC2/npc2UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC2/npc2UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC2/npc2UnhumanAngry",
                    TalkDia = "Is this still open?",
                    Dia1 = "Cool, I want some things that pink with Lotus root.",
                    Dia2 = "Lotus Blossom with Lotus Root Spirit \nfilling.",
                    DiaCurrect = "Ah, finally! Just what I wanted.\n Perfect!",
                    DiaWrong = "Wait, seriously? That’s not even \nclose!",
                    DiaHappy = "Ah, finally! Just what I wanted. \nPerfect!",
                    DiaNormal = "Hmm… alright, not bad.",
                    DiaAngry = "Hey, I’m not getting any younger here!",
                    VoicePathTalk = "Giant Pleasure",
                    VoicePathMood = "Giant Mood",
                    VoicePathWrong = "Male Angry2"
                },
                new Customer {
                    Id = 4,
                    Name = "Kid",
                    IdOrder = 310,
                    SpritePathHappy = "Customers/UnHuman/NPC3/npc3UnhumanHappy",
                    SpritePathNeutral = "Customers/UnHuman/NPC3/npc3UnhumanNormal",
                    SpritePathGrumpy = "Customers/UnHuman/NPC3/npc3UnhumanAngry",
                    TalkDia = "Hi",
                    Dia1 = "Ohh that things look delicious",
                    Dia2 = "Golden Moon Dough with Coconut Amber Filling.",
                    DiaCurrect = "Well, well… you actually nailed it \nthis time!",
                    DiaWrong = "That’s not what I asked for… but \nI’ll take it.",
                    DiaHappy = "Well, well… you actually nailed it \nthis time!",
                    DiaNormal = "Alright, that’ll do.",
                    DiaAngry = "You’re slower than I expected…",
                    VoicePathTalk = "Cartoon Talk",
                    VoicePathMood = "Kid Surprise",
                    VoicePathWrong = "Kid Angry"
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
            //return GetRandomFromList(Customers3);
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