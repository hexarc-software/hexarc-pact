import * as ts from "typescript";


// Emits: import { identifierName } from "modulePath";
export function createNamedImportDeclaration(identifierName: string, modulePath: string) {
  return ts.factory.createImportDeclaration(
    undefined,
    undefined,
    ts.factory.createImportClause(
      false,
      undefined,
      ts.factory.createNamedImports([
        ts.factory.createImportSpecifier(
          undefined,
          ts.factory.createIdentifier(identifierName))])),
    ts.factory.createStringLiteral(modulePath));
}

// Emits: export { identifierName } from "modulePath";
export function createNamedExportDeclaration(identifierName: string, modulePath: string) {
  return ts.factory.createExportDeclaration(
    undefined,
    undefined,
    false,
    ts.factory.createNamedExports([
      ts.factory.createExportSpecifier(
        undefined,
        ts.factory.createIdentifier(identifierName))]),
    ts.factory.createStringLiteral(modulePath));
}

// Emits: extends SomeClass
export function createExtendsClause(className: string) {
  return ts.factory.createHeritageClause(
    ts.SyntaxKind.ExtendsKeyword,
    [ts.factory.createExpressionWithTypeArguments(
      ts.factory.createIdentifier(className),
      undefined)]);
}

export function createBundle(statements: ts.Statement[], prepends?: ts.UnparsedSource[] | undefined) {
  return ts.factory.createBundle(
    [createSourceFile(statements)],
    prepends);
}

export function createSourceFile(statements: ts.Statement[]) {
  return ts.factory.createSourceFile(
    statements,
    ts.factory.createToken(ts.SyntaxKind.EndOfFileToken),
    ts.NodeFlags.None);
}

export const createPublicModifier = () => ts.factory.createModifier(ts.SyntaxKind.PublicKeyword);
export const createAsyncModifier = () => ts.factory.createModifier(ts.SyntaxKind.AsyncKeyword);
export const createExportModifier = () => ts.factory.createModifier(ts.SyntaxKind.ExportKeyword);
export const createReadonlyModifier = () => ts.factory.createModifier(ts.SyntaxKind.ReadonlyKeyword);