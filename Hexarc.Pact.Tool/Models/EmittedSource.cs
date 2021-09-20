namespace Hexarc.Pact.Tool.Models;

using Microsoft.CodeAnalysis.Text;

public sealed class EmittedSource
{
    public String FileName { get; }

    public SourceText SourceText { get; }

    public EmittedSource(String fileName, SourceText sourceText) =>
        (this.FileName, this.SourceText) = (fileName, sourceText);
}
