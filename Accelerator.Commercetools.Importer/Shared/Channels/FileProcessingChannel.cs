using System.Threading.Channels;

namespace Accelerator.Commercetools.Importer.Shared.Channels;

public class FileProcessingChannel<T>
{
        public Channel<T> ProcessingChannel { get; init; }

        public FileProcessingChannel()
        {
            ProcessingChannel = Channel.CreateUnbounded<T>(new UnboundedChannelOptions
            {
                SingleWriter = true,
                SingleReader = false,
                AllowSynchronousContinuations = false
            });
        }
}