import * as ts from "typescript";
import type { EnumType, EnumMember } from "../types/protocol/types";


export function emit(type: EnumType): ts.EnumDeclaration {
  const modifiers = [ts.factory.createModifier(ts.SyntaxKind.ExportKeyword)];
  const members = type.members.map(x => emitMember(x));
  return ts.factory.createEnumDeclaration(undefined, modifiers, type.name, members);
}

function emitMember(member: EnumMember): ts.EnumMember {
  return ts.factory.createEnumMember(member.name, ts.factory.createNumericLiteral(member.value));
}