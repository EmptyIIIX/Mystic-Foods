using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private IGameScene _currentScene;
        private MainMenuScene _mainMenuScene;
        private GamePlayScene _gamePlayScene;
        private DnDScene _dndScene;
        private LevelSelectScene _levelSelectScene;
        private Recipe _recipeScene;
        private CreditScene _creditScene;
        private SettingScene _settingScene;
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
            SoundManager.LoadSettings();

            _customerManager = new CustomerManager();
            _mainMenuScene = new MainMenuScene();
            _gamePlayScene = new GamePlayScene(_customerManager);
            _levelSelectScene = new LevelSelectScene();
            _recipeScene = new Recipe();
            _creditScene = new CreditScene();
            _settingScene = new SettingScene();

            //make it start at main menu
            _currentScene = _mainMenuScene;

            Window.ClientSizeChanged += OnClientSizeChanged;
            base.Initialize();
        }
        private void OnClientSizeChanged(object sender, System.EventArgs e)
        {
            _graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            _graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            _graphics.ApplyChanges();
        }
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            MediaPlayer.IsRepeating = true;
            SoundManager.MusicVolume = 0.1f;
            #region BGM
            SoundManager.AddSong("mainmenu", Content.Load<Song>("Music/BGM/Lao Lum Dab"));
            SoundManager.AddSong("daybgm", Content.Load<Song>("Music/BGM/Thai Mung"));
            SoundManager.AddSong("duskbgm", Content.Load<Song>("Music/BGM/Thai Ar Hom (Chaina)"));
            SoundManager.AddSong("nightbgm", Content.Load<Song>("Music/BGM/bgm01"));
            #endregion

            SoundManager.SfxVolume = 0.1f;
            #region SFX
            SoundManager.AddSound("Button", Content.Load<SoundEffect>("Music/SFX/Button Press"));
            SoundManager.AddSound("Button2", Content.Load<SoundEffect>("Music/SFX/Button Press2"));
            SoundManager.AddSound("Click", Content.Load<SoundEffect>("Music/SFX/Click"));
            SoundManager.AddSound("Cooking", Content.Load<SoundEffect>("Music/SFX/Cooking"));
            SoundManager.AddSound("Walking", Content.Load<SoundEffect>("Music/SFX/Walking"));
            SoundManager.AddSound("Coin", Content.Load<SoundEffect>("Music/SFX/Coin2"));
            SoundManager.AddSound("Woosh", Content.Load<SoundEffect>("Music/SFX/woosh"));
            SoundManager.AddSound("Trash", Content.Load<SoundEffect>("Music/SFX/Trash"));
            SoundManager.AddSound("Mix", Content.Load<SoundEffect>("Music/SFX/prink"));
            SoundManager.AddSound("Keep", Content.Load<SoundEffect>("Music/SFX/Mixed"));
            SoundManager.AddSound("Page", Content.Load<SoundEffect>("Music/SFX/Page"));
            #endregion

            #region Voice
            SoundManager.AddSound("Cartoon Talk", Content.Load<SoundEffect>("Music/Voice/Cartoon Talk"));

            SoundManager.AddSound("Female Ahem", Content.Load<SoundEffect>("Music/Voice/Female Ahem"));
            SoundManager.AddSound("Female Augh", Content.Load<SoundEffect>("Music/Voice/Female Augh"));
            SoundManager.AddSound("Female Augh2", Content.Load<SoundEffect>("Music/Voice/Female Augh2"));
            SoundManager.AddSound("Female Disagree1", Content.Load<SoundEffect>("Music/Voice/Female Disagree1"));
            SoundManager.AddSound("Female Disagree2", Content.Load<SoundEffect>("Music/Voice/Female Disagree2"));
            SoundManager.AddSound("Female Hmm1", Content.Load<SoundEffect>("Music/Voice/Female Hmm1"));
            SoundManager.AddSound("Female Hmm2", Content.Load<SoundEffect>("Music/Voice/Female Hmm2"));
            SoundManager.AddSound("Female Laugh1", Content.Load<SoundEffect>("Music/Voice/Female Laugh1"));
            SoundManager.AddSound("Female Laugh2", Content.Load<SoundEffect>("Music/Voice/Female Laugh2"));
            SoundManager.AddSound("Female Reject", Content.Load<SoundEffect>("Music/Voice/Female Reject"));

            SoundManager.AddSound("Giant Mood", Content.Load<SoundEffect>("Music/Voice/Giant Mood"));
            SoundManager.AddSound("Giant Pleasure", Content.Load<SoundEffect>("Music/Voice/Giant Pleasure"));

            SoundManager.AddSound("Kid Angry", Content.Load<SoundEffect>("Music/Voice/Kid Angry"));
            SoundManager.AddSound("Kid Surprise", Content.Load<SoundEffect>("Music/Voice/Kid Surprise"));

            SoundManager.AddSound("Male Hi", Content.Load<SoundEffect>("Music/Voice/Male Hi"));
            SoundManager.AddSound("Male Angry1", Content.Load<SoundEffect>("Music/Voice/Male Angry1"));
            SoundManager.AddSound("Male Angry2", Content.Load<SoundEffect>("Music/Voice/Male Angry2"));
            SoundManager.AddSound("Male Sigh", Content.Load<SoundEffect>("Music/Voice/Male Sigh"));
            #endregion

            Globals.Content = Content;
            _gameManager = new GameManager();
            _gameManager.LoadContent(Content);

            _dndScene = new DnDScene(_gameManager);

            //load scene
            _mainMenuScene.LoadContent(Content, _spriteBatch);
            _gamePlayScene.LoadContent(Content, _spriteBatch);
            _dndScene.LoadContent(Content, _spriteBatch);
            _levelSelectScene.LoadContent(Content, _spriteBatch);
            _recipeScene.LoadContent(Content, _spriteBatch);
            _creditScene.LoadContent(Content, _spriteBatch);
            _settingScene.LoadContent(Content, _spriteBatch);

        }
        protected override void Update(GameTime gameTime)
        {
            //Scene Logic
            #region Scene Logic
            if (_currentScene == _mainMenuScene)
            {
                _mainMenuScene.Update(gameTime);
                if (MainMenuScene.StartGameRequested)
                {
                    MainMenuScene.StartGameRequested = false;
                    _currentScene = _levelSelectScene;
                }
                if (_mainMenuScene.DnDRequested)
                {
                    _mainMenuScene.DnDRequested = false;
                    _currentScene = _dndScene;
                }
                if (_mainMenuScene.SettingRequested)
                {
                    _mainMenuScene.SettingRequested = false;
                    _currentScene = _settingScene;
                }
                if (_mainMenuScene.CreditRequested)
                {
                    _mainMenuScene.CreditRequested = false;
                    _currentScene = _creditScene;
                }
            }
            else if (_currentScene == _recipeScene)
            {
                _recipeScene.Update(gameTime);
                if (Recipe.isExitPage)
                {
                    _currentScene = _dndScene;
                    Recipe.isExitPage = false;
                }
            }
            else if (_currentScene == _creditScene)
            {
                _creditScene.Update(gameTime);
                if (CreditScene.ExitToMenu)
                {
                    _currentScene = _mainMenuScene;
                    CreditScene.ExitToMenu = false;
                }
            }
            else if (_currentScene == _levelSelectScene)
            {
                _levelSelectScene.Update(gameTime);
                if (_levelSelectScene.tutorial2)
                {
                    _levelSelectScene.tutorial2 = false;
                    _currentScene = _gamePlayScene;
                }
                if (_levelSelectScene.MenuRequest)
                {
                    _levelSelectScene.MenuRequest = false;
                    _currentScene = _mainMenuScene;
                }
            }
            else if (_currentScene == _settingScene)
            {
                _settingScene.Update(gameTime);
                if (_settingScene.MenuRequest)
                {
                    _settingScene.MenuRequest = false;
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
                if (_dndScene.backToCounter)
                {
                    _currentScene = _gamePlayScene;
                    _dndScene.backToCounter = false;
                }
                if (Recipe.RecipeBookRequest)
                {
                    Recipe.RecipeBookRequest = false;
                    _currentScene = _recipeScene;
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
