import type { TypeNode } from "typescript";
import type { TypeReference } from "./protocol/type_references";
import type { 
  ArrayLikeType, ClassType, DictionaryType, 
  DistinctType, DynamicType, EnumType, 
  PrimitiveType, StringEnumType, 
  StructType, TaskType, UnionType 
} from "./protocol/types";

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
  getDynamicType(typeId: string): DynamicType;
  getArrayLikeType(typeId: string): ArrayLikeType;
  getDictionaryType(typeId: string): DictionaryType;
  getTaskType(typeId: string): TaskType;
  getEnumType(typeId: string): EnumType;
  getStringEnumType(typeId: string): StringEnumType;
  getStructType(typeId: string): StructType;
  getClassType(typeId: string): ClassType;
  getUnionType(typeId: string): UnionType;
  getDistinctType(typeId: string): DistinctType;
  enumerateDistinctTypes(): DistinctType[];
}

export interface TypeReferenceEmitter {
  emit: (typeReference: TypeReference, currentNamespace: string | undefined) => TypeNode;
}
