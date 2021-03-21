import * as ts from "typescript";
import type { ObjectProperty, ObjectType } from "../types/protocol/types";
import type { TypeReference } from "../types/protocol/type_references";
import type { TypeReferenceEmitter } from "../types/tool";


export function emit(type: ObjectType, typeReferenceEmitter: TypeReferenceEmitter): ts.InterfaceDeclaration {
  const properties = type.properties.map(x => emitProperty(x, type.namespace, typeReferenceEmitter));
  return ts.factory.createInterfaceDeclaration(
    undefined,
    undefined,
    type.name,
    undefined,
    undefined,
    properties);
}

function emitProperty(
  property: ObjectProperty,
  currentNamespace: string | undefined,
  typeReferenceEmitter: TypeReferenceEmitter
): ts.TypeElement {
  const modifiers = [ts.factory.createModifier(ts.SyntaxKind.ReadonlyKeyword)];
  const questionToken = emitQuestionToken(property.type);
  const propertyType = typeReferenceEmitter.emit(property.type, currentNamespace);
  return ts.factory.createPropertySignature(
    modifiers,
    property.name,
    questionToken,
    propertyType);
}

function emitQuestionToken(typeReference: TypeReference): ts.QuestionToken | undefined {
  return typeReference.kind === "Nullable" ? ts.factory.createToken(ts.SyntaxKind.QuestionToken) : undefined;
}