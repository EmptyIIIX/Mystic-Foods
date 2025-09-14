using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Managers;
using Mystic_Foods.Time;

namespace Mystic_Foods
{
    public class GamePlayScene : IGameScene
    {
        private float TimeStage = 721f;
        private float TimePSec;

        private SpriteFont _font;
        public bool BackToMenuRequested = false;
        private KeyboardState _oldState;
        private CustomerManager _customerManager;
        private Customer _currentCustomer;
        Texture2D _customerTexture;
        private ContentManager _contentManager;

        // ระบบ Patience Meter
        private float _patienceMeter;
        private const float _patienceMeterStart = 144f;
        private float _patienceReduceTimer = 0f;
        private const float patienceInterval = 0.2f;
        Texture2D _textureHappy;
        Texture2D _textureNeutral;
        Texture2D _textureGrumpy;

        Texture2D bg, table;
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

            //TimeStage every scene
            TimePSec = 1.0f / 60.0f;
            TimeStage -= TimePSec;

            // ESC
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
            {
                BackToMenuRequested = true;
                TimeStage = 721f;
            }

            //Check time out to back to mainmenu scene
            if(TimeStage <= 0)
            {
                BackToMenuRequested = true;
                TimeStage = 721f;
            }

            // random, reset Patience
            if (state.IsKeyDown(Keys.Space) && _oldState.IsKeyUp(Keys.Space))
            {
                _currentCustomer = _customerManager.GetNextCustomer();
                _patienceMeter = _patienceMeterStart;
                LoadCustomerTextures();
            }

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

                string cust = $"Name: {_currentCustomer.Name}\nPatience Stat: {_currentCustomer.Patience:0.00}";
                Vector2 custPos = new Vector2(100, 180);
                spriteBatch.DrawString(_font, cust, custPos, Color.DarkBlue);

                // แสดงค่า Patience Meter
                string patienceText = $"Patience Left: {_patienceMeter:0}";
                spriteBatch.DrawString(_font, patienceText, new Vector2(100, 400), Color.Red);

                // Show Patience Meter แบบ progress bar
                int barX = 100, barY = 430, barW = 300, barH = 20;
                float meterPerc = _patienceMeter / _patienceMeterStart;
                Rectangle patienceRect = new Rectangle(barX, barY, (int)(barW * meterPerc), barH);

                // Texture for progress bar
                Texture2D rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                rectTexture.SetData(new[] { Color.White });
                spriteBatch.Draw(rectTexture, new Rectangle(barX, barY, barW, barH), Color.Gray * 0.4f);
                spriteBatch.Draw(rectTexture, patienceRect, Color.OrangeRed);

                //Draw Customer
                Texture2D drawTexture = _textureHappy;
                float patiencePerc = _patienceMeter / _patienceMeterStart;
                if (patiencePerc >= 2f / 3f)
                    drawTexture = _textureHappy;
                else if (patiencePerc >= 1f / 3f)
                    drawTexture = _textureNeutral;
                else
                    drawTexture = _textureGrumpy;

                //use rectangle to adjust scale
                //0.9(855, 972) 0.8(760, 864)
                Rectangle destinationRectangle = new Rectangle(300, 50, 760, 864);
                spriteBatch.Draw(drawTexture, destinationRectangle, Color.White);
            }

            //table pos recom pos.Y 890++
            spriteBatch.Draw(table, new Vector2(0, 890), Color.White);

            string Time = $"\nTimeStage: {(int)TimeStage}";
            spriteBatch.DrawString(_font, Time, new Vector2(100, 500), Color.Blue);

            string TimeS = $"\nTimePerSec: {TimePSec}";
            spriteBatch.DrawString(_font, TimeS, new Vector2(100, 600), Color.Blue);

            spriteBatch.End();
        }
    }
}