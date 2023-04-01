﻿using Summary.Extensions;
using static System.Environment;

namespace Summary.Markdown.Extensions;

/// <summary>
///     Extension methods for better rendering documentation into Markdown format.
/// </summary>
internal static class MarkdownRenderExtensions
{
    /// <summary>
    ///     Renders the specified documentation member into Markdown.
    /// </summary>
    public static Md Render(this DocMember self)
    {
        var text = new MdRenderer().Member(self).Text();

        return new($"{self.Name}.md", text);
    }

    private static string Render(IEnumerable<DocCommentNode> nodes) =>
        nodes
            .Trim()
            .SelectWithNext(Render)
            .Separated(with: "");

    internal static string Render(this DocCommentNode? node, DocCommentNode? next = default) => node switch
    {
        null => "",

        DocCommentLiteral literal => literal.Value,

        DocCommentElement { Name: "c" } code =>
            $"`{Render(code.Nodes)}`{LeadingTrivia(next)}",
        DocCommentElement { Name: "i" or "em" } code =>
            $"_{Render(code.Nodes)}_{LeadingTrivia(next)}",
        DocCommentElement { Name: "b" or "strong" } code =>
            $"**{Render(code.Nodes)}**{LeadingTrivia(next)}",
        DocCommentElement { Name: "strike" } code =>
            $"~~{Render(code.Nodes)}~~{LeadingTrivia(next)}",
        DocCommentElement { Name: "code" } code =>
            $"```cs{NewLine}{Render(code.Nodes)}```",

        DocCommentLink link => $"[`{link.Value}`](./{link.Value}.md){LeadingTrivia(next)}",

        DocCommentParamRef @ref => $"`{@ref.Value}`{LeadingTrivia(next)}",

        DocCommentElement element => element.Nodes
            .Trim()
            .SelectWithNext(Render)
            .Separated(with: ""),

        _ => node.ToString()!,
    };

    private static string LeadingTrivia(DocCommentNode? node) => node switch
    {
        DocCommentLiteral literal => literal.LeadingTrivia,
        _ => "",
    };

    // TODO: Write an efficient implementation.
    private static IEnumerable<DocCommentNode> Trim(this IEnumerable<DocCommentNode> nodes) => nodes
        .SkipWhile(x => x.IsSpace() || x.IsNewLine())
        .Reverse()
        .SkipWhile(x => x.IsSpace() || x.IsNewLine())
        .Reverse();
}