import * as ts from "typescript";
import * as Defs from "./defs";
import * as Syntax from "./syntax";
import type { IndexEmitterSettings } from "../types/tool";


export const emit = (settings: IndexEmitterSettings) =>
  Syntax.createBundle(
    emitDeclarations(settings),
    emitTypeDefinitionSources(settings.typeDefinitionPaths));

const emitTypeDefinitionSources = (paths: string[]) =>
  paths.map(x => emitTypeDefinitionSource(x));

const emitTypeDefinitionSource = (path: string) =>
  ts.createUnparsedSourceFile(`/// <reference path="${path}" />`);

const emitDeclarations = (settings: IndexEmitterSettings) =>
  [Syntax.createNamedExportDeclaration(settings.clientClassName, `./${Defs.API_MODULE_PATH}`)];