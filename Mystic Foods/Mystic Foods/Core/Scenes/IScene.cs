using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Core.Scenes
{
    /// <summary>
    /// Interface for all game scenes - Single Responsibility Principle
    /// </summary>
    public interface IScene
    {
        /// <summary>
        /// Unique scene identifier
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Called once when scene is loaded
        /// </summary>
        void LoadContent(ContentManager content);

        /// <summary>
        /// Called every frame for logic updates
        /// </summary>
        void Update(GameTime gameTime);

        /// <summary>
        /// Called every frame for rendering
        /// </summary>
        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        /// <summary>
        /// Called when scene becomes active
        /// </summary>
        void OnEnter();

        /// <summary>
        /// Called when scene is deactivated
        /// </summary>
        void OnExit();

        /// <summary>
        /// Handle screen resize
        /// </summary>
        void OnResize(int width, int height);

        /// <summary>
        /// Whether scene is currently active
        /// </summary>
        bool IsActive { get; set; }
    }
}