using System;

namespace Mystic_Foods.Domain.Models
{
    /// <summary>
    /// Base enum for all ingredient types - follows Open/Closed Principle
    /// New ingredient types can be added without modifying existing code
    /// </summary>
    public enum IngredientCategory
    {
        Filling,
        Dough,
        Wrapper,
        Flower
    }

    /// <summary>
    /// Specific filling types - using flags for bitwise operations if needed
    /// </summary>
    public enum FillingType
    {
        None = 0,
        Coconut_Amber = 10,
        Pandan_Taro_Cream = 20,
        Lotus_Root_Spirit = 30
    }

    /// <summary>
    /// Specific dough types
    /// </summary>
    public enum DoughType
    {
        None = 0,
        Jasmine_Moon = 10,
        Lotus_Blossom = 20,
        Golden_Moon = 30
    }

    /// <summary>
    /// Specific flower types for decoration
    /// </summary>
    public enum FlowerType
    {
        None = 0,
        Mali = 10,
        Rose = 20,
        Lotus = 30
    }

    /// <summary>
    /// Day phase affects visuals and potentially gameplay
    /// </summary>
    public enum DayPhase
    {
        Dawn,
        Dusk,
        Night
    }
}