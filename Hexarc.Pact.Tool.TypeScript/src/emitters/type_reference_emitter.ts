import * as ts from "typescript";
import * as TypeUtils from "../utils/type_utils";
import * as PrimitiveTypeChecker from "./primitive_type_checker";

import { TupleElement, TypeReferenceKind } from "../types/protocol/type_references";
import type { TypeReferenceEmitter, TypeRegistry } from "../types/tool";
import type {
  TypeReference, PrimitiveTypeReference, NullableTypeReference,
  ArrayTypeReference, DictionaryTypeReference, TaskTypeReference,
  TypeParameterReference, LiteralTypeReference, DistinctTypeReference,
  TupleTypeReference
} from "../types/protocol/type_references";


export function create(typeRegistry: TypeRegistry): TypeReferenceEmitter {

  function emit(typeReference: TypeReference, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.TypeNode {
    switch (typeReference.kind) {
      case TypeReferenceKind.Primitive: return emitPrimitiveTypeReference(typeReference, moduleNamespace);
      case TypeReferenceKind.Dynamic: return emitDynamicTypeReference();
      case TypeReferenceKind.Nullable: return emitNullableTypeReference(typeReference, currentNamespace, moduleNamespace);
      case TypeReferenceKind.Array: return emitArrayTypeReference(typeReference, currentNamespace, moduleNamespace);
      case TypeReferenceKind.Dictionary: return emitDictionaryTypeReference(typeReference, currentNamespace, moduleNamespace);
      case TypeReferenceKind.Task: return emitTaskTypeReference(typeReference, currentNamespace, moduleNamespace);
      case TypeReferenceKind.TypeParameter: return emitTypeParameterReference(typeReference);
      case TypeReferenceKind.Literal: return emitLiteralTypeReference(typeReference);
      case TypeReferenceKind.Tuple: return emitTupleTypeReference(typeReference, currentNamespace, moduleNamespace);
      case TypeReferenceKind.Distinct: return emitDistinctTypeReference(typeReference, currentNamespace, moduleNamespace);
      default: throw new Error(`Couldn't emit a Hexarc Pact type reference for ${JSON.stringify(typeReference)}`);
    }
  }

  function emitPrimitiveTypeReference(typeReference: PrimitiveTypeReference, moduleNamespace: string | undefined): ts.TypeReferenceNode {
    // This type emission must be synced with the primitives boilerplate.
    const type = typeRegistry.getPrimitiveType(typeReference.typeId);
    const prefix = moduleNamespace == null ? "" : `${moduleNamespace}.`;
    if (PrimitiveTypeChecker.isBoolean(type)) return ts.factory.createTypeReferenceNode(`${prefix}Boolean`);
    if (PrimitiveTypeChecker.isByte(type)) return ts.factory.createTypeReferenceNode(`${prefix}Byte`);
    if (PrimitiveTypeChecker.isSByte(type)) return ts.factory.createTypeReferenceNode(`${prefix}SByte`);
    if (PrimitiveTypeChecker.isChar(type)) return ts.factory.createTypeReferenceNode(`${prefix}Char`);
    if (PrimitiveTypeChecker.isString(type)) return ts.factory.createTypeReferenceNode(`${prefix}String`);
    if (PrimitiveTypeChecker.isInt16(type)) return ts.factory.createTypeReferenceNode(`${prefix}Int16`);
    if (PrimitiveTypeChecker.isUInt16(type)) return ts.factory.createTypeReferenceNode(`${prefix}UInt16`);
    if (PrimitiveTypeChecker.isInt32(type)) return ts.factory.createTypeReferenceNode(`${prefix}Int32`);
    if (PrimitiveTypeChecker.isUInt32(type)) return ts.factory.createTypeReferenceNode(`${prefix}UInt32`);
    if (PrimitiveTypeChecker.isInt64(type)) return ts.factory.createTypeReferenceNode(`${prefix}Int64`);
    if (PrimitiveTypeChecker.isUInt64(type)) return ts.factory.createTypeReferenceNode(`${prefix}UInt64`);
    if (PrimitiveTypeChecker.isUInt64(type)) return ts.factory.createTypeReferenceNode(`${prefix}UInt64`);
    if (PrimitiveTypeChecker.isSingle(type)) return ts.factory.createTypeReferenceNode(`${prefix}Single`);
    if (PrimitiveTypeChecker.isDouble(type)) return ts.factory.createTypeReferenceNode(`${prefix}Double`);
    if (PrimitiveTypeChecker.isDecimal(type)) return ts.factory.createTypeReferenceNode(`${prefix}Decimal`);
    if (PrimitiveTypeChecker.isGuid(type)) return ts.factory.createTypeReferenceNode(`${prefix}Guid`);
    if (PrimitiveTypeChecker.isDateTime(type)) return ts.factory.createTypeReferenceNode(`${prefix}DateTime`);
    throw new Error(`Unsupported type ${JSON.stringify(type)}`);
  }

  function emitDynamicTypeReference(): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode("any");
  }

  function emitNullableTypeReference(typeReference: NullableTypeReference, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.TypeNode {
    const nullTypeReference = ts.factory.createTypeReferenceNode("null");
    const underlyingTypeReference = emit(typeReference.underlyingType, currentNamespace, moduleNamespace);
    return ts.factory.createUnionTypeNode([underlyingTypeReference, nullTypeReference]);
  }

  function emitArrayTypeReference(typeReference: ArrayTypeReference, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.TypeNode {
    return ts.factory.createArrayTypeNode(emit(typeReference.elementType, currentNamespace, moduleNamespace));
  }

  function emitDictionaryTypeReference(typeReference: DictionaryTypeReference, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.TypeNode {
    const keyTypeReference = ts.factory.createTypeReferenceNode("string");
    const valueTypeReference = emit(typeReference.valueType, currentNamespace, moduleNamespace);
    const keyParameter = ts.factory.createParameterDeclaration(undefined, undefined, undefined, "key", undefined, keyTypeReference);
    const indexSignature = ts.factory.createIndexSignature(undefined, undefined, [keyParameter], valueTypeReference);
    return ts.factory.createTypeLiteralNode([indexSignature]);
  }

  function emitTaskTypeReference(typeReference: TaskTypeReference, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.TypeReferenceNode {
    const typeArguments = typeReference.resultType == null ? 
      [ts.factory.createTypeReferenceNode("void")] :
      [emit(typeReference.resultType, currentNamespace, moduleNamespace)];
    return ts.factory.createTypeReferenceNode("Promise", typeArguments);
  }

  function emitTypeParameterReference(typeReference: TypeParameterReference): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode(typeReference.name);
  }

  function emitLiteralTypeReference(typeReference: LiteralTypeReference): ts.TypeNode {
    return ts.factory.createLiteralTypeNode(ts.factory.createStringLiteral(typeReference.name));
  }

  function emitTupleTypeReference(typeReference: TupleTypeReference, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.TypeNode {
    const elements = emitTupleElements(typeReference.elements, currentNamespace, moduleNamespace)
    return ts.factory.createTupleTypeNode(elements);
  }

  function emitTupleElements(elements: TupleElement[], currentNamespace: string | undefined, moduleNamespace: string | undefined): (ts.TypeNode | ts.NamedTupleMember)[] {
    return (elements.every(x => x.name != null)) ?
      elements.map(x => emitNamedTupleElement(x, currentNamespace, moduleNamespace)) :
      elements.map(x => emit(x.type, currentNamespace, moduleNamespace));
  }

  function emitNamedTupleElement(element: TupleElement, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.NamedTupleMember {
    const identifier = ts.factory.createIdentifier(element.name!);
    return ts.factory.createNamedTupleMember(undefined, identifier, undefined, emit(element.type, currentNamespace, moduleNamespace))
  }

  function emitDistinctTypeReference(typeReference: DistinctTypeReference, currentNamespace: string | undefined, moduleNamespace: string | undefined): ts.TypeReferenceNode {
    const type = typeRegistry.getDistinctType(typeReference.typeId);
    const typeName = TypeUtils.isSameNamespace(type, currentNamespace) && moduleNamespace == null ? type.name : TypeUtils.computeFullName(type, moduleNamespace);
    const typeArguments = emitTypeArguments(typeReference.typeArguments, currentNamespace, moduleNamespace);
    return ts.factory.createTypeReferenceNode(typeName, typeArguments);
  }

  function emitTypeArguments(typeArguments: TypeReference[] | undefined, currentNamespace: string | undefined, moduleNamespace: string | undefined) {
    if (typeArguments != null) return typeArguments.map(x => emit(x, currentNamespace, moduleNamespace));
  }

  return { emit };
}
