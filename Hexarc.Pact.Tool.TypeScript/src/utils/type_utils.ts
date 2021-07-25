import type { TypeBase } from "../types/protocol/types";


export function isSameNamespace(type: TypeBase, namespace: string | undefined) {
  return type.namespace === namespace;
}

export function computeFullName(type: TypeBase, moduleNamespace: string | undefined) {
  const prefix = moduleNamespace != null ? `${moduleNamespace}.` : "";
  return type.namespace == null ? `${prefix}${type.name}` : `${prefix}${type.namespace}.${type.name}`;
}