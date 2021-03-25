import * as ts from "typescript";
import * as DistinctTypeEmitter from "./distinct_type_emitter";
import type { DistinctType } from "../types/protocol/types";
import type { TypeReferenceEmitter } from "../types/tool";


export function emit(types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.Bundle {
  const groups = groupTypesByNamespace(types);
  const declarations = [...groups.entries()].map(([n, t]) => emitTypes(n, t, typeReferenceEmitter)).flat();
  const sourceFile = ts.factory.createSourceFile(declarations, ts.factory.createToken(ts.SyntaxKind.EndOfFileToken), ts.NodeFlags.None);
  return ts.factory.createBundle([sourceFile]);
}

function groupTypesByNamespace(types: DistinctType[]): Map<string, DistinctType[]> {
  return types.reduce((acc, x) => {
    const namespace = x.namespace ?? "";
    if (!acc.has(namespace)) acc.set(namespace, []);
    acc.get(namespace)!.push(x);
    return acc;
  }, new Map<string, DistinctType[]>());
}

function emitTypes(namespaceName: string, types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement[] {
  if (namespaceName === "") return emitTypeWithoutNamespace(types, typeReferenceEmitter);
  else return [emitTypesInNamespace(namespaceName, types, typeReferenceEmitter)];
}

function emitTypesInNamespace(namespaceName: string, types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement {
  return ts.factory.createModuleDeclaration(
    undefined,
    [ts.factory.createModifier(ts.SyntaxKind.DeclareKeyword)],
    ts.factory.createIdentifier(namespaceName),
    ts.factory.createModuleBlock(types.map(x => DistinctTypeEmitter.emit(x, typeReferenceEmitter)).flat()),
    ts.NodeFlags.Namespace
  );
}

function emitTypeWithoutNamespace(types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement[] {
  return types.map(x => DistinctTypeEmitter.emit(x, typeReferenceEmitter)).flat();
}