import * as ts from "typescript";
import * as Defs from "./defs";
import * as Syntax from "./syntax";
import { TypeReferenceKind } from "../types/protocol/type_references";
import { Controller, HttpMethod, Method, MethodParameter } from "../types/protocol/api";
import type { TypeReferenceEmitter } from "../types/tool";


export const emit = (controller: Controller, typeDefinitionPaths: string[], typeReferenceEmitter: TypeReferenceEmitter) =>
  Syntax.createBundle(
    emitDeclarations(controller, typeReferenceEmitter),
    emitTypeDefinitionSources(typeDefinitionPaths));

const emitTypeDefinitionSources = (paths: string[]) =>
  paths.map(x => emitTypeDefinitionSource(x));

const emitTypeDefinitionSource = (path: string) =>
  ts.createUnparsedSourceFile(`/// <reference path="${path}" />`);

const emitDeclarations = (controller: Controller, typeReferenceEmitter: TypeReferenceEmitter) =>
  [...emitImports(), emitClass(controller, typeReferenceEmitter)];

const emitImports = () =>
  [emitControllerBaseImport(), emitClientBaseImport()];

const emitControllerBaseImport = () =>
  Syntax.createNamedImportDeclaration(
    Defs.CONTROLLER_BASE_CLASS_NAME,
    `../${Defs.CONTROLLER_BASE_MODULE_PATH}`);

const emitClientBaseImport = () =>
  Syntax.createNamedImportDeclaration(
    Defs.CLIENT_BASE_CLASS_NAME,
    `../${Defs.CLIENT_BASE_MODULE_PATH}`);

const emitClass = (controller: Controller, typeReferenceEmitter: TypeReferenceEmitter) =>
  ts.factory.createClassDeclaration(
    undefined,
    [Syntax.createExportModifier()],
    controller.name,
    undefined,
    [Syntax.createExtendsClause(Defs.CONTROLLER_BASE_CLASS_NAME)],
    [emitConstructor(controller), ...emitMethods(controller.methods, typeReferenceEmitter)]);

const emitConstructor = (controller: Controller) =>
  ts.factory.createConstructorDeclaration(
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

const emitMethods = (methods: Method[], typeReferenceEmitter: TypeReferenceEmitter) =>
  methods.map(x => emitMethod(x, typeReferenceEmitter));

const emitMethod = (method: Method, typeReferenceEmitter: TypeReferenceEmitter) =>
  ts.factory.createMethodDeclaration(
    undefined,
    [Syntax.createPublicModifier(), Syntax.createAsyncModifier()],
    undefined,
    method.name,
    undefined,
    undefined,
    emitMethodParameters(method.parameters, typeReferenceEmitter),
    typeReferenceEmitter.emit(method.returnType, undefined),
    emitMethodBody(method));

const emitMethodParameters = (parameters: MethodParameter[], typeReferenceEmitter: TypeReferenceEmitter) =>
  parameters.map(x => emitMethodParameter(x, typeReferenceEmitter));

const emitMethodParameter = (parameter: MethodParameter, typeReferenceEmitter: TypeReferenceEmitter) =>
  ts.factory.createParameterDeclaration(
    undefined,
    undefined,
    undefined,
    parameter.name,
    parameter.type.kind === TypeReferenceKind.Nullable ? ts.factory.createToken(ts.SyntaxKind.QuestionToken) : undefined,
    typeReferenceEmitter.emit(parameter.type, undefined),
    undefined);

const emitMethodBody = (method: Method) => {
  switch (method.httpMethod) {
    case HttpMethod.Get: return emitGetMethodBody(method);
    case HttpMethod.Post: return emitPostBody(method);
    default: throw new Error(`Not supported method: ${method.httpMethod}`);
  }
}

const emitGetMethodBody = (method: Method) =>
  ts.factory.createBlock([
    emitGetMethodArgumentsVar(method.parameters),
    emitGetJsonInvocation(method.path)
  ], true);

const emitPostBody = (method: Method) =>
  ts.factory.createBlock([
    emitPostJsonInvocation(method)
  ], true);

const emitGetMethodArgumentsVar = (parameters: MethodParameter[]) =>
  ts.factory.createVariableStatement(
    undefined,
    ts.factory.createVariableDeclarationList([
      ts.factory.createVariableDeclaration(
        "args",
        undefined,
        undefined,
        emitGetMethodArguments(parameters))],
      ts.NodeFlags.Const));

const emitGetMethodArguments = (parameters: MethodParameter[]) =>
  ts.factory.createArrayLiteralExpression(
    parameters.map(x => emitGetMethodArgument(x)),
    true);

const emitGetMethodArgument = (parameter: MethodParameter) =>
  ts.factory.createObjectLiteralExpression([
    ts.factory.createPropertyAssignment("name", ts.factory.createStringLiteral(parameter.name)),
    ts.factory.createPropertyAssignment("value", ts.factory.createIdentifier(parameter.name))], true);

const emitGetJsonInvocation = (path: string) =>
  ts.factory.createReturnStatement(
    ts.factory.createAwaitExpression(
      ts.factory.createCallExpression(
        ts.factory.createPropertyAccessExpression(
          ts.factory.createThis(),
          ts.factory.createIdentifier("getJson")),
        undefined,
        [ts.factory.createStringLiteral(path), ts.factory.createIdentifier("args")])));

const emitPostJsonInvocation = (method: Method) =>
  ts.factory.createReturnStatement(
    ts.factory.createAwaitExpression(
      ts.factory.createCallExpression(
        ts.factory.createPropertyAccessExpression(
          ts.factory.createThis(),
          ts.factory.createIdentifier("postJson")),
        undefined,
        [ts.factory.createStringLiteral(method.path), ts.factory.createIdentifier(method.parameters[0].name)])));