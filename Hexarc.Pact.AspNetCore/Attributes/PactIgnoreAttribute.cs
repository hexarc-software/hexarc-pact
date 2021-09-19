namespace Hexarc.Pact.AspNetCore.Attributes;

/// <summary>
/// Prevents a method or class from being included to a Pact schema.
/// </summary>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class PactIgnoreAttribute : Attribute { }
