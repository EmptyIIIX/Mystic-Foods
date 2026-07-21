using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Gameplay.Ingredients;

namespace Mystic_Foods.Gameplay.Cooking
{
    public sealed class TrashStation : ICookingStation
    {
        public Vector2 Position { get; }
        public Rectangle Bounds { get; }

        public TrashStation(Vector2 position, Texture2D texture)
        {
            Position = position;
            Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool CanAccept(Ingredient ingredient) => true;
        public void PlaceIngredient(Ingredient ingredient) => ingredient.ResetPosition();
        public bool TryComplete(out Food? food) { food = null; return false; }
        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos) { }
    }
}