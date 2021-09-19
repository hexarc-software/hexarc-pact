namespace Hexarc.Pact.Tool.Models;

public sealed class GenerationOptions
{
    public Boolean? OmitTimestampComment { get; }

    public GenerationOptions(Boolean? omitTimestampComment)
    {
        this.OmitTimestampComment = omitTimestampComment;
    }
}
