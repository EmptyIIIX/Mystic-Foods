using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mystic_Foods
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Scene Management
        private IGameScene _currentScene;
        private MainMenuScene _mainMenuScene;
        private GamePlayScene _gamePlayScene;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //screen resolution
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 480;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //make it start at main menu
            _mainMenuScene = new MainMenuScene();
            _gamePlayScene = new GamePlayScene();
            _currentScene = _mainMenuScene;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            // TODO: Add your drawing code here
            //draw scene
            _currentScene.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
