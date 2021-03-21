import * as ts from "typescript";
import * as TypeUtils from "../utils/type_utils";
import * as PrimitiveTypeChecker from "./primitive_type_checker";

import type { TypeReferenceEmitter, TypeRegistry } from "../types/tool";
import type {
  TypeReference, PrimitiveTypeReference, NullableTypeReference,
  ArrayTypeReference, DictionaryTypeReference, TaskTypeReference,
  TypeParameterReference, LiteralTypeReference, DistinctTypeReference
} from "../types/protocol/type_references";


export function create(typeRegistry: TypeRegistry): TypeReferenceEmitter {

  function emit(typeReference: TypeReference, currentNamespace: string | undefined): ts.TypeNode {
    switch (typeReference.kind) {
      case "Primitive": return emitPrimitiveTypeReference(typeReference);
      case "Dynamic": return emitDynamicTypeReference();
      case "Nullable": return emitNullableTypeReference(typeReference, currentNamespace);
      case "Array": return emitArrayTypeReference(typeReference, currentNamespace);
      case "Dictionary": return emitDictionaryTypeReference(typeReference, currentNamespace);
      case "Task": return emitTaskTypeReference(typeReference, currentNamespace);
      case "TypeParameter": return emitTypeParameterReference(typeReference);
      case "Literal": return emitLiteralTypeReference(typeReference);
      case "Distinct": return emitDistinctTypeReference(typeReference, currentNamespace);
      default: throw new Error(`Couldn't emit a Hexarc Pact type reference for ${JSON.stringify(typeReference)}`);
    }
  }

  function emitPrimitiveTypeReference(typeReference: PrimitiveTypeReference): ts.TypeReferenceNode {
    // This type emission must be synced with the primitives boilerplate.
    const type = typeRegistry.getPrimitiveType(typeReference.typeId);
    if (PrimitiveTypeChecker.isBoolean(type)) return ts.factory.createTypeReferenceNode("boolean");
    if (PrimitiveTypeChecker.isByte(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Byte");
    if (PrimitiveTypeChecker.isSByte(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.SByte");
    if (PrimitiveTypeChecker.isChar(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Char");
    if (PrimitiveTypeChecker.isString(type)) return ts.factory.createTypeReferenceNode("string");
    if (PrimitiveTypeChecker.isInt16(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Int16");
    if (PrimitiveTypeChecker.isUInt16(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.UInt16");
    if (PrimitiveTypeChecker.isInt32(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Int32");
    if (PrimitiveTypeChecker.isUInt32(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.UInt32");
    if (PrimitiveTypeChecker.isInt64(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Int64");
    if (PrimitiveTypeChecker.isUInt64(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.UInt64");
    if (PrimitiveTypeChecker.isUInt64(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.UInt64");
    if (PrimitiveTypeChecker.isSingle(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Single");
    if (PrimitiveTypeChecker.isDouble(type)) return ts.factory.createTypeReferenceNode("number");
    if (PrimitiveTypeChecker.isDecimal(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Decimal");
    if (PrimitiveTypeChecker.isGuid(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.Guid");
    if (PrimitiveTypeChecker.isDateTime(type)) return ts.factory.createTypeReferenceNode("Hexarc.Pact.Types.DateTime");
    throw new Error(`Unsupported type ${JSON.stringify(type)}`);
  }

  function emitDynamicTypeReference(): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode("any");
  }

  function emitNullableTypeReference(typeReference: NullableTypeReference, currentNamespace: string | undefined): ts.TypeNode {
    const nullTypeReference = ts.factory.createTypeReferenceNode("null");
    const underlyingTypeReference = emit(typeReference.underlyingType, currentNamespace);
    return ts.factory.createUnionTypeNode([underlyingTypeReference, nullTypeReference]);
  }

  function emitArrayTypeReference(typeReference: ArrayTypeReference, currentNamespace: string | undefined): ts.TypeNode {
    return ts.factory.createArrayTypeNode(emit(typeReference.elementType, currentNamespace));
  }

  function emitDictionaryTypeReference(typeReference: DictionaryTypeReference, currentNamespace: string | undefined): ts.TypeNode {
    const keyTypeReference = ts.factory.createTypeReferenceNode("string");
    const valueTypeReference = emit(typeReference.valueType, currentNamespace);
    const keyParameter = ts.factory.createParameterDeclaration(undefined, undefined, undefined, "key", undefined, keyTypeReference);
    const indexSignature = ts.factory.createIndexSignature(undefined, undefined, [keyParameter], valueTypeReference);
    return ts.factory.createTypeLiteralNode([indexSignature]);
  }

  function emitTaskTypeReference(typeReference: TaskTypeReference, currentNamespace: string | undefined): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode("Promise", [emit(typeReference.resultType, currentNamespace)]);
  }

  function emitTypeParameterReference(typeReference: TypeParameterReference): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode(typeReference.name);
  }

  function emitLiteralTypeReference(typeReference: LiteralTypeReference): ts.TypeNode {
    return ts.factory.createLiteralTypeNode(ts.factory.createStringLiteral(typeReference.name));
  }

  function emitDistinctTypeReference(typeReference: DistinctTypeReference, currentNamespace: string | undefined): ts.TypeReferenceNode {
    const type = typeRegistry.getDistinctType(typeReference.typeId);
    const typeName = TypeUtils.isSameNamespace(type, currentNamespace) ? type.name : TypeUtils.computeFullName(type);
    const typeArguments = emitTypeArguments(typeReference.typeArguments, currentNamespace);
    return ts.factory.createTypeReferenceNode(typeName, typeArguments);
  }

  function emitTypeArguments(typeArguments: TypeReference[] | undefined, currentNamespace: string | undefined) {
    if (typeArguments != null) return typeArguments.map(x => emit(x, currentNamespace));
  }

  return { emit };
}
