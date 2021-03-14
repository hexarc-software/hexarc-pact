declare namespace Pact.Tool {
  interface ClientSettings {
    readonly schemaUri: string;
    readonly clientClassName: string;
    readonly scopes?: string[];
    readonly generationOptions: GenerationOptions;
  }

  interface GenerationOptions {
    readonly omitTimestampComment?: boolean;
  }
}
