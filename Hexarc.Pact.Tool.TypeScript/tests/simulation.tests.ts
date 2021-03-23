import * as ts from "typescript";
import * as util from "util";
import { ClientSettingsReader, DistinctTypeEmitter, SchemaReader, TypeDefinitionsEmitter, TypeReferenceEmitter, TypeRegistry } from "../src/pact";


(async function read() {
  util.inspect.defaultOptions.depth = null;
  const settingsCollection = await ClientSettingsReader.read();
  console.log(settingsCollection);
  for (let settings of settingsCollection!) {
    const schema = await SchemaReader.read(settings.schemaUri, settings.scopes);
    const typeRegistry = TypeRegistry.create(schema.types);
    const typeReferenceEmitter = TypeReferenceEmitter.create(typeRegistry);
    const distinctTypes = typeRegistry.enumerateDistinctTypes();
    const definitions = TypeDefinitionsEmitter.emit(distinctTypes, typeReferenceEmitter);
    const file = ts.createSourceFile("source.ts", "", ts.ScriptTarget.ESNext, false, ts.ScriptKind.TS);
    const printer = ts.createPrinter({ newLine: ts.NewLineKind.LineFeed });
    const result = printer.printList(ts.ListFormat.MultiLine, definitions, file);
    console.log(result);
  }
})();
