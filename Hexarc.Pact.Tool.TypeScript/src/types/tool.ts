import type { TypeReference } from "./protocol/type_references";
import type { TypeNode } from "typescript";


export interface ClientSettings {
  readonly schemaUri: string;
  readonly clientClassName: string;
  readonly scopes?: string[];
  readonly generationOptions: GenerationOptions;
}

export interface GenerationOptions {
  readonly omitTimestampComment?: boolean;
}

export interface TypeReferenceEmitter {
  emit: (typeReference: TypeReference, currentNamespace?: string) => TypeNode;
}
