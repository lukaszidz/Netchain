namespace Netchain.Core.Events;

public delegate Task AsyncEventHandler<TEventArgs>(object sender, TEventArgs e);
