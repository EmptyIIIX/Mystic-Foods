using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Composition;
using Mystic_Foods.Core.DI;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.Scenes;

namespace Mystic_Foods
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private Container _container;
        private SceneManager _sceneManager;
        private IGraphicsService _graphicsService;
        private IInputService _inputService;
        
        // Temporary - for TutorialScene compatibility until refactored
        public static bool wasTutorial = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1920,
                PreferredBackBufferHeight = 1080,
                IsFullScreen = false,
                SynchronizeWithVerticalRetrace = true
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Mystic Foods";
        }

        protected override void Initialize()
        {
            _container = GameCompositionRoot.Build(Content, GraphicsDevice);
            
            _sceneManager = _container.Resolve<SceneManager>();
            _graphicsService = _container.Resolve<IGraphicsService>();
            _inputService = _container.Resolve<IInputService>();

            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            _inputService.Update();
            _graphicsService.OnResize(Window.ClientBounds.Width, Window.ClientBounds.Height);
            _sceneManager.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _sceneManager.Draw(gameTime, _graphicsService.SpriteBatch);
            base.Draw(gameTime);
        }

        protected override void UnloadContent()
        {
            _sceneManager?.OnResize(0, 0);
            _container?.Clear();
            base.UnloadContent();
        }
    }
}