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
        //private int _selectedIndex = 0;
        //private string[] _menuItems = { "Start Game", "Drag&Drop", "Exit" }; //selectable text
        public bool StartGameRequested = false; //check if start game
        public bool DnDRequested = false;
        public bool ExitRequested = false; //check if exit game

        public Texture2D NameTitle;
        public Texture2D PlayBtn, SettingBtn, ExitBtn;
        public Button _playBtn, _settingBtn, _exitBtn;

        private KeyboardState _oldState; //make it only pressable (can't hold)
        private MouseState _oldMouseState;

        // เก็บ bounding box ของแต่ละเมนู (เพื่อคลิกได้)
        //private List<Rectangle> _menuItemRects = new List<Rectangle>();

        Texture2D Menu_bg;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");
            Menu_bg = content.Load<Texture2D>("Environments/BG/MenuBG");

            NameTitle = content.Load<Texture2D>("Etc/NameTitle");
            PlayBtn = content.Load<Texture2D>("Etc/option_start game");
            SettingBtn = content.Load<Texture2D>("Etc/option_setting");
            ExitBtn = content.Load<Texture2D>("Etc/option_exit");

            _playBtn = new Button(PlayBtn, _font, "", new Rectangle(225, 400, 512, 100));
            _playBtn.Click += PlayBtn_Click;

            _settingBtn = new Button(SettingBtn, _font, "", new Rectangle(225, 400 + PlayBtn.Height + 20, 512, 100));
            _settingBtn.Click += SettingBtn_Click;

            _exitBtn = new Button(ExitBtn, _font, "", new Rectangle(1920 - ExitBtn.Width - 20, 1080 - ExitBtn.Height - 20, 80, 100));
            _exitBtn.Click += ExitBtn_Click;

            // สร้าง rectangle ของแต่ละเมนูสำหรับตรวจ mouse
            //_menuItemRects.Clear();
            //for (int i = 0; i < _menuItems.Length; i++)
            //{
            //    Vector2 size = _font.MeasureString(_menuItems[i]);
            //    Rectangle rect = new Rectangle(
            //        (int)((800 - size.X) / 2),
            //        200 + i * 60,
            //        (int)size.X,
            //        (int)size.Y
            //    );
            //    _menuItemRects.Add(rect);
            //}
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            _playBtn.Update();
            _settingBtn.Update();
            _exitBtn.Update();

            #region
            //if (state.IsKeyDown(Keys.Down) && _oldState.IsKeyUp(Keys.Down))
            //    _selectedIndex = (_selectedIndex + 1) % _menuItems.Length;

            //if (state.IsKeyDown(Keys.Up) && _oldState.IsKeyUp(Keys.Up))
            //    _selectedIndex = (_selectedIndex - 1 + _menuItems.Length) % _menuItems.Length;

            ////game scene selection logic
            //if (state.IsKeyDown(Keys.Enter) && _oldState.IsKeyUp(Keys.Enter))
            //{
            //    if (_selectedIndex == 0) StartGameRequested = true;
            //    if (_selectedIndex == 1) DnDRequested = true;
            //    if (_selectedIndex == 2) ExitRequested = true;
            //}

            //// เมาส์อยู่ตำแหน่งไหนให้ไฮไลท์เมนูนั้น (hover)
            //Point mousePos = mouse.Position;
            //for (int i = 0; i < _menuItemRects.Count; i++)
            //{
            //    if (_menuItemRects[i].Contains(mousePos))
            //    {
            //        _selectedIndex = i;
            //    }
            //}
            // เช็คคลิกซ้าย (Mouse.LeftButton เพิ่งกดจาก unpress)
            //if (mouse.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            //{
            //    for (int i = 0; i < _menuItemRects.Count; i++)
            //    {
            //        if (_menuItemRects[i].Contains(mouse.Position))
            //        {
            //            // ขอ scene ตามปุ่มคลิก
            //            if (i == 0) StartGameRequested = true;
            //            if (i == 1) DnDRequested = true;
            //            if (i == 2) ExitRequested = true;
            //        }
            //    }
            //}
            #endregion
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
            _exitBtn.DrawHomeBtn(spriteBatch);


            #region
            //string title = "Main Menu";
            //Vector2 titleSize = _font.MeasureString(title);
            //spriteBatch.DrawString(
            //    _font,
            //    title,
            //    new Vector2((800 - titleSize.X) / 2, 80),
            //    Color.White);

            //for (int i = 0; i < _menuItems.Length; i++)
            //{
            //    Color color = (i == _selectedIndex) ? Color.Yellow : Color.White;
            //    Vector2 size = _font.MeasureString(_menuItems[i]);
            //    spriteBatch.DrawString(
            //        _font,
            //        _menuItems[i],
            //        new Vector2((800 - size.X) / 2, 200 + i * 60),
            //        color);
            //}
            #endregion

            spriteBatch.End();
        }

        public void PlayBtn_Click(object sender, EventArgs e)
        {
            StartGameRequested = true;
        }
        public void SettingBtn_Click(Object sender, EventArgs e)
        {

        }
        public void ExitBtn_Click(Object sender, EventArgs e)
        {
            ExitRequested = true;
        }
    }
}
