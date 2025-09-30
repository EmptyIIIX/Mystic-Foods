using Mystic_Foods.Systems; // เรียกใช้ Button
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Mystic_Foods
{
    public class MainMenuScene : IGameScene
    {
        private GraphicsDeviceManager _graphics;
        private SpriteFont _font;
        private Texture2D _buttonTexture;
        private Texture2D _title;
        private Texture2D _menuBg;

        // ปุ่ม
        private List<Button> _buttons = new List<Button>();

        // flags
        public bool StartGameRequested = false;
        public bool DnDRequested = false;
        public bool ExitRequested = false;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");
            _menuBg = content.Load<Texture2D>("Environments/BG/MenuBG");
            _title = content.Load<Texture2D>("UI/title");

            // โหลด texture สำหรับปุ่ม (ใส่สี่เหลี่ยมธรรมดาหรือ UI ปุ่มจริงก็ได้)
            _buttonTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _buttonTexture.SetData(new[] { Color.White }); // ปุ่มพื้นสีขาว

            // สร้างปุ่ม
            #region button
            _buttons.Clear();

            var startBtn = new Button(_buttonTexture, _font, "Start Game",
                new Rectangle(300, 200, 200, 50));
            startBtn.Click += (s, e) => StartGameRequested = true;

            var dndBtn = new Button(_buttonTexture, _font, "Drag&Drop",
                new Rectangle(300, 260, 200, 50));
            dndBtn.Click += (s, e) => DnDRequested = true;

            var exitBtn = new Button(_buttonTexture, _font, "Exit",
                new Rectangle(300, 320, 200, 50));
            exitBtn.Click += (s, e) => ExitRequested = true;

            _buttons.Add(startBtn);
            _buttons.Add(dndBtn);
            _buttons.Add(exitBtn);
            #endregion
        }

        public void Update(GameTime gameTime)
        {
            foreach (var btn in _buttons)
                btn.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(_menuBg, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(_title, new Vector2(0, 0), Color.White);

            foreach (var btn in _buttons)
                btn.Draw(spriteBatch);

            spriteBatch.End();
        }
    }
}
