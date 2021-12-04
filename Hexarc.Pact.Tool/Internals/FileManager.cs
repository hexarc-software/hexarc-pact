using Hexarc.Pact.Tool.Models;

namespace Hexarc.Pact.Tool.Internals;

public sealed class FileManager
{
    private String OutputDirectory { get; }

    private String ModelsPath { get; }

    private String ControllersPath { get; }

    public FileManager(String outputDirectory)
    {
        this.OutputDirectory = outputDirectory;
        this.ModelsPath = Path.Combine(this.OutputDirectory, "Models");
        this.ControllersPath = Path.Combine(this.OutputDirectory, "Controllers");
    }

    public void Save(EmittedApi emittedApi)
    {
        this.PrepareDirectories();
        this.SaveEmittedClient(emittedApi.Client);
        this.SaveEmittedModels(emittedApi.Models);
        this.SaveEmittedControllers(emittedApi.Controllers);
    }

    private void PrepareDirectories()
    {
        DirectoryOperations.CreateOrClear(this.OutputDirectory);
        Directory.CreateDirectory(this.ModelsPath);
        Directory.CreateDirectory(this.ControllersPath);
    }

    private void SaveEmittedClient(EmittedSource emittedClient) =>
        this.SaveSourceText(Path.Combine(this.OutputDirectory, emittedClient.FileName), emittedClient.SourceText);

    private void SaveEmittedModels(IEnumerable<EmittedSource> emittedModels)
    {
        foreach (var emittedModel in emittedModels)
        {
            this.SaveEmittedModel(emittedModel);
        }
    }

    private void SaveEmittedModel(EmittedSource emittedModel) =>
        this.SaveSourceText(Path.Combine(this.ModelsPath, emittedModel.FileName), emittedModel.SourceText);

    private void SaveEmittedControllers(IEnumerable<EmittedSource> emittedControllers)
    {
        foreach (var emittedSource in emittedControllers)
        {
            this.SaveEmittedController(emittedSource);
        }
    }

    private void SaveEmittedController(EmittedSource emittedController) =>
        this.SaveSourceText(Path.Combine(this.ControllersPath, emittedController.FileName), emittedController.SourceText);

    private void SaveSourceText(String path, SourceText sourceText)
    {
        using var file = File.CreateText(path);
        sourceText.Write(file);
    }
}
