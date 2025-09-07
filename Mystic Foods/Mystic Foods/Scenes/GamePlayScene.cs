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
        private SpriteFont _font;
        public bool BackToMenuRequested = false;
        private KeyboardState _oldState;
        private CustomerManager _customerManager;
        private Customer _currentCustomer;

        Texture2D morning, sunset, midnight;
        // Morning
        Texture2D pink_morning, man_morning;
        // Sunset
        //Midnight

        // ระบบ Patience Meter
        private float _patienceMeter;
        private const float _patienceMeterStart = 144f;
        private float _patienceReduceTimer = 0f;
        private const float patienceInterval = 0.2f; // reduce frequency

        //private Morning morning;
        private CustomerType customerType;
        int aaa;
        public GamePlayScene(CustomerManager cm)
        {
            _customerManager = cm;
            _currentCustomer = _customerManager.GetRandomCustomer();
            _patienceMeter = _patienceMeterStart;
        }

        public void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            // Load font
            _font = content.Load<SpriteFont>("MainFont");
            // Load Environments
            //morning = content.Load<Texture2D>("Environments/view_morning");
            //sunset = content.Load<Texture2D>("Environments/view_sunset");
            //midnight = content.Load<Texture2D>("Environments/view_midnight");
            // Load customers
            pink_morning = content.Load<Texture2D>("Human/pink_morning");
            man_morning = content.Load<Texture2D>("Human/man_morning");
        }

        public void Update(GameTime gameTime)
        {
            var state = Keyboard.GetState();

            // ESC
            if (state.IsKeyDown(Keys.Escape) && _oldState.IsKeyUp(Keys.Escape))
                BackToMenuRequested = true;

            // random, reset Patience
            if (state.IsKeyDown(Keys.Space) && _oldState.IsKeyUp(Keys.Space))
            {
                _currentCustomer = _customerManager.GetRandomCustomer();
                _patienceMeter = _patienceMeterStart;
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

            _oldState = state;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.GraphicsDevice.Clear(Color.DarkSeaGreen);
            //backgrounds = Backgrounds.Morning;
            customerType = CustomerType.pinkMorning;
            spriteBatch.Begin();

            //switch (backgrounds)// Draw backgrounds
            //{
            //    case Backgrounds.Morning:
            //        spriteBatch.Draw(morning, new Vector2(0, 0), Color.White);
            //        break;

            //    case Backgrounds.Sunset:
            //        aaa = 2 + 1;
            //        break;

            //    case Backgrounds.Midnight:
            //        aaa = 3 + 1;
            //        break;

            //    default:
            //        aaa = aaa + 1;
            //        break;
            //}

            switch (customerType)
            {
                case CustomerType.pinkMorning:
                    spriteBatch.Draw(pink_morning, new Vector2(400, 0), Color.White);
                    break;
                case CustomerType.pinkSunset:
                    aaa += 1;
                    break;
                default:
                    break;
            }

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
                string cust = $"Name: {_currentCustomer.Name}\nPatience Stat: {_currentCustomer.Patience:0.00}\nPreference: {_currentCustomer.Preference}\nVIP: {_currentCustomer.IsVIP}\nMood: {_currentCustomer.Mood}";
                Vector2 custPos = new Vector2(100, 180);
                spriteBatch.DrawString(_font, cust, custPos, Color.DarkBlue);

                // แสดงค่า Patience Meter
                string patienceText = $"Patience Left: {_patienceMeter:0}";
                spriteBatch.DrawString(_font, patienceText, new Vector2(100, 320), Color.Red);

                // Show Patience Meter แบบ progress bar
                int barX = 300, barY = 350, barW = 300, barH = 20;
                float meterPerc = _patienceMeter / _patienceMeterStart;
                Rectangle patienceRect = new Rectangle(barX, barY, (int)(barW * meterPerc), barH);

                // Texture for progress bar
                Texture2D rectTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                rectTexture.SetData(new[] { Color.White });
                spriteBatch.Draw(rectTexture, new Rectangle(barX, barY, barW, barH), Color.Gray * 0.4f);
                spriteBatch.Draw(rectTexture, patienceRect, Color.OrangeRed);
            }

            spriteBatch.End();
        }
    }
}