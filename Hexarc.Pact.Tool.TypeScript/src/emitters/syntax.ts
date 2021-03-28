import * as ts from "typescript";


// Emits: import { identifierName } from "modulePath";
export const createNamedImportDeclaration = (identifierName: string, modulePath: string) =>
  ts.factory.createImportDeclaration(
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

// Emits: extends SomeClass
export const createExtendsClause = (className: string) =>
  ts.factory.createHeritageClause(
    ts.SyntaxKind.ExtendsKeyword,
    [ts.factory.createExpressionWithTypeArguments(
      ts.factory.createIdentifier(className),
      undefined)]);

export const createBundle = (statements: ts.Statement[], prepends?: ts.UnparsedSource[] | undefined) =>
  ts.factory.createBundle(
    [createSourceFile(statements)],
    prepends);

export const createSourceFile = (statements: ts.Statement[]) =>
  ts.factory.createSourceFile(
    statements,
    ts.factory.createToken(ts.SyntaxKind.EndOfFileToken),
    ts.NodeFlags.None);

export const createPublicModifier = () => ts.factory.createModifier(ts.SyntaxKind.PublicKeyword);
export const createExportModifier = () => ts.factory.createModifier(ts.SyntaxKind.ExportKeyword);
export const createReadonlyModifier = () => ts.factory.createModifier(ts.SyntaxKind.ReadonlyKeyword);