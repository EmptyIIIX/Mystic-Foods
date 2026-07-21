using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mystic_Foods.Domain.Models;
using Mystic_Foods.Systems;

namespace Mystic_Foods.Gameplay.Ingredients
{
    /// <summary>
    /// Data-driven ingredient definition - follows Open/Closed Principle
    /// New ingredients added via data, not code changes
    /// </summary>
    public readonly record struct IngredientData(
        int Id,
        string Name,
        IngredientCategory Category,
        string TexturePath,
        string OnPlateTexturePath,
        Vector2 DefaultPosition,
        int SortOrder
    );

    /// <summary>
    /// Runtime ingredient instance - single class, no inheritance hierarchy
    /// Implements IDraggable/ITargetable for drag-drop system
    /// </summary>
    public sealed class Ingredient : IDraggable, ITargetable
    {
        public int Id { get; }
        public IngredientData Data { get; }
        public Vector2 Position { get; set; }
        public bool IsOnStation { get; private set; }
        public Texture2D Texture { get; }
        public Texture2D OnPlateTexture { get; }

        public Rectangle Rectangle => new(
            (int)(Position.X - Texture.Width / 2),
            (int)(Position.Y - Texture.Height / 2),
            Texture.Width,
            Texture.Height
        );

        public Vector2 Size => new(Texture.Width, Texture.Height);

        public Ingredient(IngredientData data, Texture2D texture, Texture2D onPlateTexture = null)
        {
            Id = data.Id;
            Data = data;
            Texture = texture;
            OnPlateTexture = onPlateTexture;
            Position = data.DefaultPosition;
        }

        public void SetOnStation(bool onStation)
        {
            IsOnStation = onStation;
        }

        public void ResetPosition() => Position = Data.DefaultPosition;

        public Rectangle GetRectangle(Vector2 cameraPos) => new(
            (int)(Position.X - Texture.Width / 2 - cameraPos.X),
            (int)(Position.Y - Texture.Height / 2 - cameraPos.Y),
            Texture.Width,
            Texture.Height
        );
    }
}