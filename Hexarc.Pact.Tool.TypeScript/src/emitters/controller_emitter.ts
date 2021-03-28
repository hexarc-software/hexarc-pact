import * as ts from "typescript";
import * as Defs from "./defs";
import * as Syntax from "./syntax";
import type { Controller } from "../types/protocol/api";


export const emit = (controller: Controller) =>
  Syntax.createBundle(emitDeclarations(controller));

const emitDeclarations = (controller: Controller) =>
  [...emitImports(), emitClass(controller)];

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

const emitClass = (controller: Controller) =>
  ts.factory.createClassDeclaration(
    undefined,
    [Syntax.createExportModifier()],
    controller.name,
    undefined,
    [Syntax.createExtendsClause(Defs.CONTROLLER_BASE_CLASS_NAME)],
    [emitConstructor(controller)]);

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
          [ts.factory.createIdentifier("clientBase"), ts.factory.createIdentifier("path")]))]));