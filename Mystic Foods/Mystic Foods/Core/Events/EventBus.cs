using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Mystic_Foods.Domain.Models;

namespace Mystic_Foods.Core.Events
{
    /// <summary>
    /// Simple event bus for decoupled communication between components
    /// Follows Observer pattern - publishers don't know subscribers
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _handlers = new();

        /// <summary>
        /// Subscribe to an event type
        /// </summary>
        public static void Subscribe<T>(Action<T> handler) where T : notnull
        {
            var type = typeof(T);
            if (!_handlers.TryGetValue(type, out var list))
                _handlers[type] = list = new List<Delegate>();
            list.Add(handler);
        }

        /// <summary>
        /// Unsubscribe from an event type
        /// </summary>
        public static void Unsubscribe<T>(Action<T> handler) where T : notnull
        {
            if (_handlers.TryGetValue(typeof(T), out var list))
                list.Remove(handler);
        }

        /// <summary>
        /// Publish an event to all subscribers
        /// </summary>
        public static void Publish<T>(T args) where T : notnull
        {
            if (_handlers.TryGetValue(typeof(T), out var list))
            {
                foreach (var handler in list.ToArray())
                {
                    try
                    {
                        ((Action<T>)handler)(args);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"EventBus error in {handler.Method.Name}: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Clear all subscriptions (use on scene change or game shutdown)
        /// </summary>
        public static void Clear()
        {
            _handlers.Clear();
        }
    }

    // ===== Core Game Events =====

    public readonly record struct IngredientPlaced(
        int IngredientId,
        IngredientCategory Category,
        Vector2 Position
    );

    public readonly record struct FoodCreated(
        int RecipeId,
        int FillingId,
        int DoughId,
        int FlowerId,
        Vector2 Position
    );

    public readonly record struct FoodCooked(
        int FoodId,
        Vector2 Position
    );

    public readonly record struct FoodDecorated(
        int FoodId,
        int FlowerId,
        Vector2 Position
    );

    public readonly record struct FoodServed(
        bool CorrectOrder,
        float Payment,
        int CustomerId
    );

    public readonly record struct CustomerChanged(
        int CustomerId,
        string CustomerName,
        int RequiredRecipeId,
        string DialogueFirst,
        string DialogueSecond,
        string DialogueCorrect,
        string DialogueWrong
    );

    public readonly record struct PhaseChanged(DayPhase Phase);

    public readonly record struct MoneyChanged(float TotalMoney, float Revenue, float Cost);

    public readonly record struct GamePhaseEnded(float Profit);
}