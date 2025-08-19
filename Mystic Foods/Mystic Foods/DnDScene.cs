using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Mystic_Foods
{
    public class DnDScene : IGameScene
    {
        private Drag_DropManager _dragDropManager;
        private SpriteFont _font;
        private bool _contentLoaded = false;
        private KeyboardState _oldState;

        public bool BackToMenuRequested = false;

        public DnDScene(Drag_DropManager dragDropManager)
        {
            _dragDropManager = dragDropManager;
        }

        public void LoadContent(ContentManager content)
        {
            if (_contentLoaded) return;
            _font = content.Load<SpriteFont>("MainFont");
            // ใส่ texture/sprite ต่างๆ ตามที่ต้องการ (หรือ load ที่ Drag_DropManager)
            _contentLoaded = true;
        }

        public void Update(GameTime gameTime)
        {
            // ใช้การลากวางตามปกติ
            _dragDropManager.Update(gameTime);

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

            spriteBatch.Begin();
            // วาด UI หรือหัวข้อก็ได้
            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);
            spriteBatch.End();

            // วาด Drag & Drop Object
            spriteBatch.Begin();
            _dragDropManager.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}