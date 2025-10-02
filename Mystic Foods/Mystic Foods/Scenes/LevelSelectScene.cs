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

namespace Mystic_Foods.Scenes
{
    public class LevelSelectScene : IGameScene
    {
        private GraphicsDeviceManager _graphics;
        private SpriteFont _font;
        private Texture2D _buttonTexture;

        public bool MenuRequest = false;
        public bool gameplayRequest = false;

        Texture2D bg;
        Button dayButton, duskButton, nightButton;

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _font = content.Load<SpriteFont>("MainFont");

            bg = content.Load<Texture2D>("Environments/BG/MenuBG");

            _buttonTexture = content.Load<Texture2D>("Etc/HomeBtn");

            dayButton = new Button(_buttonTexture, _font, "Day", new Rectangle(100, 200, 200, 60));
            dayButton.Click += DayButton_Click;
            duskButton = new Button(_buttonTexture, _font, "Dusk", new Rectangle(100, 280, 200, 60));
            duskButton.Click += DuskButton_Click;
            nightButton = new Button(_buttonTexture, _font, "Night", new Rectangle(100, 360, 200, 60));
            nightButton.Click += NightButton_Click;
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            dayButton.Update();
            duskButton.Update();
            nightButton.Update();

            // กด Esc เพื่อกลับเมนู
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MenuRequest = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);
            dayButton.Draw(spriteBatch);
            duskButton.Draw(spriteBatch);
            nightButton.Draw(spriteBatch);

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
    }
}
