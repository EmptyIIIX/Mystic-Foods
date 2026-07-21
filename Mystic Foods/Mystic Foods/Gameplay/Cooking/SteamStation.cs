using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Gameplay.Cooking;
using Mystic_Foods.Gameplay.Ingredients;

namespace Mystic_Foods.Gameplay.Cooking
{
    public sealed class SteamStation : ICookingStation
    {
        public Vector2 Position { get; }
        public Rectangle Bounds { get; }
        private Food? _cookingFood;
        private float _timer;
        private const float CookTime = 3f;

        public SteamStation(Vector2 position, Texture2D texture)
        {
            Position = position;
            Bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public bool CanAccept(Ingredient ingredient) => false;

        public bool CanAcceptFood(Food food) => _cookingFood == null;

        public void PlaceFood(Food food)
        {
            _cookingFood = food;
            _timer = CookTime;
            food.Position = Position;
        }

        public void PlaceIngredient(Ingredient ingredient) { }

        public void Update(GameTime gameTime)
        {
            if (_cookingFood != null)
            {
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timer <= 0)
                {
                    _cookingFood.MarkCooked();
                    _timer = 0;
                }
            }
        }

        public bool TryComplete(out Food? food)
        {
            if (_cookingFood != null && _cookingFood.IsCooked)
            {
                food = _cookingFood;
                _cookingFood = null;
                return true;
            }
            food = null;
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 cameraPos) 
        { 
            /* draw steamer + progress bar if _timer > 0 */
        }
    }
}