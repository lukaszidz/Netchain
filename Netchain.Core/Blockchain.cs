namespace Netchain.Core
{
    public sealed class Blockchain
    {
        private readonly LinkedList<Transaction> _transactions = new();

        public Guid NodeId { get; } = Guid.NewGuid();

        public ICollection<Transaction> Transactions => _transactions;

        public void CreateTransaction(Guid sender, Guid recipient, int amount)
        {
            var transaction = new Transaction(amount, sender, recipient);
            _transactions.AddLast(transaction);
        }
    }
}
