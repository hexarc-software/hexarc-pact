using System;
using Microsoft.CodeAnalysis.Text;

namespace Hexarc.Pact.Tool.Models
{
    public sealed class EmittedSource
    {
        public String FileName { get; }

        public SourceText SourceText { get; }

        public EmittedSource(String fileName, SourceText sourceText) =>
            (this.FileName, this.SourceText) = (fileName, sourceText);
    }
}
