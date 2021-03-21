import type { TypeBase } from "../types/protocol/types";


export function isSameNamespace(type: TypeBase, namespace: string | undefined) {
  return type.namespace === namespace;
}

export function computeFullName(type: TypeBase) {
  return type.namespace == null ? type.name : `${type.namespace}.${type.name}`;
}