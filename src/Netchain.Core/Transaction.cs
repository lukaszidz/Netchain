namespace Netchain.Core;

public readonly struct Transaction(int value, Guid sender, Guid recipient)
{
    public Guid Id { get; } = Guid.NewGuid();
    public int Value { get; } = value;
    public Guid Sender { get; } = sender;
    public Guid Recipient { get; } = recipient;

    public override bool Equals(object obj)
    {
        if (obj is Transaction other)
        {
            return Id.Equals(other.Id);
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Transaction left, Transaction right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Transaction left, Transaction right)
    {
        return !(left == right);
    }
}
