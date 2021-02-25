using System;
using System.Collections.Generic;
using System.Reflection;
using Hexarc.Annotations;
using Hexarc.Pact.AspNetCore.Extensions;
using Hexarc.Pact.Protocol.TypeProviders;
using Microsoft.AspNetCore.Mvc;

namespace Hexarc.Pact.AspNetCore.Internals
{
    public sealed class TypeChecker
    {
        private IReadOnlySet<Guid> PrimitiveTypeIds { get; }

        private IReadOnlySet<Guid> DynamicTypeIds { get; }

        private IReadOnlySet<Guid> ArrayLikeTypeIds { get; }

        private IReadOnlySet<Guid> DictionaryTypeIds { get; }

        private IReadOnlySet<Guid> TaskTypeIds { get; }

        public TypeChecker(
            PrimitiveTypeProvider primitiveTypeProvider,
            DynamicTypeProvider dynamicTypeProvider,
            ArrayLikeTypeProvider arrayLikeTypeProvider,
            DictionaryTypeProvider dictionaryTypeProvider,
            TaskTypeProvider taskTypeProvider)
        {
            this.PrimitiveTypeIds = primitiveTypeProvider.TypeIds;
            this.DynamicTypeIds = dynamicTypeProvider.TypeIds;
            this.ArrayLikeTypeIds = arrayLikeTypeProvider.TypeIds;
            this.DictionaryTypeIds = dictionaryTypeProvider.TypeIds;
            this.TaskTypeIds = taskTypeProvider.TypeIds;
        }

        public Boolean IsActionResultOfT(Type type) =>
            type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(ActionResult<>));

        public Boolean IsNullableReferenceProperty(PropertyInfo propertyInfo) =>
            propertyInfo.GetCustomAttribute<NullableReferenceAttribute>() is not null;

        public Boolean IsNullableValueType(Type type) => Nullable.GetUnderlyingType(type) is not null;

        public Boolean IsPrimitiveType(Type type) => this.PrimitiveTypeIds.Contains(type.GUID);

        public Boolean IsDynamicType(Type type) => this.DynamicTypeIds.Contains(type.GUID);

        public Boolean IsArrayType(Type type) => type.IsArray;

        public Boolean IsArrayLikeType(Type type) => this.ArrayLikeTypeIds.Contains(type.GUID);

        public Boolean IsDictionaryType(Type type) => this.DictionaryTypeIds.Contains(type.GUID);

        public Boolean IsGenericParameter(Type type) => type.IsGenericParameter;

        public Boolean IsTaskType(Type type) => this.TaskTypeIds.Contains(type.GUID);

        public Boolean IsUnionType(Type type) => type.GetCustomAttribute<UnionTagAttribute>() is not null;

        public Boolean IsEnumType(Type type) => type.IsEnum;

        public Boolean IsStringEnumType(Type type) => type.IsEnum && type.SupportJsonStringEnumConversion();

        public Boolean IsClassType(Type type) => type.IsClass;

        public Boolean IsStructType(Type type) => type.IsValueType;
    }
}
