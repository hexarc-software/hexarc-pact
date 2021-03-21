import type { PrimitiveType } from "../types/protocol/types";


interface KnownPrimitiveType {
  readonly name: string;
  readonly namespace?: string;
}

module KnownPrimitiveTypes {
  export const Boolean: KnownPrimitiveType = { name: "Boolean", namespace: "System" };
  export const Byte: KnownPrimitiveType = { name: "Byte", namespace: "System" };
  export const SByte: KnownPrimitiveType = { name: "SByte", namespace: "System" };
  export const Char: KnownPrimitiveType = { name: "Char", namespace: "System" };
  export const Int16: KnownPrimitiveType = { name: "Int16", namespace: "System" };
  export const UInt16: KnownPrimitiveType = { name: "UInt16", namespace: "System" };
  export const Int32: KnownPrimitiveType = { name: "Int32", namespace: "System" };
  export const UInt32: KnownPrimitiveType = { name: "UInt32", namespace: "System" };
  export const Int64: KnownPrimitiveType = { name: "Int64", namespace: "System" };
  export const UInt64: KnownPrimitiveType = { name: "UInt64", namespace: "System" };
  export const Single: KnownPrimitiveType = { name: "Single", namespace: "System" };
  export const Double: KnownPrimitiveType = { name: "Double", namespace: "System" };
  export const Decimal: KnownPrimitiveType = { name: "Decimal", namespace: "System" };
  export const String: KnownPrimitiveType = { name: "String", namespace: "System" };
  export const Guid: KnownPrimitiveType = { name: "Guid", namespace: "System" };
  export const DateTime: KnownPrimitiveType = { name: "DateTime", namespace: "System" };
}

const createCheckerFor = (known: KnownPrimitiveType) => (given: PrimitiveType) =>
  known.name === given.name && known.namespace === known.namespace;

export const isBoolean = createCheckerFor(KnownPrimitiveTypes.Boolean);
export const isByte = createCheckerFor(KnownPrimitiveTypes.Byte);
export const isSByte = createCheckerFor(KnownPrimitiveTypes.SByte);
export const isChar = createCheckerFor(KnownPrimitiveTypes.Char);
export const isInt16 = createCheckerFor(KnownPrimitiveTypes.Int16);
export const isUInt16 = createCheckerFor(KnownPrimitiveTypes.UInt16);
export const isInt32 = createCheckerFor(KnownPrimitiveTypes.Int32);
export const isUInt32 = createCheckerFor(KnownPrimitiveTypes.UInt32);
export const isInt64 = createCheckerFor(KnownPrimitiveTypes.Int64);
export const isUInt64 = createCheckerFor(KnownPrimitiveTypes.UInt64);
export const isSingle = createCheckerFor(KnownPrimitiveTypes.Single);
export const isDouble = createCheckerFor(KnownPrimitiveTypes.Double);
export const isDecimal = createCheckerFor(KnownPrimitiveTypes.Decimal);
export const isString = createCheckerFor(KnownPrimitiveTypes.String);
export const isGuid = createCheckerFor(KnownPrimitiveTypes.Guid);
export const isDateTime = createCheckerFor(KnownPrimitiveTypes.DateTime);