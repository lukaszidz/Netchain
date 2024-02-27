namespace Netchain.Server.Responses;

public sealed class TransactionResponse
{
    public Guid Id { get; set; }
    public int Value { get; set; }
    public Guid Sender { get; set; }
    public Guid Recipient { get; set; }
}
