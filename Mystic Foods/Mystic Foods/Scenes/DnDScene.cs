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

        Texture2D bg, ArrowCam;
        private Vector2 scroll_factor = new Vector2(5.0f, 1);
        public static Vector2 cameraPos = Vector2.Zero;
        private float CameraSpeed = 0f;
        private int CameraLeftBoundary2 = 5;
        private int CameraLeftBoundary1 = 100;
        private int CameraRightBoundary1 = 1805;
        private int CameraRightBoundary2 = 1900;

        Texture2D table, table_2;
        Texture2D steam2;
        Texture2D steamBar;
        float currentSteam;
        public Button DnD_menuButton;

        public static Button _cookingBtn, _serveBtn;
        public static Texture2D CookingBtn, ServeBtn;
        public static bool isCountDownSteam = false;
        public bool ServeRequest = false;
        private bool SteamRequest = false;
        private List<Rectangle> btnItemRect = new List<Rectangle>();

        public bool BackToMenuRequested = false;
        public static bool isClickCook = false;
        public bool BackToGame = false;

        public Texture2D boxfilling;
        public Texture2D boxdough;

        public DnDScene(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _gamePlayScene = new GamePlayScene();
            if (_contentLoaded) return;
            bg = content.Load<Texture2D>("Environments/Cooking/CookingMorningBG");
            steam2 = content.Load<Texture2D>("Environments/tools/steamer2 - test");// test
            table = content.Load<Texture2D>("Environments/tools/Table");
            table_2 = content.Load<Texture2D>("Environments/tools/Table_2");
            ArrowCam = content.Load<Texture2D>("Etc/PointArrow");
            CookingBtn = content.Load<Texture2D>("Etc/CookBtn");
            steamBar = content.Load<Texture2D>("Etc/steam_bar");
            ServeBtn = content.Load<Texture2D>("Etc/ServeBtn");
            _font = content.Load<SpriteFont>("MainFont");

            boxdough = content.Load<Texture2D>("foods/hitbox_dough");
            boxfilling= content.Load<Texture2D>("foods/hitbox_filling");

            currentSteam = steamBar.Height - 4;

            _cookingBtn = new Button(CookingBtn, _font, " ", new Rectangle(1800 + (818 / 2) - (CookingBtn.Width / 2), 900, 262, 109));
            _cookingBtn.Click += CookingBtn_Click;

            _serveBtn = new Button(ServeBtn, _font, " ", new Rectangle(3856, 262, 262, 109));
            _serveBtn.Click += ServeBtn_Click;

            DnD_menuButton = new Button(table, _font, " ", new Rectangle(900, 500, 128, 63));
            DnD_menuButton.Click += DnDMenuButton_Click;

            Globals.SpriteBatch = spriteBatch;
            _contentLoaded = true;
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

                if (GameManager.readySteam) _cookingBtn.UpdateStaticBtn(cameraPos);

                if (GameManager.HasFood)
                {
                    _serveBtn.UpdateStaticBtn(cameraPos);
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

                if (currentSteam > 0 && isClickCook)
                {
                    currentSteam -= 3.56f;
                }
                else if (currentSteam <= 0 && isClickCook)
                {
                    currentSteam = 0;
                }
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
            spriteBatch.Draw(table_2, new Vector2(3800 - table_2.Width, 143) - cameraPos, Color.White);
            spriteBatch.Draw(boxdough, new Vector2(250, 250) - cameraPos, Color.White);
            spriteBatch.Draw(boxfilling, new Vector2(500, 250) - cameraPos, Color.White);
            _gameManager.Draw(cameraPos);
            spriteBatch.End();

            spriteBatch.Begin();

            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);

                spriteBatch.DrawString(_font, $"Position mouse : {_mousePosition}", new Vector2(100, 680), Color.Blue);
                /*
                spriteBatch.DrawString(_font, $"SelectIndex : {_selectedIndex}", new Vector2(100, 90), Color.White);
                 */
                spriteBatch.DrawString(_font, $"IdFilling : {GameManager.IdFilling}", new Vector2(500, 500), Color.Blue);
                spriteBatch.DrawString(_font, $"IdDough : {GameManager.IdDough}", new Vector2(500, 530), Color.Blue);
                spriteBatch.DrawString(_font, $"IdFood : {GameManager.IdFood}", new Vector2(500, 560), Color.Blue);
                spriteBatch.DrawString(_font, $"Time steam : {(int)GameManager.countSteam}", new Vector2(500, 590), Color.Blue);
                spriteBatch.DrawString(_font, $"isClickCook : {isClickCook}", new Vector2(500, 620), Color.Blue);
                spriteBatch.DrawString(_font, $"Weight : {GamePlayScene.weight}", new Vector2(500, 650), Color.Blue);

            if (GameManager.readySteam)
            {
                if (GameManager.countSteam > 0 && isClickCook)
                {
                    spriteBatch.Draw(steam2, new Vector2(1805, 85) - cameraPos, Color.White);
                    spriteBatch.Draw(steamBar, new Vector2(2280 + (steam2.Width / 2), 200) - cameraPos, new Rectangle(0, 0, 120, 610), Color.White);
                    spriteBatch.Draw(steamBar, new Rectangle(2280 - (int)cameraPos.X + (steam2.Width / 2), 204 - (int)cameraPos.Y, 120, (int)currentSteam), new Rectangle(120, 4, 120, 606), Color.White);
                }
                else currentSteam = steamBar.Height - 4;

                _cookingBtn.DrawCooking(spriteBatch, cameraPos);
            }

            // Draw button serve, steam
            if (GameManager.HasFood)
            {
                _serveBtn.DrawCooking(spriteBatch, cameraPos);
            }

            #region UI info

            //profile
            spriteBatch.Draw(GamePlayScene.profile, new Vector2(0, 0), Color.White);
            //Date and Time
            int Days = 1;//สำหรับเปลี่ยนวันตามเงื่อนไขต่างๆที่เราต้องการ
            spriteBatch.Draw(GamePlayScene.dayBox, new Vector2(GamePlayScene.profile.Width + 10, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(GamePlayScene.profile.Width + 135, (GamePlayScene.menuBox.Height / 5) + 20), Color.Black);
            //time
            string Time = $"{(int)GamePlayScene.TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(GamePlayScene.profile.Width + 145, (GamePlayScene.menuBox.Height / 5) + 55), Color.Blue);

            /* สำรองไว้ก่อน
            spriteBatch.Draw(moneyBox, new Vector2(dayBox.Width + 110, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{TotalMoney}", new Vector2(1920 - menuBox.Width - (moneyBox.Width / 2) - 25, (menuBox.Height / 5) + (moneyBox.Height / 4) + 10), Color.Yellow);
            */

            spriteBatch.Draw(GamePlayScene.moneyBox, new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 10, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{GamePlayScene.TotalMoney}", new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + (GamePlayScene.moneyBox.Width / 2) + 35, (GamePlayScene.menuBox.Height / 5) + 36), Color.Yellow);

            Vector2 EmotionPos = new Vector2(GamePlayScene.moneyBox.Width + GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 10, GamePlayScene.menuBox.Height / 5);
            Vector2 percentPantiencePos = new Vector2(EmotionPos.X + 145, GamePlayScene.menuBox.Height / 5 + 36);

            string patienceText = $"{GamePlayScene._patienceMeter:0}%";
            spriteBatch.DrawString(_font, patienceText, percentPantiencePos, Color.Yellow);
            GamePlayScene.DrawEmotionIcon(_font, spriteBatch, EmotionPos);

            #endregion

            if (GamePlayScene.isPaused)
            {
                spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);

                //DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
                spriteBatch.DrawString(_font, "Paused", new Vector2(900, 300), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);


                // should create separate menu button
                DnD_menuButton.Draw(spriteBatch);
            }
            GamePlayScene._pauseButton.Draw(spriteBatch);

            if (cameraPos.X < 4200 - 1920) spriteBatch.Draw(ArrowCam, new Vector2(1920 - ArrowCam.Width, 540), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0f);//ทางขวาของจอ
            if (cameraPos.X > 0) spriteBatch.Draw(ArrowCam, new Vector2(0, 540), Color.White);//ทางซ้ายของจอ
            spriteBatch.End();
        }
        public void CookingBtn_Click(object sender, EventArgs e)
        {
            //GameManager.readySteam = true;
            isCountDownSteam = true;
            isClickCook = true;
        }
        public void ServeBtn_Click(object sender, EventArgs e)
        {
            if (GameManager.HasFood)
            {
                ServeRequest = true;
                _gameManager.ServeFood();
            }
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