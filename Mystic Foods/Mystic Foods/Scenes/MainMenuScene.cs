using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mystic_Foods
{
    public class MainMenuScene : IGameScene
    {
        private GraphicsDeviceManager _graphics;
        private SpriteFont _font; //font use to draw string
        public bool StartGameRequested = false; //check if start game
        public bool DnDRequested = false;
        public bool ExitRequested = false; //check if exit game
        public bool CreditRequested = false;

        public Texture2D NameTitle;
        public Texture2D PlayBtn, SettingBtn, CreditBtn, ExitBtn;
        public Button _playBtn, _settingBtn, _creditBtn, _exitBtn;

        private KeyboardState _oldState; //make it only pressable (can't hold)
        private MouseState _oldMouseState;

        Texture2D Menu_bg;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");
            Menu_bg = content.Load<Texture2D>("Environments/BG/MenuBG");

            NameTitle = content.Load<Texture2D>("Etc/NameTitle");
            PlayBtn = content.Load<Texture2D>("Etc/option_start game");
            SettingBtn = content.Load<Texture2D>("Etc/option_setting");
            CreditBtn = content.Load<Texture2D>("Etc/option_credit");
            ExitBtn = content.Load<Texture2D>("Etc/option_exit");

            _playBtn = new Button(PlayBtn, _font, "", new Rectangle(225, 400, 512, 100));
            _playBtn.Click += PlayBtn_Click;

            _settingBtn = new Button(SettingBtn, _font, "", new Rectangle(225, 400 + PlayBtn.Height + 20, 512, 100));
            _settingBtn.Click += SettingBtn_Click;

            _creditBtn = new Button(CreditBtn, _font, "", new Rectangle(225, 400 + (PlayBtn.Height * 2) + 40, 512, 100));
            _creditBtn.Click += CreditBtn_Click;

            _exitBtn = new Button(ExitBtn, _font, "", new Rectangle(1920 - ExitBtn.Width - 20, 1080 - ExitBtn.Height - 20, 80, 100));
            _exitBtn.Click += ExitBtn_Click;
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            _playBtn.Update();
            _settingBtn.Update();
            _creditBtn.Update();
            _exitBtn.Update();

            _oldState = state; //update keyboard status
            _oldMouseState = mouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(Menu_bg, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(NameTitle, new Vector2(100, 120), Color.White);

            _playBtn.DrawHomeBtn(spriteBatch);
            _settingBtn.DrawHomeBtn(spriteBatch);
            _creditBtn.DrawHomeBtn(spriteBatch);
            _exitBtn.DrawHomeBtn(spriteBatch);

            if (CreditRequested) spriteBatch.DrawString(_font, "Hello World!", new Vector2(960 , 540), Color.White);

            spriteBatch.End();
        }

        public void PlayBtn_Click(object sender, EventArgs e)
        {
            StartGameRequested = true;
        }
        public void SettingBtn_Click(object sender, EventArgs e)
        {

        }
        public void CreditBtn_Click(object sender, EventArgs e)
        {
            CreditRequested = !CreditRequested;
        }
        public void ExitBtn_Click(object sender, EventArgs e)
        {
            ExitRequested = true;
        }
    }
}
