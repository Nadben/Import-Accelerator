using Microsoft.EntityFrameworkCore;

namespace Accelerator.Commercetools.Importer.GenericEtlPipeline;

// [WorkflowTask(3)]
public class LoadingWorker<T, TContext, TU> 
    where T : class
    where TContext : DbContext 
    where TU : class
{
    
}