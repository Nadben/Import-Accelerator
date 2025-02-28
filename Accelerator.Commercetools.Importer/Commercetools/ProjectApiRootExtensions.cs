using System.Net;
using commercetools.Base.Client.Error;
using commercetools.Sdk.ImportApi.Client;
using commercetools.Sdk.ImportApi.Models.Common;
using commercetools.Sdk.ImportApi.Models.Importcontainers;

namespace Accelerator.Commercetools.Importer.Commercetools;

static class ProjectApiRootExtensions
{
    public static async Task<ProjectApiRoot> EnsureContainerIsCreated(this ProjectApiRoot projectApiRoot,
        string containerName,
        IImportResourceType resourceType)
    {
        try
        {
            await projectApiRoot.ImportContainers()
                .Post(new ImportContainerDraft { ResourceType = resourceType, Key = containerName })
                .ExecuteAsync();
        }
        catch (BadRequestException  exception)
        {
            if (exception.StatusCode == (int)HttpStatusCode.BadRequest)
            {
                await projectApiRoot.ImportContainers()
                    .WithImportContainerKeyValue(containerName)
                    .Delete()
                    .ExecuteAsync();
                
                await projectApiRoot.EnsureContainerIsCreated(containerName, resourceType);
            }
        }

        return projectApiRoot;
    } 
}