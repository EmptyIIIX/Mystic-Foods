using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Mystic_Foods.Scenes
{
    public class LevelSelectScene : IGameScene
    {
        private GraphicsDeviceManager _graphics;
        private SpriteFont _font;
        private Texture2D dayBtn, duskBtn, nightBtn, homeBtn;

        public bool MenuRequest = false;
        public bool gameplayRequest = false;

        Texture2D bg;
        Button dayButton, duskButton, nightButton, homeButton;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");

            bg = content.Load<Texture2D>("Environments/BG/MenuBG");

            homeBtn = content.Load<Texture2D>("Etc/option_exit");
            dayBtn = content.Load<Texture2D>("Etc/dayBtn");
            duskBtn = content.Load<Texture2D>("Etc/duskBtn");
            nightBtn = content.Load<Texture2D>("Etc/nightBtn");

            dayButton = new Button(dayBtn, _font, "", new Rectangle(240, 300, 360, 640));
            dayButton.Click += DayButton_Click;
            duskButton = new Button(duskBtn, _font, "", new Rectangle(780, 300, 360, 640));
            duskButton.Click += DuskButton_Click;
            nightButton = new Button(nightBtn, _font, "", new Rectangle(1320, 300, 360, 640));
            nightButton.Click += NightButton_Click;
            homeButton = new Button(homeBtn, _font, "", new Rectangle(50, 50, 80, 100));
            homeButton.Click += HomeButton_Click;
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            dayButton.Update();
            duskButton.Update();
            nightButton.Update();
            homeButton.Update();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MenuRequest = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // ใช้ GraphicsDevice จาก spriteBatch เพื่อ clear หน้าจอ
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);

            string text = "Choose the opening hours";
            float scale = 3.0f;

            // วัดขนาดข้อความหลัง scale
            Vector2 textSize = _font.MeasureString(text) * scale;

            // ใช้ GraphicsDevice จาก spriteBatch
            int screenWidth = spriteBatch.GraphicsDevice.Viewport.Width;
            int screenHeight = spriteBatch.GraphicsDevice.Viewport.Height;

            // คำนวณตำแหน่งให้อยู่กลางจอ
            Vector2 position = new Vector2((screenWidth - textSize.X) / 2f,100);

            spriteBatch.Begin();
            spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);

            spriteBatch.DrawString(_font, text, position, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

            dayButton.Draw(spriteBatch);
            duskButton.Draw(spriteBatch);
            nightButton.Draw(spriteBatch);
            homeButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DayButton_Click(object sender, EventArgs e)
        {
            gameplayRequest = true;
            GamePlayScene.CurrentPhase = GamePlayScene.DayPhase.Dawn;
        }

        private void DuskButton_Click(object sender, EventArgs e)
        {
            gameplayRequest = true;
            GamePlayScene.CurrentPhase = GamePlayScene.DayPhase.Dusk;
        }

        private void NightButton_Click(object sender, EventArgs e)
        {
            gameplayRequest = true;
            GamePlayScene.CurrentPhase = GamePlayScene.DayPhase.Night;
        }
        public void HomeButton_Click(object sender, EventArgs e)
        {
            MenuRequest = true;
        }
    }
}
