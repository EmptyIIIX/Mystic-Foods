using System.Collections.Generic;
using System.Diagnostics.Metrics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mystic_Foods
{
    public class DnDScene : IGameScene
    {
        public GraphicsDeviceManager _graphics;
        private GameManager _gameManager;
        private GamePlayScene _gamePlayScene;

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
            _gamePlayScene = new GamePlayScene();
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

            GamePlayScene.TimePSec = 1.0f / 60.0f;
            GamePlayScene.TimeStage -= GamePlayScene.TimePSec;

            if (GamePlayScene.TimeStage <= 0)
            {
                BackToMenuRequested = true;
                GamePlayScene.TimeStage = 721f;
            }

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
            if (GameManager.HasFood)
            {
                if (btnItemRect[0].Contains(mousePos))
                {
                    _selectedIndex = 0;
                }
                else
                {
                    _selectedIndex = 2;
                }
            }
            else
            {
                _selectedIndex = 2;
            }

            // ตรวจสอบการคลิกปุ่ม Serve เฉพาะเมื่อมีอาหาร
            if (mouse.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            {
                if (GameManager.HasFood && btnItemRect[0].Contains(mouse.Position))
                {
                    ServeRequest = true;
                    _gameManager.ServeFood(); // รีเซ็ตอาหารและสถานะ
                }
            }
            #region btn test
            //for (int i = 0; i < btnItemRect.Count; i++)
            //{
            //    if (btnItemRect[0].Contains(mousePos))
            //    {
            //        _selectedIndex = 0;
            //    }
            //    else if (btnItemRect[1].Contains(mousePos))
            //    {
            //        _selectedIndex = 1;
            //    }
            //    else _selectedIndex = 2;
            //}

            ////คลิกปุ่ม Serve or steam
            //if (mouse.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
            //{
            //    for (int i = 0; i < btnItemRect.Count; i++)
            //    {
            //        if (btnItemRect[i].Contains(mouse.Position))
            //        {
            //            if (i == 0) 
            //            { 
            //                ServeRequest = true; 

            //            }
            //            if (i == 1) 
            //            { 
            //                SteamRequest = true; 
            //            }
            //        }
            //    }
            //}
            #endregion
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
            spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(table, new Vector2(100, 100), Color.White);
            _gameManager.Draw();

            // Draw button serve, steam
            if (GameManager.HasFood)
            {
                Color color = (_selectedIndex == 0) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, btnItems[0], new Vector2(1700, 900), color);
            }
            // ปุ่ม Steam (ถ้ายังคงต้องการให้แสดงเสมอ)
            //Color steamColor = (_selectedIndex == 1) ? Color.Yellow : Color.White;
            //spriteBatch.DrawString(_font, btnItems[1], new Vector2(0, 0), steamColor);
            spriteBatch.End();

            spriteBatch.Begin();
            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);

                /*
                spriteBatch.DrawString(_font, $"Position mouse : {_mousePosition}", new Vector2(100, 60), Color.White);
                spriteBatch.DrawString(_font, $"SelectIndex : {_selectedIndex}", new Vector2(100, 90), Color.White);
                spriteBatch.DrawString(_font, $"IdFilling : {GameManager.IdFilling}", new Vector2(500, 500), Color.Blue);
                spriteBatch.DrawString(_font, $"IdDough : {GameManager.IdDough}", new Vector2(500, 530), Color.Blue);
                spriteBatch.DrawString(_font, $"IdFood : {GameManager.IdFood}", new Vector2(500, 560), Color.Blue);
                 */
                #region UI info

                //Date and Time
                int Days = 1;//สำหรับเปลี่ยนวันตามเงื่อนไขต่างๆที่เราต้องการ
            spriteBatch.Draw(GamePlayScene.dayBox, new Vector2(GamePlayScene.profile.Width + 10, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(GamePlayScene.profile.Width + 110, (GamePlayScene.menuBox.Height / 5) + 20), Color.Black);

            spriteBatch.Draw(GamePlayScene.moneyBox, new Vector2(1920 - GamePlayScene.menuBox.Width - GamePlayScene.moneyBox.Width - 50, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{GamePlayScene.TotalMoney}", new Vector2(1920 - GamePlayScene.menuBox.Width - (GamePlayScene.moneyBox.Width / 2) - 25, (GamePlayScene.menuBox.Height / 5) + (GamePlayScene.moneyBox.Height / 4) + 10), Color.Yellow);
            //table pos recom pos.Y 890++
            spriteBatch.Draw(GamePlayScene.counter, new Vector2(0, 1080 - GamePlayScene.counter.Height), Color.White);

            string Time = $"{(int)GamePlayScene.TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(GamePlayScene.profile.Width + 110, (GamePlayScene.menuBox.Height / 5) + 55), Color.Blue);
            #endregion
            if (GamePlayScene.isPaused)
            {
                spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);

                //DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
                spriteBatch.DrawString(_font, "Paused", new Vector2(900, 300), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                GamePlayScene._menuButton.Draw(spriteBatch);
            }
            GamePlayScene._pauseButton.Draw(spriteBatch);
            spriteBatch.End();
        }
    }
}