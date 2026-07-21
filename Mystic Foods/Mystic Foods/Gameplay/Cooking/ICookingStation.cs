using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Gameplay.Ingredients;

namespace Mystic_Foods.Gameplay.Cooking
{
    public interface ICookingStation
    {
        Vector2 Position { get; }
        Rectangle Bounds { get; }
        bool CanAccept(Ingredient ingredient);
        void PlaceIngredient(Ingredient ingredient);
        bool TryComplete(out Food? food);
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch, Vector2 cameraPos);
    }
}