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
    public class LevelSelectScene : IScene
    {
        private readonly IGraphicsService _graphics;
        private readonly IInputService _input;
        private readonly Container _container;

        private SpriteFont _font;
        public bool MenuRequest { get; private set; }
        public bool GameplayRequest { get; private set; }

        private Texture2D _bg;
        private Texture2D _dayBtnTex, _duskBtnTex, _nightBtnTex, _homeBtnTex;
        private Button _dayButton, _duskButton, _nightButton, _homeButton;

        public string Name { get; } = "LevelSelect";
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; } = true;

        public LevelSelectScene(Container container)
        {
            _container = container;
            _graphics = container.Resolve<IGraphicsService>();
            _input = container.Resolve<IInputService>();
        }

        public void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("MainFont");
            _bg = content.Load<Texture2D>("Environments/BG/MenuBG");

            _homeBtnTex = content.Load<Texture2D>("Etc/option_exit");
            _dayBtnTex = content.Load<Texture2D>("LevelUI/Morning");
            _duskBtnTex = content.Load<Texture2D>("LevelUI/Evening");
            _nightBtnTex = content.Load<Texture2D>("LevelUI/Night");

            _dayButton = new Button(new ButtonConfig
            {
                Texture = _dayBtnTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(240, 300, 360, 640),
                OnClick = _ => { GamePlayScene.CurrentPhase = GamePlayScene.DayPhase.Dawn; GameplayRequest = true; }
            });

            _duskButton = new Button(new ButtonConfig
            {
                Texture = _duskBtnTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(780, 300, 360, 640),
                OnClick = _ => { GamePlayScene.CurrentPhase = GamePlayScene.DayPhase.Dusk; GameplayRequest = true; }
            });

            _nightButton = new Button(new ButtonConfig
            {
                Texture = _nightBtnTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(1320, 300, 360, 640),
                OnClick = _ => { GamePlayScene.CurrentPhase = GamePlayScene.DayPhase.Night; GameplayRequest = true; }
            });

            _homeButton = new Button(new ButtonConfig
            {
                Texture = _homeBtnTex,
                Font = _font,
                Text = "",
                Bounds = new Rectangle(50, 50, 80, 100),
                OnClick = _ => MenuRequest = true
            });
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _input.Update();

            _dayButton.Update(gameTime);
            _duskButton.Update(gameTime);
            _nightButton.Update(gameTime);
            _homeButton.Update(gameTime);

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                MenuRequest = true;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            _graphics.Begin();
            _graphics.Draw(_bg, new Rectangle(0, 0, 1920, 1080), null, Color.White);

            string text = "Choose the opening hours";
            float scale = 3.0f;
            Vector2 textSize = _font.MeasureString(text) * scale;
            int screenWidth = _graphics.Viewport.Width;
            Vector2 position = new Vector2((screenWidth - textSize.X) / 2f, 100);

            _graphics.DrawString(_font, text, position, Color.White, 0f, Vector2.Zero, scale);

            _dayButton.Draw(gameTime, _graphics.SpriteBatch);
            _duskButton.Draw(gameTime, _graphics.SpriteBatch);
            _nightButton.Draw(gameTime, _graphics.SpriteBatch);
            _homeButton.Draw(gameTime, _graphics.SpriteBatch);
            _graphics.End();
        }

        public void OnEnter()
        {
            MenuRequest = GameplayRequest = false;
        }

        public void OnExit() { }
        public void OnResize(int width, int height) { }
    }
}