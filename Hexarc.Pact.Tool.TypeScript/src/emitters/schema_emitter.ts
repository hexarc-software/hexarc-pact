import * as path from "path";
import * as ts from "typescript";
import * as Directory from "../utils/directory";
import * as File from "../utils/file";
import * as StringConverter from "../utils/string_converter";
import * as TypeRegistry from "../utils/type_registry";
import * as TypeReferenceEmitter from "./type_reference_emitter";

import * as DistinctTypeBundleEmitter from "./distinct_type_bundle_emitter";
import * as ControllerEmitter from "./controller_emitter";
import * as ClientEmitter from "./client_emitter";
import * as IndexBundleEmitter from "./index_bundle_emitter";

import type { Controller, Schema } from "../types/protocol/api";
import { ClientSettings, ImportedController, SchemaEmitter } from "../types/tool";


const BOOTSTRAP_PATH = "bootstrap";
const CONTROLLERS_PATH = "controllers";
const TYPES_PATH = "types";

const PRIMITIVE_TYPES_FILE_NAME = "primitive_types.d.ts";
const DISTINCT_TYPES_FILE_NAME = "distinct_types.d.ts";
const API_BASE_FILE_NAME = "client_base.ts";
const CONTROLLER_BASE_FILE_NAME = "controller_base.ts";
const API_FILE_NAME = "api.ts";
const INDEX_FILE_NAME = "index.ts";

export function create(schema: Schema, clientSettings: ClientSettings): SchemaEmitter {
  const outputDirectory = clientSettings.outputDirectory;
  const bootstrapDirectory = path.join(outputDirectory, BOOTSTRAP_PATH);
  const controllersDirectory = path.join(outputDirectory, CONTROLLERS_PATH);
  const typesDirectory = path.join(outputDirectory, TYPES_PATH);

  const primitiveTypesFilePath = path.join(typesDirectory, PRIMITIVE_TYPES_FILE_NAME);
  const distinctTypesFilePath = path.join(typesDirectory, DISTINCT_TYPES_FILE_NAME);
  const clientBaseFilePath = path.join(bootstrapDirectory, API_BASE_FILE_NAME);
  const controllerBaseFilePath = path.join(bootstrapDirectory, CONTROLLER_BASE_FILE_NAME);
  const apiFilePath = path.join(outputDirectory, API_FILE_NAME);
  const indexFilePath = path.join(outputDirectory, INDEX_FILE_NAME);

  const typeRegistry = TypeRegistry.create(schema.types);
  const typeReferenceEmitter = TypeReferenceEmitter.create(typeRegistry);
  const printer = ts.createPrinter({ newLine: ts.NewLineKind.LineFeed });

  async function prepare() {
    await Directory.clear(outputDirectory);
    await Directory.create(outputDirectory);
    await Directory.create(bootstrapDirectory);
    await Directory.create(typesDirectory);
    await Directory.create(controllersDirectory);
  }

  async function emitBootstrap() {
    await File.copy(path.resolve(__dirname, "../../bootstrap/client_base.ts"), clientBaseFilePath);
    await File.copy(path.resolve(__dirname, "../../bootstrap/controller_base.ts"), controllerBaseFilePath);
  }

  async function emitDistinctTypesFile() {
    const distinctTypes = typeRegistry.enumerateDistinctTypes();
    const bundle = DistinctTypeBundleEmitter.emit(distinctTypes, typeReferenceEmitter);
    const result = printer.printBundle(bundle);
    await File.writeString(distinctTypesFilePath, result);
  }

  async function emitPrimitiveTypesFile() {
    await File.copy(path.resolve(__dirname, "../../bootstrap/primitive_types.d.ts"), primitiveTypesFilePath);
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
    return path.join(controllersDirectory, fileName);
  }

  async function emitController(controller: Controller, filePath: string) {
    const bundle = ControllerEmitter.emit(controller);
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
    await File.writeString(apiFilePath, result);
  }

  async function emitIndex() {
    const primitiveTypesPath = path.join(TYPES_PATH, PRIMITIVE_TYPES_FILE_NAME);
    const distinctTypesPath = path.join(TYPES_PATH, DISTINCT_TYPES_FILE_NAME);
    const typeDefinitionPaths = [primitiveTypesPath, distinctTypesPath];
    const bundle = IndexBundleEmitter.emit({ typeDefinitionPaths });
    const result = printer.printBundle(bundle);
    await File.writeString(indexFilePath, result);
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