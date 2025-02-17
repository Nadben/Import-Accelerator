using System.Threading.Channels;

namespace Accelerator.Commercetools.Importer.Shared.Extension;

public static class ChannelExtension
{
    public static async IAsyncEnumerable<T[]> ReadAllBatches<T>(
        this ChannelReader<T> channelReader, int batchSize)
    {
        List<T> buffer = new();
        while (true)
        {
            T item;
            try { item = await channelReader.ReadAsync(); }
            catch (ChannelClosedException) { break; }
            buffer.Add(item);
            if (buffer.Count == batchSize)
            {
                yield return buffer.ToArray();
                buffer.Clear();
            }
        }
        if (buffer.Count > 0) yield return buffer.ToArray();
        await channelReader.Completion;
    }
}