namespace Netchain.Core.Events;

public sealed class BlockAdded(Block block) : EventArgs
{
    public Block Block { get; } = block;
}
