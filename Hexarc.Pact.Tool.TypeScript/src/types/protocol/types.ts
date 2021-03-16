import type { TypeReference } from "./type_references";


export interface TypeBase {
  readonly kind: string;
  readonly id: string;
  readonly namespace?: string;
  readonly name: string;
  readonly isReference: boolean;
}

export interface PrimitiveType extends TypeBase {
  readonly kind: "Primitive";
}

export interface DynamicType extends TypeBase {
  readonly kind: "Dynamic";
}

export interface ArrayLikeType extends TypeBase {
  readonly kind: "ArrayLike";
}

export interface DictionaryType extends TypeBase {
  readonly kind: "Dictionary";
}

export interface TaskType extends TypeBase {
  readonly kind: "Task";
}

export interface EnumType extends TypeBase {
  readonly kind: "Enum";
  readonly members: EnumMember[];
}

export interface EnumMember {
  readonly name: string;
  readonly value: number;
}

export interface StringEnumType {
  readonly kind: "StringEnum";
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
  readonly kind: "Struct";
}

export interface ClassType extends ObjectType {
  readonly kind: "Class";
}

export interface UnionType {
  readonly kind: "Union";
  readonly tagName: string;
  readonly cases: ClassType[];
}

export type Type =
  | PrimitiveType
  | DynamicType
  | ArrayLikeType
  | DynamicType
  | TaskType
  | EnumType
  | StringEnumType
  | StructType
  | UnionType;
