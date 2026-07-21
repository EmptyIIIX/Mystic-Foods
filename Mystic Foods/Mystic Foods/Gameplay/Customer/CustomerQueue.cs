using System.Collections.Generic;
using System.Linq;

namespace Mystic_Foods.Gameplay.Customer
{
    public sealed class CustomerQueue
    {
        private readonly Queue<Mystic_Foods.Customer> _queue = new();

        public CustomerQueue(IEnumerable<Mystic_Foods.Customer> customers)
        {
            foreach (var customer in customers)
                _queue.Enqueue(customer);
        }

        public void Enqueue(Mystic_Foods.Customer customer)
            => _queue.Enqueue(customer);

        public Mystic_Foods.Customer? Next()
            => _queue.TryDequeue(out var next) ? next : null;
    }
}