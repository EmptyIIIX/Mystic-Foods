using System;
using System.Collections.Generic;
using System.Linq;
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
        private GamePlayScene _gamePlayScene;

        private SpriteFont _font;
        private bool _contentLoaded = false;
        private KeyboardState _oldState;
        private MouseState _oldMouseState;
        private int _selectedIndex = 2;
        private bool Scroll = false;
        private Vector2 emotion = new Vector2(800, 162 / 5);

        Texture2D bg;
        public Vector2 cameraPos = Vector2.Zero;
        private float CameraSpeed = 0f;
        private Vector2 scroll_factor = new Vector2(5.0f, 1);
        private int CameraLeftBoundary2 = 5;
        private int CameraLeftBoundary1 = 100;
        private int CameraRightBoundary1 = 1805;
        private int CameraRightBoundary2 = 1900;

        Texture2D table;
        Texture2D steam2;
        public Button DnD_menuButton;

        private string[] btnItems = { "Serve", "Steam" };// it'll be change
        public static Button _cookingBtn, _OkBtn;
        public static Texture2D CookingBtn;
        public static bool isCountDownSteam = false;
        public bool ServeRequest = false;
        private bool SteamRequest = false;
        private List<Rectangle> btnItemRect = new List<Rectangle>();

        public bool BackToMenuRequested = false;
        public static bool isClickCook = false;
        public bool BackToGame = false;

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
            table = content.Load<Texture2D>("Environments/tools/Table");
            CookingBtn = content.Load<Texture2D>("Etc/CookBtn");
            steam2 = content.Load<Texture2D>("Environments/tools/steamer2");
            //OkBtn = content.Load<Texture2D>("ServeBtn");

            _cookingBtn = new Button(CookingBtn, _font, " ", new Rectangle(2490 - (CookingBtn.Width / 2), 900, 262, 109));
            _cookingBtn.Click += CookingBtn_Click;

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

            DnD_menuButton = new Button(table, _font, " ", new Rectangle(900, 500, 128, 63));
            DnD_menuButton.Click += DnDMenuButton_Click;
        }
        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            Point mousePos = mouse.Position;

            DragDropManager.SetCamera(cameraPos);
            GamePlayScene._pauseButton.Update();
            Globals.Update(gameTime);

            if (GamePlayScene.isPaused)
            {
                #region Stop the game

                DnD_menuButton.Update();

                #endregion
            }
            else
            {
                #region Playing the game

                _gameManager.Update();

                if (isCountDownSteam)
                {
                    GameManager.countSteam -= GamePlayScene.TimePSec;
                    if (GameManager.countSteam <= 0f)
                    {
                        GameManager.countSteam = 0.0f;
                        if (GameManager.readySteam && !GameManager.isChangeFood && _gameManager._food.Any())
                        {
                            var food = _gameManager._food.First();
                            _gameManager.ChangeFood(food);
                            isClickCook = false;
                        }
                    }
                
                }

                if (GamePlayScene.TimeStage <= 0)
                {
                    BackToMenuRequested = true;
                    GamePlayScene.TimeStage = GamePlayScene.TimeDefault;
                }

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

                //TimeStage every scene
                GamePlayScene.TimePSec = 1.0f / 60.0f;
                GamePlayScene.TimeStage -= GamePlayScene.TimePSec;

                // Patience reduce logic
                GamePlayScene._patienceMeter -= GamePlayScene._patienceDecreaseRate * deltaTime;

                //Customer leave
                if (GamePlayScene._patienceMeter <= 0)
                {
                    BackToGame = true;
                }

                if (GameManager.readySteam) _cookingBtn.UpdateStaticBtn(cameraPos);
                #endregion
            }


            #region scroll camera
            // เลื่อนกล้องเมื่อเมาส์อยู่ใกล้ขอบซ้ายหรือขวา
            Scroll = false;
            if (mouse.X <= CameraLeftBoundary2) CameraSpeed = 30f;
            else if (mouse.X <= CameraLeftBoundary1 && mouse.X > CameraLeftBoundary2) CameraSpeed = 10f;

            if (mouse.X >= CameraRightBoundary2) CameraSpeed = 30f;
            else if (mouse.X >= CameraRightBoundary1 && mouse.X < CameraRightBoundary2) CameraSpeed = 10f;

            if (mouse.X <= CameraLeftBoundary1)
            {
                Scroll = true;
                cameraPos.X -= CameraSpeed;
                if (cameraPos.X < 0) cameraPos.X = 0; //จำกัดขอบซ้าย
            }
            else if (mouse.X >= CameraRightBoundary1)
            {
                Scroll = true;
                cameraPos.X += CameraSpeed;

                if (cameraPos.X > 4200 - 1920) cameraPos.X = 4200 - 1920; //จำกัดขอบขวา
            }
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
            spriteBatch.Draw(bg, -cameraPos, Color.White);

            // วาด table ตาม camera
            spriteBatch.Draw(table, new Vector2(304, 143) - cameraPos, Color.White);
            _gameManager.Draw(cameraPos);
            spriteBatch.End();

            spriteBatch.Begin();
            

            // Draw button serve, steam
            if (GameManager.HasFood)
            {
                Color color = (_selectedIndex == 0) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_font, btnItems[0], new Vector2(1700, 900), color);
            }
            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);

                /*
                spriteBatch.DrawString(_font, $"Position mouse : {_mousePosition}", new Vector2(100, 60), Color.White);
                spriteBatch.DrawString(_font, $"SelectIndex : {_selectedIndex}", new Vector2(100, 90), Color.White);
                 */
                spriteBatch.DrawString(_font, $"IdFilling : {GameManager.IdFilling}", new Vector2(500, 500), Color.Blue);
                spriteBatch.DrawString(_font, $"IdDough : {GameManager.IdDough}", new Vector2(500, 530), Color.Blue);
                spriteBatch.DrawString(_font, $"IdFood : {GameManager.IdFood}", new Vector2(500, 560), Color.Blue);
                spriteBatch.DrawString(_font, $"Time steam : {(int)GameManager.countSteam}", new Vector2(500, 590), Color.Blue);
                spriteBatch.DrawString(_font, $"ready to cook steam : {isClickCook}", new Vector2(500, 620), Color.Blue);
                spriteBatch.DrawString(_font, $"Weight : {GamePlayScene.weight}", new Vector2(500, 650), Color.Blue);

            #region UI info

            //profile
            spriteBatch.Draw(GamePlayScene.profile, new Vector2(0, 0), Color.White);
            //Date and Time
            int Days = 1;//สำหรับเปลี่ยนวันตามเงื่อนไขต่างๆที่เราต้องการ
            spriteBatch.Draw(GamePlayScene.dayBox, new Vector2(GamePlayScene.profile.Width + 10, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(GamePlayScene.profile.Width + 110, (GamePlayScene.menuBox.Height / 5) + 20), Color.Black);
            //time
            string Time = $"{(int)GamePlayScene.TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(GamePlayScene.profile.Width + 110, (GamePlayScene.menuBox.Height / 5) + 55), Color.Blue);

            /* สำรองไว้ก่อน
            spriteBatch.Draw(moneyBox, new Vector2(dayBox.Width + 110, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{TotalMoney}", new Vector2(1920 - menuBox.Width - (moneyBox.Width / 2) - 25, (menuBox.Height / 5) + (moneyBox.Height / 4) + 10), Color.Yellow);
            */

            spriteBatch.Draw(GamePlayScene.moneyBox, new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 10, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{GamePlayScene.TotalMoney}", new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + (GamePlayScene.moneyBox.Width / 2) + 35, (GamePlayScene.menuBox.Height / 5) + 20), Color.Yellow);

            Vector2 EmotionPos = new Vector2(GamePlayScene.moneyBox.Width + GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 10, GamePlayScene.menuBox.Height / 5);
            string patienceText = $"{GamePlayScene._patienceMeter:0}%";
            spriteBatch.DrawString(_font, patienceText, new Vector2(EmotionPos.X + (GamePlayScene._happy.Width / 5) + 10, EmotionPos.Y + GamePlayScene._happy.Height), Color.Black);
            GamePlayScene.DrawEmotionIcon(_font, spriteBatch, EmotionPos);

            #endregion
            if (GameManager.readySteam)
            {
                if (GameManager.countSteam > 0 && isClickCook) 
                {
                    spriteBatch.Draw(steam2, new Vector2(2475 - steam2.Width / 2, 460 - steam2.Height / 2) - cameraPos, Color.White);
                }
                _cookingBtn.DrawCooking(spriteBatch, cameraPos);
            }
            if (GamePlayScene.isPaused)
            {
                spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);

                //DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
                spriteBatch.DrawString(_font, "Paused", new Vector2(900, 300), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);


                // should create separate menu button
                DnD_menuButton.Draw(spriteBatch);
            }
            GamePlayScene._pauseButton.Draw(spriteBatch);
            spriteBatch.End();
        }
        public void CookingBtn_Click(object sender, EventArgs e)
        {
            //GameManager.readySteam = true;
            isCountDownSteam = true;
            isClickCook = true;
        }
        public void DnDMenuButton_Click(object sender, EventArgs e)
        {
            //reset Scene
            GamePlayScene._patienceMeter = GamePlayScene._patienceMeterStart;
            GamePlayScene.TimeStage = GamePlayScene.TimeDefault;
            GamePlayScene.isPaused = false;

            BackToMenuRequested = true;
        }
    }
}