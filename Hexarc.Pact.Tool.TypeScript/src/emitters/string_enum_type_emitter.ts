import * as ts from "typescript";
import type { StringEnumType } from "../types/protocol/types";


export function emit(type: StringEnumType): ts.EnumDeclaration {
  const modifiers = [ts.factory.createModifier(ts.SyntaxKind.ExportKeyword)];
  const members = type.members.map(x => emitMember(x));
  return ts.factory.createEnumDeclaration(undefined, modifiers, type.name, members);
}

function emitMember(member: string): ts.EnumMember {
  return ts.factory.createEnumMember(member, ts.factory.createStringLiteral(member));
}