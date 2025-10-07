using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Mystic_Foods.GamePlayScene;

namespace Mystic_Foods
{
    public class DnDScene : IGameScene
    {
        public GraphicsDeviceManager _graphics;
        private GameManager _gameManager;
        private GamePlayScene _gamePlayScene;

        public bool backToCounter = false;

        private SpriteFont _font;
        private bool _contentLoaded = false;
        private KeyboardState _oldState;
        private MouseState _oldMouseState;
        private int _selectedIndex = 2;
        private bool Scroll = false;
        private Vector2 emotion = new Vector2(800, 162 / 5);

        Texture2D bgDawn, bgDusk, bgNight, ArrowCam;
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

        Texture2D LogOrder, LogInfo, Oklog, prevTexture;
        public Button _logOrderBtn;
        public static Button _okLogBtn, _prevBtn;
        public bool isLog = false;

        public static Button _cookingBtn, _serveBtn;
        public static Texture2D CookingBtn, ServeBtn;
        public static bool isCountDownSteam = false;
        public bool ServeRequest = false;
        private bool SteamRequest = false;
        private List<Rectangle> btnItemRect = new List<Rectangle>();

        public static bool isClickCook = false;
        public bool BackToGame = false;

        public static Texture2D boxfilling;
        public static Texture2D boxdough;

        private List<Rectangle> _boxfilling = new List<Rectangle>();
        private List<Rectangle> _boxdough = new List<Rectangle>();

        #region Setting
        private Texture2D header, settingBG;
        private Texture2D musicIcon, muteMusicIcon;
        private Texture2D sfxIcon, muteSfxIcon;
        private Texture2D barBg, barFill, knob;
        private Vector2 musicBarPos = new Vector2(600, 425);
        private Vector2 sfxBarPos = new Vector2(600, 675);
        private const float barScale = 1.0f;

        private bool _draggingMusic = false;
        private bool _draggingSfx = false;
        #endregion
        public DnDScene(GameManager gameManager)
        {
            _gameManager = gameManager;
        }
        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _gamePlayScene = new GamePlayScene();
            if (_contentLoaded) return;

            _font = content.Load<SpriteFont>("MainFont");

            bgDawn = content.Load<Texture2D>("Environments/Cooking/CookingMorningBG");
            bgDusk = content.Load<Texture2D>("Environments/Cooking/CookingSunsetBG");
            bgNight = content.Load<Texture2D>("Environments/Cooking/CookingMidnightBG");

            steam2 = content.Load<Texture2D>("Environments/tools/steamer2 - test");// test
            table = content.Load<Texture2D>("Environments/tools/Table");
            table_2 = content.Load<Texture2D>("Environments/tools/Table_2");
            ArrowCam = content.Load<Texture2D>("Etc/PointArrow");
            CookingBtn = content.Load<Texture2D>("Etc/CookBtn");
            steamBar = content.Load<Texture2D>("Etc/steam_bar");
            ServeBtn = content.Load<Texture2D>("Etc/ServeBtn");

            boxdough = content.Load<Texture2D>("foods/hitbox_dough");
            boxfilling = content.Load<Texture2D>("foods/hitbox_filling");
            prevTexture = content.Load<Texture2D>("UI/previous2");

            LogOrder = content.Load<Texture2D>("DialogueUI/LogButton");
            LogInfo = content.Load<Texture2D>("DialogueUI/LogInformation");
            Oklog = content.Load<Texture2D>("DialogueUI/okLog");

            _boxfilling.Add(new Rectangle(759, 218, 283, 154));
            _boxfilling.Add(new Rectangle(759 + boxfilling.Width + 16, 218, 283, 154));
            _boxfilling.Add(new Rectangle(759 + 2 * (boxfilling.Width + 16), 218, 283, 154));
            foreach (var rect in _boxfilling) DragDropManager.AddHitbox(rect);

            _boxdough.Add(new Rectangle(371, 215, 280, 183));
            _boxdough.Add(new Rectangle(371, 215 + boxdough.Height + 12, 280, 183));
            _boxdough.Add(new Rectangle(371, 215 + 2 * (boxdough.Height + 12), 280, 183));
            foreach(var rect in _boxdough) DragDropManager.AddHitbox(rect);

            currentSteam = steamBar.Height - 4;

            _cookingBtn = new Button(CookingBtn, CookingBtn, _font, " ", new Rectangle(1800 + (818 / 2) - (CookingBtn.Width / 2), 900, 262, 109));
            _cookingBtn.Click += CookingBtn_Click;

            _serveBtn = new Button(ServeBtn, ServeBtn, _font, " ", new Rectangle(3856, 262, 262, 109));
            _serveBtn.Click += ServeBtn_Click;

            _logOrderBtn = new Button(LogOrder, LogOrder, _font, "", new Rectangle(1200, 0, 94, 134));
            _logOrderBtn.Click += LogBtn_Click;

            _okLogBtn = new Button(Oklog, Oklog, _font, "", new Rectangle(1450, 840, 300, 150));
            _okLogBtn.Click += okLogBtn_Click;

            _prevBtn = new Button(prevTexture, prevTexture, _font, "", new Rectangle(0, GamePlayScene.menuBox.Height / 5, prevTexture.Width, prevTexture.Height));
            _prevBtn.Click += previousButton_Click;

            #region setting
            header = content.Load<Texture2D>("UI/setting/Setting_Word");
            settingBG = content.Load<Texture2D>("UI/setting/Setting_BG");

            musicIcon = content.Load<Texture2D>("UI/setting/Music_UI");
            muteMusicIcon = content.Load<Texture2D>("UI/setting/MuteSong");
            sfxIcon = content.Load<Texture2D>("UI/setting/SoundEffect");
            muteSfxIcon = content.Load<Texture2D>("UI/setting/MuteSoundEffect");

            barBg = content.Load<Texture2D>("UI/setting/IncreaseSound_BG_UI");
            barFill = content.Load<Texture2D>("UI/setting/IncreaseSound_UI");
            knob = content.Load<Texture2D>("UI/setting/SoundButton");
            #endregion

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
            GamePlayScene._menuButton.Update();
            Globals.Update(gameTime);

            if (GamePlayScene.TimeStage <= 0)
            {
                //_gamePlayScene.BackToMenuRequested = true;
                GamePlayScene.TimeStage = GamePlayScene.TimeDefault;
                GamePlayScene.isEndLv = true;
            }

            if (GamePlayScene.isPaused)
            {
                #region Stopping the game

                GamePlayScene._homeButton.Update();
                GamePlayScene._exitButton.Update();
                //GamePlayScene._resumeButton.Update();
                GamePlayScene._settingButton.Update();
                if (GamePlayScene.isClickExit)
                {
                    GamePlayScene._yesExit.Update();
                    GamePlayScene._noExit.Update();
                }
                if (GamePlayScene.showSettings == true)
                {
                    Vector2 musicBarPos = new Vector2(600, 425);
                    Vector2 sfxBarPos = new Vector2(600, 675);

                    float mx = mouse.X;
                    float my = mouse.Y;

                    Rectangle musicBarRect = new Rectangle((int)musicBarPos.X, (int)musicBarPos.Y, barBg.Width, barBg.Height);
                    Rectangle sfxBarRect = new Rectangle((int)sfxBarPos.X, (int)sfxBarPos.Y, barBg.Width, barBg.Height);

                    // --- Mouse Drag Volume ---
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        if (!_draggingMusic && !_draggingSfx)
                        {
                            if (musicBarRect.Contains(mx, my))
                                _draggingMusic = true;
                            else if (sfxBarRect.Contains(mx, my))
                                _draggingSfx = true;
                        }
                    }
                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        _draggingMusic = false;
                        _draggingSfx = false;
                    }

                    if (_draggingMusic)
                    {
                        float newVol = MathHelper.Clamp((mx - musicBarRect.X) / (float)musicBarRect.Width, 0f, 1f);
                        SoundManager.SetMusicVolume(newVol);
                    }
                    else if (_draggingSfx)
                    {
                        float newVol = MathHelper.Clamp((mx - sfxBarRect.X) / (float)sfxBarRect.Width, 0f, 1f);
                        SoundManager.SetSfxVolume(newVol);
                    }
                }
                else
                {
                    _resumeButton.Update();
                }
                #endregion
            }
            else if (GamePlayScene.isEndLv == false) 
            {
                #region Playing the game

                //TimeStage every scene
                GamePlayScene.TimePSec = 1.0f / 60.0f;
                GamePlayScene.TimeStage -= GamePlayScene.TimePSec;

                _gameManager.Update();
                _prevBtn.Update();

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

                if (GameManager.readySteam) _cookingBtn.UpdateStaticBtn(cameraPos);

                // ตรวจสอบการคลิกปุ่ม Serve เฉพาะเมื่อมีอาหาร
                if (GameManager.HasFood)
                {
                    _serveBtn.UpdateStaticBtn(cameraPos);
                }

                // Patience reduce logic
                GamePlayScene._patienceMeter -= GamePlayScene._patienceDecreaseRate * deltaTime;

                //Customer leave
                if (GamePlayScene._patienceMeter <= 0)
                {
                    GameManager.countDia = 2;
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
            else if (GamePlayScene.isEndLv)
            {
                GamePlayScene._OkButton.Update();
            }

            // ESC to pause
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            {
                GamePlayScene.isPaused = !GamePlayScene.isPaused;
                GamePlayScene.isClickExit = false;
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
                _gamePlayScene.BackToMenuRequested = true;
            }

            _logOrderBtn.Update();
            _okLogBtn.Update();

            _oldState = state;
            _oldMouseState = mouse;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            MouseState _mousePosition = Mouse.GetState();
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateGray);

            spriteBatch.Begin();
            #region Background
            //spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);

            Texture2D background = bgDawn;

            switch (CurrentPhase)
            {
                case DayPhase.Dawn:
                    background = bgDawn;
                    break;
                case DayPhase.Dusk:
                    background = bgDusk;
                    break;
                case DayPhase.Night:
                    background = bgNight;
                    break;
            }

            spriteBatch.Draw(background, -cameraPos, Color.White);
            //spriteBatch.Draw(bgBox, new Vector2(0, 0), Color.White*0.5f);
            #endregion

            // วาด table ตาม camera
            spriteBatch.Draw(table, new Vector2(304, 143) - cameraPos, Color.White);
            spriteBatch.Draw(table_2, new Vector2(3800 - table_2.Width, 143) - cameraPos, Color.White);
            // วาด filling hitboxes → worldPos - cameraPos
            foreach (var rect in _boxfilling)
            {
                var drawRect = new Rectangle(
                    rect.X - (int)cameraPos.X,
                    rect.Y - (int)cameraPos.Y,
                    rect.Width,
                    rect.Height
                );
                spriteBatch.Draw(boxfilling, drawRect, Color.Transparent);
            }

            // วาด dough hitboxes → worldPos - cameraPos
            foreach (var rect in _boxdough)
            {
                var drawRect = new Rectangle(
                    rect.X - (int)cameraPos.X,
                    rect.Y - (int)cameraPos.Y,
                    rect.Width,
                    rect.Height
                );
                spriteBatch.Draw(boxdough, drawRect, Color.Transparent);
            }

            _gameManager.Draw(cameraPos);
            spriteBatch.End();

            spriteBatch.Begin();

            /*
            spriteBatch.DrawString(_font, "Drag & Drop Mode (Press ESC to Main Menu)", new Vector2(100, 30), Color.White);
            spriteBatch.DrawString(_font, $"Position mouse : {_mousePosition}", new Vector2(100, 680), Color.Blue);
            spriteBatch.DrawString(_font, $"SelectIndex : {_selectedIndex}", new Vector2(100, 90), Color.White);
            spriteBatch.DrawString(_font, $"Time steam : {(int)GameManager.countSteam}", new Vector2(500, 590), Color.Blue);
            spriteBatch.DrawString(_font, $"Weight : {GamePlayScene.weight}", new Vector2(500, 650), Color.Blue);
            spriteBatch.DrawString(_font, $"CounDia : {GameManager.countDia}", new Vector2(500, 590), Color.Blue);
            spriteBatch.DrawString(_font, $"isClickCook : {isClickCook}", new Vector2(500, 620), Color.Blue);
            spriteBatch.DrawString(_font, $"IdFilling : {GameManager.IdFilling}", new Vector2(500, 500), Color.Blue);
            spriteBatch.DrawString(_font, $"IdDough : {GameManager.IdDough}", new Vector2(500, 530), Color.Blue);
            spriteBatch.DrawString(_font, $"IdFood : {GameManager.IdFood}", new Vector2(500, 560), Color.Blue);
            spriteBatch.DrawString(_font, $"IdFlower : {GameManager.IdFlower}", new Vector2(500, 590), Color.Blue);
             */

            if (GameManager.readySteam)
            {
                if (GameManager.countSteam > 0 && isClickCook)
                {
                    spriteBatch.Draw(steam2, new Vector2(1805, 145) - cameraPos, Color.White);
                    spriteBatch.Draw(steamBar, new Vector2(2200 + (steam2.Width / 2), 200) - cameraPos, new Rectangle(0, 0, 120, 610), Color.White);
                    spriteBatch.Draw(steamBar, new Rectangle(2200 - (int)cameraPos.X + (steam2.Width / 2), 204 - (int)cameraPos.Y, 120, (int)currentSteam), new Rectangle(120, 4, 120, 606), Color.White);
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

            //Date and Time
            int Days = 1;//สำหรับเปลี่ยนวันตามเงื่อนไขต่างๆที่เราต้องการ
            spriteBatch.Draw(GamePlayScene.dayBox, new Vector2(GamePlayScene.profile.Width + 30, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(GamePlayScene.profile.Width + 155, (GamePlayScene.menuBox.Height / 5) + 20), Color.Black);
            //time
            string Time = $"{(int)GamePlayScene.TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(GamePlayScene.profile.Width + 165, (GamePlayScene.menuBox.Height / 5) + 55), Color.Black);

            spriteBatch.Draw(GamePlayScene.moneyBox, new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 30, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{GamePlayScene.TotalMoney}", new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + (GamePlayScene.moneyBox.Width / 2) + 55, (GamePlayScene.menuBox.Height / 5) + 36), Color.Black);

            Vector2 EmotionPos = new Vector2(GamePlayScene.moneyBox.Width + GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 30, GamePlayScene.menuBox.Height / 5);
            Vector2 percentPantiencePos = new Vector2(EmotionPos.X + 165, GamePlayScene.menuBox.Height / 5 + 36);

            string patienceText = $"{GamePlayScene._patienceMeter:0}%";
            spriteBatch.DrawString(_font, patienceText, percentPantiencePos, Color.Black);
            GamePlayScene.DrawEmotionIcon(_font, spriteBatch, EmotionPos);

            _prevBtn.Draw(spriteBatch);

            #endregion

            _logOrderBtn.Draw(spriteBatch);
            if (isLog)
            {
                spriteBatch.Draw(LogInfo, new Vector2(130, 160), Color.White);
                if (GameManager.countDia == 3)
                {
                    spriteBatch.DrawString(_font, "1. " + GamePlayScene._currentCustomer.Dia1, new Vector2(400, 300), Color.Black);
                    spriteBatch.DrawString(_font, "2. " + GamePlayScene._currentCustomer.Dia2, new Vector2(400, 400), Color.Black);

                }
                else spriteBatch.DrawString(_font, "1. " + GamePlayScene._currentCustomer.Dia1, new Vector2(400, 300), Color.Black);
                _okLogBtn.Draw(spriteBatch);
            }

            if (cameraPos.X < 4200 - 1920) spriteBatch.Draw(ArrowCam, new Vector2(1920 - ArrowCam.Width, 540), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0f);//ทางขวาของจอ
            if (cameraPos.X > 0) spriteBatch.Draw(ArrowCam, new Vector2(0, 540), Color.White);//ทางซ้ายของจอ

            if (GamePlayScene.isPaused)
            {
                spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);
                if (showSettings == true)
                {
                    spriteBatch.Draw(settingBG, new Vector2(224, 175), Color.White);

                    spriteBatch.Draw(header, new Vector2((1920 - header.Width) / 2, 120), Color.White);

                    DrawVolumeBar(spriteBatch, musicBarPos, SoundManager.MusicVolume, SoundManager.MusicVolume > 0, musicIcon, muteMusicIcon);
                    DrawVolumeBar(spriteBatch, sfxBarPos, SoundManager.SfxVolume, SoundManager.SfxVolume > 0, sfxIcon, muteSfxIcon);
                }
                else
                {
                    GamePlayScene._resumeButton.Draw(spriteBatch);
                }
                //DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
                //spriteBatch.DrawString(_font, "Paused", new Vector2(900, 300), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);

                // should create separate menu button
                GamePlayScene._settingButton.Draw(spriteBatch);
                GamePlayScene._homeButton.Draw(spriteBatch);
                GamePlayScene._exitButton.Draw(spriteBatch);
                if (GamePlayScene.isClickExit)
                {
                    spriteBatch.Draw(GamePlayScene.logExit, new Rectangle(448, 263, 1024, 534), Color.White);
                    GamePlayScene._yesExit.DrawHover(spriteBatch);
                    GamePlayScene._noExit.DrawHover(spriteBatch);
                }
            }
            else if (GamePlayScene.isEndLv)
            {
                GamePlayScene.Profit = GamePlayScene.Revenue - GamePlayScene.Cost;
                spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);
                spriteBatch.Draw(GamePlayScene.revenueBox, new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(_font, $"{GamePlayScene.Revenue}", new Vector2(1250, 350), Color.Green);
                spriteBatch.DrawString(_font, $"{GamePlayScene.Cost}", new Vector2(1250, 460), Color.Red);
                spriteBatch.DrawString(_font, $"{GamePlayScene.Profit}", new Vector2(1250, 720), Color.Black);
                GamePlayScene._OkButton.Draw(spriteBatch);
            }
            GamePlayScene._menuButton.Draw(spriteBatch);

            spriteBatch.End();
        }

        private void DrawVolumeBar(SpriteBatch spriteBatch, Vector2 position, float volume, bool notMuted, Texture2D normalIcon, Texture2D muteIcon)
        {
            spriteBatch.Draw(notMuted ? normalIcon : muteIcon, new Vector2(position.X - 250, position.Y - 30), Color.White);

            spriteBatch.Draw(barBg, position, null, Color.White, 0f, Vector2.Zero, barScale, SpriteEffects.None, 0f);
            float fillWidth = barFill.Width * volume;
            Rectangle sourceRect = new Rectangle(0, 0, (int)fillWidth, barFill.Height);

            spriteBatch.Draw(barFill, position, sourceRect, Color.White, 0f, Vector2.Zero, barScale, SpriteEffects.None, 0f);

            Vector2 knobPos = new Vector2(position.X + fillWidth - knob.Width / 2, position.Y - 5);
            spriteBatch.Draw(knob, knobPos, Color.White);

            spriteBatch.DrawString(_font, $"{(int)(volume * 100)}%", new Vector2(position.X + barBg.Width + 50, position.Y), Color.Black);
        }
        public async void CookingBtn_Click(object sender, EventArgs e)
        {
            //GameManager.readySteam = true;
            isCountDownSteam = true;
            isClickCook = true;
            SoundManager.PlaySfx("Cooking");
            await Task.Delay(3000);
            SoundManager.StopSfx("Cooking");
        }
        public void ServeBtn_Click(object sender, EventArgs e)
        {
            if (GameManager.HasFood)
            {
                ServeRequest = true;
                GamePlayScene.served = true;
                _gameManager.ServeFood();
            }
        }
        public void LogBtn_Click(object sender, EventArgs e)
        {
            isLog = !isLog;
        }
        public void okLogBtn_Click(object sender, EventArgs e)
        {
            isLog = false;
            //GamePlayScene.isEndLv = false;     for test
        }
        public void previousButton_Click(object sender, EventArgs e)
        {
            backToCounter = true;
        }
    }
}