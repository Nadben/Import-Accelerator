using Accelerator.Schema.Generator.Parsers.Model;

namespace Accelerator.Schema.Generator.Parsers.Interface;

public interface ISchemaParser
{
    ClassSchema Parse(string fileContent, string fileName, string folderName);
    bool CanHandle(string fileExtension);
}