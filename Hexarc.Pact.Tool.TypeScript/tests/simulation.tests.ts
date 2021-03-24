import * as util from "util";
import { ClientSettingsReader, SchemaEmitter, SchemaReader } from "../src/pact";


(async function read() {
  util.inspect.defaultOptions.depth = null;
  const settingsCollection = await ClientSettingsReader.read();
  console.log(settingsCollection);
  for (let settings of settingsCollection!) {
    try {
      const schema = await SchemaReader.read(settings.schemaUri, settings.scopes);
      const schemaEmitter = SchemaEmitter.create(schema, settings);
      await schemaEmitter.prepare();
      await schemaEmitter.emitBootstrap();
      await schemaEmitter.emitTypes();
    } catch (e) {
      console.error(e);
    }
  }
})();
