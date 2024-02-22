namespace Netchain.Core;

public readonly struct Transaction(int value, Guid sender, Guid recipient)
{
    public Guid Id { get; } = Guid.NewGuid();
    public int Value { get; } = value;
    public Guid Sender { get; } = sender;
    public Guid Recipient { get; } = recipient;
}
