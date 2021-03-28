import * as ts from "typescript";
import * as Defs from "./defs";
import * as Syntax from "./syntax";
import * as StringConverter from "../utils/string_converter";
import type { EmitClientSettings, ImportedController } from "../types/tool";


export const emit = (settings: EmitClientSettings) =>
  Syntax.createBundle(emitDeclarations(settings));

const emitDeclarations = (settings: EmitClientSettings) =>
  [...emitImports(settings.controllers), emitClientClass(settings)];

const emitImports = (controller: ImportedController[]) =>
  [emitClientBaseImport(), ...emitControllersImport(controller)];

const emitClientBaseImport = (): ts.ImportDeclaration =>
  Syntax.createNamedImportDeclaration(Defs.CLIENT_BASE_CLASS_NAME, Defs.CLIENT_BASE_MODULE_PATH);

const emitControllersImport = (controllers: ImportedController[]) =>
  controllers.map(x => Syntax.createNamedImportDeclaration(x.className, x.modulePath));

const emitClientClass = ({ clientClassName, controllers }: EmitClientSettings) =>
  ts.factory.createClassDeclaration(
    undefined,
    [Syntax.createExportModifier()],
    clientClassName,
    undefined,
    [Syntax.createExtendsClause(Defs.CLIENT_BASE_CLASS_NAME)],
    [...emitControllerProperties(controllers)]);

const emitControllerProperties = (controllers: ImportedController[]) =>
  controllers.map(x => emitControllerProperty(x));

// Emits: public readonly some = new SomeController(this);
const emitControllerProperty = (controller: ImportedController) =>
  ts.factory.createPropertyDeclaration(
    undefined,
    [Syntax.createPublicModifier(), Syntax.createReadonlyModifier()],
    toControllerPropertyName(controller),
    undefined,
    undefined,
    ts.factory.createNewExpression(
      ts.factory.createIdentifier(controller.className),
      undefined,
      [ts.factory.createThis()]));

const toControllerPropertyName = (controller: ImportedController) =>
  StringConverter.lowerFirstLetter(
    StringConverter.removeSuffix(controller.className, Defs.CONTROLLER_SUFFIX));