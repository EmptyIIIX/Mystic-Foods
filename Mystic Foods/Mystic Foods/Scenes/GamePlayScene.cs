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
        private float TimeStage = 721f;
        private float TimePSec;

        private SpriteFont _font;
        public bool BackToMenuRequested = false;
        public bool DnDRequested = false;
        private KeyboardState _oldState;
        private CustomerManager _customerManager;
        private Customer _currentCustomer;
        private ContentManager _contentManager;

        //pause
        private bool isPaused = false;
        private Button _pauseButton;
        private Button _menuButton;

        // ระบบ Patience Meter
        private float _patienceMeter;
        private const float _patienceMeterStart = 100f;
        private float _patienceReduceTimer = 0f;
        private const float patienceInterval = 0.2f;
        Texture2D _textureHappy;
        Texture2D _textureNeutral;
        Texture2D _textureGrumpy;

        Texture2D bg, table, bgBox, dayBox, moneyBox, menuBox, profile;
        Texture2D wButton;
        Texture2D _happy, _natural, _angry;

        private Texture2D _rectTexture;

        public GamePlayScene(CustomerManager cm)
        {
            _customerManager = cm;
            _currentCustomer = _customerManager.GetNextCustomer();
            _patienceMeter = _patienceMeterStart;
        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            _contentManager = content;
            _font = content.Load<SpriteFont>("MainFont");
            LoadCustomerTextures();

            _rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            _rectTexture.SetData(new[] { Color.White });

            bg = content.Load<Texture2D>("Environments/BG/orderBG_morning");
            table = content.Load<Texture2D>("Environments/Counter/orderCounter_morning");
            bgBox = content.Load<Texture2D>("Etc/OutLine");
            dayBox = content.Load<Texture2D>("Etc/Day");
            moneyBox = content.Load<Texture2D>("Etc/Money");
            _happy = content.Load<Texture2D>("Emote/EmoteHappy");
            _natural = content.Load<Texture2D>("Emote/EmoteNatural");
            _angry = content.Load<Texture2D>("Emote/EmoteAngry");
            menuBox = content.Load<Texture2D>("Emote/EmoteMenu");
            profile = content.Load<Texture2D>("Etc/Cat1");

            wButton = content.Load<Texture2D>("DialogueUI/WhatButton");

            _pauseButton = new Button(menuBox, _font, " ", new Rectangle(1670, 10, menuBox.Width, menuBox.Height));
            _pauseButton.Click += PauseButton_Click;
            _menuButton = new Button(wButton, _font, " ", new Rectangle(900, 500, 128, 63));
            _menuButton.Click += MenuButton_Click;
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
            int Orders = 0;
            // random, reset Patience
            if (state.IsKeyDown(Keys.Space) && _oldState.IsKeyUp(Keys.Space))
            {
                _currentCustomer = _customerManager.GetNextCustomer();
                _patienceMeter = _patienceMeterStart;
                LoadCustomerTextures();
            }

            //Check time out to back to mainmenu scene
            if (TimeStage <= 0)
            {
                BackToMenuRequested = true;
                TimeStage = 721f;
            }

            // ESC
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            {
                BackToMenuRequested = true;
                TimeStage = 721f;
            }

            //Button
            _pauseButton.Update();
            _menuButton.Update();

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

            string text = "Game Scene!\nPress ESC to menu\nPress SPACE to random customer";
            Vector2 size = _font.MeasureString(text);
            spriteBatch.DrawString(
                _font,
                text,
                new Vector2((800 - size.X) / 2, 60),
                Color.Black);

            // Show Customer data
            if (_currentCustomer != null)
            {
                spriteBatch.Draw(bg, new Vector2(0, 0), Color.White);
                spriteBatch.Draw(bgBox, new Vector2(0, 0), Color.White);

                string cust = $"Name: {_currentCustomer.Name}\nPatience Stat: {_currentCustomer.Patience:0.00}";
                Vector2 custPos = new Vector2(100, 180);
                spriteBatch.DrawString(_font, cust, custPos, Color.DarkBlue);

                /*
                // Show Patience Meter แบบ progress bar
                int barX = 100, barY = 430, barW = 300, barH = 20;
                float meterPerc = _patienceMeter / _patienceMeterStart;
                Rectangle patienceRect = new Rectangle(barX, barY, (int)(barW * meterPerc), barH);

                // Texture for progress bar
                Texture2D rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                rectTexture.SetData(new[] { Color.White });
                spriteBatch.Draw(rectTexture, new Rectangle(barX, barY, barW, barH), Color.Gray * 0.4f);
                spriteBatch.Draw(rectTexture, patienceRect, Color.OrangeRed);
                */

                // แสดงค่า Patience Meter
                Vector2 EmotionPos = new Vector2(850, menuBox.Height / 5);//สำหรับตำแหน่งของอีโมจิอารมณ์
                string patienceText = $"{_patienceMeter:0}%";
                spriteBatch.DrawString(_font, patienceText, new Vector2(EmotionPos.X + (_happy.Width / 5), EmotionPos.Y + _happy.Height), Color.Black);

                //Draw Customer & Patience
                Texture2D drawTexture = _textureHappy;
                float patiencePerc = _patienceMeter / _patienceMeterStart;
                if (patiencePerc >= 2f / 3f)
                {
                    drawTexture = _textureHappy;
                    spriteBatch.Draw(_happy, EmotionPos, Color.White);
                }
                else if (patiencePerc >= 1f / 3f)
                {
                    drawTexture = _textureNeutral;
                    spriteBatch.Draw(_natural, EmotionPos, Color.White);
                }
                else
                {
                    drawTexture = _textureGrumpy;
                    spriteBatch.Draw(_angry, EmotionPos, Color.White);
                }

                //use rectangle to adjust scale
                //0.9(855, 972) 0.8(760, 864)
                Rectangle destinationRectangle = new Rectangle(300, 75, 760, 864);
                spriteBatch.Draw(drawTexture, destinationRectangle, Color.White);
            }

            //profile
            spriteBatch.Draw(profile, new Vector2(0, 0), Color.White );

            //Date and Time
            int Days = 2;//สำหรับเปลี่ยนวันตามเงื่อนไขต่างๆที่เราต้องการ
            string Time = $"{(int)TimeStage}";
            spriteBatch.Draw(dayBox, new Vector2(profile.Width + 10, menuBox.Height / 5), Color.White);
            spriteBatch.DrawString(_font, $"Day {Days}", new Vector2(profile.Width + 110, (menuBox.Height / 5) + 20), Color.Black);
            spriteBatch.DrawString(_font, Time, new Vector2(profile.Width + 110, (menuBox.Height / 5) + 55), Color.Blue);

            //UI bar
            spriteBatch.Draw(moneyBox, new Vector2(1920 - menuBox.Width - moneyBox.Width - 50, menuBox.Height / 5), Color.White);

            //Environment and table pos recom pos.Y 890++
            spriteBatch.Draw(table, new Vector2(0, 1080 - 152), Color.White);

            /*
            string TimeS = $"\nTimePerSec: {TimePSec}";
            spriteBatch.DrawString(_font, TimeS, new Vector2(100, 600), Color.Blue);
            */

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

        private void PauseButton_Click(object sender, EventArgs e)
        {
            isPaused = !isPaused;
        }
        private void MenuButton_Click(Object sender, EventArgs e)
        {
            //reset Scene
            _currentCustomer = _customerManager.GetNextCustomer();
            _patienceMeter = _patienceMeterStart;
            LoadCustomerTextures();
            TimeStage = 721f;
            isPaused = false;

            BackToMenuRequested = true;
        }
    }
}