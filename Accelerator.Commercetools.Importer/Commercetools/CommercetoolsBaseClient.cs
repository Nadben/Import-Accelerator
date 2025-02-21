using commercetools.Sdk.ImportApi.Client;
using commercetools.Sdk.ImportApi.Models.Common;

namespace Accelerator.Commercetools.Importer.Commercetools;

public abstract class CommercetoolsBaseClient<T> 
    where T : IImportResource
{
    public ProjectApiRoot ProjectApiRoot { get; set; }

    protected CommercetoolsBaseClient(ProjectApiRoot projectApiRoot)
    {
        ProjectApiRoot = projectApiRoot;
    }
}