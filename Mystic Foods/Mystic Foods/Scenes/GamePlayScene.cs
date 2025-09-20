using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Systems;
using Mystic_Foods.Time;
using System;

namespace Mystic_Foods
{
    public class GamePlayScene : IGameScene
    {
        public static float TimeStage = 721f;
        public static float TimePSec;

        private SpriteFont _font;
        public bool BackToMenuRequested = false;
        public bool DnDRequested = false;
        private KeyboardState _oldState;
        private CustomerManager _customerManager;
        public static Customer _currentCustomer;
        private ContentManager _contentManager;

        //public static bool _what = false; เอาออก เพราะจะติดตรงการพูด dialogue

        //pause
        public static bool isPaused = false;
        public static Button _pauseButton;
        public static Button _menuButton;
        public Button _yesButton;
        public Button _whatButton;

        // ระบบ Patience Meter
        private float _patienceMeter;
        private const float _patienceMeterStart = 100f;
        private float _patienceReduceTimer = 0f;
        private const float patienceInterval = 0.2f;
        Texture2D _textureHappy;
        Texture2D _textureNeutral;
        Texture2D _textureGrumpy;

        public static Texture2D bg, counter, bgBox, dayBox, moneyBox, menuBox, profile;
        public static Texture2D whatButton, yesButton, diaBox;
        public static Texture2D _happy, _natural, _angry;

        public static Texture2D _rectTexture;

        public static float TotalMoney = 100.0f;
        public static float pay;
        public static float price = 120.0f;
        public static float weight = 0.0f;

        public GamePlayScene(CustomerManager cm)
        {
            _customerManager = cm;
            _currentCustomer = _customerManager.GetNextCustomer();
            _patienceMeter = _patienceMeterStart;
        }

        public GamePlayScene() {}

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _contentManager = content;
            _font = content.Load<SpriteFont>("MainFont");
            LoadCustomerTextures();

            _rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _rectTexture.SetData(new[] { Color.White });

            bg = content.Load<Texture2D>("Environments/BG/orderBG_morning");
            counter = content.Load<Texture2D>("Environments/Counter/orderCounter_morning");
            bgBox = content.Load<Texture2D>("Etc/OutLine");
            dayBox = content.Load<Texture2D>("Etc/Day");
            moneyBox = content.Load<Texture2D>("Etc/Money");
            _happy = content.Load<Texture2D>("Emote/EmoteHappy");
            _natural = content.Load<Texture2D>("Emote/EmoteNatural");
            _angry = content.Load<Texture2D>("Emote/EmoteAngry");
            menuBox = content.Load<Texture2D>("Emote/EmoteMenu");
            profile = content.Load<Texture2D>("Etc/Cat1");

            whatButton = content.Load<Texture2D>("DialogueUI/WhatButton");
            yesButton = content.Load<Texture2D>("DialogueUI/YesButton");
            diaBox = content.Load<Texture2D>("DialogueUI/DialogueBox");

            _pauseButton = new Button(menuBox, _font, " ", new Rectangle(1670, 10, 231, 162));
            _pauseButton.Click += PauseButton_Click;
            _menuButton = new Button(whatButton, _font, " ", new Rectangle(900, 500, 128, 63));
            _menuButton.Click += MenuButton_Click;
            _yesButton = new Button(yesButton, _font, " ", new Rectangle(1400, 500, 128, 63));
            _yesButton.Click += YesButton_Click;
            _whatButton = new Button(whatButton, _font, " ", new Rectangle(1550, 500, 128, 63));
            _whatButton.Click += WhatButton_Click;
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

            // random, reset Patience
            if (state.IsKeyDown(Keys.Space) && _oldState.IsKeyUp(Keys.Space))
            {
                _currentCustomer = _customerManager.GetNextCustomer();
                _patienceMeter = _patienceMeterStart;
                LoadCustomerTextures();
                GameManager.countDia = 2;
            }

            //Check time out to back to mainmenu scene
            if (TimeStage <= 0)
            {
                BackToMenuRequested = true;
                TimeStage = 721f;
            }

            // ESC
            //if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            //{
            //    BackToMenuRequested = true;
            //    TimeStage = 721f;
            //}

            //Button
            _pauseButton.Update();
            _menuButton.Update();
            _yesButton.Update();
            _whatButton.Update();

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

            if (!isPaused)
            {
                //TimeStage every scene
                TimePSec = 1.0f / 60.0f;
                TimeStage -= TimePSec;

                // Patience reduce logic
                _patienceReduceTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_patienceReduceTimer >= patienceInterval && _currentCustomer != null)
                {
                    float patiencePerSecond = _currentCustomer.Patience;
                    float reduceAmount = patiencePerSecond * patienceInterval; // reducing equation

                    _patienceMeter -= reduceAmount;
                    if (_patienceMeter < 0) _patienceMeter = 0;
                    _patienceReduceTimer = 0;
                }
                //Customer leave
                if (_patienceMeter <= 0)
                {
                    _currentCustomer = _customerManager.GetNextCustomer();
                    _patienceMeter = _patienceMeterStart;
                    LoadCustomerTextures();
                }
            }
            _oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSeaGreen);
            spriteBatch.Begin();
            spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);
            spriteBatch.Draw(bgBox, new Vector2(0, 0), Color.White);

            /*
            string text = "Game Scene!\nPress ESC to menu\nPress SPACE to random customer";
            Vector2 size = _font.MeasureString(text);
            spriteBatch.DrawString(
                _font,
                text,
                new Vector2((800 - size.X) / 2, 60),
                Color.Black);
             */
            // Show Customer data
            if (_currentCustomer != null)
            {

                //Customer stats
                //string cust = $"Name: {_currentCustomer.Name}\nPatience Stat: {_currentCustomer.Patience:0.00}";
                string cust = $"Name: {_currentCustomer.Name}";
                Vector2 custPos = new Vector2(100, 180);
                spriteBatch.DrawString(_font, cust, custPos, Color.DarkBlue);

                // แสดงค่า Patience Meter
                Vector2 EmotionPos = new Vector2(850, menuBox.Height / 5);//สำหรับตำแหน่งของอีโมจิอารมณ์
                string patienceText = $"{_patienceMeter:0}%";
                spriteBatch.DrawString(_font, patienceText, new Vector2(EmotionPos.X + (_happy.Width / 5), EmotionPos.Y + _happy.Height), Color.Black);

                //Draw Customer & Patience
                DrawEmotion(spriteBatch, EmotionPos);

                //use rectangle to adjust scale
                //0.9(855, 972) 0.8(760, 864)
                
            }

            #region UI-info

            //profile
            //spriteBatch.Draw(profile, new Vector2(0, 0), Color.White);
            //Date and Time
            int Days = 1;//สำหรับเปลี่ยนวันตามเงื่อนไขต่างๆที่เราต้องการ
            spriteBatch.Draw(dayBox, new Vector2(profile.Width + 10, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(profile.Width + 110, (menuBox.Height / 5) + 20), Color.Black);

            spriteBatch.Draw(moneyBox, new Vector2(1920 - menuBox.Width - moneyBox.Width - 50, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"{TotalMoney}", new Vector2(1920 - menuBox.Width - (moneyBox.Width / 2) - 25, (menuBox.Height / 5) + (moneyBox.Height / 4) + 10), Color.Yellow);
            //table pos recom pos.Y 890++
            spriteBatch.Draw(counter, new Vector2(0, 1080 - counter.Height), Color.White);

            string Time = $"{(int)TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(profile.Width + 110, (menuBox.Height / 5) + 55), Color.Blue);
            #endregion

            #region Dialouge
            //Dia
            spriteBatch.Draw(diaBox, new Vector2(900, 200), Color.White);
            Vector2 diaPos = new Vector2(900, 200);

            _yesButton.Draw(spriteBatch);

            switch (GameManager.countDia)
            {
                case 0:
                    spriteBatch.DrawString(_font, _currentCustomer.DiaWrong, new Vector2(1000, 300), Color.Black);
                    _whatButton.Draw(spriteBatch);
                    break;
                case 1:
                    spriteBatch.DrawString(_font, _currentCustomer.DiaCurrect, new Vector2(1000, 300), Color.Black);
                    break;
                case 2:
                    spriteBatch.DrawString(_font, _currentCustomer.Dia1, new Vector2(1000, 300), Color.Black);
                    _whatButton.Draw(spriteBatch);

                    break;
                case 3:
                    spriteBatch.DrawString(_font, _currentCustomer.Dia2, new Vector2(1000, 300), Color.Black);

                    break;
                case -1:
                    spriteBatch.DrawString(_font, "", new Vector2(1000, 300), Color.Black);
                    break;
            }

            #endregion

            /*
            spriteBatch.DrawString(_font, $" IdOrder : {_currentCustomer.IdOrder}", new Vector2(1000, diaBoxPos.Y + (diaBoxPos.Y / 2) + 100), Color.Black);
            spriteBatch.DrawString(_font, $" CurrectOrder : {GameManager.IsCurrectOrder}", new Vector2(1000, diaBoxPos.Y + (diaBoxPos.Y / 2) + 200), Color.Black);

            string TimeS = $"\nTimePerSec: {TimePSec}";
            */
            spriteBatch.DrawString(_font, $"Count Dialogue : {GameManager.countDia}", new Vector2(100, 300), Color.Blue);
            //spriteBatch.DrawString(_font, $"_what : {_what}", new Vector2(100, 400), Color.Blue);


            if (isPaused)
            {
                spriteBatch.Draw(_rectTexture, new Rectangle(0, 0, 1920, 1080), Color.Black * 0.5f);

                //DrawString(SpriteFont font, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
                spriteBatch.DrawString(_font, "Paused", new Vector2(900, 300), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0f);
                _menuButton.Draw(spriteBatch);
            }
            _pauseButton.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void PauseButton_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
        }
        public void MenuButton_Click(Object sender, EventArgs e)
        {
            //reset Scene
            _currentCustomer = _customerManager.GetNextCustomer();
            _patienceMeter = _patienceMeterStart;
            LoadCustomerTextures();
            TimeStage = 721f;
            isPaused = false;

            BackToMenuRequested = true;
        }
        public void DrawEmotion(SpriteBatch spriteBatch, Vector2 EmotionPos)
        {
            float patiencePerc = _patienceMeter / _patienceMeterStart;
            Texture2D drawTexture;
            if (patiencePerc >= 2f / 3f)
            {
                drawTexture = _textureHappy;
                spriteBatch.Draw(_happy, EmotionPos, Color.White);
                weight = 1.00f;
            }
            else if (patiencePerc >= 1f / 3f)
            {
                drawTexture = _textureNeutral;
                spriteBatch.Draw(_natural, EmotionPos, Color.White);
                weight = 0.75f;
            }
            else
            {
                drawTexture = _textureGrumpy;
                spriteBatch.Draw(_angry, EmotionPos, Color.White);
                weight = 0.50f;
            }

            Rectangle destinationRectangle = new Rectangle(300, 75, 760, 864);
            spriteBatch.Draw(drawTexture, destinationRectangle, Color.White);
        }
        private void YesButton_Click(Object sender, EventArgs e)
        {
            GameManager.countDia = -1;
            DnDRequested = true;
        }
        private void WhatButton_Click(Object sender, EventArgs e)
        {
            GameManager.countDia = 3;
        }
    }
}