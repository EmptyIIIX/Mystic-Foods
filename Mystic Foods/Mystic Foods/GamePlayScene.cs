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
    public class GamePlayScene : IGameScene
    {
        private SpriteFont _font;
        public bool BackToMenuRequested = false;
        private KeyboardState _oldState;

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("MainFont");
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
                BackToMenuRequested = true;
            _oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSeaGreen);
            spriteBatch.Begin();

            string text = "Game Scene!\nPress ESC to return to the menu";
            Vector2 size = _font.MeasureString(text);
            spriteBatch.DrawString(
                _font,
                text,
                new Vector2((800 - size.X) / 2, 200), //800- size.X /2 make it in the center of screen
                Color.Black);

            spriteBatch.End();
        }
    }
}
