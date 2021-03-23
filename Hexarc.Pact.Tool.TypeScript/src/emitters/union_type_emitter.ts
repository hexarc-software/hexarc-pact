import * as ts from "typescript";
import * as InterfaceEmitter from "./interface_type_emitter";

import type { UnionType } from "../types/protocol/types";
import type { TypeReferenceEmitter } from "../types/tool";


export function emit(type: UnionType, typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement[] {
  const cases = emitCaseInterfaces(type, typeReferenceEmitter);
  const union = emitUnionDeclaration(type);
  return [...cases, union];
}

function emitCaseInterfaces(type: UnionType, typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement[] {
  return type.cases.map(x => InterfaceEmitter.emit(x, typeReferenceEmitter));
}

function emitUnionDeclaration(type: UnionType): ts.TypeAliasDeclaration {
  const cases = type.cases.map(x => ts.factory.createTypeReferenceNode(x.name));
  const union = ts.factory.createUnionTypeNode(cases);
  return ts.factory.createTypeAliasDeclaration(undefined, undefined, type.name, undefined, union);
}