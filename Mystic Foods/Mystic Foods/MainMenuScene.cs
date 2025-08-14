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
        private SpriteFont _font; //font use to draw string
        private int _selectedIndex = 0;
        private string[] _menuItems = { "Start Game", "Exit" }; //selectable text
        public bool StartGameRequested = false; //check if start game
        public bool ExitRequested = false; //check if exit game
        private KeyboardState _oldState; //make it only pressable (can't hold)

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("MainFont");
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Down) && _oldState.IsKeyUp(Keys.Down))
                _selectedIndex = (_selectedIndex + 1) % _menuItems.Length;

            if (state.IsKeyDown(Keys.Up) && _oldState.IsKeyUp(Keys.Up))
                _selectedIndex = (_selectedIndex - 1 + _menuItems.Length) % _menuItems.Length;

            //game scene selection logic
            if (state.IsKeyDown(Keys.Enter) && _oldState.IsKeyUp(Keys.Enter))
            {
                if (_selectedIndex == 0) StartGameRequested = true;
                if (_selectedIndex == 1) ExitRequested = true;
            }

            _oldState = state; //update keyboard status
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateBlue);

            spriteBatch.Begin();

            string title = "Main Menu";
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
