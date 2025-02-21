using commercetools.Sdk.Api.Client;

namespace Accelerator.Commercetools.Importer.Commercetools;

public class HttpApi : IHttpApi
{
    public HttpApi(ProjectApiRoot client)
    {
        Client = client;
    }

    public ProjectApiRoot Client { get; set; }
}

public interface IHttpApi;