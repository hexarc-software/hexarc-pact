import type { TypeReference } from "./type_references";


export const enum TypeKind {
  Primitive = "Primitive",
  Dynamic = "Dynamic",
  ArrayLike = "ArrayLike",
  Dictionary = "Dictionary",
  Task = "Task",
  Enum = "Enum",
  StringEnum = "StringEnum",
  Struct = "Struct",
  Class = "Class",
  Union = "Union"
}

export interface TypeBase {
  readonly kind: string;
  readonly id: string;
  readonly namespace?: string;
  readonly name: string;
  readonly isReference: boolean;
}

export interface PrimitiveType extends TypeBase {
  readonly kind: TypeKind.Primitive;
}

export interface DynamicType extends TypeBase {
  readonly kind: TypeKind.Dynamic;
}

export interface ArrayLikeType extends TypeBase {
  readonly kind: TypeKind.ArrayLike;
}

export interface DictionaryType extends TypeBase {
  readonly kind: TypeKind.Dictionary;
}

export interface TaskType extends TypeBase {
  readonly kind: TypeKind.Task;
}

export interface EnumType extends TypeBase {
  readonly kind: TypeKind.Enum;
  readonly members: EnumMember[];
}

export interface EnumMember {
  readonly name: string;
  readonly value: number;
}

export interface StringEnumType extends TypeBase {
  readonly kind: TypeKind.StringEnum;
  readonly members: string[];
}

export interface ObjectType extends TypeBase {
  readonly typeParameters?: string[];
  readonly properties: ObjectProperty[];
}

export interface ObjectProperty {
  readonly type: TypeReference;
  readonly name: string;
}

export interface StructType extends ObjectType {
  readonly kind: TypeKind.Struct;
}

export interface ClassType extends ObjectType {
  readonly kind: TypeKind.Class;
}

export interface UnionType extends TypeBase {
  readonly kind: TypeKind.Union;
  readonly tagName: string;
  readonly cases: ClassType[];
}

export type DistinctType =
  | EnumType
  | StringEnumType
  | StructType
  | ClassType
  | UnionType;

export type Type =
  | PrimitiveType
  | DynamicType
  | ArrayLikeType
  | DictionaryType
  | DynamicType
  | TaskType
  | DistinctType;
