using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;

namespace Mystic_Foods
{
    public class DnDScene : IGameScene
    {
        private GameManager _gameManager;

        private SpriteFont _font;
        private bool _contentLoaded = false;
        private KeyboardState _oldState;

        public bool BackToMenuRequested = false;

        public DnDScene(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            if (_contentLoaded) return;
            _font = content.Load<SpriteFont>("MainFont");

            _contentLoaded = true;
            Globals.SpriteBatch = spriteBatch;
        }

        public void Update(GameTime gameTime)
        {
            // ใช้การลากวางตามปกติ
            Globals.Update(gameTime);
            _gameManager.Update();

            // กด ESC เพื่อกลับเมนู
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            {
                BackToMenuRequested = true;
            }
            _oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateGray);

            // วาด UI หรือหัวข้อก็ได้
            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);
            spriteBatch.End();

            // วาด Drag & Drop Object
            spriteBatch.Begin();
            _gameManager.Draw();
            spriteBatch.End();
        }
    }
}