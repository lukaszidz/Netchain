namespace Netchain.Core.Events;

public sealed class TransactionAdded(Transaction transaction) : EventArgs
{
    public Transaction Transaction { get; } = transaction;
}
