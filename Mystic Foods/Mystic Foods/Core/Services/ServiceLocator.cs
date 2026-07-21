using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mystic_Foods.Core.Services
{
    /// <summary>
    /// Simple Service Locator for Dependency Injection
    /// Allows decoupling components from concrete implementations
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> _services = new();
        private static readonly Dictionary<Type, Func<object>> _factories = new();

        /// <summary>
        /// Register a service instance
        /// </summary>
        public static void Register<T>(T instance) where T : class
        {
            _services[typeof(T)] = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        /// <summary>
        /// Register a service factory for lazy initialization
        /// </summary>
        public static void RegisterFactory<T>(Func<T> factory) where T : class
        {
            _factories[typeof(T)] = () => factory() ?? throw new InvalidOperationException($"Factory for {typeof(T).Name} returned null");
        }

        /// <summary>
        /// Get a registered service
        /// </summary>
        public static T Get<T>() where T : class
        {
            var type = typeof(T);
            
            if (_services.TryGetValue(type, out var instance))
                return (T)instance;

            if (_factories.TryGetValue(type, out var factory))
            {
                var created = (T)factory();
                _services[type] = created;
                return created;
            }

            throw new InvalidOperationException($"Service {type.Name} not registered. Call Register<T>() or RegisterFactory<T>() first.");
        }

        /// <summary>
        /// Try to get a service, returns null if not found
        /// </summary>
        public static T TryGet<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var instance))
                return (T)instance;

            if (_factories.TryGetValue(typeof(T), out var factory))
            {
                var created = (T)factory();
                _services[typeof(T)] = created;
                return created;
            }

            return null;
        }

        /// <summary>
        /// Check if a service is registered
        /// </summary>
        public static bool IsRegistered<T>() where T : class
        {
            return _services.ContainsKey(typeof(T)) || _factories.ContainsKey(typeof(T));
        }

        /// <summary>
        /// Unregister a service
        /// </summary>
        public static void Unregister<T>() where T : class
        {
            _services.Remove(typeof(T));
            _factories.Remove(typeof(T));
        }

        /// <summary>
        /// Clear all services
        /// </summary>
        public static void Clear()
        {
            _services.Clear();
            _factories.Clear();
        }
    }
}