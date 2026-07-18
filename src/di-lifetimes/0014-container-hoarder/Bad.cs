#:package Microsoft.Extensions.DependencyInjection@10.*
#:property PublishAot=false

// Exhibit #0014: transient disposables resolved from the root container

using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddTransient<ReportBuffer>();

using var root = services.BuildServiceProvider();

var trackers = HandleRequests(root, count: 100);

GC.Collect();
GC.WaitForPendingFinalizers();
GC.Collect();

var alive = trackers.Count(t => t.IsAlive);

Console.WriteLine($"Requests handled: {trackers.Count}");
Console.WriteLine($"Buffers disposed: {ReportBuffer.DisposedCount}");
Console.WriteLine($"Still in memory:  {alive} (~{alive} MB)");

if (alive > 0)
{
    throw new InvalidOperationException(
        $"{alive} transient buffers are pinned by the root container until shutdown");
}

Console.WriteLine("All buffers released. Memory stays flat.");

// A separate method, so no stack slot of ours keeps the buffers alive by accident.
[MethodImpl(MethodImplOptions.NoInlining)]
static List<WeakReference> HandleRequests(ServiceProvider root, int count)
{
    var trackers = new List<WeakReference>();

    for (int i = 0; i < count; i++)
    {
        var buffer = root.GetRequiredService<ReportBuffer>(); // 💥 transient, from the ROOT
        buffer.Render(requestId: i);
        trackers.Add(new WeakReference(buffer));
        // request finished; the buffer is garbage now. Or is it?
    }

    return trackers;
}

class ReportBuffer : IDisposable
{
    public static int DisposedCount;

    private readonly byte[] _memory = new byte[1_000_000]; // ~1 MB per request

    public void Render(int requestId) => _memory[0] = (byte)requestId;

    public void Dispose() => DisposedCount++;
}
