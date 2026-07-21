using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Mystic_Foods.Core.DI;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.Scenes;
using Mystic_Foods.Core.UI;
using Mystic_Foods.Gameplay.Cooking;
using Mystic_Foods.Managers;

namespace Mystic_Foods.Scenes
{
    public class DnDScene : IScene
    {
        private readonly Container _container;
        private readonly IGraphicsService _graphics;
        private readonly IInputService _input;

        private SpriteFont _font;
        private ContentManager _content;
        private bool _contentLoaded = false;
        private KeyboardState _oldState;
        private MouseState _oldMouseState;
        private int _selectedIndex = 2;
        private bool _scroll = false;
        private Vector2 _emotion = new Vector2(800, 162 / 5);

        private Texture2D _bg, _arrowCam;
        private Vector2 _scrollFactor = new Vector2(5.0f, 1);
        public static Vector2 CameraPos = Vector2.Zero;
        private float _cameraSpeed = 0f;
        private int _cameraLeftBoundary2 = 5;
        private int _cameraLeftBoundary1 = 100;
        private int _cameraRightBoundary1 = 1805;
        private int _cameraRightBoundary2 = 1900;

        private Texture2D _table, _table2;
        private Texture2D _steam2;
        private Texture2D _steamBar;
        private float _currentSteam;

        private Texture2D _logOrder, _logInfo, _okLog;
        private Button _logOrderBtn;
        private Button _okLogBtn;
        private bool _isLog = false;

        private Button _cookingBtn, _serveBtn;
        private Texture2D _cookingBtnTex, _serveBtnTex;
        public static bool IsCountDownSteam = false;
        public bool ServeRequest = false;
        private bool _steamRequest = false;
        private List<Rectangle> _btnItemRect = new List<Rectangle>();

        public static bool IsClickCook = false;
        public bool BackToGame = false;

        public static Texture2D BoxFilling;
        public static Texture2D BoxDough;

        private List<Rectangle> _boxFilling = new List<Rectangle>();
        private List<Rectangle> _boxDough = new List<Rectangle>();

        public string Name => "DnD";
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; } = true;

        public DnDScene(Container container)
        {
            _container = container;
            _graphics = container.Resolve<IGraphicsService>();
            _input = container.Resolve<IInputService>();
        }

        public void LoadContent(ContentManager content)
        {
            _content = content;
            if (_contentLoaded) return;

            _bg = content.Load<Texture2D>("Environments/Cooking/CookingMorningBG");
            _steam2 = content.Load<Texture2D>("Environments/tools/steamer2 - test");
            _table = content.Load<Texture2D>("Environments/tools/Table");
            _table2 = content.Load<Texture2D>("Environments/tools/Table_2");
            _arrowCam = content.Load<Texture2D>("Etc/PointArrow");
            _cookingBtnTex = content.Load<Texture2D>("Etc/CookBtn");
            _steamBar = content.Load<Texture2D>("Etc/steam_bar");
            _serveBtnTex = content.Load<Texture2D>("Etc/ServeBtn");
            _font = content.Load<SpriteFont>("MainFont");

            BoxDough = content.Load<Texture2D>("foods/hitbox_dough");
            BoxFilling = content.Load<Texture2D>("foods/hitbox_filling");

            _logOrder = content.Load<Texture2D>("DialogueUI/LogButton");
            _logInfo = content.Load<Texture2D>("DialogueUI/LogInformation");
            _okLog = content.Load<Texture2D>("DialogueUI/okLog");

            _boxFilling.Add(new Rectangle(759, 218, 283, 154));
            _boxFilling.Add(new Rectangle(759 + BoxFilling.Width + 16, 218, 283, 154));
            _boxFilling.Add(new Rectangle(759 + 2 * (BoxFilling.Width + 16), 218, 283, 154));
            foreach (var rect in _boxFilling) DragDropManager.AddHitbox(rect);

            _boxDough.Add(new Rectangle(371, 215, 280, 183));
            _boxDough.Add(new Rectangle(371, 215 + BoxDough.Height + 12, 280, 183));
            _boxDough.Add(new Rectangle(371, 215 + 2 * (BoxDough.Height + 12), 280, 183));
            foreach (var rect in _boxDough) DragDropManager.AddHitbox(rect);

            _currentSteam = _steamBar.Height - 4;

            _cookingBtn = new Button(new ButtonConfig
            {
                Texture = _cookingBtnTex,
                Font = _font,
                Text = " ",
                Bounds = new Rectangle(1800 + (818 / 2) - (_cookingBtnTex.Width / 2), 900, 262, 109),
                OnClick = _ => CookingBtn_Click()
            });

            _serveBtn = new Button(new ButtonConfig
            {
                Texture = _serveBtnTex,
                Font = _font,
                Text = " ",
                Bounds = new Rectangle(3856, 262, 262, 109),
                OnClick = _ => ServeBtn_Click()
            });

            _logOrderBtn = new Button(new ButtonConfig
            {
                Texture = _logOrder,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(1200, 0, 94, 134),
                OnClick = _ => LogBtn_Click()
            });

            _okLogBtn = new Button(new ButtonConfig
            {
                Texture = _okLog,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(1450, 840, 300, 150),
                OnClick = _ => OkLogBtn_Click()
            });

            _contentLoaded = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _input.Update();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var state = Keyboard.GetState();
            var mouse = Mouse.GetState();

            Point mousePos = mouse.Position;
            DragDropManager.SetCamera(CameraPos);

            GamePlayScene._menuButton?.Update(gameTime);

            if (GamePlayScene.TimeStage <= 0)
            {
                GamePlayScene.TimeStage = GamePlayScene.TimeDefault;
                GamePlayScene.isEndLv = true;
            }

            if (GamePlayScene.isPaused)
            {
                GamePlayScene._homeButton?.Update(gameTime);
                GamePlayScene._exitButton?.Update(gameTime);
                GamePlayScene._resumeButton?.Update(gameTime);
                if (GamePlayScene.isClickExit)
                {
                    GamePlayScene._yesExit?.Update(gameTime);
                    GamePlayScene._noExit?.Update(gameTime);
                }
            }
            else if (!GamePlayScene.isEndLv)
            {
                GamePlayScene.TimePSec = 1.0f / 60.0f;
                GamePlayScene.TimeStage -= GamePlayScene.TimePSec;

                if (IsCountDownSteam)
                {
                    GameManager.CountSteam -= GamePlayScene.TimePSec;
                    if (GameManager.CountSteam <= 0f)
                    {
                        GameManager.CountSteam = 0.0f;
                        if (GameManager.ReadySteam && !GameManager.IsChangeFood && GameManager.Instance.Foods.Count > 0)
                        {
                            var food = GameManager.Instance.Foods[0];
                            GameManager.Instance.ChangeFood(food);
                            IsClickCook = false;
                        }
                    }
                }

                GamePlayScene._patienceMeter -= GamePlayScene._patienceDecreaseRate * dt;

                if (GamePlayScene._patienceMeter <= 0)
                {
                    GamePlayScene.countDia = 2;
                    BackToGame = true;
                }

                if (_currentSteam > 0 && IsClickCook)
                {
                    _currentSteam -= 3.56f;
                }
                else if (_currentSteam <= 0 && IsClickCook)
                {
                    _currentSteam = 0;
                }
            }
            else if (GamePlayScene.isEndLv)
            {
                GamePlayScene._OkButton?.Update(gameTime);
            }

            #region Scroll Camera
            _scroll = false;
            if (mouse.X <= _cameraLeftBoundary2) _cameraSpeed = 30f;
            else if (mouse.X <= _cameraLeftBoundary1 && mouse.X > _cameraLeftBoundary2) _cameraSpeed = 10f;

            if (mouse.X >= _cameraRightBoundary2) _cameraSpeed = 30f;
            else if (mouse.X >= _cameraRightBoundary1 && mouse.X < _cameraRightBoundary2) _cameraSpeed = 10f;

            if (mouse.X <= _cameraLeftBoundary1)
            {
                _scroll = true;
                CameraPos.X -= _cameraSpeed;
                if (CameraPos.X < 0) CameraPos.X = 0;
            }
            else if (mouse.X >= _cameraRightBoundary1)
            {
                _scroll = true;
                CameraPos.X += _cameraSpeed;
                if (CameraPos.X > 4200 - 1920) CameraPos.X = 4200 - 1920;
            }
            #endregion

            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            {
                GamePlayScene.instance.BackToMenuRequested = true;
            }

            _logOrderBtn.Update(gameTime);

            _oldState = state;
            _oldMouseState = mouse;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            MouseState mousePos = Mouse.GetState();
            spriteBatch.GraphicsDevice.Clear(Color.DarkSlateGray);

            spriteBatch.Begin();
            spriteBatch.Draw(_bg, -CameraPos, Color.White);

            spriteBatch.Draw(_table, new Vector2(304, 143) - CameraPos, Color.White);
            spriteBatch.Draw(_table2, new Vector2(3800 - _table2.Width, 143) - CameraPos, Color.White);

            foreach (var rect in _boxFilling)
            {
                var drawRect = new Rectangle(
                    rect.X - (int)CameraPos.X,
                    rect.Y - (int)CameraPos.Y,
                    rect.Width,
                    rect.Height);
                spriteBatch.Draw(BoxFilling, drawRect, Color.Transparent);
            }

            foreach (var rect in _boxDough)
            {
                var drawRect = new Rectangle(
                    rect.X - (int)CameraPos.X,
                    rect.Y - (int)CameraPos.Y,
                    rect.Width,
                    rect.Height);
                spriteBatch.Draw(BoxDough, drawRect, Color.Transparent);
            }

            GameManager.Instance.Draw(CameraPos);
            spriteBatch.End();

            spriteBatch.Begin();

            if (GameManager.ReadySteam)
            {
                if (GameManager.CountSteam > 0 && IsClickCook)
                {
                    spriteBatch.Draw(_steam2, new Vector2(1805, 145) - CameraPos, Color.White);
                    spriteBatch.Draw(_steamBar, new Vector2(2200 + (_steam2.Width / 2), 200) - CameraPos, new Rectangle(0, 0, 120, 610), Color.White);
                    spriteBatch.Draw(_steamBar, new Rectangle(2200 - (int)CameraPos.X + (_steam2.Width / 2), 204 - (int)CameraPos.Y, 120, (int)_currentSteam), new Rectangle(120, 4, 120, 606), Color.White);
                }
                else _currentSteam = _steamBar.Height - 4;

                _cookingBtn.Draw(gameTime, spriteBatch);
            }

            if (GameManager.HasFood)
            {
                _serveBtn.Draw(gameTime, spriteBatch);
            }

            #region UI Info
            spriteBatch.Draw(GamePlayScene.profile, new Vector2(0, 0), Color.White);
            int Days = 1;
            spriteBatch.Draw(GamePlayScene.dayBox, new Vector2(GamePlayScene.profile.Width + 10, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(GamePlayScene.profile.Width + 135, (GamePlayScene.menuBox.Height / 5) + 20), Color.Black);
            string Time = $"{(int)GamePlayScene.TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(GamePlayScene.profile.Width + 145, (GamePlayScene.menuBox.Height / 5) + 55), Color.Black);

            spriteBatch.Draw(GamePlayScene.moneyBox, new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 10, GamePlayScene.menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{GamePlayScene.TotalMoney}", new Vector2(GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + (GamePlayScene.moneyBox.Width / 2) + 35, (GamePlayScene.menuBox.Height / 5) + 36), Color.Black);

            Vector2 EmotionPos = new Vector2(GamePlayScene.moneyBox.Width + GamePlayScene.profile.Width + GamePlayScene.dayBox.Width + 10, GamePlayScene.menuBox.Height / 5);
            Vector2 percentPantiencePos = new Vector2(EmotionPos.X + 145, GamePlayScene.menuBox.Height / 5 + 36);
            string patienceText = $"{GamePlayScene._patienceMeter:0}%";
            spriteBatch.DrawString(_font, patienceText, percentPantiencePos, Color.Black);
            GamePlayScene.DrawEmotionIcon(_font, spriteBatch, EmotionPos);
            #endregion

            _logOrderBtn.Draw(gameTime, spriteBatch);
            if (_isLog)
            {
                spriteBatch.Draw(_logInfo, new Vector2(130, 160), Color.White);
                if (GamePlayScene.countDia == 3)
                {
                    spriteBatch.DrawString(_font, "1. " + GamePlayScene._currentCustomer?.Dia1, new Vector2(400, 300), Color.Black);
                    spriteBatch.DrawString(_font, "2. " + GamePlayScene._currentCustomer?.Dia2, new Vector2(400, 400), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(_font, "1. " + GamePlayScene._currentCustomer?.Dia1, new Vector2(400, 300), Color.Black);
                }
                _okLogBtn.Draw(gameTime, spriteBatch);
            }

            if (CameraPos.X < 4200 - 1920) spriteBatch.Draw(_arrowCam, new Vector2(1920 - _arrowCam.Width, 540), null, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.FlipHorizontally, 0f);
            if (CameraPos.X > 0) spriteBatch.Draw(_arrowCam, new Vector2(0, 540), Color.White);

            if (GamePlayScene.isPaused)
            {
                spriteBatch.Draw(GamePlayScene._rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);
                spriteBatch.DrawString(_font, "Paused", new Vector2(900, 300), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                GamePlayScene._resumeButton?.Draw(gameTime, spriteBatch);
                GamePlayScene._homeButton?.Draw(gameTime, spriteBatch);
                GamePlayScene._exitButton?.Draw(gameTime, spriteBatch);
                if (GamePlayScene.isClickExit)
                {
                    spriteBatch.Draw(GamePlayScene.logExit, new Rectangle(448, 263, 1024, 534), Color.White);
                    GamePlayScene._yesExit?.Draw(gameTime, spriteBatch);
                    GamePlayScene._noExit?.Draw(gameTime, spriteBatch);
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
                GamePlayScene._OkButton?.Draw(gameTime, spriteBatch);
            }
            GamePlayScene._menuButton?.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void CookingBtn_Click()
        {
            IsCountDownSteam = true;
            IsClickCook = true;
        }

        private void ServeBtn_Click()
        {
            if (GameManager.HasFood)
            {
                ServeRequest = true;
                GamePlayScene.served = true;
                GameManager.Instance.ServeFood();
            }
        }

        private void LogBtn_Click()
        {
            _isLog = !_isLog;
        }

        private void OkLogBtn_Click()
        {
            _isLog = false;
        }

        public void OnEnter() { }
        public void OnExit() { }
        public void OnResize(int width, int height) { }
    }
}