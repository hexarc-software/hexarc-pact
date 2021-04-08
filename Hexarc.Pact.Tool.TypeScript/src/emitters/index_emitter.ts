import * as ts from "typescript";
import * as Defs from "./defs";
import * as Syntax from "./syntax";
import type { IndexEmitterSettings } from "../types/tool";


export function emit(settings: IndexEmitterSettings) {
  return Syntax.createBundle(
    emitDeclarations(settings),
    emitTypeDefinitionSources(settings.typeDefinitionPaths));
}

function emitTypeDefinitionSources(paths: string[]) {
  return paths.map(x => emitTypeDefinitionSource(x));
}

function emitTypeDefinitionSource(path: string) {
  return ts.createUnparsedSourceFile(`/// <reference path="${path}" />`);
}

function emitDeclarations(settings: IndexEmitterSettings) {
  return [
    Syntax.createNamedExportDeclaration(Defs.HTTP_ERROR_CLASS_NAME, `./${Defs.HTTP_ERROR_MODULE_PATH}`),
    Syntax.createNamedExportDeclaration(settings.clientClassName, `./${Defs.API_MODULE_PATH}`)
  ];
}