import * as util from "util";
import * as ClientSettingsReader from "../src/utils/client_settings_reader";
import * as SchemaReader from "../src/utils/schema_reader";


(async function read() {
  util.inspect.defaultOptions.depth = null;
  const settingsCollection = await ClientSettingsReader.read();
  console.log(settingsCollection);
  for (let settings of settingsCollection!) {
    const schema = await SchemaReader.read(settings.schemaUri, settings.scopes);
    console.log(schema);
  }
})();
