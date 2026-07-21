using System.Collections.Generic;

namespace Mystic_Foods.Gameplay.Economy
{
    public sealed record Transaction(string Description, float Amount, TransactionType Type);

    public enum TransactionType { Income, Expense }

    public sealed class TransactionLog
    {
        private readonly List<Transaction> _transactions = new();
        public IReadOnlyList<Transaction> All => _transactions;

        public void Record(string desc, float amount, TransactionType type) 
            => _transactions.Add(new Transaction(desc, amount, type));
    }
}