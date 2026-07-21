using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Domain.Models;
using Mystic_Foods.Gameplay.Ingredients;

namespace Mystic_Foods.Gameplay.Cooking
{
    public sealed class PlateStation : ICookingStation
    {
        public Vector2 Position { get; }
        public Rectangle Bounds { get; }
        private Ingredient? _filling;
        private Ingredient? _dough;
        private Ingredient? _wrapper;

        public PlateStation(Vector2 position, Texture2D texture)
        {
            Position = position;
            Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool CanAccept(Ingredient ing) => ing.Data.Category switch
        {
            IngredientCategory.Filling => _filling == null,
            IngredientCategory.Dough => _dough == null,
            IngredientCategory.Wrapper => _wrapper == null,
            _ => false
        };

        public void PlaceIngredient(Ingredient ingredient)
        {
            switch (ingredient.Data.Category)
            {
                case IngredientCategory.Filling: _filling = ingredient; break;
                case IngredientCategory.Dough: _dough = ingredient; break;
                case IngredientCategory.Wrapper: _wrapper = ingredient; break;
            }
            ingredient.Position = Position;
            ingredient.SetOnStation(true);
        }

        public bool TryComplete(out Food? food)
        {
            if (_filling != null && _dough != null && _wrapper != null)
            {
                food = new Food(_filling.Id, _dough.Id, Position);
                _filling = _dough = _wrapper = null;
                return true;
            }
            food = null;
            return false;
        }

        public void Update(GameTime gameTime) { }
        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos) { /* draw plate texture */ }
    }
}