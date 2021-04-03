import { ClientSettingsReader, SchemaReader, SchemaEmitter } from "./pact";


(async function () {
  try {
    const settingsCollection = await ClientSettingsReader.read();
    for (let settings of settingsCollection!) {
      const schema = await SchemaReader.read(settings.schemaUri, settings.scopes);
      const schemaEmitter = SchemaEmitter.create(schema, settings);
      await schemaEmitter.prepare();
      await schemaEmitter.emit();
    }
  } catch (e) {
    console.error(e);
  }
}());