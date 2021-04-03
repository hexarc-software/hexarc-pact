import * as ts from "typescript";
import * as Defs from "./defs";
import * as Syntax from "./syntax";
import * as StringConverter from "../utils/string_converter";
import type { EmitClientSettings, ImportedController } from "../types/tool";


export function emit(settings: EmitClientSettings) {
  return Syntax.createBundle(emitDeclarations(settings));
}

function emitDeclarations(settings: EmitClientSettings) {
  return [...emitImports(settings.controllers), emitClientClass(settings)];
}

function emitImports(controller: ImportedController[]) {
  return [emitClientBaseImport(), ...emitControllersImport(controller)];
}

function emitClientBaseImport() {
  return Syntax.createNamedImportDeclaration(
    Defs.CLIENT_BASE_CLASS_NAME,
    `./${Defs.CLIENT_BASE_MODULE_PATH}`);
}

function emitControllersImport(controllers: ImportedController[]) {
  return controllers.map(x => Syntax.createNamedImportDeclaration(x.className, x.modulePath));
}

function emitClientClass({ clientClassName, controllers }: EmitClientSettings) {
  return ts.factory.createClassDeclaration(
    undefined,
    [Syntax.createExportModifier()],
    clientClassName,
    undefined,
    [Syntax.createExtendsClause(Defs.CLIENT_BASE_CLASS_NAME)],
    [...emitControllerProperties(controllers)]);
}

function emitControllerProperties(controllers: ImportedController[]) {
  return controllers.map(x => emitControllerProperty(x));
}

// Emits: public readonly some = new SomeController(this);
function emitControllerProperty(controller: ImportedController) {
  return ts.factory.createPropertyDeclaration(
    undefined,
    [Syntax.createPublicModifier(), Syntax.createReadonlyModifier()],
    toControllerPropertyName(controller),
    undefined,
    undefined,
    ts.factory.createNewExpression(
      ts.factory.createIdentifier(controller.className),
      undefined,
      [ts.factory.createThis()]));
}

function toControllerPropertyName(controller: ImportedController) {
  return StringConverter.lowerFirstLetter(
    StringConverter.removeSuffix(controller.className, Defs.CONTROLLER_SUFFIX));
}