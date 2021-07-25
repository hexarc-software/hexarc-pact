import * as ts from "typescript";
import * as DistinctTypeEmitter from "./distinct_type_emitter";
import type { DistinctType } from "../types/protocol/types";
import type { TypeReferenceEmitter } from "../types/tool";


export function emit(types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.Bundle {
  const declarations = emitAllTypes(types, typeReferenceEmitter);
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

function emitAllTypes(types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter) {
  return [
    ...emitPrimitiveTypes(),
    ...emitGroupedDistinctTypes(types, typeReferenceEmitter)
  ];
}

function emitPrimitiveTypes(): ts.DeclarationStatement[] {
  const modifiers = [ts.factory.createModifier(ts.SyntaxKind.ExportKeyword)];
  return [
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Boolean", undefined, ts.factory.createTypeReferenceNode("boolean")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Byte", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "SByte", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Char", undefined, ts.factory.createTypeReferenceNode("string")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "String", undefined, ts.factory.createTypeReferenceNode("string")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Int16", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "UInt16", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Int32", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "UInt32", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Int64", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "UInt64", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Single", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Double", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Decimal", undefined, ts.factory.createTypeReferenceNode("number")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "DateTime", undefined, ts.factory.createTypeReferenceNode("Date | string")),
    ts.factory.createTypeAliasDeclaration(undefined, modifiers, "Guid", undefined, ts.factory.createTypeReferenceNode("string")),
  ];
}

function emitGroupedDistinctTypes(types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter) {
  const groups = groupTypesByNamespace(types);
  return [...groups.entries()].map(([n, t]) => emitDistinctTypes(n, t, typeReferenceEmitter)).flat();
}

function emitDistinctTypes(namespaceName: string, types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement[] {
  if (namespaceName === "") return emitTypeWithoutNamespace(types, typeReferenceEmitter);
  else return [emitTypesInNamespace(namespaceName, types, typeReferenceEmitter)];
}

function emitTypesInNamespace(namespaceName: string, types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement {
  return ts.factory.createModuleDeclaration(
    undefined,
    [ts.factory.createModifier(ts.SyntaxKind.ExportKeyword)],
    ts.factory.createIdentifier(namespaceName),
    ts.factory.createModuleBlock(types.map(x => DistinctTypeEmitter.emit(x, typeReferenceEmitter)).flat()),
    undefined
  );
}

function emitTypeWithoutNamespace(types: DistinctType[], typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement[] {
  return types.map(x => DistinctTypeEmitter.emit(x, typeReferenceEmitter)).flat();
}