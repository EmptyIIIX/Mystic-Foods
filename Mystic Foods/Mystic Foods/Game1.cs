using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Core.Scenes;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.UI;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods
{
    /// <summary>
    /// Main game class implementing Service Locator pattern for dependency injection
    /// Follows Single Responsibility Principle - orchestrates game components
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SceneManager _sceneManager;
        private CustomerManager _customerManager;
        private GameManager _gameManager;
        private SoundManager _soundManager;
        private ScreenManager _screenManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Screen.PrimaryScreen.Bounds.Width,
                PreferredBackBufferHeight = Screen.PrimaryScreen.Bounds.Height,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = true
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Mystic Foods";
        }

        protected override void Initialize()
        {
            // Setup Service Locator with all dependencies
            SetupServiceLocator();
            
            // Initialize Graphics Service
            _graphics.InitializeGraphicsDevice(1920, 1080);
            ServiceLocator.Register<GraphicsService>(new GraphicsService());
            ServiceLocator.Get<IGraphicsService>().Initialize(_graphics.GraphicsDevice, 1920, 1080);
            
            // Set screen service
            var screenService = ServiceLocator.Get<IGraphicsService>();
            _screenManager = new ScreenManager();
            _screenManager.Initialize(_graphics, 1920, 1080, false);
            ServiceLocator.Register<ScreenManager>(_screenManager);
            
            // Initialize Scene Manager
            _sceneManager = new SceneManager(ServiceLocator.Get<ContentManager>(), _graphics.GraphicsDevice);
            ServiceLocator.Register<SceneManager>(_sceneManager);
            
            // Register managers and systems
            _customerManager = new CustomerManager();
            ServiceLocator.Register<CustomerManager>(_customerManager);
            
            _gameManager = new GameManager();
            ServiceLocator.Register<GameManager>(_gameManager);
            
            _soundManager = new SoundManager();
            ServiceLocator.Register<SoundManager>(_soundManager);
            
            // Initialize game components
            InitializeGameComponents();
            
            base.Initialize();
        }

        private void SetupServiceLocator()
        {
            // Register core services first
            ServiceLocator.Register<IInputService>(new InputService());
        }

        private void InitializeGameComponents()
        {
            // Initialize core game systems
            _gameManager.LoadContent(Content);
            
            // Create scene instances using dependency injection
            var mainMenuScene = new MainMenuScene();
            var gamePlayScene = new GamePlayScene(_customerManager);
            var levelSelectScene = new LevelSelectScene();
            var tutorialScene = new TutorialScene();
            var settingScene = new SettingScene();
            var dndScene = new DnDScene(_gameManager);
            
            // Register scenes with SceneManager
            _sceneManager.RegisterScene("MainMenu", mainMenuScene);
            _sceneManager.RegisterScene("GamePlay", gamePlayScene);
            _sceneManager.RegisterScene("LevelSelect", levelSelectScene);
            _sceneManager.RegisterScene("Tutorial", tutorialScene);
            _sceneManager.RegisterScene("Settings", settingScene);
            _sceneManager.RegisterScene("DnD", dndScene);
            
            // Start with main menu
            _sceneManager.ChangeScene("MainMenu");
        }

        protected override void LoadContent()
        {
            // Content loading is handled by SceneManager
        }

        protected override void Update(GameTime gameTime)
        {
            // Update services
            ServiceLocator.Get<IInputService>().Update();
            ServiceLocator.Get<IGraphicsService>().UpdateViewport(
                Window.ClientBounds.Width,
                Window.ClientBounds.Height
            );
            
            // Update SceneManager
            _sceneManager.Update(gameTime);
            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _graphics.GraphicsDevice.Clear(Color.Black);
            _sceneManager.Draw(gameTime);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _sceneManager?.OnResize(0, 0);
            ServiceLocator.Clear();
            base.UnloadContent();
        }
    }
}