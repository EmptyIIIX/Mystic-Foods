using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Core.Services
{
    /// <summary>
    /// Abstraction for graphics operations with virtual resolution support
    /// </summary>
    public interface IGraphicsService
    {
        int VirtualWidth { get; }
        int VirtualHeight { get; }
        int ActualWidth { get; }
        int ActualHeight { get; }
        float ScaleX { get; }
        float ScaleY { get; }
        float UniformScale { get; }
        Matrix TransformMatrix { get; }
        Rectangle Viewport { get; }
        Texture2D WhiteTexture { get; }
        SpriteBatch SpriteBatch { get; }
        
        void Initialize(GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight);
        void OnResize(int width, int height);
        Rectangle ScaleRectangle(Rectangle rect);
        Vector2 ScaleVector(Vector2 vec);
        Point ScalePoint(Point pt);
        Vector2 ScreenToVirtual(Vector2 screenPos);
        Vector2 VirtualToScreen(Vector2 virtualPos);
        
        // SpriteBatch wrappers
        void Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, 
                   SamplerState samplerState = null, DepthStencilState depthStencilState = null, 
                   RasterizerState rasterizerState = null, Effect effect = null, 
                   Matrix? transformMatrix = null);
        void End();
        void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, 
                  float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
        void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color);
        void DrawString(SpriteFont font, string text, Vector2 position, Color color, 
                        float rotation = 0, Vector2 origin = default, float scale = 1, 
                        SpriteEffects effects = SpriteEffects.None, float layerDepth = 0);
        Vector2 MeasureString(SpriteFont font, string text);
    }
}