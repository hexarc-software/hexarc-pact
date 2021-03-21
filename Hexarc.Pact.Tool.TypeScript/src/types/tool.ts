import type { TypeReference } from "./protocol/type_references";
import type { DistinctType, PrimitiveType } from "./protocol/types";
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

export interface TypeRegistry {
  getPrimitiveType(typeId: string): PrimitiveType;
  getDistinctType(typeId: string): DistinctType;
}

export interface TypeReferenceEmitter {
  emit: (typeReference: TypeReference, currentNamespace: string | undefined) => TypeNode;
}
