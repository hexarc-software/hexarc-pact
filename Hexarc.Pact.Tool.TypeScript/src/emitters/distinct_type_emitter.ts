import * as ts from "typescript";

import { DistinctType, TypeKind } from "../types/protocol/types";
import type { TypeReferenceEmitter } from "../types/tool";

import * as EnumTypeEmitter from "./enum_type_emitter";
import * as StringEnumTypeEmitter from "./string_enum_type_emitter";
import * as InterfaceTypeEmitter from "./interface_type_emitter";
import * as UnionTypeEmitter from "./union_type_emitter";


export function emit(type: DistinctType, typeReferenceEmitter: TypeReferenceEmitter): ts.DeclarationStatement[] {
  switch(type.kind) {
    case TypeKind.Enum: return [EnumTypeEmitter.emit(type)];
    case TypeKind.StringEnum: return [StringEnumTypeEmitter.emit(type)];
    case TypeKind.Class: return [InterfaceTypeEmitter.emit(type, typeReferenceEmitter)];
    case TypeKind.Struct: return [InterfaceTypeEmitter.emit(type, typeReferenceEmitter)];
    case TypeKind.Union: return UnionTypeEmitter.emit(type, typeReferenceEmitter);
    default: throw new Error(`Unknown type ${JSON.stringify(type)}`);
  }
}