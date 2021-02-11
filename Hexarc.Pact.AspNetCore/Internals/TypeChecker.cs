using System;
using System.Collections.Generic;
using System.Reflection;
using Hexarc.Annotations;
using Hexarc.Pact.Protocol.TypeProviders;

namespace Hexarc.Pact.AspNetCore.Internals
{
    public sealed class TypeChecker
    {
        private IReadOnlySet<Guid> PrimitiveTypeIds { get; }

        private IReadOnlySet<Guid> ArrayLikeTypeIds { get; }

        private IReadOnlySet<Guid> DictionaryTypeIds { get; }

        private IReadOnlySet<Guid> TaskTypeIds { get; }

        public TypeChecker(
            PrimitiveTypeProvider primitiveTypeProvider,
            ArrayLikeTypeProvider arrayLikeTypeProvider,
            DictionaryTypeProvider dictionaryTypeProvider,
            TaskTypeProvider taskTypeProvider)
        {
            this.PrimitiveTypeIds = primitiveTypeProvider.TypeIds;
            this.ArrayLikeTypeIds = arrayLikeTypeProvider.TypeIds;
            this.DictionaryTypeIds = dictionaryTypeProvider.TypeIds;
            this.TaskTypeIds = taskTypeProvider.TypeIds;
        }

        public Boolean IsNullableReferenceProperty(PropertyInfo propertyInfo) =>
            propertyInfo.GetCustomAttribute<NullableReferenceAttribute>() is not null;

        public Boolean IsNullableValueType(Type type) => Nullable.GetUnderlyingType(type) is not null;

        public Boolean IsPrimitiveType(Type type) => this.PrimitiveTypeIds.Contains(type.GUID);

        public Boolean IsArrayType(Type type) => type.IsArray;

        public Boolean IsArrayLikeType(Type type) => this.ArrayLikeTypeIds.Contains(type.GUID);

        public Boolean IsDictionaryType(Type type) => this.DictionaryTypeIds.Contains(type.GUID);

        public Boolean IsGenericParameter(Type type) => type.IsGenericParameter;

        public Boolean IsTaskType(Type type) => this.TaskTypeIds.Contains(type.GUID);

        public Boolean IsUnionType(Type type) => type.GetCustomAttribute<UnionTagAttribute>() is not null;

        public Boolean IsEnumType(Type type) => type.IsEnum;

        public Boolean IsClassType(Type type) => type.IsClass;

        public Boolean IsStructType(Type type) => type.IsValueType;
    }
}
