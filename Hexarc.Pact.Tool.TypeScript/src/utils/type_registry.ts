import { TypeKind} from "../types/protocol/types";
import type { TypeRegistry } from "../types/tool";
import type { 
  Type, DistinctType, DynamicType, 
  PrimitiveType, UnionType, ArrayLikeType,
  DictionaryType, ClassType, EnumType, 
  StringEnumType, StructType, TaskType
} from "../types/protocol/types";


export function create(types: Type[]): TypeRegistry {

  const typeGroups = types.reduce((acc, x) => {
    if (!acc.has(x.kind)) acc.set(x.kind, []);
    acc.get(x.kind)!.push(x);
    return acc;
  }, new Map<TypeKind, Type[]>());

  function extractGroup<TType extends Type>(kind: TypeKind): Map<string, TType> {
    if (!typeGroups.has(kind)) return new Map<string, TType>();
    else return new Map(typeGroups.get(kind)!.map(x => [x.id, x as TType] as [TypeKind, TType]));
  }

  const primitiveTypes = extractGroup<PrimitiveType>(TypeKind.Primitive);
  const dynamicTypes = extractGroup<DynamicType>(TypeKind.Dynamic);
  const arrayLikeTypes = extractGroup<ArrayLikeType>(TypeKind.ArrayLike);
  const dictionaryTypes = extractGroup<DictionaryType>(TypeKind.Primitive);
  const taskTypes = extractGroup<TaskType>(TypeKind.Primitive);
  const enumTypes = extractGroup<EnumType>(TypeKind.Primitive);
  const stringEnumTypes = extractGroup<StringEnumType>(TypeKind.Primitive);
  const structTypes = extractGroup<StructType>(TypeKind.Primitive);
  const classTypes = extractGroup<ClassType>(TypeKind.Primitive);
  const unionTypes =  extractGroup<UnionType>(TypeKind.Union);

  function throwNotFound(key: string): never {
    throw new Error(`Not found ${key}`);
  }

  function getTypeFor<TType extends Type>(map: Map<string, TType>) {
    return function(typeId: string): TType {
      return map.get(typeId) ?? throwNotFound(typeId);
    }
  }

  const getPrimitiveType = getTypeFor<PrimitiveType>(primitiveTypes);
  const getDynamicType = getTypeFor<DynamicType>(dynamicTypes);
  const getArrayLikeType = getTypeFor<ArrayLikeType>(arrayLikeTypes);
  const getDictionaryType = getTypeFor<DictionaryType>(dictionaryTypes);
  const getTaskType = getTypeFor<TaskType>(taskTypes);
  const getEnumType = getTypeFor<EnumType>(enumTypes);
  const getStringEnumType = getTypeFor<StringEnumType>(stringEnumTypes);
  const getStructType = getTypeFor<StructType>(structTypes);
  const getClassType = getTypeFor<ClassType>(classTypes);
  const getUnionType = getTypeFor<UnionType>(unionTypes);

  function getDistinctType(typeId: string): DistinctType {
    if (enumTypes.has(typeId)) return enumTypes.get(typeId)!;
    if (stringEnumTypes.has(typeId)) return stringEnumTypes.get(typeId)!;
    if (structTypes.has(typeId)) return structTypes.get(typeId)!;
    if (classTypes.has(typeId)) return classTypes.get(typeId)!;
    if (unionTypes.has(typeId)) return unionTypes.get(typeId)!;
    throwNotFound(typeId);
  }

  function enumerateDistinctTypes(): DistinctType[] {
    return [
      ...enumTypes.values(),
      ...stringEnumTypes.values(),
      ...structTypes.values(),
      ...classTypes.values(),
      ...unionTypes.values()
    ];
  }

  return { 
    getPrimitiveType, getDynamicType, getArrayLikeType, 
    getDictionaryType, getTaskType, getEnumType, getStringEnumType, 
    getStructType, getClassType, getUnionType, getDistinctType,
    enumerateDistinctTypes
  };
}