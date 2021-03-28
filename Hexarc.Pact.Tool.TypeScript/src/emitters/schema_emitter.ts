import * as path from "path";
import * as ts from "typescript";
import * as Directory from "../utils/directory";
import * as File from "../utils/file";
import * as StringConverter from "../utils/string_converter";
import * as TypeRegistry from "../utils/type_registry";
import * as TypeReferenceEmitter from "./type_reference_emitter";

import * as Defs from "./defs";
import * as DistinctTypesEmitter from "./distinct_types_emitter";
import * as ControllerEmitter from "./controller_emitter";
import * as ClientEmitter from "./client_emitter";
import * as IndexBundleEmitter from "./index_emitter";

import type { Controller, Schema } from "../types/protocol/api";
import { ClientSettings, ImportedController, SchemaEmitter } from "../types/tool";


export function create(schema: Schema, clientSettings: ClientSettings): SchemaEmitter {
  const outputDirectory = clientSettings.outputDirectory;

  const typeRegistry = TypeRegistry.create(schema.types);
  const typeReferenceEmitter = TypeReferenceEmitter.create(typeRegistry);
  const printer = ts.createPrinter({ newLine: ts.NewLineKind.LineFeed });

  async function prepare() {
    await Directory.clear(outputDirectory);
    await Directory.create(outputDirectory);
    await Directory.create(path.join(outputDirectory, Defs.BOOTSTRAP_FOLDER_NAME));
    await Directory.create(path.join(outputDirectory, Defs.TYPES_FOLDER_NAME));
    await Directory.create(path.join(outputDirectory, Defs.CONTROLLERS_FOLDER_NAME));
  }

  async function emitBootstrap() {
    await File.copy(
      path.resolve(__dirname, Defs.CLIENT_BASE_SOURCE_PATH), 
      path.join(outputDirectory, Defs.CLIENT_BASE_TARGET_PATH));
    await File.copy(
      path.resolve(__dirname, Defs.CONTROLLER_BASE_SOURCE_PATH), 
      path.join(outputDirectory, Defs.CONTROLLER_BASE_TARGET_PATH));
  }

  async function emitDistinctTypesFile() {
    const distinctTypes = typeRegistry.enumerateDistinctTypes();
    const bundle = DistinctTypesEmitter.emit(distinctTypes, typeReferenceEmitter);
    const result = printer.printBundle(bundle);
    await File.writeString(path.join(outputDirectory, Defs.DISTINCT_TYPES_TARGET_PATH), result);
  }

  async function emitPrimitiveTypesFile() {
    await File.copy(
      path.resolve(__dirname, Defs.PRIMITIVES_TYPES_SOURCE_PATH), 
      path.join(outputDirectory, Defs.PRIMITIVES_TYPES_TARGET_PATH));
  }

  async function emitTypes() {
    await emitPrimitiveTypesFile();
    await emitDistinctTypesFile();
  }

  async function emitControllers() {
    schema.controllers
      .map(controller => ({ controller, filePath: computeControllerPath(controller) }))
      .forEach(async ({ controller, filePath }) => await emitController(controller, `${filePath}.ts`));
  }

  function computeControllerPath(controller: Controller) {
    const fileName = StringConverter.fromPascalToUnderscore(controller.name);
    return path.join(outputDirectory, Defs.CONTROLLERS_FOLDER_NAME, fileName);
  }

  async function emitController(controller: Controller, filePath: string) {
    const typeDefinitionPaths = [`../${Defs.PRIMITIVES_TYPES_TARGET_PATH}`, `../${Defs.DISTINCT_TYPES_TARGET_PATH}`];
    const bundle = ControllerEmitter.emit(controller, typeDefinitionPaths, typeReferenceEmitter);
    const result = printer.printBundle(bundle);
    await File.writeString(filePath, result);
  }

  function computeImportedController(controller: Controller): ImportedController {
    return {
      className: controller.name,
      modulePath: `./controllers/${StringConverter.fromPascalToUnderscore(controller.name)}`
    };
  }

  async function emitApiClient() {
    const importedControllers = schema.controllers.map(x => computeImportedController(x));
    const bundle = ClientEmitter.emit({ 
      clientClassName: clientSettings.clientClassName, 
      controllers: importedControllers,
      apiBasePath: ""
    });
    const result = printer.printBundle(bundle);
    await File.writeString(path.join(outputDirectory, Defs.API_TARGET_PATH), result);
  }

  async function emitIndex() {
    const typeDefinitionPaths = [Defs.PRIMITIVES_TYPES_TARGET_PATH, Defs.DISTINCT_TYPES_TARGET_PATH];
    const bundle = IndexBundleEmitter.emit({ typeDefinitionPaths, clientClassName: clientSettings.clientClassName });
    const result = printer.printBundle(bundle);
    await File.writeString(path.join(outputDirectory, Defs.INDEX_TARGET_PATH), result);
  }

  async function emit() {
    await emitBootstrap();
    await emitTypes();
    await emitControllers();
    await emitApiClient();
    await emitIndex();
  }

  return { prepare, emit, emitBootstrap, emitTypes, emitControllers, emitApiClient, emitIndex };
}