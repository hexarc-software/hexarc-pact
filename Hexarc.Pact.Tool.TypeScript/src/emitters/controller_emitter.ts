import * as ts from "typescript";
import type { Controller } from "../types/protocol/api";


export function emit(controller: Controller): ts.Bundle {
  const sourceFile = ts.factory.createSourceFile(
    emitDeclarations(controller),
    ts.factory.createToken(ts.SyntaxKind.EndOfFileToken),
    ts.NodeFlags.None);
  return ts.factory.createBundle([sourceFile]);
}

function emitDeclarations(controller: Controller): ts.Statement[] {
  const controllerBaseImport = emitControllerBaseImport();
  const clientBaseImport = emitClientBaseImport();
  const classDeclaration = emitClass(controller);
  return [controllerBaseImport, clientBaseImport, classDeclaration];
}

function emitControllerBaseImport(): ts.ImportDeclaration {
  const importSpec = ts.factory.createImportSpecifier(undefined, ts.factory.createIdentifier("ControllerBase"));
  const namedImport = ts.factory.createNamedImports([importSpec]);
  const importClause = ts.factory.createImportClause(false, undefined, namedImport);
  const pathLiteral = ts.factory.createStringLiteral("../bootstrap/controller_base");
  return ts.factory.createImportDeclaration(
    undefined,
    undefined,
    importClause,
    pathLiteral
  );
}

function emitClientBaseImport(): ts.ImportDeclaration {
  const importSpec = ts.factory.createImportSpecifier(undefined, ts.factory.createIdentifier("ClientBase"));
  const namedImport = ts.factory.createNamedImports([importSpec]);
  const importClause = ts.factory.createImportClause(false, undefined, namedImport);
  const pathLiteral = ts.factory.createStringLiteral("../bootstrap/client_base");
  return ts.factory.createImportDeclaration(
    undefined,
    undefined,
    importClause,
    pathLiteral
  );
}

function emitClass(controller: Controller): ts.ClassDeclaration {
  return ts.factory.createClassDeclaration(
    undefined,
    [ts.factory.createModifier(ts.SyntaxKind.ExportKeyword)],
    controller.name,
    undefined,
    [emitHeritageClause()],
    [emitConstructor(controller)]
  );
}

function emitHeritageClause(): ts.HeritageClause {
  return ts.factory.createHeritageClause(
    ts.SyntaxKind.ExtendsKeyword,
    [ts.factory.createExpressionWithTypeArguments(
      ts.factory.createIdentifier("ControllerBase"),
      undefined
    )]
  );
}

function emitConstructor(controller: Controller): ts.ConstructorDeclaration {
  return ts.factory.createConstructorDeclaration(
    undefined,
    [ts.factory.createModifier(ts.SyntaxKind.PublicKeyword)],
    [
      ts.factory.createParameterDeclaration(
        undefined,
        undefined,
        undefined,
        "clientBase",
        undefined,
        ts.factory.createTypeReferenceNode("ClientBase")
      ), 
      ts.factory.createParameterDeclaration(
        undefined,
        undefined,
        undefined,
        "path",
        undefined,
        ts.factory.createTypeReferenceNode("string"),
        ts.factory.createStringLiteral(controller.path)
      )
    ],
    ts.factory.createBlock([
      ts.factory.createExpressionStatement(
        ts.factory.createCallExpression(
          ts.factory.createSuper(),
          undefined,
          [
            ts.factory.createIdentifier("clientBase"),
            ts.factory.createIdentifier("path")
          ]
        )
      )
    ])
  );
}