import * as ts from "typescript";
import * as Defs from "./defs";
import * as Syntax from "./syntax";
import { TypeReferenceKind } from "../types/protocol/type_references";
import { Controller, HttpMethod, Method, MethodParameter } from "../types/protocol/api";
import type { TypeReferenceEmitter } from "../types/tool";


export function emit(controller: Controller, typeDefinitionPaths: string[], typeReferenceEmitter: TypeReferenceEmitter) {
  return Syntax.createBundle(
    emitDeclarations(controller, typeReferenceEmitter),
    emitTypeDefinitionSources(typeDefinitionPaths));
}

function emitTypeDefinitionSources(paths: string[]) {
  return paths.map(x => emitTypeDefinitionSource(x));
}

function emitTypeDefinitionSource(path: string) {
  return ts.createUnparsedSourceFile(`/// <reference path="${path}" />`);
}

function emitDeclarations(controller: Controller, typeReferenceEmitter: TypeReferenceEmitter) {
  return [...emitImports(), emitClass(controller, typeReferenceEmitter)];
}

function emitImports() {
  return [emitControllerBaseImport(), emitClientBaseImport()];
}

function emitControllerBaseImport() {
  return Syntax.createNamedImportDeclaration(
    Defs.CONTROLLER_BASE_CLASS_NAME,
    `../${Defs.CONTROLLER_BASE_MODULE_PATH}`);
}

function emitClientBaseImport() {
  return Syntax.createNamedImportDeclaration(
    Defs.CLIENT_BASE_CLASS_NAME,
    `../${Defs.CLIENT_BASE_MODULE_PATH}`);
}

function emitClass(controller: Controller, typeReferenceEmitter: TypeReferenceEmitter) {
  return ts.factory.createClassDeclaration(
    undefined,
    [Syntax.createExportModifier()],
    controller.name,
    undefined,
    [Syntax.createExtendsClause(Defs.CONTROLLER_BASE_CLASS_NAME)],
    [emitConstructor(controller), ...emitMethods(controller.methods, typeReferenceEmitter)]);
}

function emitConstructor(controller: Controller) {
  return ts.factory.createConstructorDeclaration(
    undefined,
    [Syntax.createPublicModifier()],
    [ts.factory.createParameterDeclaration(
      undefined,
      undefined,
      undefined,
      ts.factory.createIdentifier("clientBase"),
      undefined,
      ts.factory.createTypeReferenceNode(Defs.CLIENT_BASE_CLASS_NAME)),
    ts.factory.createParameterDeclaration(
      undefined,
      undefined,
      undefined,
      ts.factory.createIdentifier("path"),
      undefined,
      ts.factory.createTypeReferenceNode("string"),
      ts.factory.createStringLiteral(controller.path))],
    ts.factory.createBlock([
      ts.factory.createExpressionStatement(
        ts.factory.createCallExpression(
          ts.factory.createSuper(),
          undefined,
          [ts.factory.createIdentifier("clientBase"), ts.factory.createIdentifier("path")]))],
      true));
}

function emitMethods(methods: Method[], typeReferenceEmitter: TypeReferenceEmitter) {
  return methods.map(x => emitMethod(x, typeReferenceEmitter));
}

function emitMethod(method: Method, typeReferenceEmitter: TypeReferenceEmitter) {
  return ts.factory.createMethodDeclaration(
    undefined,
    [Syntax.createPublicModifier(), Syntax.createAsyncModifier()],
    undefined,
    method.name,
    undefined,
    undefined,
    emitMethodParameters(method.parameters, typeReferenceEmitter),
    typeReferenceEmitter.emit(method.returnType, undefined),
    emitMethodBody(method));
}

function emitMethodParameters(parameters: MethodParameter[], typeReferenceEmitter: TypeReferenceEmitter) {
  return parameters.map(x => emitMethodParameter(x, typeReferenceEmitter));
}

function emitMethodParameter(parameter: MethodParameter, typeReferenceEmitter: TypeReferenceEmitter) {
  return ts.factory.createParameterDeclaration(
    undefined,
    undefined,
    undefined,
    parameter.name,
    parameter.type.kind === TypeReferenceKind.Nullable ? ts.factory.createToken(ts.SyntaxKind.QuestionToken) : undefined,
    typeReferenceEmitter.emit(parameter.type, undefined),
    undefined);
}

function emitMethodBody(method: Method) {
  switch (method.httpMethod) {
    case HttpMethod.Get: return emitGetMethodBody(method);
    case HttpMethod.Post: return emitPostBody(method);
    default: throw new Error(`Not supported method: ${method.httpMethod}`);
  }
}

function emitGetMethodBody(method: Method) {
  const hasArguments = method.parameters.length > 0;
  return ts.factory.createBlock(
    hasArguments 
      ? [emitGetMethodArgumentsVar(method.parameters), emitGetJsonInvocation(method.path, hasArguments)]
      : [emitGetJsonInvocation(method.path)], true);
}

function emitPostBody(method: Method) {
  return ts.factory.createBlock([
    emitPostJsonInvocation(method)
  ], true);
}

function emitGetMethodArgumentsVar(parameters: MethodParameter[]) {
  return ts.factory.createVariableStatement(
    undefined,
    ts.factory.createVariableDeclarationList([
      ts.factory.createVariableDeclaration(
        "args",
        undefined,
        undefined,
        emitGetMethodArguments(parameters))],
      ts.NodeFlags.Const));
}

function emitGetMethodArguments(parameters: MethodParameter[]) {
  return ts.factory.createArrayLiteralExpression(parameters.map(x => emitGetMethodArgument(x)));
}

function emitGetMethodArgument(parameter: MethodParameter) {
  return ts.factory.createObjectLiteralExpression([
    ts.factory.createPropertyAssignment("name", ts.factory.createStringLiteral(parameter.name)),
    ts.factory.createPropertyAssignment("value", ts.factory.createIdentifier(parameter.name))], false);
}

function emitGetJsonInvocation(path: string, hasArguments: boolean = false) {
  return ts.factory.createReturnStatement(
    ts.factory.createAwaitExpression(
      ts.factory.createCallExpression(
        ts.factory.createPropertyAccessExpression(
          ts.factory.createThis(),
          ts.factory.createIdentifier("getJson")),
        undefined,
        hasArguments 
          ? [ts.factory.createStringLiteral(path), ts.factory.createIdentifier("args")]
          : [ts.factory.createStringLiteral(path)])));
}

function emitPostJsonInvocation(method: Method) {
  return ts.factory.createReturnStatement(
    ts.factory.createAwaitExpression(
      ts.factory.createCallExpression(
        ts.factory.createPropertyAccessExpression(
          ts.factory.createThis(),
          ts.factory.createIdentifier("postJson")),
        undefined,
        [ts.factory.createStringLiteral(method.path), ts.factory.createIdentifier(method.parameters[0].name)])));
}