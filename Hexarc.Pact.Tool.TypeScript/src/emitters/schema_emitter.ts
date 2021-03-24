import * as path from "path";
import * as ts from "typescript";
import * as Directory from "../utils/directory";
import * as File from "../utils/file";
import * as TypeRegistry from "../utils/type_registry";
import * as TypeReferenceEmitter from "./type_reference_emitter";
import * as TypeDefinitionsEmitter from "./type_definitions_emitter";

import type { Schema } from "../types/protocol/api";
import type { ClientSettings, SchemaEmitter } from "../types/tool";


const BOOTSTRAP_PATH = "bootstrap";
const CONTROLLERS_PATH = "controllers";
const TYPES_PATH = "types";

const PRIMITIVE_TYPES_FILE_NAME = "primitive_types.d.ts";
const DISTINCT_TYPES_FILE_NAME = "distinct_types.d.ts";

export function create(schema: Schema, clientSettings: ClientSettings): SchemaEmitter {
  const outputDirectory = clientSettings.outputDirectory;
  const bootstrapDirectory = path.join(outputDirectory, BOOTSTRAP_PATH);
  const controllersDirectory = path.join(outputDirectory, CONTROLLERS_PATH);
  const typesDirectory = path.join(outputDirectory, TYPES_PATH);

  const primitiveTypesFilePath = path.join(typesDirectory, PRIMITIVE_TYPES_FILE_NAME);
  const distinctTypesFilePath = path.join(typesDirectory, DISTINCT_TYPES_FILE_NAME);

  const typeRegistry = TypeRegistry.create(schema.types);
  const typeReferenceEmitter = TypeReferenceEmitter.create(typeRegistry);

  const sourceFile = ts.createSourceFile("source.ts", "", ts.ScriptTarget.ESNext, false, ts.ScriptKind.TS);
  const printer = ts.createPrinter({ newLine: ts.NewLineKind.LineFeed });

  async function prepare() {
    await Directory.clear(outputDirectory);
    await Directory.create(outputDirectory);
    await Directory.create(bootstrapDirectory);
    await Directory.create(typesDirectory);
    await Directory.create(controllersDirectory);
  }

  async function emitBootstrap() {
    
  }

  async function emitDistinctTypesFile() {
    const distinctTypes = typeRegistry.enumerateDistinctTypes();
    const distinctDefinitions = TypeDefinitionsEmitter.emit(distinctTypes, typeReferenceEmitter);
    const result = printer.printList(ts.ListFormat.MultiLine, distinctDefinitions, sourceFile);
    await File.writeString(distinctTypesFilePath, result);
  }

  async function emitPrimitiveTypesFile() {
    const source = path.resolve(__dirname, "../../bootstrap/primitive_types.d.ts");
    await File.copy(source, primitiveTypesFilePath);
  }

  async function emitTypes() {
    await emitPrimitiveTypesFile();
    await emitDistinctTypesFile();
  }

  async function emitControllers() {

  }

  async function emitApiClient() {
    
  }

  async function emitIndex() {
    
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