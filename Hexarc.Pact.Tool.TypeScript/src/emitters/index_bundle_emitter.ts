import * as ts from "typescript";


export function emit(settings: IndexEmitterSettings): ts.Bundle {
  const typeDefinitionSources = settings.typeDefinitionPaths.map(emitTypeDefinitionSource);
  return ts.factory.createBundle([], typeDefinitionSources);
}

interface IndexEmitterSettings {
  typeDefinitionPaths: string[];
}

function emitTypeDefinitionSource(path: string): ts.UnparsedSource {
  return ts.createUnparsedSourceFile(`/// <reference path="${path}" />`)
}