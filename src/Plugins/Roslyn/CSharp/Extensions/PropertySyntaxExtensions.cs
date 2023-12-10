﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Summary.Extensions;

namespace Summary.Roslyn.CSharp.Extensions;

/// <summary>
///     A set of extension methods that help converting different syntax nodes into <see cref="DocProperty" />.
/// </summary>
internal static class PropertySyntaxExtensions
{
    /// <summary>
    ///     Converts the given event field declaration into a sequence of <see cref="DocProperty" /> for each declared variable.
    /// </summary>
    public static DocProperty[] Properties(this EventFieldDeclarationSyntax self) =>
        self.Declaration.Variables.Select(x => x.Property(self)).ToArray();

    /// <summary>
    ///     Converts the given property declaration into <see cref="DocProperty" />.
    /// </summary>
    public static DocProperty Property(this PropertyDeclarationSyntax self) =>
        new()
        {
            Type = self.Type.Type(),
            FullyQualifiedName = self.FullyQualifiedName(),
            Name = self.Name()!,
            Declaration = $"{self.AttributesDeclaration()}{self.Modifiers} {self.Type} {self.Identifier} {self.AccessorsDeclaration()}",
            Access = self.Access(),
            Comment = self.Comment(),
            DeclaringType = self.DeclaringType(),
            Deprecation = self.AttributeLists.Deprecation(),
            Accessors = self.Accessors(),
            Generated = false,
            Event = false,
        };

    /// <summary>
    ///     Converts the given parameter node into <see cref="DocProperty" />.
    /// </summary>
    /// <remarks>
    ///     It makes sense to convert a parameter node into a property if it's a record property declared as a parameter.
    /// </remarks>
    public static DocProperty Property(this ParameterSyntax self) =>
        new()
        {
            Type = self.Type!.Type(),
            FullyQualifiedName = self.FullyQualifiedName(),
            Name = self.Name()!,
            Declaration = $"{self.AttributeLists.AttributesDeclaration()}public {self.Type} {self.Identifier} {{ get; }}",
            Access = AccessModifier.Public,
            Comment = DocComment.Empty,
            DeclaringType = self.DeclaringType(),
            Deprecation = self.AttributeLists.Deprecation(),
            Accessors = DocPropertyAccessor.Defaults(),
            Generated = true,
            Event = false,
        };

    /// <summary>
    ///     Converts the given event property declaration into <see cref="DocProperty" />.
    /// </summary>
    public static DocProperty Property(this EventDeclarationSyntax self) =>
        new()
        {
            Type = self.Type!.Type(),
            FullyQualifiedName = self.FullyQualifiedName(),
            Name = self.Name()!,
            Declaration = $"{self.AttributesDeclaration()}{self.Modifiers} {self.Type} {self.Identifier} {self.AccessorsDeclaration()}",
            Access = self.Access(),
            Comment = self.Comment(),
            DeclaringType = self.DeclaringType(),
            Deprecation = self.AttributeLists.Deprecation(),
            Accessors = self.Accessors(),
            Generated = false,
            Event = true,
        };

    /// <summary>
    ///     Converts the given event field declaration into <see cref="DocProperty" />.
    /// </summary>
    /// <remarks>
    ///     Only the first variable in the declaration is converted.
    /// </remarks>
    public static DocProperty Property(this EventFieldDeclarationSyntax self) =>
        self.Declaration.Variables[index: 0].Property(self);

    /// <summary>
    ///     Converts the given indexer declaration into <see cref="DocIndexer" /> property.
    /// </summary>
    public static DocIndexer Indexer(this IndexerDeclarationSyntax self) =>
        new()
        {
            Type = self.Type.Type(),
            FullyQualifiedName = self.FullyQualifiedName(),
            Name = self.Name()!,
            Declaration = $"{self.AttributesDeclaration()}{self.Modifiers} {self.Type} this{self.ParameterList} {self.AccessorsDeclaration()}",
            Access = self.Access(),
            Comment = self.Comment(),
            DeclaringType = self.DeclaringType(),
            Deprecation = self.AttributeLists.Deprecation(),
            Accessors = self.Accessors(),
            Params = self.ParameterList.Params(),
            Generated = false,
            Event = false,
        };

    private static DocProperty Property(this VariableDeclaratorSyntax self, EventFieldDeclarationSyntax field) =>
        new()
        {
            Type = field.Declaration.Type.Type(),
            FullyQualifiedName = self.FullyQualifiedName(),
            Name = self.Name()!,
            Declaration = $"{field.AttributesDeclaration()}{field.Modifiers} event {field.Declaration.Type} {self.Identifier}",
            Access = field.Access(),
            Comment = field.Comment(),
            DeclaringType = self.DeclaringType(),
            Deprecation = field.AttributeLists.Deprecation(),
            // TODO: Fix.
            Accessors = Array.Empty<DocPropertyAccessor>(),
            Generated = false,
            Event = true,
        };

    private static DocPropertyAccessor[] Accessors(this PropertyDeclarationSyntax self) =>
        self.AccessorList?.Accessors.Select(x => x.Accessor(self)).ToArray() ?? DocPropertyAccessor.Defaults();

    private static DocPropertyAccessor[] Accessors(this IndexerDeclarationSyntax self) =>
        self.AccessorList?.Accessors.Select(x => x.Accessor(self)).ToArray() ?? DocPropertyAccessor.Defaults();

    private static DocPropertyAccessor[] Accessors(this EventDeclarationSyntax self) =>
        self.AccessorList?.Accessors.Select(x => x.Accessor(self)).ToArray() ?? DocPropertyAccessor.Defaults();

    private static DocPropertyAccessor Accessor(this AccessorDeclarationSyntax self, MemberDeclarationSyntax parent) =>
        new()
        {
            Access = self.Access() ?? parent.Access(),
            Kind = Kind(self),
        };

    private static AccessModifier? Access(this AccessorDeclarationSyntax self)
    {
        if (self.Modifiers.Any(SyntaxKind.PublicKeyword))
            return AccessModifier.Public;
        if (self.Modifiers.Any(SyntaxKind.ProtectedKeyword))
            return AccessModifier.Protected;
        if (self.Modifiers.Any(SyntaxKind.InternalKeyword))
            return AccessModifier.Internal;
        if (self.Modifiers.Any(SyntaxKind.PrivateKeyword))
            return AccessModifier.Private;

        return null;
    }

    private static AccessorKind Kind(this AccessorDeclarationSyntax self) => self.Keyword.Kind() switch
    {
        SyntaxKind.GetKeyword => AccessorKind.Get,
        SyntaxKind.SetKeyword => AccessorKind.Set,
        SyntaxKind.InitKeyword => AccessorKind.Init,
        SyntaxKind.AddKeyword => AccessorKind.Add,
        SyntaxKind.RemoveKeyword => AccessorKind.Remove,

        _ => throw new InvalidOperationException($"Unrecognized accessor: {self}"),
    };

    private static string AccessorsDeclaration(this PropertyDeclarationSyntax self) =>
        self.AccessorList?.AccessorsDeclaration() ?? "{ get; }";

    private static string AccessorsDeclaration(this IndexerDeclarationSyntax self) =>
        self.AccessorList?.AccessorsDeclaration() ?? "{ get; }";

    private static string AccessorsDeclaration(this EventDeclarationSyntax self) =>
        self.AccessorList?.AccessorsDeclaration() ?? "{ add; remove; }";

    private static string AccessorsDeclaration(this AccessorListSyntax self) =>
        $"{{ {self.Accessors.Select(x => $"{x.Modifiers.ToString().Space()}{x.Keyword}; ").Separated("")}}}";
}