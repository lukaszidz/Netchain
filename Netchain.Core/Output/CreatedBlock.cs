namespace Netchain.Core.Output;

public readonly struct CreatedBlock(int index, int proof)
{
    public int Index { get; } = index;
    public int Proof { get; } = proof;
}
