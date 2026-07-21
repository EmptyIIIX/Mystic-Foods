using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Mystic_Foods.Core.DI;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.Scenes;
using Mystic_Foods.Core.UI;

namespace Mystic_Foods.Scenes
{
    public class MainMenuScene : IScene
    {
        private readonly IGraphicsService _graphics;
        private readonly IInputService _input;
        private readonly Container _container;

        private SpriteFont _font;
        public bool StartGameRequested { get; private set; }
        public bool DnDRequested { get; private set; }
        public bool ExitRequested { get; private set; }
        public bool CreditRequested { get; private set; }
        public bool SettingRequested { get; private set; }

        private Texture2D _nameTitle;
        private Texture2D _menuBg;
        private Button _playBtn, _settingBtn, _creditBtn, _exitBtn;

        public string Name { get; } = "MainMenu";
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; } = true;

        public MainMenuScene(Container container)
        {
            _container = container;
            _graphics = container.Resolve<IGraphicsService>();
            _input = container.Resolve<IInputService>();
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("MainFont");
            _menuBg = content.Load<Texture2D>("Environments/BG/MenuBG");
            _nameTitle = content.Load<Texture2D>("Etc/NameTitle");

            var playTex = content.Load<Texture2D>("Etc/option_start game");
            var settingTex = content.Load<Texture2D>("Etc/option_setting");
            var creditTex = content.Load<Texture2D>("Etc/option_credit");
            var exitTex = content.Load<Texture2D>("Etc/option_exit");

            int btnW = 512, btnH = 100;
            int startX = 225, startY = 400, gap = 20;

            _playBtn = new Button(new ButtonConfig
            {
                Texture = playTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(startX, startY, btnW, btnH),
                OnClick = _ => StartGameRequested = true
            });

            _settingBtn = new Button(new ButtonConfig
            {
                Texture = settingTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(startX, startY + btnH + gap, btnW, btnH),
                OnClick = _ => SettingRequested = true
            });

            _creditBtn = new Button(new ButtonConfig
            {
                Texture = creditTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(startX, startY + 2 * (btnH + gap), btnW, btnH),
                OnClick = _ => CreditRequested = !CreditRequested
            });

            _exitBtn = new Button(new ButtonConfig
            {
                Texture = exitTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(1920 - exitTex.Width - 20, 1080 - exitTex.Height - 20, 80, 100),
                OnClick = _ => ExitRequested = true
            });

            MediaPlayer.IsRepeating = true;
            // SoundManager.PlaySong("mainmenu"); // TODO: integrate with new audio service
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _input.Update();

            _playBtn.Update(gameTime);
            _settingBtn.Update(gameTime);
            _creditBtn.Update(gameTime);
            _exitBtn.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
                {
                    if (!IsVisible) return;

                    _graphics.Begin();
                    _graphics.Draw(_menuBg, new Rectangle(0, 0, 1920, 1080), null, Color.White);
                    _graphics.Draw(_nameTitle, new Rectangle(100, 120, _nameTitle.Width, _nameTitle.Height), null, Color.White);

                    _playBtn.Draw(gameTime, _graphics.SpriteBatch);
                    _settingBtn.Draw(gameTime, _graphics.SpriteBatch);
                    _creditBtn.Draw(gameTime, _graphics.SpriteBatch);
                    _exitBtn.Draw(gameTime, _graphics.SpriteBatch);

                    if (CreditRequested)
                    {
                        _graphics.DrawString(_font, "Hello World!", new Vector2(960, 540), Color.White);
                    }
                    _graphics.End();
                }

        public void OnEnter()
        {
            StartGameRequested = DnDRequested = ExitRequested = CreditRequested = SettingRequested = false;
        }

        public void OnExit() { }
        public void OnResize(int width, int height) { }
    }
}