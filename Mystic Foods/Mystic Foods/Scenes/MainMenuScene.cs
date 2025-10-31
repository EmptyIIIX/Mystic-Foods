using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Mystic_Foods.Managers;
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
        public static bool StartGameRequested = false; //check if start game
        public bool DnDRequested = false;
        public bool ExitRequested = false; //check if exit game
        public bool CreditRequested = false;
        public bool SettingRequested = false;

        public Texture2D NameTitle, settingBG;
        public Texture2D PlayBtn, SettingBtn, CreditBtn, ExitBtn, ExitBtn_hover;
        public Button _playBtn, _settingBtn, _creditBtn, _exitBtn;

        private KeyboardState _oldState; //make it only pressable (can't hold)
        private MouseState _oldMouseState;

        Texture2D Menu_bg;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("BoldFont");
            Menu_bg = content.Load<Texture2D>("Environments/BG/MenuBG");

            NameTitle = content.Load<Texture2D>("UI/title");
            PlayBtn = content.Load<Texture2D>("UI/new_game");
            SettingBtn = content.Load<Texture2D>("UI/setting");
            CreditBtn = content.Load<Texture2D>("UI/credit");
            ExitBtn = content.Load<Texture2D>("UI/exit");
            settingBG = content.Load<Texture2D>("UI/setting/Setting_BG");
            //ExitBtn_hover = content.Load<Texture2D>("UI/exit");

            //_playBtn = new Button(PlayBtn, _font, "", new Rectangle(225, 400, 512, 100));
            //_playBtn.Click += PlayBtn_Click;
            _playBtn = new Button(PlayBtn, PlayBtn, _font, "", new Rectangle(225, 500, 512, 100));
            _playBtn.Click += PlayBtn_Click;
            _exitBtn = new Button(ExitBtn, ExitBtn, _font, "", new Rectangle(225, 500 + 100 + 10, 512, 100));
            _exitBtn.Click += ExitBtn_Click;
            _creditBtn = new Button(CreditBtn, CreditBtn, _font, "", new Rectangle(1920 - CreditBtn.Width - 20, 1080 - (CreditBtn.Height * 2) - 40, 88, 88));
            _creditBtn.Click += CreditBtn_Click;
            _settingBtn = new Button(SettingBtn, SettingBtn, _font, "", new Rectangle(1920 - CreditBtn.Width - 20, 1080 - CreditBtn.Height - 20, 88, 88));
            _settingBtn.Click += SettingBtn_Click;

            SoundManager.PlaySong("mainmenu");
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

            if (CreditRequested)
            {
                spriteBatch.Draw(settingBG, new Rectangle(1050, 250, 500, 500), Color.White);
                spriteBatch.DrawString(_font, "Nah", new Vector2(1225, 400), Color.Black);
            }

            spriteBatch.End();
        }

        public async void PlayBtn_Click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            await Task.Delay(100);
            StartGameRequested = true;
        }
        public async void SettingBtn_Click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            await Task.Delay(100);
            SettingRequested = true;
        }
        public async void CreditBtn_Click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            await Task.Delay(100);
            //CreditRequested = !CreditRequested;
            DnDRequested = true;
        }
        public async void ExitBtn_Click(object sender, EventArgs e)
        {
            SoundManager.PlaySfx("Click");
            await Task.Delay(200);
            ExitRequested = true;
        }
    }
}
