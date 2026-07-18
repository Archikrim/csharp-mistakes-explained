// Exhibit #0010: the fix

using System.Runtime.CompilerServices;

// The dashboard opens a widget, uses it, closes it. Properly this time.
var weakRef = OpenUseAndCloseWidget();

GC.Collect();
GC.WaitForPendingFinalizers();
GC.Collect();

Console.WriteLine($"Widget closed. Still in memory: {weakRef.IsAlive}");

OrderFeed.Publish("order #1002"); // silence - no ghost

if (weakRef.IsAlive)
{
    throw new InvalidOperationException(
        "The closed widget is alive: the static event still holds its handler");
}

Console.WriteLine("Widget collected. Rest in peace.");

// A separate method, so no stack slot of ours keeps the widget alive by accident.
[MethodImpl(MethodImplOptions.NoInlining)]
static WeakReference OpenUseAndCloseWidget()
{
    var widget = new DashboardWidget("Sales widget");
    OrderFeed.Publish("order #1001");
    widget.Dispose(); // closing MEANS unsubscribing
    return new WeakReference(widget);
}

static class OrderFeed
{
    public static event Action<string>? OrderShipped;

    public static void Publish(string order) => OrderShipped?.Invoke(order);
}

class DashboardWidget : IDisposable
{
    private readonly string _name;

    public DashboardWidget(string name)
    {
        _name = name;
        OrderFeed.OrderShipped += OnOrderShipped;
    }

    public void Dispose() => OrderFeed.OrderShipped -= OnOrderShipped;

    private void OnOrderShipped(string order)
        => Console.WriteLine($"[{_name}] rendering {order}");
}
