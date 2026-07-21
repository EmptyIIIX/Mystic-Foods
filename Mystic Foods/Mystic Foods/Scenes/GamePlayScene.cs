using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Mystic_Foods.Core.DI;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.Scenes;
using Mystic_Foods.Core.UI;
using Mystic_Foods.Gameplay.Cooking;
using Mystic_Foods.Gameplay.Customer;
using Mystic_Foods.Gameplay.Economy;
using Mystic_Foods.Gameplay.Ingredients;
using Mystic_Foods.Gameplay.Recipes;
using LegacyCustomer = Mystic_Foods.Customer;

namespace Mystic_Foods
{
    public class GamePlayScene : IScene
    {
        private readonly IGraphicsService _graphics;
        private readonly IInputService _input;
        private readonly Container _container;

        private readonly CustomerQueue _customerQueue;
        private readonly Wallet _wallet;

        private SpriteFont _font;
        private ContentManager _content;
        private GraphicsDevice _device;

        public bool BackToMenuRequested { get; set; }
        public bool DnDRequested { get; set; }
        public bool ExitRequest { get; set; }

        // Static state (kept for DnDScene backward compatibility)
        public static float TimeDefault = 121f;
        public static float TimeStage = TimeDefault;
        public static float TimePSec;
        public enum DayPhase { Dawn, Dusk, Night }
        public static DayPhase CurrentPhase;
        public static bool isRecipeInGame = false;
        public static LegacyCustomer _currentCustomer;
        public static bool served;
        public static bool isEndLv = false;
        public static bool isPaused = false;
        public static bool isClickExit = false;
        public static float TotalMoney = 100f;
        public static float Revenue = 0f;
        public static float Cost = 0f;
        public static float Profit = 0f;
        public static float weight = 0.0f;
        public static float price = 0f;
        public static float pay = 0f;
        public static int countDia = 2;

        public static Button _menuButton, _resumeButton, _homeButton, _exitButton;
        public static Button _yesExit, _noExit, _OkButton;
        public static Texture2D yesExit, noExit, logExit;
        public static Texture2D profile, dayBox, moneyBox, menuBox, revenueBox;
        public static Texture2D _rectTexture;
        public static float _patienceMeter;
        public static float _patienceMeterStart = 100f;
        public static float _patienceDecreaseRate = 1.28f;
        public static Texture2D _textureHappy, _textureNeutral, _textureGrumpy;
        public static Texture2D spriteEmoIcon;

        // Non-static textures
        private Texture2D texDawn, texDusk, texNight;
        private Texture2D counterDawn, counterDusk, counterNight;
        private Texture2D bgBox, uiBox;
        private Texture2D homeBtnTex, resumeBtnTex, exitBtnTex, okBtnTex;
        private Texture2D whatBtnTex, yesBtnTex, noBtnTex, diaBox;
        private Texture2D bg;

        // Non-static buttons
        private Button _yesButton, _whatButton;
        private Button _servedYesButton;

        // IScene
        public string Name => "GamePlay";
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; } = true;

        // Singleton for DnDScene access to instance members
        public static GamePlayScene instance;

        public GamePlayScene(Container container)
        {
            _container = container;
            _graphics = container.Resolve<IGraphicsService>();
            _input = container.Resolve<IInputService>();
            _customerQueue = container.Resolve<CustomerQueue>();
            _wallet = container.Resolve<Wallet>();
            instance = this;
            GetCustomerByPhase();
            _patienceMeter = _patienceMeterStart;
        }

        public GamePlayScene() { instance = this; }

        public void LoadContent(ContentManager content)
        {
            _content = content;
            _font = content.Load<SpriteFont>("MainFont");
            _device = _graphics.SpriteBatch.GraphicsDevice;
            LoadCustomerTextures();

            _rectTexture = new Texture2D(_device, 1, 1);
            _rectTexture.SetData(new[] { Color.White });

            bg = content.Load<Texture2D>("Environments/BG/orderBG_morning");
            texDawn = content.Load<Texture2D>("Environments/BG/orderBG_morning");
            texDusk = content.Load<Texture2D>("Environments/BG/orderBG_sunset");
            texNight = content.Load<Texture2D>("Environments/BG/orderBG_midnight");

            counterDawn = content.Load<Texture2D>("Environments/Counter/orderCounter_morning");
            counterDusk = content.Load<Texture2D>("Environments/Counter/orderCounter_sunset");
            counterNight = content.Load<Texture2D>("Environments/Counter/orderCounter_midnight");

            spriteEmoIcon = content.Load<Texture2D>("Emote/sprite_emotion_icon");
            bgBox = content.Load<Texture2D>("Etc/OutLine");
            dayBox = content.Load<Texture2D>("Etc/Day");
            moneyBox = content.Load<Texture2D>("Etc/Money");
            menuBox = content.Load<Texture2D>("Emote/EmoteMenu");
            uiBox = content.Load<Texture2D>("UI/UIBOX");
            profile = content.Load<Texture2D>("Etc/Cat1");
            homeBtnTex = content.Load<Texture2D>("Etc/HomeBtn");
            resumeBtnTex = content.Load<Texture2D>("Etc/PauseBtn");
            exitBtnTex = content.Load<Texture2D>("Etc/ExitBtn");
            logExit = content.Load<Texture2D>("DialogueUI/ConfirmExit_UI");
            yesExit = content.Load<Texture2D>("DialogueUI/LeaveAnyway_BeforeClick");
            noExit = content.Load<Texture2D>("DialogueUI/KeepPlaying_BeforeClick");
            okBtnTex = content.Load<Texture2D>("DialogueUI/okLog");
            revenueBox = content.Load<Texture2D>("DialogueUI/Revenue");
            whatBtnTex = content.Load<Texture2D>("DialogueUI/WhatButton");
            yesBtnTex = content.Load<Texture2D>("DialogueUI/YesButton");
            diaBox = content.Load<Texture2D>("DialogueUI/DialogueBox");

            _menuButton = new Button(new ButtonConfig
            {
                Texture = menuBox, Font = _font, Text = " ",
                Bounds = new Rectangle(1670, 10, 231, 162),
                OnClick = _ => MenuButton_Click()
            });

            _homeButton = new Button(new ButtonConfig
            {
                Texture = homeBtnTex, Font = _font, Text = " ",
                Bounds = new Rectangle(1970 - menuBox.Width, menuBox.Height + 20, 100, 106),
                OnClick = _ => HomeButton_Click()
            });

            _exitButton = new Button(new ButtonConfig
            {
                Texture = exitBtnTex, Font = _font, Text = "",
                Bounds = new Rectangle(1970 - menuBox.Width, menuBox.Height + homeBtnTex.Height + 80, 100, 106),
                OnClick = _ => ExitButton_Click()
            });

            _resumeButton = new Button(new ButtonConfig
            {
                Texture = resumeBtnTex, Font = _font, Text = " ",
                Bounds = new Rectangle(1040 - resumeBtnTex.Width, 540 - resumeBtnTex.Height, 180, 165),
                OnClick = _ => ResumeButton_Click()
            });

            _yesExit = new Button(new ButtonConfig
            {
                Texture = yesExit, Font = _font, Text = "",
                Bounds = new Rectangle(426, 682, 375, 170),
                OnClick = _ => YesExitButton_Click()
            });

            _noExit = new Button(new ButtonConfig
            {
                Texture = noExit, Font = _font, Text = "",
                Bounds = new Rectangle(1138, 686, 384, 163),
                OnClick = _ => NoExitButton_Click()
            });

            _OkButton = new Button(new ButtonConfig
            {
                Texture = okBtnTex, Font = _font, Text = "",
                Bounds = new Rectangle(1450, 840, 300, 150),
                OnClick = _ => OkEndButton_Click()
            });

            _yesButton = new Button(new ButtonConfig
            {
                Texture = yesBtnTex, Font = _font, Text = " ",
                Bounds = new Rectangle(1400, 500, 128, 63),
                OnClick = _ => YesButton_Click()
            });

            _whatButton = new Button(new ButtonConfig
            {
                Texture = whatBtnTex, Font = _font, Text = " ",
                Bounds = new Rectangle(1550, 500, 128, 63),
                OnClick = _ => WhatButton_Click()
            });

            _servedYesButton = new Button(new ButtonConfig
            {
                Texture = yesBtnTex, Font = _font, Text = " ",
                Bounds = new Rectangle(1400, 500, 128, 63),
                OnClick = _ => ServedYes_Click()
            });
        }

        private void LoadCustomerTextures()
        {
            if (_currentCustomer == null) return;
            _textureHappy = _content?.Load<Texture2D>(_currentCustomer.SpritePathHappy);
            _textureNeutral = _content?.Load<Texture2D>(_currentCustomer.SpritePathNeutral);
            _textureGrumpy = _content?.Load<Texture2D>(_currentCustomer.SpritePathGrumpy);
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _input.Update();
            var state = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _menuButton?.Update(gameTime);

            if (isPaused)
            {
                _exitButton?.Update(gameTime);
                _homeButton?.Update(gameTime);
                _resumeButton?.Update(gameTime);
                if (isClickExit)
                {
                    _yesExit?.Update(gameTime);
                    _noExit?.Update(gameTime);
                }
            }
            else if (!isEndLv)
            {
                TimePSec = 1.0f / 60.0f;
                TimeStage -= TimePSec;
                _patienceMeter -= TimePSec * _patienceDecreaseRate;

                if (_patienceMeter <= 0)
                {
                    GetCustomerByPhase();
                    _patienceMeter = _patienceMeterStart;
                    LoadCustomerTextures();
                }
                if (TimeStage <= 0)
                {
                    isEndLv = true;
                    TimeStage = TimeDefault;
                }
                _whatButton?.Update(gameTime);
                if (served) _servedYesButton?.Update(gameTime);
                else _yesButton?.Update(gameTime);
            }
            else
            {
                _OkButton?.Update(gameTime);
            }

            if (state.IsKeyDown(Keys.P) && Keyboard.GetState().IsKeyUp(Keys.P))
            {
                isPaused = !isPaused;
            }
            if (isPaused) return;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            _graphics.Begin();

            // Background
            Texture2D background = CurrentPhase switch
            {
                DayPhase.Dusk => texDusk,
                DayPhase.Night => texNight,
                _ => texDawn
            };
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            // Customer sprite
            Texture2D drawTexture = _textureNeutral;
            if (_textureNeutral != null)
            {
                float patiencePerc = _patienceMeter / _patienceMeterStart;
                drawTexture = countDia switch
                {
                    0 => _textureGrumpy ?? _textureNeutral,
                    1 => patiencePerc >= 2f / 3f ? _textureHappy ?? _textureNeutral :
                         patiencePerc >= 1f / 3f ? _textureNeutral :
                         _textureGrumpy ?? _textureNeutral,
                    _ => _textureNeutral
                };
                spriteBatch.Draw(drawTexture, new Vector2(200, 0), null, Color.White, 0f, Vector2.Zero, 0.9f, SpriteEffects.None, 0f);
            }

            // Counter
            Texture2D counter = CurrentPhase switch
            {
                DayPhase.Dusk => counterDusk,
                DayPhase.Night => counterNight,
                _ => counterDawn
            };
            spriteBatch.Draw(counter, new Vector2(0, 1080 - counter.Height), Color.White);
            spriteBatch.Draw(bgBox, new Vector2(0, 0), Color.White * 0.5f);

            // UI
            spriteBatch.Draw(uiBox, new Vector2(244, 32), Color.White);
            spriteBatch.Draw(uiBox, new Vector2(508, 32), Color.White);
            spriteBatch.Draw(uiBox, new Vector2(772, 32), Color.White);
            spriteBatch.Draw(profile, new Vector2(0, 0), Color.White);

            int Days = 1;
            spriteBatch.Draw(dayBox, new Vector2(profile.Width + 10, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(profile.Width + 135, (menuBox.Height / 5) + 20), Color.Black);
            string Time = $"{(int)TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(profile.Width + 145, (menuBox.Height / 5) + 55), Color.Black);

            spriteBatch.Draw(moneyBox, new Vector2(profile.Width + dayBox.Width + 10, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{TotalMoney}", new Vector2(profile.Width + dayBox.Width + (moneyBox.Width / 2) + 35, (menuBox.Height / 5) + 36), Color.Black);

            Vector2 EmotionPos = new Vector2(moneyBox.Width + profile.Width + dayBox.Width + 10, menuBox.Height / 5);
            Vector2 percentPantiencePos = new Vector2(EmotionPos.X + 145, menuBox.Height / 5 + 36);
            spriteBatch.DrawString(_font, $"{_patienceMeter:0}%", percentPantiencePos, Color.Black);
            DrawEmotionIcon(_font, spriteBatch, EmotionPos);

            // Dialogue
            spriteBatch.Draw(diaBox, new Vector2(900, 200), Color.White);
            if (served) _servedYesButton?.Draw(gameTime, spriteBatch);
            else _yesButton?.Draw(gameTime, spriteBatch);

            switch (countDia)
            {
                case 0:
                    spriteBatch.DrawString(_font, _currentCustomer?.DiaWrong, new Vector2(1000, 300), Color.Black);
                    _whatButton?.Draw(gameTime, spriteBatch);
                    break;
                case 1:
                    spriteBatch.DrawString(_font, _currentCustomer?.DiaCurrect, new Vector2(1000, 300), Color.Black);
                    break;
                case 2:
                    spriteBatch.DrawString(_font, _currentCustomer?.Dia1, new Vector2(1000, 300), Color.Black);
                    _whatButton?.Draw(gameTime, spriteBatch);
                    break;
                case 3:
                    spriteBatch.DrawString(_font, _currentCustomer?.Dia2, new Vector2(1000, 300), Color.Black);
                    break;
            }

            // Pause / End overlays
            if (isPaused)
            {
                spriteBatch.Draw(_rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);
                _resumeButton?.Draw(gameTime, spriteBatch);
                _homeButton?.Draw(gameTime, spriteBatch);
                _exitButton?.Draw(gameTime, spriteBatch);
                if (isClickExit)
                {
                    spriteBatch.Draw(logExit, new Rectangle(448, 263, 1024, 534), Color.White);
                    _yesExit?.Draw(gameTime, spriteBatch);
                    _noExit?.Draw(gameTime, spriteBatch);
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
                _OkButton?.Draw(gameTime, spriteBatch);
            }
            _menuButton?.Draw(gameTime, spriteBatch);
            _graphics.End();
        }

        public static void DrawEmotionIcon(SpriteFont font, SpriteBatch spriteBatch, Vector2 EmotionPos)
        {
            float patiencePerc = _patienceMeter / _patienceMeterStart;
            Rectangle source;
            if (patiencePerc >= 2f / 3f)
            {
                source = new Rectangle(0, 0, 264, 104);
                weight = 1.0f;
            }
            else if (patiencePerc >= 1f / 3f)
            {
                source = new Rectangle(0, 104, 264, 104);
                weight = 0.75f;
            }
            else
            {
                source = new Rectangle(0, 208, 264, 104);
                weight = 0.25f;
            }
            spriteBatch.Draw(spriteEmoIcon, EmotionPos, source, Color.White);
        }

        public void GetCustomerByPhase()
        {
            if (_customerQueue != null)
            {
                _currentCustomer = _customerQueue.Next();
            }
        }

        // Button handlers
        private void YesButton_Click() { DnDRequested = true; }
        private void WhatButton_Click() { countDia = 3; }
        private void MenuButton_Click() { isPaused = !isPaused; isClickExit = false; }
        private void HomeButton_Click()
        {
            GetCustomerByPhase();
            _patienceMeter = _patienceMeterStart;
            LoadCustomerTextures();
            TimeStage = TimeDefault;
            isPaused = false; isEndLv = false;
            countDia = 2; isClickExit = false;
            BackToMenuRequested = true;
        }
        private void ServedYes_Click()
        {
            GetCustomerByPhase();
            _patienceMeter = _patienceMeterStart;
            LoadCustomerTextures();
            countDia = 2; served = false;
        }
        private void ResumeButton_Click() { isPaused = false; }
        private void ExitButton_Click() { isClickExit = !isClickExit; }
        private void YesExitButton_Click() { ExitRequest = true; }
        private void NoExitButton_Click() { isClickExit = false; }
        private void OkEndButton_Click() { isEndLv = false; BackToMenuRequested = true; }

        public void OnEnter() { }
        public void OnExit() { }
        public void OnResize(int width, int height) { }
    }
}