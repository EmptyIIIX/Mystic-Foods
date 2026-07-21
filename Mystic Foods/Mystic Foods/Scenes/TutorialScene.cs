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
    public class TutorialScene : IScene
    {
        private readonly Container _container;
        private readonly IGraphicsService _graphics;
        private readonly IInputService _input;

        private SpriteFont _font;
        private ContentManager _content;

        private Texture2D _page1, _page2, _page3, _page4;
        private Texture2D _nextPageTex, _backPageTex, _exitPageTex, _topicPointTex;
        private Button _nextBtn, _backBtn, _exitBtn;

        public int CurrentPage { get; private set; } = 1;
        public int MaxPage { get; } = 4;

        public string Name => "Tutorial";
        public bool IsActive { get; set; }
        public bool IsVisible { get; set; } = true;

        public TutorialScene(Container container)
        {
            _container = container;
            _graphics = container.Resolve<IGraphicsService>();
            _input = container.Resolve<IInputService>();
        }

        public void LoadContent(ContentManager content)
        {
            _content = content;
            _font = content.Load<SpriteFont>("MainFont");

            _page1 = content.Load<Texture2D>("Tutorial/levelscene");
            _page2 = content.Load<Texture2D>("Tutorial/หนังสือที่จะขึ้นก่อนเริ่มเกม");
            _page3 = content.Load<Texture2D>("Tutorial/หนังสือแบบเลือกดูประวัติได้");
            _page4 = content.Load<Texture2D>("Tutorial/levelscene");

            _nextPageTex = content.Load<Texture2D>("Tutorial/nextpage");
            _backPageTex = content.Load<Texture2D>("Tutorial/backpage");
            _exitPageTex = content.Load<Texture2D>("Tutorial/exitpage");
            _topicPointTex = content.Load<Texture2D>("Tutorial/topicPoint");

            _nextBtn = new Button(new ButtonConfig
            {
                Texture = _nextPageTex, Font = _font, Text = "",
                Bounds = new Rectangle(1674, 838, 136, 134),
                OnClick = _ => NextPage()
            });

            _backBtn = new Button(new ButtonConfig
            {
                Texture = _backPageTex, Font = _font, Text = "",
                Bounds = new Rectangle(110, 838, 136, 134),
                OnClick = _ => PrevPage()
            });

            _exitBtn = new Button(new ButtonConfig
            {
                Texture = _exitPageTex, Font = _font, Text = "",
                Bounds = new Rectangle(1720, 106, 90, 79),
                OnClick = _ => ExitTutorial()
            });
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            _input.Update();

            CurrentPage = Math.Clamp(CurrentPage, 1, MaxPage);

            if (!Game1.wasTutorial)
            {
                _nextBtn.Update(gameTime);
                _backBtn.Update(gameTime);
                if (CurrentPage == MaxPage) _exitBtn.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            _graphics.Begin();

            Texture2D currentPageTex = CurrentPage switch
            {
                1 => _page1,
                2 => _page2,
                3 => _page3,
                4 => _page4,
                _ => _page1
            };

            spriteBatch.Draw(currentPageTex, Vector2.Zero, Color.White);

            if (CurrentPage < MaxPage) _nextBtn.Draw(gameTime, spriteBatch);
            if (CurrentPage > 1) _backBtn.Draw(gameTime, spriteBatch);
            if (CurrentPage == MaxPage) _exitBtn.Draw(gameTime, spriteBatch);

            _graphics.End();
        }

        public void OnEnter() { CurrentPage = 1; }
        public void OnExit() { }
        public void OnResize(int width, int height) { }

        private void NextPage() => CurrentPage++;
        private void PrevPage() => CurrentPage--;
        private void ExitTutorial() => Game1.wasTutorial = true;
    }
}