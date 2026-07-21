using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Gameplay.Cooking
{
    public sealed class Food
    {
        public int FillingId { get; }
        public int DoughId { get; }
        public int? FlowerId { get; private set; }
        public int RecipeId { get; private set; }
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public bool IsCooked { get; private set; }
        public bool IsDecorated => FlowerId.HasValue;
        public float Weight { get; private set; } = 1f;
        public int Price { get; private set; }
        public int Cost { get; private set; }

        public Food(int fillingId, int doughId, Vector2 position)
        {
            FillingId = fillingId;
            DoughId = doughId;
            Position = position;
        }

        public void ApplyRecipe(Mystic_Foods.Gameplay.Recipes.Recipe recipe)
        {
            RecipeId = recipe.Id;
            Texture = recipe.BaseTexture;
            Price = recipe.BasePrice;
            Weight = recipe.WeightMultiplier;
        }

        public void Decorate(int flowerId, Texture2D decoratedTexture, int decoratedPrice)
        {
            FlowerId = flowerId;
            Texture = decoratedTexture;
            Price = decoratedPrice;
        }

        public void MarkCooked() => IsCooked = true;
    }
}