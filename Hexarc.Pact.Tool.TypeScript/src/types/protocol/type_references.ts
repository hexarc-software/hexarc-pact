export const enum TypeReferenceKind {
  Array = "Array",
  Dictionary = "Dictionary",
  Distinct = "Distinct",
  TypeParameter = "TypeParameter",
  Nullable = "Nullable",
  Task = "Task",
  Primitive = "Primitive",
  Dynamic = "Dynamic",
  Literal = "Literal",
  Tuple = "Tuple"
}

export interface ArrayTypeReference {
  readonly kind: TypeReferenceKind.Array;
  readonly arrayLikeTypeId?: string;
  readonly elementType: TypeReference;
}

export interface DictionaryTypeReference {
  readonly kind: TypeReferenceKind.Dictionary;
  readonly typeId: string;
  readonly keyType: TypeReference;
  readonly valueType: TypeReference;
}

export interface DistinctTypeReference {
  readonly kind: TypeReferenceKind.Distinct;
  readonly typeId: string;
  readonly typeArguments?: TypeReference[];
}

export interface TypeParameterReference {
  readonly kind: TypeReferenceKind.TypeParameter;
  readonly name: string;
}

export interface NullableTypeReference {
  readonly kind: TypeReferenceKind.Nullable;
  readonly underlyingType: TypeReference;
}

export interface TaskTypeReference {
  readonly kind: TypeReferenceKind.Task;
  readonly typeId?: string;
  readonly resultType: TypeReference;
}

export interface PrimitiveTypeReference {
  readonly kind: TypeReferenceKind.Primitive;
  readonly typeId: string;
}

export interface DynamicTypeReference {
  readonly kind: TypeReferenceKind.Dynamic;
  readonly typeId: string;
}

export interface LiteralTypeReference {
  readonly kind: TypeReferenceKind.Literal;
  readonly name: string;
}

export interface TupleTypeReference {
  readonly kind: TypeReferenceKind.Tuple;
  readonly elements: TupleElement[];
}

export interface TupleElement {
  readonly type: TypeReference;
  readonly name?: string;
}

export type TypeReference =
  | ArrayTypeReference
  | DictionaryTypeReference
  | DistinctTypeReference
  | TypeParameterReference
  | NullableTypeReference
  | TaskTypeReference
  | PrimitiveTypeReference
  | DynamicTypeReference
  | LiteralTypeReference
  | TupleTypeReference;
