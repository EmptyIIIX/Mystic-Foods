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
        public GraphicsDeviceManager _graphics;
        private GameManager _gameManager;

        private SpriteFont _font;
        private bool _contentLoaded = false;
        private KeyboardState _oldState;

        private bool Scroll = false;

        Texture2D bg;
        //private Vector2 cameraPos = new Vector2();
        //private Vector2 scroll_factor = new Vector2(5.0f, 1);
        //private int CameraLeftBoundary = 5;
        //private int CameraRightBoundary = 1900;

        Texture2D table;

        public bool BackToMenuRequested = false;

        public DnDScene(GameManager gameManager)
        {
            _gameManager = gameManager;
        }


        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            if (_contentLoaded) return;
            _font = content.Load<SpriteFont>("MainFont");
            bg = content.Load<Texture2D>("Environments/bg_morning");
            table = content.Load<Texture2D>("Environments/Table");
            _contentLoaded = true;
            Globals.SpriteBatch = spriteBatch;
        }

        public void Update(GameTime gameTime)
        {
            // ใช้การลากวางตามปกติ
            Globals.Update(gameTime);
            _gameManager.Update();

            MouseState _mousePosition = Mouse.GetState();

            //Scroll = false;
            //if (_mousePosition.X <= CameraLeftBoundary)
            //{
            //    Scroll = true;
            //    cameraPos.X -= 5 * scroll_factor.X;
            //    if (cameraPos.X < 0) cameraPos.X = 0;
            //}
            //else if (_mousePosition.X >= CameraRightBoundary)
            //{
            //    Scroll = true;
            //    cameraPos.X += 5 * scroll_factor.X;
            //    //จำกัดไม่ให้กล้องเลื่อนเกินขอบขวาของภาพ (4200 - 1920)
            //    if (cameraPos.X > 4200 - 1920) cameraPos.X = 4200 - 1920;
            //}

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
            MouseState _mousePosition = Mouse.GetState();
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateGray);

            spriteBatch.Begin();
            //spriteBatch.Draw(bg, -cameraPos, new Rectangle(0, 0, 4200, 1080), Color.White);
            spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(table, new Vector2(100, 100), Color.White);
            _gameManager.Draw();
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);
            spriteBatch.DrawString(_font, $"Position mouse : {_mousePosition}", new Vector2(100, 60), Color.White);
            //spriteBatch.DrawString(_font, $"scrolling : {Scroll}", new Vector2(100, 90), Color.White);
            //spriteBatch.DrawString(_font, $"Scroll Factor : {scroll_factor.X}", new Vector2(100, 120), Color.White);
            spriteBatch.End();
        }
    }
}