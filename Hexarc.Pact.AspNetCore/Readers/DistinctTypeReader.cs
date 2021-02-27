using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Namotion.Reflection;
using Hexarc.Annotations;
using Hexarc.Pact.Protocol.Extensions;
using Hexarc.Pact.Protocol.TypeReferences;
using Hexarc.Pact.Protocol.Types;
using Hexarc.Pact.AspNetCore.Extensions;
using Hexarc.Pact.AspNetCore.Internals;
using Hexarc.Pact.AspNetCore.Models;

namespace Hexarc.Pact.AspNetCore.Readers
{
    /// <summary>
    /// This class provides an ability to read distinct types of the Hexarc Pact protocol.
    /// </summary>
    public sealed class DistinctTypeReader
    {
        private TypeChecker TypeChecker { get; }

        private TypeReferenceReader TypeReferenceReader { get; }

        /// <summary>
        /// Creates an instance of the DistinctTypeReader class.
        /// </summary>
        /// <param name="typeChecker">The type checker to determine what a target Hexarc Pact type for a given one.</param>
        /// <param name="typeReferenceReader">The type reference reader used during type reading.</param>
        public DistinctTypeReader(TypeChecker typeChecker, TypeReferenceReader typeReferenceReader) =>
            (this.TypeChecker, this.TypeReferenceReader) = (typeChecker, typeReferenceReader);

        /// <summary>
        /// Reads a Hexarc Pact distinct type from a .NET system type.
        /// </summary>
        /// <param name="type">The .NET system type to read from.</param>
        /// <returns>The Hexarc Pact distinct type read from the given .NET system type.</returns>
        public DistinctType Read(System.Type type) => type switch
        {
            var x when this.TypeChecker.IsStringEnumType(x) => this.ReadStringEnumType(x),
            var x when this.TypeChecker.IsEnumType(x) => this.ReadEnumType(x),
            var x when this.TypeChecker.IsUnionType(x) => this.ReadUnionType(x),
            var x when this.TypeChecker.IsStructType(x) => this.ReadStructType(x),
            var x when this.TypeChecker.IsClassType(x) => this.ReadClassType(x),
            _ => throw new InvalidOperationException($"Could not read a Hexarc Pact type from {type}")
        };

        private StringEnumType ReadStringEnumType(System.Type type) =>
            new(type.GUID, type.Namespace, type.Name, Enum.GetNames(type));

        private EnumType ReadEnumType(System.Type type) =>
            new(type.GUID, type.Namespace, type.Name, this.ReadEnumMembers(type, Enum.GetNames(type)));

        private EnumMember[] ReadEnumMembers(System.Type type, String[] memberNames) =>
            memberNames.Select(x => this.ReadEnumMember(type, x)).ToArray();

        private EnumMember ReadEnumMember(System.Type type, String memberName) =>
            new(memberName, (Int32) Enum.Parse(type, memberName));

        private UnionType ReadUnionType(System.Type type)
        {
            var tagName = type.GetCustomAttribute<UnionTagAttribute>()!.TagPropertyName;
            var cases = type.GetCustomAttributes<UnionCaseAttribute>()
                .Select(x => this.ReadUnionCase(x, tagName))
                .ToArray();
            return new UnionType(type.GUID, type.Namespace, type.Name, tagName, cases);
        }

        private StructType ReadStructType(System.Type type) =>
            new(type.GUID, type.Namespace, type.NameWithoutGenericArity(),
                this.ReadGenericParameters(type.GetGenericArguments()),
                this.ReadObjectProperties(type.GetPublicInstanceProperties()));

        private ClassType ReadClassType(System.Type type) =>
            new(type.GUID, type.Namespace, type.NameWithoutGenericArity(),
                this.ReadGenericParameters(type.GetGenericArguments()),
                this.ReadObjectProperties(type.GetPublicInstanceProperties()));

        private ClassType ReadClassType(System.Type type, UnionTag tag) =>
            new(type.GUID, type.Namespace, type.NameWithoutGenericArity(),
                this.ReadGenericParameters(type.GetGenericArguments()),
                this.ReadObjectProperties(type.GetPublicInstanceProperties(), tag));

        private ClassType ReadUnionCase(UnionCaseAttribute @case, String tagName) =>
            this.ReadClassType(@case.CaseType, new UnionTag(tagName, @case.TagPropertyValue));

        private String[]? ReadGenericParameters(System.Type[] genericParameters) =>
            genericParameters.Length != 0
                ? genericParameters.Any(x => !x.IsGenericParameter)
                    ? throw new InvalidOperationException("Each object generic definition must be open")
                    : genericParameters.Select(x => x.Name).ToArray()
                : default;

        private ObjectProperty[] ReadObjectProperties(ContextualPropertyInfo[] properties, UnionTag tag) =>
            properties.Where(x => x.GetAttribute<JsonIgnoreAttribute>() is null)
                .Select(x => this.ReadObjectProperty(x, tag)).ToArray();

        private ObjectProperty[] ReadObjectProperties(ContextualPropertyInfo[] properties) =>
            properties.Where(x => x.GetAttribute<JsonIgnoreAttribute>() is null)
                .Select(this.ReadObjectProperty).ToArray();

        private ObjectProperty ReadObjectProperty(ContextualPropertyInfo property, UnionTag tag) =>
            property.PropertyInfo.IsUnionTag(tag)
                ? this.ReadUnionTagProperty(tag)
                : this.ReadObjectProperty(property);

        private ObjectProperty ReadObjectProperty(ContextualPropertyInfo property) =>
            new(this.ReadObjectPropertyType(property), property.Name);

        private ObjectProperty ReadUnionTagProperty(UnionTag tag) =>
            new(new LiteralTypeReference(tag.Value), tag.Name);

        private TypeReference ReadObjectPropertyType(ContextualPropertyInfo property) =>
            this.TypeReferenceReader.Read(property);
    }
}
