using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hexarc.Pact.Protocol.Types;
using Type = Hexarc.Pact.Protocol.Types.Type;

namespace Hexarc.Pact.Tool.Internals
{
    public sealed class TypeRegistry
    {
        private Dictionary<Guid, PrimitiveType> PrimitiveTypes { get; }

        private Dictionary<Guid, DynamicType> DynamicTypes { get; }

        private Dictionary<Guid, ArrayLikeType> ArrayLikeTypes { get; }

        private Dictionary<Guid, DictionaryType> DictionaryTypes { get; }

        private Dictionary<Guid, TaskType> TaskTypes { get; }

        private Dictionary<Guid, EnumType> EnumTypes { get; }

        private Dictionary<Guid, StructType> StructTypes { get; }

        private Dictionary<Guid, ClassType> ClassTypes { get; }

        private Dictionary<Guid, UnionType> UnionTypes { get; }

        private TypeRegistry(
            IEnumerable<PrimitiveType> primitiveTypes,
            IEnumerable<DynamicType> dynamicTypes,
            IEnumerable<ArrayLikeType> arrayLikeTypes,
            IEnumerable<DictionaryType> dictionaryTypes,
            IEnumerable<TaskType> taskTypes,
            IEnumerable<EnumType> enumTypes,
            IEnumerable<StructType> structTypes,
            IEnumerable<ClassType> classTypes,
            IEnumerable<UnionType> unionTypes)
        {
            this.PrimitiveTypes = primitiveTypes.ToDictionary(x => x.Id, x => x);
            this.DynamicTypes = dynamicTypes.ToDictionary(x => x.Id, x => x);
            this.ArrayLikeTypes = arrayLikeTypes.ToDictionary(x => x.Id, x => x);
            this.DictionaryTypes = dictionaryTypes.ToDictionary(x => x.Id, x => x);
            this.TaskTypes = taskTypes.ToDictionary(x => x.Id, x => x);
            this.EnumTypes = enumTypes.ToDictionary(x => x.Id, x => x);
            this.ClassTypes = classTypes.ToDictionary(x => x.Id, x => x);
            this.StructTypes = structTypes.ToDictionary(x => x.Id, x => x);
            this.UnionTypes = unionTypes.ToDictionary(x => x.Id, x => x);
        }

        public static TypeRegistry FromTypes(IEnumerable<Type> types)
        {
            var groups = types.GroupBy(x => x.Kind, x => x).ToDictionary(x => x.Key, x=> x.ToArray());
            var primitiveTypes = groups.GetValueOrDefault(TypeKind.Primitive, Array.Empty<Type>()).Cast<PrimitiveType>();
            var dynamicTypes = groups.GetValueOrDefault(TypeKind.Dynamic, Array.Empty<Type>()).Cast<DynamicType>();
            var arrayLikeTypes = groups.GetValueOrDefault(TypeKind.ArrayLike, Array.Empty<Type>()).Cast<ArrayLikeType>();
            var dictionaryTypes = groups.GetValueOrDefault(TypeKind.Dictionary, Array.Empty<Type>()).Cast<DictionaryType>();
            var taskTypes = groups.GetValueOrDefault(TypeKind.Task, Array.Empty<Type>()).Cast<TaskType>();
            var enumTypes = groups.GetValueOrDefault(TypeKind.Enum, Array.Empty<Type>()).Cast<EnumType>();
            var structTypes = groups.GetValueOrDefault(TypeKind.Struct, Array.Empty<Type>()).Cast<StructType>();
            var classTypes = groups.GetValueOrDefault(TypeKind.Class, Array.Empty<Type>()).Cast<ClassType>();
            var unionTypes = groups.GetValueOrDefault(TypeKind.Union, Array.Empty<Type>()).Cast<UnionType>();
            return new TypeRegistry(primitiveTypes, dynamicTypes, arrayLikeTypes, dictionaryTypes, taskTypes, enumTypes, structTypes, classTypes, unionTypes);
        }

        public PrimitiveType GetPrimitiveType(Guid typeId) => this.PrimitiveTypes[typeId];

        public DynamicType GetDynamicTypeType(Guid typeId) => this.DynamicTypes[typeId];

        public ArrayLikeType GetArrayLikeType(Guid typeId) => this.ArrayLikeTypes[typeId];

        public DictionaryType GetDictionaryType(Guid typeId) => this.DictionaryTypes[typeId];

        public TaskType GetTaskType(Guid typeId) => this.TaskTypes[typeId];

        public TaskType GetTaskType(Guid? typeId) => typeId is null
            ? new TaskType(typeof(Task<>))
            : this.GetTaskType(typeId.Value);

        public EnumType GetEnumType(Guid typeId) => this.EnumTypes[typeId];

        public StructType GetStructType(Guid typeId) => this.StructTypes[typeId];

        public ClassType GetClassType(Guid typeId) => this.ClassTypes[typeId];

        public UnionType GetUnionType(Guid typeId) => this.UnionTypes[typeId];

        public DistinctType GetDistinctType(Guid typeId)
        {
            if (this.EnumTypes.TryGetValue(typeId, out var @enum)) return @enum;
            if (this.StructTypes.TryGetValue(typeId, out var @struct)) return @struct;
            if (this.ClassTypes.TryGetValue(typeId, out var @class)) return @class;
            if (this.UnionTypes.TryGetValue(typeId, out var union)) return union;
            throw new KeyNotFoundException();
        }
    }
}
