using System;
using System.Collections.Generic;

namespace Mystic_Foods.Core.DI
{
    /// <summary>
    /// Lightweight DI Container - replaces ServiceLocator
    /// Follows Dependency Inversion Principle: depends on abstractions, not concretions
    /// </summary>
    public sealed class Container
    {
        private readonly Dictionary<Type, object> _singletons = new();
        private readonly Dictionary<Type, Func<object>> _factories = new();

        public void RegisterSingleton<T>(T instance) where T : class
        {
            _singletons[typeof(T)] = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public void RegisterFactory<T>(Func<T> factory) where T : class
        {
            _factories[typeof(T)] = () => factory() ?? throw new InvalidOperationException($"Factory for {typeof(T).Name} returned null");
        }

        public T Resolve<T>() where T : class
        {
            var type = typeof(T);
            if (_singletons.TryGetValue(type, out var instance))
                return (T)instance;
            if (_factories.TryGetValue(type, out var factory))
            {
                var created = (T)factory();
                _singletons[type] = created;
                return created;
            }
            throw new InvalidOperationException($"Service {type.Name} not registered. Call RegisterSingleton<T>() or RegisterFactory<T>() first.");
        }

        public bool TryResolve<T>(out T service) where T : class
        {
            try { service = Resolve<T>(); return true; }
            catch { service = null; return false; }
        }

        public bool IsRegistered<T>() where T : class
        {
            var type = typeof(T);
            return _singletons.ContainsKey(type) || _factories.ContainsKey(type);
        }

        public void Clear()
        {
            _singletons.Clear();
            _factories.Clear();
        }
    }
}