using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Core.Services;
using Mystic_Foods.Core.UI;

namespace Mystic_Foods.Core.Scenes
{
    /// <summary>
    /// Base scene class implementing Template Method pattern
    /// Provides common functionality for all scenes
    /// </summary>
    public abstract class BaseScene : IScene
    {
        protected readonly IInputService _input;
        protected readonly IGraphicsService _graphics;
        protected ContentManager _content;
        protected SpriteBatch _spriteBatch;
        protected bool _isInitialized = false;
        protected bool _isLoaded = false;

        public string Name { get; protected set; }
        public bool IsActive { get; protected set; }
        public bool IsVisible { get; protected set; } = true;

        protected BaseScene(string name)
        {
            Name = name;
            _input = ServiceLocator.Get<IInputService>();
            _graphics = ServiceLocator.Get<IGraphicsService>();
        }

        // Template Method pattern - defines the algorithm skeleton
        public virtual void Initialize()
        {
            if (_isInitialized) return;
            OnInitialize();
            _isInitialized = true;
        }

        public virtual void LoadContent(ContentManager content, SpriteBatch spriteBatch)
        {
            if (_isLoaded) return;
            _content = content;
            _spriteBatch = spriteBatch;
            OnLoadContent();
            _isLoaded = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsActive) return;
            _input.Update();
            OnUpdate(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (!IsVisible) return;
            OnDraw(gameTime);
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void OnResize(int width, int height) { }

        // Abstract methods subclasses must implement
        protected abstract void OnInitialize();
        protected abstract void OnLoadContent();
        protected abstract void OnUpdate(GameTime gameTime);
        protected abstract void OnDraw(GameTime gameTime);

        // Helper methods
        protected T LoadTexture<T>(string assetName) where T : Texture2D
        {
            return _content.Load<T>(assetName);
        }

        protected SpriteFont LoadFont(string assetName)
        {
            return _content.Load<SpriteFont>(assetName);
        }

        protected Button CreateButton(ButtonConfig config)
        {
            return new Button(config);
        }

        protected Rectangle GetScaledRect(Rectangle baseRect)
        {
            return _graphics.ScaleRectangle(baseRect);
        }

        protected Vector2 GetScaledPos(Vector2 basePos)
        {
            return _graphics.ScaleVector(basePos);
        }
    }
}