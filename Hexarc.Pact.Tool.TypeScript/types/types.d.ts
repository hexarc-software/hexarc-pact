/// <reference path="./type_references.d.ts" />

declare namespace Pact.Protocol.Types {
  interface TypeBase {
    readonly kind: string;
    readonly id: string;
    readonly namespace?: string;
    readonly name: string;
    readonly isReference: boolean;
  }

  interface PrimitiveType extends TypeBase {
    readonly kind: "Primitive";
  }

  interface DynamicType extends TypeBase {
    readonly kind: "Dynamic";
  }

  interface ArrayLikeType extends TypeBase {
    readonly kind: "ArrayLike";
  }

  interface DictionaryType extends TypeBase {
    readonly kind: "Dictionary";
  }

  interface TaskType extends TypeBase {
    readonly kind: "Task";
  }

  interface EnumType extends TypeBase {
    readonly kind: "Enum";
    readonly members: EnumMember[];
  }

  interface EnumMember {
    readonly name: string;
    readonly value: number;
  }

  interface StringEnumType {
    readonly kind: "StringEnum";
    readonly members: string[];
  }

  interface ObjectType extends TypeBase {
    readonly typeParameters?: string[];
    readonly properties: ObjectProperty[];
  }

  interface ObjectProperty {
    readonly type: TypeReferences.TypeReference;
    readonly name: string;
  }

  interface StructType extends ObjectType {
    readonly kind: "Struct";
  }

  interface ClassType extends ObjectType {
    readonly kind: "Class";
  }

  interface UnionType {
    readonly kind: "Union";
    readonly tagName: string;
    readonly cases: ClassType[];
  }

  type Type =
    | PrimitiveType
    | DynamicType
    | ArrayLikeType
    | DynamicType
    | TaskType
    | EnumType
    | StringEnumType
    | StructType
    | UnionType;
}
