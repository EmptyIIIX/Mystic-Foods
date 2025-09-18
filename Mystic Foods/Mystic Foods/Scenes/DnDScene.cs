using System.Collections.Generic;
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
        private MouseState _oldMouseState;
        private int _selectedIndex = 2;

        private bool Scroll = false;

        Texture2D bg;
        //private Vector2 cameraPos = new Vector2();
        //private Vector2 scroll_factor = new Vector2(5.0f, 1);
        //private int CameraLeftBoundary = 5;
        //private int CameraRightBoundary = 1900;

        Texture2D table;

        private string[] btnItems = { "Serve", "Steam" };
        public bool ServeRequest = false;
        private bool SteamRequest = false;
        private List<Rectangle> btnItemRect = new List<Rectangle>();

        public bool BackToMenuRequested = false;

        public DnDScene(GameManager gameManager)
        {
            _gameManager = gameManager;
        }


        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            if (_contentLoaded) return;
            _font = content.Load<SpriteFont>("MainFont");
            bg = content.Load<Texture2D>("Environments/Cooking/CookingMorningBG");
            table = content.Load<Texture2D>("Environments/Table");
            _contentLoaded = true;
            Globals.SpriteBatch = spriteBatch;

            btnItemRect.Clear();
            //Rectangle for "Serve" button
            Vector2 size0 = _font.MeasureString(btnItems[0]);
            Rectangle rect0 = new Rectangle(
                //(int)((1920 / 2) - btnItems[0].Length),
                1700,
                900,
                (int)size0.X,
                (int)size0.Y
            );
            btnItemRect.Add(rect0);

            //Rectangle for "Steam" button
            Vector2 size1 = _font.MeasureString(btnItems[1]);
            Rectangle rect1 = new Rectangle(
                0,
                0,
                (int)size1.X,
                (int)size1.Y
            );
            btnItemRect.Add(rect1);

        }

        public void Update(GameTime gameTime)
        {
            // ใช้การลากวางตามปกติ
            Globals.Update(gameTime);
            _gameManager.Update();

            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();
            #region scroll camera
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
            #endregion

            Point mousePos = mouse.Position;
            for (int i = 0; i < btnItemRect.Count; i++)
            {
                if (btnItemRect[0].Contains(mousePos))
                {
                    _selectedIndex = 0;
                }
                else if (btnItemRect[1].Contains(mousePos))
                {
                    _selectedIndex = 1;
                }
                else _selectedIndex = 2;
            }

            //คลิกปุ่ม Serve or steam
            if (mouse.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                for (int i = 0; i < btnItemRect.Count; i++)
                {
                    if (btnItemRect[i].Contains(mouse.Position))
                    {
                        if (i == 0) 
                        { 
                            ServeRequest = true; 
                            
                        }
                        if (i == 1) 
                        { 
                            SteamRequest = true; 
                        }
                    }
                }
            }

            // กด ESC เพื่อกลับเมนู
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            {
                BackToMenuRequested = true;
            }
            _oldState = state;
            _oldMouseState = mouse;
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

            //Draw button serve, steam
            for (int i = 0; i < btnItems.Length; i++)
            {
                Color color = (i == _selectedIndex) ? Color.Yellow : Color.White;
                if (i == 0)
                {
                    //spriteBatch.DrawString(_font, btnItems[0], new Vector2((1920 / 2) - btnItems[0].Length, 900), color);
                    spriteBatch.DrawString(_font, btnItems[0], new Vector2(1700, 900), color);
                }
                else if(i == 1)
                {
                    spriteBatch.DrawString(_font, btnItems[1], new Vector2(0, 0), color);
                }
            }
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);
            spriteBatch.DrawString(_font, $"Position mouse : {_mousePosition}", new Vector2(100, 60), Color.White);
            spriteBatch.DrawString(_font, $"SelectIndex : {_selectedIndex}", new Vector2(100, 90), Color.White);//สำหรับดู _selectedIndex เพื่อเช็คสถานะของhoverปุ่ม
            //spriteBatch.DrawString(_font, $"scrolling : {Scroll}", new Vector2(100, 90), Color.White);
            //spriteBatch.DrawString(_font, $"Scroll Factor : {scroll_factor.X}", new Vector2(100, 120), Color.White);
            spriteBatch.End();
        }
    }
}