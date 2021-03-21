import * as ts from "typescript";

import type { DistinctType } from "../types/protocol/types";
import type { TypeReferenceEmitter } from "../types/tool";

import * as EnumTypeEmitter from "./enum_type_emitter";
import * as StringEnumTypeEmitter from "./string_enum_type_emitter";
import * as InterfaceTypeEmitter from "./interface_type_emitter";
import * as UnionTypeEmitter from "./union_type_emitter";


export function emit(type: DistinctType, typeReferenceEmitter: TypeReferenceEmitter): ts.Declaration[] {
  switch(type.kind) {
    case "Enum": return [EnumTypeEmitter.emit(type)];
    case "StringEnum": return [StringEnumTypeEmitter.emit(type)];
    case "Class": return [InterfaceTypeEmitter.emit(type, typeReferenceEmitter)];
    case "Struct": return [InterfaceTypeEmitter.emit(type, typeReferenceEmitter)];
    case "Union": return UnionTypeEmitter.emit(type, typeReferenceEmitter);
    default: throw new Error(`Unknown type ${JSON.stringify(type)}`);
  }
}
