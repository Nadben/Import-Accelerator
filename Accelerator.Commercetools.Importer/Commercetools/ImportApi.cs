using commercetools.Sdk.ImportApi.Client;

namespace Accelerator.Commercetools.Importer.Commercetools;

public class ImportApi : IImportApi
{
    public ImportApi(ProjectApiRoot client)
    {
        Client = client;
    }

    public ProjectApiRoot Client { get; set; }
}

public interface IImportApi;