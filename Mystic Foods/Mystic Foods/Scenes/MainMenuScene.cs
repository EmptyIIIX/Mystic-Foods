using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private int _selectedIndex = 0;
        private string[] _menuItems = { "Start Game", "Drag&Drop", "Exit" }; //selectable text
        public bool StartGameRequested = false; //check if start game
        public bool DnDRequested = false;
        public bool ExitRequested = false; //check if exit game

        private KeyboardState _oldState; //make it only pressable (can't hold)
        private MouseState _oldMouseState;

        //BG
        Texture2D _bg;

        // เก็บ bounding box ของแต่ละเมนู (เพื่อคลิกได้)
        private List<Rectangle> _menuItemRects = new List<Rectangle>();

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");

            // สร้าง rectangle ของแต่ละเมนูสำหรับตรวจ mouse
            _menuItemRects.Clear();
            for (int i = 0; i < _menuItems.Length; i++)
            {
                Vector2 size = _font.MeasureString(_menuItems[i]);
                Rectangle rect = new Rectangle(
                    (int)((800 - size.X) / 2),
                    200 + i * 60,
                    (int)size.X,
                    (int)size.Y
                );
                _menuItemRects.Add(rect);
            }

            _bg = content.Load<Texture2D>("Environments/BG/MainMenuBG");
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            if (state.IsKeyDown(Keys.Down) && _oldState.IsKeyUp(Keys.Down))
                _selectedIndex = (_selectedIndex + 1) % _menuItems.Length;

            if (state.IsKeyDown(Keys.Up) && _oldState.IsKeyUp(Keys.Up))
                _selectedIndex = (_selectedIndex - 1 + _menuItems.Length) % _menuItems.Length;

            //game scene selection logic
            if (state.IsKeyDown(Keys.Enter) && _oldState.IsKeyUp(Keys.Enter))
            {
                if (_selectedIndex == 0) StartGameRequested = true;
                if (_selectedIndex == 1) DnDRequested = true;
                if (_selectedIndex == 2) ExitRequested = true;
            }

            // เมาส์อยู่ตำแหน่งไหนให้ไฮไลท์เมนูนั้น (hover)
            Point mousePos = mouse.Position;
            for (int i = 0; i < _menuItemRects.Count; i++)
            {
                if (_menuItemRects[i].Contains(mousePos))
                {
                    _selectedIndex = i;
                }
            }
            // เช็คคลิกซ้าย (Mouse.LeftButton เพิ่งกดจาก unpress)
            if (mouse.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                for (int i = 0; i < _menuItemRects.Count; i++)
                {
                    if (_menuItemRects[i].Contains(mouse.Position))
                    {
                        // ขอ scene ตามปุ่มคลิก
                        if (i == 0) StartGameRequested = true;
                        if (i == 1) DnDRequested = true;
                        if (i == 2) ExitRequested = true;
                    }
                }
            }

            _oldState = state; //update keyboard status
            _oldMouseState = mouse;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(_bg, new Vector2(0, 0), Color.White);
            string title = "Mystic Food";
            Vector2 titleSize = _font.MeasureString(title);
            spriteBatch.DrawString(
                _font,
                title,
                new Vector2((800 - titleSize.X) / 2, 80),
                Color.White);

            for (int i = 0; i < _menuItems.Length; i++)
            {
                Color color = (i == _selectedIndex) ? Color.Yellow : Color.White;
                Vector2 size = _font.MeasureString(_menuItems[i]);
                spriteBatch.DrawString(
                    _font,
                    _menuItems[i],
                    new Vector2((800 - size.X) / 2, 200 + i * 60),
                    color);
            }

            spriteBatch.End();
        }
    }
}
