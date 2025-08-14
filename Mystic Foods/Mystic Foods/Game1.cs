using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mystic_Foods
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Drag_DropManager _drag_dropManager;
        //Scene Management
        private IGameScene _currentScene;
        private MainMenuScene _mainMenuScene;
        private GamePlayScene _gamePlayScene;

        private CustomerManager _customerManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //screen resolution
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            _customerManager = new CustomerManager();

            _drag_dropManager = new Drag_DropManager();
            _mainMenuScene = new MainMenuScene();
            _gamePlayScene = new GamePlayScene(_customerManager);

            //make it start at main menu
            _currentScene = _mainMenuScene;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here
            //load texture , add sprite into Drag_DropManager
            Texture2D boxTexture = Content.Load<Texture2D>("Assets/Asset");

            //Lastest sprite will be top layer and first choosed for drag & drop
            _drag_dropManager.AddSprite(new Drag_Drop("Asset", boxTexture, new Vector2(150, 150)));
            //load scene
            _mainMenuScene.LoadContent(Content);
            _gamePlayScene.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {

            //Scene Logic
            if (_currentScene == _mainMenuScene)
            {
                _mainMenuScene.Update(gameTime);
                if (_mainMenuScene.StartGameRequested)
                {
                    _mainMenuScene.StartGameRequested = false;
                    _currentScene = _gamePlayScene;
                }
                if (_mainMenuScene.ExitRequested)
                {
                    Exit();
                }
            }
            else if (_currentScene == _gamePlayScene)
            {
                _gamePlayScene.Update(gameTime);
                if (_gamePlayScene.BackToMenuRequested)
                {
                    _gamePlayScene.BackToMenuRequested = false;
                    _currentScene = _mainMenuScene;
                }
            }

            // TODO: Add your update logic here
            _drag_dropManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            _drag_dropManager.Draw(_spriteBatch);
            _spriteBatch.End();
            //draw scene
            _currentScene.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
