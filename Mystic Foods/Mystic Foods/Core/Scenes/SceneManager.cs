using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mystic_Foods.Core.Services;

namespace Mystic_Foods.Core.Scenes
{
    /// <summary>
    /// Manages scene transitions using State pattern
    /// Follows Single Responsibility Principle - handles only scene lifecycle
    /// </summary>
    public class SceneManager
    {
        private ContentManager _content;
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;
        
        private IScene _currentScene;
        private IScene _nextScene;
        private string _currentSceneName;
        private string _nextSceneName;
        
        private float _transitionAlpha;
        private float _transitionSpeed = 2f;
        private bool _isTransitioning;
        private bool _transitionToBlack = true;
        private Texture2D _whiteTexture;
        
        private System.Collections.Generic.Dictionary<string, IScene> _scenes = new();

        public SceneManager(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _content = content;
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
            _whiteTexture = new Texture2D(graphicsDevice, 1, 1);
            _whiteTexture.SetData(new[] { Color.White });
        }

        public void RegisterScene(string name, IScene scene)
        {
            if (_scenes.ContainsKey(name))
                throw new System.ArgumentException($"Scene '{name}' already registered.");
            
            _scenes[name] = scene;
            scene.LoadContent(_content);
        }

        public void ChangeScene(string sceneName)
        {
            if (!_scenes.TryGetValue(sceneName, out var scene))
                throw new System.ArgumentException($"Scene '{sceneName}' not found.");

            if (_currentScene != null)
            {
                _currentScene.OnExit();
            }

            _currentScene = scene;
            _currentSceneName = sceneName;
            _currentScene.OnEnter();
        }

        public void ChangeSceneWithTransition(string sceneName, float speed = 2f)
        {
            if (!_scenes.TryGetValue(sceneName, out var scene))
                throw new System.ArgumentException($"Scene '{sceneName}' not found.");

            _nextScene = scene;
            _nextSceneName = sceneName;
            _isTransitioning = true;
            _transitionAlpha = 0f;
            _transitionSpeed = speed;
        }

        public void Update(GameTime gameTime)
        {
            if (_isTransitioning)
            {
                UpdateTransition(gameTime);
            }
            else if (_currentScene != null)
            {
                _currentScene.Update(gameTime);
            }
        }

        private void UpdateTransition(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_transitionToBlack)
            {
                _transitionAlpha += _transitionSpeed * deltaTime;
                
                if (_transitionAlpha >= 1f)
                {
                    _transitionAlpha = 1f;
                    
                    // Switch scene at peak darkness
                    _currentScene?.OnExit();
                    _currentScene = _nextScene;
                    _currentSceneName = _nextSceneName;
                    _currentScene.OnEnter();
                    
                    _transitionToBlack = false;
                }
            }
            else
            {
                _transitionAlpha -= _transitionSpeed * deltaTime;
                
                if (_transitionAlpha <= 0f)
                {
                    _transitionAlpha = 0f;
                    _isTransitioning = false;
                    _transitionToBlack = true;
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            _currentScene?.Draw(gameTime);
            
            if (_isTransitioning)
            {
                DrawTransitionOverlay();
            }
        }

        private void DrawTransitionOverlay()
        {
            var viewport = _graphicsDevice.Viewport;
            var rect = new Rectangle(0, 0, viewport.Width, viewport.Height);
            
            _spriteBatch.Begin();
            _spriteBatch.Draw(_whiteTexture, rect, Color.Black * _transitionAlpha);
            _spriteBatch.End();
        }

        public void OnResize(int width, int height)
        {
            foreach (var scene in _scenes.Values)
            {
                scene.OnResize(width, height);
            }
        }

        public IScene CurrentScene => _currentScene;
        public string CurrentSceneName => _currentSceneName;
        public bool IsTransitioning => _isTransitioning;
    }
}