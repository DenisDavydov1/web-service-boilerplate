namespace BoilerPlate.Core.Extensions;

public static class AsyncPrimitivesExtensions
{
    public static async Task Lock(this SemaphoreSlim semaphore, Func<Task> task, CancellationToken ct = default)
    {
        await semaphore.WaitAsync(ct);
        try
        {
            await task();
        }
        finally
        {
            semaphore.Release();
        }
    }

    public static async Task<T> Lock<T>(this SemaphoreSlim semaphore, Func<Task<T>> task, CancellationToken ct = default)
    {
        await semaphore.WaitAsync(ct);
        try
        {
            return await task();
        }
        finally
        {
            semaphore.Release();
        }
    }
}