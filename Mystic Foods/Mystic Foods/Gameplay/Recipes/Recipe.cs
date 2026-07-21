using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Gameplay.Recipes
{
    public sealed record Recipe(
        int Id,
        string Name,
        int FillingId,
        int DoughId,
        int? FlowerId,
        Texture2D BaseTexture,
        Texture2D? DecoratedTexture,
        int BasePrice,
        float WeightMultiplier
    );
}