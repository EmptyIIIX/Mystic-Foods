using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Mystic_Foods.Domain.Models;
using Mystic_Foods.Gameplay.Ingredients;

namespace Mystic_Foods.Gameplay.Ingredients
{
    public sealed class IngredientRegistry
    {
        private readonly Dictionary<int, IngredientData> _byId = new();
        private readonly Dictionary<IngredientCategory, List<IngredientData>> _byCategory = new();

        public void Register(IngredientData data)
        {
            _byId[data.Id] = data;
            if (!_byCategory.TryGetValue(data.Category, out var list))
                _byCategory[data.Category] = list = new();
            list.Add(data);
            list.Sort((a, b) => a.SortOrder.CompareTo(b.SortOrder));
        }

        public IngredientData? Get(int id) => _byId.TryGetValue(id, out var d) ? d : null;
        public IReadOnlyList<IngredientData> GetAll(IngredientCategory cat) 
            => _byCategory.TryGetValue(cat, out var list) ? list : System.Array.Empty<IngredientData>();
    }
}