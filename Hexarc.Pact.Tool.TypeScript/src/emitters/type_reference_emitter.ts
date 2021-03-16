import * as ts from "typescript";
import type { TypeReferenceEmitter } from "../types/tool";
import type {
  TypeReference, PrimitiveTypeReference, NullableTypeReference,
  ArrayTypeReference, DictionaryTypeReference, TaskTypeReference,
  TypeParameterReference, LiteralTypeReference, DistinctTypeReference
} from "../types/protocol/type_references";


export function create(): TypeReferenceEmitter {

  function emit(typeReference: TypeReference, currentNamespace?: string): ts.TypeNode {
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
    throw new Error("Not implemented");
  }

  function emitDynamicTypeReference(): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode("any");
  }

  function emitNullableTypeReference(typeReference: NullableTypeReference, currentNamespace?: string): ts.TypeNode {
    const nullTypeReference = ts.factory.createTypeReferenceNode("null");
    const underlyingTypeReference = emit(typeReference.underlyingType, currentNamespace);
    return ts.factory.createUnionTypeNode([underlyingTypeReference, nullTypeReference]);
  }

  function emitArrayTypeReference(typeReference: ArrayTypeReference, currentNamespace?: string): ts.TypeNode {
    return ts.factory.createArrayTypeNode(emit(typeReference.elementType, currentNamespace));
  }

  function emitDictionaryTypeReference(typeReference: DictionaryTypeReference, currentNamespace?: string): ts.TypeNode {
    const keyTypeReference = ts.factory.createTypeReferenceNode("string");
    const valueTypeReference = emit(typeReference.valueType, currentNamespace);
    const keyParameter = ts.factory.createParameterDeclaration(undefined, undefined, undefined, "key", undefined, keyTypeReference);
    const indexSignature = ts.factory.createIndexSignature(undefined, undefined, [keyParameter], valueTypeReference);
    return ts.factory.createTypeLiteralNode([indexSignature]);
  }

  function emitTaskTypeReference(typeReference: TaskTypeReference, currentNamespace?: string): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode("Promise", [emit(typeReference.resultType, currentNamespace)]);
  }

  function emitTypeParameterReference(typeReference: TypeParameterReference): ts.TypeReferenceNode {
    return ts.factory.createTypeReferenceNode(typeReference.name);
  }

  function emitLiteralTypeReference(typeReference: LiteralTypeReference): ts.TypeNode {
    return ts.factory.createLiteralTypeNode(ts.factory.createStringLiteral(typeReference.name));
  }

  function emitDistinctTypeReference(typeReference: DistinctTypeReference, currentNamespace?: string): ts.TypeReferenceNode {
    throw new Error("Not implemented");
  }

  return { emit };
}
