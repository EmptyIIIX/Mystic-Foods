using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Scenes;
using Mystic_Foods.Systems;

namespace Mystic_Foods
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private GameManager _gameManager;

        //Scene Management
        private IGameScene _currentScene;
        private MainMenuScene _mainMenuScene;
        private GamePlayScene _gamePlayScene;
        private DnDScene _dndScene;
        private LevelSelectScene _levelSelectScene;

        private CustomerManager _customerManager;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            //screen resolution
            //เก็บค่าขนาดหน้าจอของdevice
            int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //set up หน้าต่างเกม
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
            Window.IsBorderless = true;//better fullscreen
            _graphics.ApplyChanges();
        }
        protected override void Initialize()
        {
            _customerManager = new CustomerManager();
            _mainMenuScene = new MainMenuScene();
            _gamePlayScene = new GamePlayScene(_customerManager);
            _levelSelectScene = new LevelSelectScene();

            //make it start at main menu
            _currentScene = _mainMenuScene;

            Window.ClientSizeChanged += OnClientSizeChanged;
            base.Initialize();
        }
        private void OnClientSizeChanged(object sender, System.EventArgs e)
        {
            //อัปเดตขนาด back bufferเมื่อหน้าต่างเปลี่ยนขนาด
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphics.ApplyChanges();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Globals.Content = Content;
            _gameManager = new GameManager();
            _gameManager.LoadContent(Content);

            _dndScene = new DnDScene(_gameManager);

            //load scene
            _mainMenuScene.LoadContent(Content, _spriteBatch);
            _gamePlayScene.LoadContent(Content, _spriteBatch);
            _dndScene.LoadContent(Content, _spriteBatch);
            _levelSelectScene.LoadContent(Content, _spriteBatch);

        }
        protected override void Update(GameTime gameTime)
        {
            //Scene Logic
            #region Scene Logic
            if (_currentScene == _mainMenuScene)
            {
                _mainMenuScene.Update(gameTime);
                if (_mainMenuScene.StartGameRequested)
                {
                    _mainMenuScene.StartGameRequested = false;
                    _currentScene = _levelSelectScene;
                }
                if (_mainMenuScene.DnDRequested)
                {
                    _mainMenuScene.DnDRequested = false;
                    _currentScene = _dndScene;
                }
            }
            else if (_currentScene == _levelSelectScene)
            {
                _levelSelectScene.Update(gameTime);
                if (_levelSelectScene.gameplayRequest)
                {
                    _levelSelectScene.gameplayRequest = false;
                    _currentScene = _gamePlayScene;
                }
                if (_levelSelectScene.MenuRequest)
                {
                    _levelSelectScene.MenuRequest = false;
                    _currentScene = _mainMenuScene;
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
                else if (_gamePlayScene.DnDRequested)
                {
                    _gamePlayScene.DnDRequested = false;
                    _currentScene = _dndScene;
                }
            }
            else if (_currentScene == _dndScene)
            {
                _dndScene.Update(gameTime);
                if (_gamePlayScene.BackToMenuRequested)
                {
                    _gamePlayScene.BackToMenuRequested = false;
                    _currentScene = _mainMenuScene;
                }
                if (_dndScene.ServeRequest)
                {
                    _dndScene.ServeRequest = false;
                    _currentScene = _gamePlayScene;
                }
                if (_dndScene.BackToGame)
                {
                    _dndScene.BackToGame = false;
                    _currentScene = _gamePlayScene;
                }
            }
            //Check exit game
            if (_mainMenuScene.ExitRequested || _gamePlayScene.ExitRequest) Exit();
            #endregion

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            //draw scene
            _currentScene.Draw(_spriteBatch);

            base.Draw(gameTime);
        }
    }
}
