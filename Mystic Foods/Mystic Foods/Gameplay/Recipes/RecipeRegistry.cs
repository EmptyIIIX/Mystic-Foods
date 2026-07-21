using System.Collections.Generic;
using Mystic_Foods.Gameplay.Recipes;

namespace Mystic_Foods.Gameplay.Recipes
{
    public sealed class RecipeRegistry
    {
        private readonly Dictionary<int, Recipe> _byId = new();
        private readonly Dictionary<(int Filling, int Dough), Recipe> _byBase = new();
        private readonly Dictionary<(int Filling, int Dough, int Flower), Recipe> _byDecorated = new();

        public void Register(Recipe recipe)
        {
            _byId[recipe.Id] = recipe;
            _byBase[(recipe.FillingId, recipe.DoughId)] = recipe;
            if (recipe.FlowerId.HasValue)
                _byDecorated[(recipe.FillingId, recipe.DoughId, recipe.FlowerId.Value)] = recipe;
        }

        public Recipe? GetByIngredients(int fillingId, int doughId, int? flowerId = null)
        {
            if (flowerId.HasValue && _byDecorated.TryGetValue((fillingId, doughId, flowerId.Value), out var decorated))
                return decorated;
            return _byBase.TryGetValue((fillingId, doughId), out var baseRecipe) ? baseRecipe : null;
        }
    }
}