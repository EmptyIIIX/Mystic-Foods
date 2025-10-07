using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;
using Mystic_Foods.Time;
using System;
using System.Formats.Tar;
using System.Reflection.PortableExecutable;
using System.Threading;
using System.Threading.Tasks;

namespace Mystic_Foods
{
    public class GamePlayScene : IGameScene
    {
        public static float TimeDefault = 241f;
        public static float TimeStage = TimeDefault;
        public static float TimePSec;

        public enum DayPhase { Dawn, Dusk, Night }
        public static DayPhase CurrentPhase;
        Texture2D texDawn, texDusk, texNight;

        private SpriteFont _font, _font2;
        public bool BackToMenuRequested = false;
        public bool DnDRequested = false;
        public bool ExitRequest = false;
        public static bool showSettings = false;
        private KeyboardState _oldState;
        private CustomerManager _customerManager;
        public static Customer _currentCustomer;
        private ContentManager _contentManager;
        private DnDScene _dnDScene;

        public static bool served;
        public static bool isEndLv = false;
        public Button _servedYesButton;

        public static bool isPaused = false;
        public static Button _menuButton, _resumeButton, _homeButton, _tutorialButton, _settingButton, _exitButton;
        public static Button _yesExit, _noExit;
        public static Texture2D yesExit, yesExit_hover, noExit, noExit_hover, logExit;
        public Button _yesButton;
        public Button _whatButton;
        public static Button _OkButton;

        public static float _patienceMeter;        // current patience
        public static float _patienceMeterStart = 100f;   // default / max patience
        public static float _patienceDecreaseRate = 1.28f; // decrease rate
        public static Texture2D _textureHappy;
        public static Texture2D _textureNeutral;
        public static Texture2D _textureGrumpy;

        public static Texture2D bgBox, dayBox, moneyBox, menuBox, profile, uiBox;
        public static Texture2D homeBtn, resumeBtn, exitBtn, okBtn, settingBtn;
        public static Texture2D spriteEmoIcon;
        public static Texture2D revenueBox;
        public static Texture2D whatButton, whatButton_hover, yesButton, yesButton_hover, diaBox;
        public static Texture2D _happy, _natural, _angry;
        public static Texture2D counterDawn, counterDusk, counterNight;

        public static Texture2D _rectTexture;

        public static float TotalMoney = 100.0f;
        public static float Revenue = 0.0f;
        public static float Cost = 0.0f;
        public static float pay;
        public static float price = 40.0f;
        public static float weight = 0.0f;
        public static float Profit = 0.0f;

        public static bool isClickExit = false;

        public static bool isSkip = false;

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

        #region Typing
        private string fullText = "";
        private string displayedText = "";
        private float typingSpeed = 0.005f; //lower the number, faster the typo
        private float typingTimer = 0f;
        private int charIndex = 0;
        #endregion

        public GamePlayScene(CustomerManager cm)
        {
            _customerManager = cm;
            GetCustomerByPhase();
            _patienceMeter = _patienceMeterStart;
        }
        public GamePlayScene() {}
        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _contentManager = content;
            _font = content.Load<SpriteFont>("MainFont");
            _font2 = content.Load<SpriteFont>("DiaFont");
            LoadCustomerTextures();

            _rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _rectTexture.SetData(new[] { Color.White });

            //Cat = content.Load<Texture2D>("Etc/Cat");
            texDawn = content.Load<Texture2D>("Environments/BG/orderBG_morning");
            texDusk = content.Load<Texture2D>("Environments/BG/orderBG_sunset");
            texNight = content.Load<Texture2D>("Environments/BG/orderBG_midnight");

            counterDawn = content.Load<Texture2D>("Environments/Counter/orderCounter_morning");
            counterDusk = content.Load<Texture2D>("Environments/Counter/orderCounter_sunset");
            counterNight = content.Load<Texture2D>("Environments/Counter/orderCounter_midnight");

            spriteEmoIcon = content.Load<Texture2D>("Emote/sprite_emotion_icon");

            revenueBox = content.Load<Texture2D>("DialogueUI/Revenue");
            okBtn = content.Load<Texture2D>("DialogueUI/okLog");

            bgBox = content.Load<Texture2D>("Etc/OutLine");
            dayBox = content.Load<Texture2D>("Etc/Day");
            moneyBox = content.Load<Texture2D>("Etc/Money");
            menuBox = content.Load<Texture2D>("Emote/EmoteMenu");
            uiBox = content.Load<Texture2D>("UI/UIBOX");

            profile = content.Load<Texture2D>("Etc/Cat1");
            homeBtn = content.Load<Texture2D>("Etc/HomeBtn");
            exitBtn = content.Load<Texture2D>("Etc/ExitBtn");
            settingBtn = content.Load<Texture2D>("UI/setting/settingButton");
            logExit = content.Load<Texture2D>("DialogueUI/ConfirmExit_UI");
            yesExit = content.Load<Texture2D>("DialogueUI/LeaveAnyway_BeforeClick");
            yesExit_hover = content.Load<Texture2D>("DialogueUI/LeaveAnyway_AfterClick");
            noExit = content.Load<Texture2D>("DialogueUI/KeepPlaying_BeforeClick");
            noExit_hover = content.Load<Texture2D>("DialogueUI/KeepPlaying_AfterClick");
            resumeBtn = content.Load<Texture2D>("Etc/PauseBtn");

            whatButton = content.Load<Texture2D>("DialogueUI/WhatButton");
            yesButton = content.Load<Texture2D>("DialogueUI/YesButton");
            whatButton_hover = content.Load<Texture2D>("DialogueUI/WhatButtonHover");
            yesButton_hover = content.Load<Texture2D>("DialogueUI/YesButtonHover");
            diaBox = content.Load<Texture2D>("DialogueUI/DialogueBox2");

            _menuButton = new Button(menuBox, menuBox, _font, " ", new Rectangle(1670, 10, 231, 162));
            _menuButton.Click += MenuButton_Click;
            _homeButton = new Button(homeBtn, homeBtn, _font, " ", new Rectangle(1970 - menuBox.Width, menuBox.Height + 20, 100, 106));//real size (50, 53) 
            _homeButton.Click += HomeButton_Click;
            _yesButton = new Button(yesButton, yesButton_hover, _font, " ", new Rectangle(1400, 500, 128, 63));
            _yesButton.Click += YesButton_Click;
            _whatButton = new Button(whatButton, whatButton_hover, _font, " ", new Rectangle(1550, 500, 128, 63));
            _whatButton.Click += WhatButton_Click;
            _servedYesButton = new Button(yesButton, yesButton_hover, _font, " ", new Rectangle(1400, 500, 128, 63));
            _servedYesButton.Click += ServedYes_Click;
            _settingButton = new Button(settingBtn, settingBtn, _font, "", new Rectangle(1970 - menuBox.Width, menuBox.Height + homeBtn.Height + 65, 100, 106));
            _settingButton.Click += SettingButton_Click;
            _exitButton = new Button(exitBtn, exitBtn, _font, "", new Rectangle(1970 - menuBox.Width, menuBox.Height + homeBtn.Height + settingBtn.Height + 90, 100, 106));// 61, 67
            _exitButton.Click += ExitButton_Click;
            _resumeButton = new Button(resumeBtn, resumeBtn, _font, " ", new Rectangle(1040 - resumeBtn.Width, 540 - resumeBtn.Height / 2, 180, 165));
            _resumeButton.Click += ResumeButton_Click;

            _yesExit = new Button(yesExit, yesExit_hover, _font, "", new Rectangle(1138, 686, 375, 170));
            _yesExit.Click += yesExitButton_Click;
            _noExit = new Button(noExit, noExit_hover, _font, "", new Rectangle(426, 682, 384, 163));
            _noExit.Click += noExitButton_Click;

            _OkButton = new Button(okBtn, okBtn, _font, "", new Rectangle(1450, 840, 300, 150));
            _OkButton.Click += OkEndButton_Click;

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
        }

        private void LoadCustomerTextures()
        {
            _textureHappy = _contentManager.Load<Texture2D>(_currentCustomer.SpritePathHappy);
            _textureNeutral = _contentManager.Load<Texture2D>(_currentCustomer.SpritePathNeutral);
            _textureGrumpy = _contentManager.Load<Texture2D>(_currentCustomer.SpritePathGrumpy);
        }
        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (isSkip == false )
            {
                SkipCustomer();
                isSkip = true;
            }

            //Button
            _menuButton.Update();
            if (isPaused)
            {
                #region Stopping the game

                _exitButton.Update();
                _homeButton.Update();
                _settingButton.Update();
                if (isClickExit)
                {
                    _yesExit.Update();
                    _noExit.Update();
                }

                if (showSettings == true)
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
            else if (isEndLv == false) 
            {
                #region Playing the game
                if (mouse.LeftButton == ButtonState.Pressed && charIndex < fullText.Length)
                {
                    displayedText = fullText;
                    charIndex = fullText.Length;
                }
                // random, reset Patience
                if (state.IsKeyDown(Keys.Space) && _oldState.IsKeyUp(Keys.Space))
                {
                    GetCustomerByPhase();
                    _patienceMeter = _patienceMeterStart;
                    LoadCustomerTextures();
                    GameManager.countDia = 2;
                }
                _whatButton.Update();
                
                if(served == true)
                {
                    _servedYesButton.Update();
                } else
                {
                    _yesButton.Update();
                }

                //TimeStage every scene
                TimePSec = 1.0f / 60.0f;
                TimeStage -= TimePSec;

                // Patience reduce logic
                //_patienceMeter -= _patienceDecreaseRate * deltaTime;
                _patienceMeter -= TimePSec * _patienceDecreaseRate;
                //Customer leave
                if (_patienceMeter <= 0)
                {
                    GetCustomerByPhase();
                    _patienceMeter = _patienceMeterStart;
                    LoadCustomerTextures();
                }
                //Check time out to back to mainmenu scene
                if (TimeStage <= 0)
                {
                    //BackToMenuRequested = true;
                    isEndLv = true;
                    TimeStage = TimeDefault;
                }
                #endregion
            }
            if (charIndex < fullText.Length)
            {
                typingTimer += deltaTime;
                if (typingTimer >= typingSpeed)
                {
                    typingTimer = 0f;
                    charIndex++;
                    displayedText = fullText.Substring(0, charIndex);
                }
            }
            else if (isEndLv)
            {
                //DnDScene._okLogBtn.Update();
                _OkButton.Update();
            }

            // ESC to pause
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            {
                isPaused = !isPaused;
                isClickExit = false;
            }

            //P
            if (state.IsKeyDown(Keys.P) && _oldState.IsKeyUp(Keys.P))
            {
                isPaused = !isPaused;
            }
            if (isPaused)
            {
                _oldState = state;
                return;
            }

                _oldState = state;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSeaGreen);
            spriteBatch.Begin();

            #region Background
            //spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);

            Texture2D background = texDawn;

            switch (CurrentPhase)
            {
                case DayPhase.Dawn:
                    background = texDawn;
                    break;
                case DayPhase.Dusk:
                    background = texDusk;
                    break;
                case DayPhase.Night:
                    background = texNight;
                    break;
            }

            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);
            //spriteBatch.Draw(bgBox, new Vector2(0, 0), Color.White*0.5f);
            #endregion

            #region Customer
            Texture2D drawTexture = _textureNeutral;
            float patiencePerc = _patienceMeter / _patienceMeterStart;
            switch (GameManager.countDia)
            {
                case 0:
                    drawTexture = _textureGrumpy;
                    break;
                case 1:
                    if (patiencePerc >= 2f / 3f)
                    {
                        drawTexture = _textureHappy;
                    }
                    else if (patiencePerc >= 1f / 3f)
                    {
                        drawTexture = _textureNeutral;
                    }
                    else
                    {
                        drawTexture = _textureGrumpy;
                    }
                    break;
                case 2:
                    drawTexture = _textureNeutral;
                    break;
                case 3:
                    drawTexture = _textureNeutral;
                    break;
                case -1:
                    drawTexture = _textureNeutral;
                    break;
            }
            spriteBatch.Draw(drawTexture, new Vector2(200, 0), null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            #endregion

            #region Counter
            //Counter
            Texture2D Counter = counterDawn;

            switch (CurrentPhase)
            {
                case DayPhase.Dawn:
                    Counter = counterDawn;
                    break;
                case DayPhase.Dusk:
                    Counter = counterDusk;
                    break;
                case DayPhase.Night:
                    Counter = counterNight;
                    break;
            }
            spriteBatch.Draw(Counter, new Vector2(0, 1080 - Counter.Height), Color.White);
            //table pos
            //spriteBatch.Draw(Cat, new Vector2(1000, 600), Color.White);
            spriteBatch.Draw(bgBox, new Vector2(0, 0), Color.White*0.5f);
            #endregion

            #region UI

            #region detailing
            spriteBatch.Draw(uiBox, new Vector2(10, 32), Color.White);
            spriteBatch.Draw(uiBox, new Vector2(uiBox.Width + 10, 32), Color.White);
            spriteBatch.Draw(uiBox, new Vector2(uiBox.Width * 2 + 10, 32), Color.White);
            #endregion

            //profile
            //spriteBatch.Draw(profile, new Vector2(0, 0), Color.White);
            //Date and Time
            int Days = 1;//สำหรับเปลี่ยนวันตามเงื่อนไขต่างๆที่เราต้องการ
            spriteBatch.Draw(dayBox, new Vector2(10, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(135, (menuBox.Height / 5) + 20), Color.Black);
            //time
            string Time = $"{(int)TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(145, (menuBox.Height / 5) + 55), Color.Black);

            spriteBatch.Draw(moneyBox, new Vector2(dayBox.Width + 10, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{TotalMoney}", new Vector2(dayBox.Width + (moneyBox.Width / 2) + 35, (menuBox.Height / 5) + 36), Color.Black);

            //Draw Emotion
            // แสดงค่า Patience Meter
            Vector2 EmotionPos = new Vector2(moneyBox.Width + dayBox.Width + 10, menuBox.Height / 5);//สำหรับตำแหน่งของอีโมจิอารมณ์
            Vector2 percentPantiencePos = new Vector2(EmotionPos.X + 145, menuBox.Height / 5 + 36);

            string patienceText = $"{_patienceMeter:0}%";
            DrawEmotionIcon(_font, spriteBatch, EmotionPos);
            spriteBatch.DrawString(_font, patienceText, percentPantiencePos, Color.Black);
            #endregion

            #region Dialouge
            //Dia
            spriteBatch.Draw(diaBox, new Vector2(900, 200), Color.White);
            Vector2 diaPos = new Vector2(900, 200);

            if (served == true)
            {
                _servedYesButton.DrawHover(spriteBatch);
            } else
            {
                _yesButton.DrawHover(spriteBatch);
            }

            switch (GameManager.countDia)
            {
                case 0:
                    if (fullText != _currentCustomer.DiaWrong)
                        StartTyping(_currentCustomer.DiaWrong);

                    spriteBatch.DrawString(_font2, displayedText, new Vector2(1000, 275), Color.Black);
                    _whatButton.DrawHover(spriteBatch);
                    break;

                case 1:
                    if (fullText != _currentCustomer.DiaCurrect)
                        StartTyping(_currentCustomer.DiaCurrect);

                    spriteBatch.DrawString(_font2, displayedText, new Vector2(1000, 275), Color.Black);
                    break;

                case 2:
                    if (fullText != _currentCustomer.TalkDia)
                        StartTyping(_currentCustomer.TalkDia);

                    spriteBatch.DrawString(_font2, displayedText, new Vector2(1000, 275), Color.Black);
                    _whatButton.DrawHover(spriteBatch);
                    break;
                case 3:
                    if (fullText != _currentCustomer.Dia1)
                        StartTyping(_currentCustomer.Dia1);

                    spriteBatch.DrawString(_font2, displayedText, new Vector2(1000, 275), Color.Black);
                    _whatButton.DrawHover(spriteBatch);
                    break;

                case 4:
                    if (fullText != _currentCustomer.Dia2)
                        StartTyping(_currentCustomer.Dia2);

                    spriteBatch.DrawString(_font2, displayedText, new Vector2(1000, 275), Color.Black);
                    break;
            }


            #endregion

            /*
            spriteBatch.DrawString(_font, $" IdOrder : {_currentCustomer.IdOrder}", new Vector2(1000, 400), Color.Black);
            spriteBatch.DrawString(_font, $" CurrectOrder : {GameManager.IsCurrectOrder}", new Vector2(1000, diaBoxPos.Y + (diaBoxPos.Y / 2) + 200), Color.Black);
            string TimeS = $"\nTimePerSec: {TimePSec}";
            spriteBatch.DrawString(_font, $"Count Dialogue : {GameManager.countDia}", new Vector2(100, 300), Color.Blue);
            spriteBatch.DrawString(_font, $"_what : {_what}", new Vector2(100, 400), Color.Blue);
            */

            if (isPaused)
            {
                //DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
                //spriteBatch.DrawString(_font, "Paused", new Vector2(900, 300), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                spriteBatch.Draw(_rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);
                if (showSettings == true)
                {
                    spriteBatch.Draw(settingBG, new Vector2(224, 175), Color.White);

                    spriteBatch.Draw(header, new Vector2((1920 - header.Width) / 2, 120), Color.White);

                    DrawVolumeBar(spriteBatch, musicBarPos, SoundManager.MusicVolume, SoundManager.MusicVolume > 0, musicIcon, muteMusicIcon);
                    DrawVolumeBar(spriteBatch, sfxBarPos, SoundManager.SfxVolume, SoundManager.SfxVolume > 0, sfxIcon, muteSfxIcon);
                }
                else
                {
                    _resumeButton.Draw(spriteBatch);
                }
                _settingButton.Draw(spriteBatch);
                _homeButton.Draw(spriteBatch);
                _exitButton.Draw(spriteBatch);
                if (isClickExit)
                {
                    spriteBatch.Draw(logExit, new Rectangle(448, 263, 1024, 534), Color.White);
                    _yesExit.DrawHover(spriteBatch);
                    _noExit.DrawHover(spriteBatch);
                }
            }
            else if (isEndLv)
            {
                Profit = Revenue - Cost;
                spriteBatch.Draw(_rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);
                spriteBatch.Draw(revenueBox, new Vector2(100, 100), Color.White);
                spriteBatch.DrawString(_font, $"{Revenue}", new Vector2(1250, 320), Color.Green, 0, Vector2.Zero, 3.0f, SpriteEffects.None, 0);
                spriteBatch.DrawString(_font, $"{Cost}", new Vector2(1250, 430), Color.Red, 0, Vector2.Zero, 3.0f, SpriteEffects.None, 0);
                spriteBatch.DrawString(_font, $"{Profit}", new Vector2(1250, 690), Color.Black, 0, Vector2.Zero, 3.0f, SpriteEffects.None, 0);
                //DnDScene._okLogBtn.Draw(spriteBatch);
                _OkButton.Draw(spriteBatch);
            }
                _menuButton.Draw(spriteBatch);

            spriteBatch.End();
        }
        public static void DrawEmotionIcon(SpriteFont _font, SpriteBatch spriteBatch, Vector2 EmotionPos)
        {
            float patiencePerc = _patienceMeter / _patienceMeterStart;
            if (patiencePerc >= 2f / 3f)
            {
                //spriteBatch.Draw(_happy, EmotionPos, Color.White);
                spriteBatch.Draw(spriteEmoIcon, EmotionPos, new Rectangle(0, 0, 264, 104), Color.White);
                weight = 1.0f;
            }
            else if (patiencePerc >= 1f / 3f)
            {
                //spriteBatch.Draw(_natural, EmotionPos, Color.White);
                spriteBatch.Draw(spriteEmoIcon, EmotionPos, new Rectangle(0, 104, 264, 104), Color.White);
                weight = 0.75f;
            }
            else
            {
                //spriteBatch.Draw(_angry, EmotionPos, Color.White);
                spriteBatch.Draw(spriteEmoIcon, EmotionPos, new Rectangle(0, 208, 264, 104), Color.White);
                weight = 0.25f;
            }
            //string patienceText = $"{_patienceMeter:0}%";
            //spriteBatch.DrawString(_font, patienceText, new Vector2(EmotionPos.X + (_happy.Width / 5), EmotionPos.Y + _happy.Height), Color.Black);
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

        private void YesButton_Click(Object sender, EventArgs e)
        {
            DnDRequested = true;
            DnDScene.cameraPos = Vector2.Zero;
        }
        private void WhatButton_Click(Object sender, EventArgs e)
        {
            if (GameManager.countDia < 4)
            {
                GameManager.countDia++;
            }
        }
        public void MenuButton_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
            isClickExit = false;
        }
        public void HomeButton_Click(Object sender, EventArgs e)
        {
            //reset Scene
            GetCustomerByPhase();
            _patienceMeter = _patienceMeterStart;
            LoadCustomerTextures();
            TimeStage = TimeDefault;
            isPaused = false;
            isEndLv = false;
            GameManager.countDia = 2;
            isClickExit = false;
            BackToMenuRequested = true;
        }
        public void ServedYes_Click(Object sender, EventArgs e)
        {
            GetCustomerByPhase();
            _patienceMeter = _patienceMeterStart;
            LoadCustomerTextures();
            GameManager.countDia = 2;
            served = false;
        }
        public void ResumeButton_Click(object sender, EventArgs e)
        {
            isPaused = false;
        }
        private void ExitButton_Click(object sender, EventArgs e)
        {
            isClickExit = !isClickExit;
        }
        private void yesExitButton_Click(object sender, EventArgs e)
        {
            ExitRequest = true;
        }
        private void noExitButton_Click(object sender, EventArgs e)
        {
            isClickExit = false;
        }
        
        private void OkEndButton_Click(object sender, EventArgs e)
        {
            isEndLv = false;
            BackToMenuRequested = true;
        }
        private void SettingButton_Click(object sender, EventArgs e)
        {
            showSettings = !showSettings;
        }

        public void GetCustomerByPhase()
        {
            switch (CurrentPhase)
            {
                case DayPhase.Dawn:
                    _currentCustomer = _customerManager.GetNextCustomer();
                    break;

                case DayPhase.Dusk:
                    _currentCustomer = _customerManager.GetNextCustomer2();
                    break;

                case DayPhase.Night:
                    if (!_customerManager.IsEventFinished)
                    {
                        _currentCustomer = _customerManager.GetEventCustomer();
                    }
                    else
                    {
                        _currentCustomer = _customerManager.GetNextCustomer3();
                    }
                    break;
            }
        }
        public void SkipCustomer()
        {
            GetCustomerByPhase();
            _patienceMeter = _patienceMeterStart;
            LoadCustomerTextures();
            GameManager.countDia = 2;
        }

        private void StartTyping(string text)
        {
            fullText = text;
            displayedText = "";
            charIndex = 0;
            typingTimer = 0f;
        }

        public async void wait()
        {
            await Task.Delay(100);
        }
    }
}