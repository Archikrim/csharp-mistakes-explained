// Exhibit #0010: a static event that keeps its subscribers alive

using System.Runtime.CompilerServices;

// The dashboard opens a widget, uses it, closes it. Or so it thinks.
var weakRef = OpenUseAndCloseWidget();

GC.Collect();
GC.WaitForPendingFinalizers();
GC.Collect();

Console.WriteLine($"Widget closed. Still in memory: {weakRef.IsAlive}");

OrderFeed.Publish("order #1002"); // 💥 the ghost still renders

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
    return new WeakReference(widget); // widget goes out of scope here - "closed"
}

static class OrderFeed
{
    public static event Action<string>? OrderShipped;

    public static void Publish(string order) => OrderShipped?.Invoke(order);
}

class DashboardWidget
{
    private readonly string _name;

    public DashboardWidget(string name)
    {
        _name = name;
        OrderFeed.OrderShipped += OnOrderShipped; // subscribed in the ctor, unsubscribed never
    }

    private void OnOrderShipped(string order)
        => Console.WriteLine($"[{_name}] rendering {order}");
}
