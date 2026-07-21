using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Domain.Models;
using Mystic_Foods.Gameplay.Cooking;
using Mystic_Foods.Gameplay.Ingredients;

namespace Mystic_Foods.Gameplay.Cooking
{
    public sealed class DecorationStation : ICookingStation
    {
        public Vector2 Position { get; }
        public Rectangle Bounds { get; }

        public DecorationStation(Vector2 position, Texture2D texture)
        {
            Position = position;
            Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool CanAccept(Ingredient ingredient) => ingredient.Data.Category == IngredientCategory.Flower;

        public void PlaceIngredient(Ingredient ingredient) { }

        public bool TryComplete(out Food? food) { food = null; return false; }

        public void Update(GameTime gameTime) { }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos) { }
    }
}