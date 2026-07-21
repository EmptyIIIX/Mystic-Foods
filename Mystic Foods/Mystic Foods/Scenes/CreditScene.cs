using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Core.DI;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.Scenes;
using Mystic_Foods.Core.UI;

namespace Mystic_Foods.Scenes
{
    public class CreditScene : IScene
    {
        private readonly Container _container;
        private readonly IGraphicsService _graphics;
        private readonly IInputService _input;

        private SpriteFont _font1, _font2, _font3;
        private ContentManager _content;

        private Texture2D _creditBg;
        private Button _exitBtn;

        public static bool ExitToMenu { get; private set; } = false;
        public static bool IsCreditActive { get; private set; } = true;

        public string Name => "Credit";
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; } = true;

        public CreditScene(Container container)
        {
            _container = container;
            _graphics = container.Resolve<IGraphicsService>();
            _input = container.Resolve<IInputService>();
        }

        public void LoadContent(ContentManager content)
        {
            _content = content;
            _font1 = content.Load<SpriteFont>("BoldFont");
            _font2 = content.Load<SpriteFont>("MainFont");
            _font3 = content.Load<SpriteFont>("DiaFont");

            _creditBg = content.Load<Texture2D>("Credit/credit_bg");

            _exitBtn = new Button(new ButtonConfig
            {
                Texture = content.Load<Texture2D>("Credit/exit_btn"),
                Font = _font2,
                Text = "",
                Bounds = new Rectangle(1700, 950, 100, 50),
                OnClick = _ => ExitToMenu = true
            });
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            _input.Update();
            _exitBtn.Update(gameTime);

            if (ExitToMenu)
            {
                IsCreditActive = false;
                ExitToMenu = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            _graphics.Begin();

            spriteBatch.Draw(_creditBg, Vector2.Zero, Color.White);

            // Draw credit text
            spriteBatch.DrawString(_font1, "MYSTIC FOODS", new Vector2(500, 100), Color.Gold, 0, Vector2.Zero, 2f, SpriteEffects.None, 0);
            spriteBatch.DrawString(_font2, "Developed by CAMT Game Dev Team", new Vector2(450, 200), Color.White);
            spriteBatch.DrawString(_font2, "Programmers: Team Mystic", new Vector2(450, 250), Color.White);
            spriteBatch.DrawString(_font2, "Artists: Team Mystic", new Vector2(450, 300), Color.White);
            spriteBatch.DrawString(_font2, "Music: Team Mystic", new Vector2(450, 350), Color.White);
            spriteBatch.DrawString(_font3, "Special thanks to all contributors", new Vector2(400, 450), Color.LightGray);

            _exitBtn.Draw(gameTime, spriteBatch);

            _graphics.End();
        }

        public void OnEnter()
        {
            ExitToMenu = false;
            IsCreditActive = true;
        }

        public void OnExit()
        {
            IsCreditActive = false;
        }

        public void OnResize(int width, int height) { }
    }
}