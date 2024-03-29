import * as ts from "typescript";
import type { ObjectProperty, ObjectType } from "../types/protocol/types";
import type { TypeReference } from "../types/protocol/type_references";
import type { TypeReferenceEmitter } from "../types/tool";


export function emit(type: ObjectType, typeReferenceEmitter: TypeReferenceEmitter): ts.InterfaceDeclaration {
  const modifiers = [ts.factory.createModifier(ts.SyntaxKind.ExportKeyword)];
  const properties = type.properties.map(x => emitProperty(x, type.namespace, typeReferenceEmitter));
  const typeParameters = emitTypeParameters(type.typeParameters);
  return ts.factory.createInterfaceDeclaration(
    undefined,
    modifiers,
    type.name,
    typeParameters,
    undefined,
    properties);
}

function emitTypeParameters(typeParameters: string[] | undefined) {
  if (typeParameters != null) return typeParameters.map(x => ts.factory.createTypeParameterDeclaration(x));
}

function emitProperty(
  property: ObjectProperty,
  currentNamespace: string | undefined,
  typeReferenceEmitter: TypeReferenceEmitter
): ts.TypeElement {
  const modifiers = [ts.factory.createModifier(ts.SyntaxKind.ReadonlyKeyword)];
  const questionToken = emitQuestionToken(property.type);
  const propertyType = typeReferenceEmitter.emit(property.type, currentNamespace, undefined);
  return ts.factory.createPropertySignature(
    modifiers,
    property.name,
    questionToken,
    propertyType);
}

function emitQuestionToken(typeReference: TypeReference): ts.QuestionToken | undefined {
  return typeReference.kind === "Nullable" ? ts.factory.createToken(ts.SyntaxKind.QuestionToken) : undefined;
}