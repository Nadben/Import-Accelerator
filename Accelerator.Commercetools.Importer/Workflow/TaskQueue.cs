using System.Threading.Channels;

namespace Accelerator.Commercetools.Importer.Workflow;

public class TaskQueue
{
    private readonly Channel<Func<CancellationToken, Task>> _taskChannel;

    public TaskQueue(int capacity = 100)
    {
        // Create a bounded channel to limit the number of tasks in the queue
        _taskChannel = Channel.CreateBounded<Func<CancellationToken, Task>>(new BoundedChannelOptions(capacity)
        {
            FullMode = BoundedChannelFullMode.Wait // Wait if the queue is full
        });
    }

    /// <summary>
    /// Enqueues a task to the queue.
    /// </summary>
    /// <param name="task">A function representing the task to enqueue.</param>
    /// <param name="cancellationToken">A token to cancel the enqueue operation.</param>
    /// <returns>A task that completes when the task is enqueued.</returns>
    public async Task EnqueueAsync(Func<CancellationToken, Task> task, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(task);
        await _taskChannel.Writer.WriteAsync(task, cancellationToken);
    }

    /// <summary>
    /// Dequeues a task from the queue.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the dequeue operation.</param>
    /// <returns>A function representing the dequeued task.</returns>
    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _taskChannel.Reader.ReadAsync(cancellationToken);
    }
}   