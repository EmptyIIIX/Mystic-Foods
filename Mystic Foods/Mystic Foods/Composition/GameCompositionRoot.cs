using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Mystic_Foods.Core.DI;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.Scenes;
using Mystic_Foods.Gameplay.Cooking;
using Mystic_Foods.Gameplay.Customer;
using Mystic_Foods.Gameplay.Economy;
using Mystic_Foods.Gameplay.Ingredients;
using Mystic_Foods.Gameplay.Recipes;
using Mystic_Foods.Scenes;
namespace Mystic_Foods.Composition
{
    public static class GameCompositionRoot
    {
        public static Container Build(ContentManager content, GraphicsDevice graphicsDevice)
        {
            var container = new Container();

            // --- Core services ---
                        var sceneManager = new SceneManager(content, graphicsDevice);
                        container.RegisterSingleton<SceneManager>(sceneManager);

                        var inputService = new InputService();
                        var graphicsService = new GraphicsService();
                        graphicsService.Initialize(graphicsDevice, 1920, 1080);

                        container.RegisterSingleton<IInputService>(inputService);
                        container.RegisterSingleton<IGraphicsService>(graphicsService);

                        // Also register in legacy ServiceLocator for Button, BaseScene, etc.
                        ServiceLocator.Register<IInputService>(inputService);
                        ServiceLocator.Register<IGraphicsService>(graphicsService);

            // --- Gameplay registries (data-driven) ---
            var ingredientRegistry = new IngredientRegistry();
            var recipeRegistry = new RecipeRegistry();
            
            // Create customer data using legacy Mystic_Foods.Customer type
                        var customers = new List<Mystic_Foods.Customer>
                        {
                            new Mystic_Foods.Customer
                            {
                                Id = 1,
                                Name = "ลูกค้าคนที่ 1",
                                IdOrder = 10,
                                SpritePathHappy = "Customers/Human/Dawn/NPC1/npc1HumanHappy",
                                SpritePathNeutral = "Customers/Human/Dawn/NPC1/npc1HumanNormal",
                                SpritePathGrumpy = "Customers/Human/Dawn/NPC1/npc1HumanAngry",
                                Dia1 = "สวัสดีครับ ขอสั่งอาหารหน่อยได้ไหม",
                                Dia2 = "เมนูไหนขายดีบ้างครับ",
                                DiaCurrect = "อร่อยมากครับ ขอบคุณ!",
                                DiaWrong = "อืม... นี่ไม่ใช่ที่สั่งหรอก",
                                DiaHappy = "ยอดเยี่ยมเลยครับ!",
                                DiaNormal = "กินได้นะครับ",
                                DiaAngry = "ไม่อร่อยเลย..."
                            },
                            new Mystic_Foods.Customer
                            {
                                Id = 2,
                                Name = "ลูกค้าคนที่ 2",
                                IdOrder = 20,
                                SpritePathHappy = "Customers/Human/Dawn/NPC2/npc2HumanHappy",
                                SpritePathNeutral = "Customers/Human/Dawn/NPC2/npc2HumanNormal",
                                SpritePathGrumpy = "Customers/Human/Dawn/NPC2/npc2HumanAngry",
                                Dia1 = "หิวจังเลย วันนี้ทำอะไรดี",
                                Dia2 = "แนะนำเมนูแนะนำหน่อย",
                                DiaCurrect = "อร่อยสุดๆ เลย!",
                                DiaWrong = "เอ้า นี่ผิดออเดอร์แล้ว",
                                DiaHappy = "เยี่ยมมาก!",
                                DiaNormal = "ปกติปกติ",
                                DiaAngry = "เสียใจจัง"
                            }
                        };
            
            var customerQueue = new CustomerQueue(customers);

            container.RegisterSingleton(ingredientRegistry);
            container.RegisterSingleton(recipeRegistry);
            container.RegisterSingleton(customerQueue);

            // --- Cooking stations ---
            container.RegisterSingleton(new PlateStation(new Vector2(1068, 639), LoadOrFallback<Texture2D>(content, "foods/Plate")));
            container.RegisterSingleton(new DecorationStation(new Vector2(4200 - 958, 644), LoadOrFallback<Texture2D>(content, "foods/Plate")));
            container.RegisterSingleton(new SteamStation(new Vector2(1800 + 409/2, 520), LoadOrFallback<Texture2D>(content, "Environments/tools/steamer1")));
            container.RegisterSingleton(new TrashStation(new Vector2(160, 800), LoadOrFallback<Texture2D>(content, "Etc/White_Tako")));

            // --- Economy ---
                        container.RegisterSingleton(new Wallet(100f));
                        container.RegisterSingleton(new TransactionLog());

                        // --- Scenes (created with DI container, registered with SceneManager) ---
                        sceneManager = container.Resolve<SceneManager>();

                        var mainMenuScene = new MainMenuScene(container);
                        var levelSelectScene = new LevelSelectScene(container);
                        var gamePlayScene = new GamePlayScene(container);
                        var dndScene = new DnDScene(container);
                        var settingScene = new SettingScene(container);
                        var tutorialScene = new TutorialScene(container);
                        var creditScene = new CreditScene(container);

                        sceneManager.RegisterScene("MainMenu", mainMenuScene);
                        sceneManager.RegisterScene("LevelSelect", levelSelectScene);
                        sceneManager.RegisterScene("GamePlay", gamePlayScene);
                        sceneManager.RegisterScene("DnD", dndScene);
                        sceneManager.RegisterScene("Setting", settingScene);
                        sceneManager.RegisterScene("Tutorial", tutorialScene);
                        sceneManager.RegisterScene("Credit", creditScene);

                        // Start with MainMenu
                        sceneManager.ChangeScene("MainMenu");

                        return container;
        }

        /// <summary>Build-time safe texture load. Returns a 1x1 white texture on missing content.</summary>
        private static T LoadOrFallback<T>(ContentManager content, string path) where T : Texture2D
        {
            try { return content.Load<T>(path); }
            catch (ContentLoadException) { return null; }
        }
    }
}