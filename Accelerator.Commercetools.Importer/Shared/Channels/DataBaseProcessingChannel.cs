using System.Threading.Channels;

namespace Accelerator.Commercetools.Importer.Shared.Channels;

public class DataBaseProcessingChannel<T>  where T : class 
{
    public Channel<T> ProcessingChannel { get; init; }

    public DataBaseProcessingChannel()
    {
        ProcessingChannel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions
        {
            SingleWriter = true,
            SingleReader = false,
            AllowSynchronousContinuations = false
        });
    }
}