﻿using Summary.Pipes;

namespace Summary.Roslyn.CSharp;

public class InlineInheritDocPipe : IPipe<Doc[], Doc>
{
    // TODO: [x] Base types (1 level)
    // TODO: [x] Base interfaces
    // TODO: [x] Base methods
    // TODO: [x] Base crefs
    // TODO: [ ] Base fields
    // TODO: [ ] Base properties
    // TODO: [ ] Base indexers
    // TODO: [ ] Base events
    // TODO: [ ] Crefs: base types (methods)
    // TODO: [ ] Crefs: base types (types)
    // TODO: [ ] Complex merging rules
    // TODO: [ ] Base types (n levels)
    // TODO: [ ] Inheriting inherited docs?
    public Task<Doc> Run(Doc[] input)
    {
        var members = input.SelectMany(x => x.Members).ToArray();
        var inlined = members.Select(Inline).ToArray();

        return Task.FromResult(new Doc(inlined));

        DocMember Inline(DocMember member)
        {
            return member switch
            {
                DocTypeDeclaration type => type with
                {
                    Members = type.Members.Select(Inline).ToArray(),
                    Comment = InlineComment(member.Comment),
                },
                _ => member with { Comment = InlineComment(member.Comment) },
            };

            DocComment InlineComment(DocComment comment) =>
                new(comment.Nodes.SelectMany(InlineNode).ToArray());

            IEnumerable<DocCommentNode> InlineNode(DocCommentNode node)
            {
                if (node is DocCommentInheritDoc inheritDoc)
                {
                    foreach (var x in Inlined(inheritDoc))
                        yield return x;
                }
                else
                {
                    yield return node;
                }

                IEnumerable<DocCommentNode> Inlined(DocCommentInheritDoc doc)
                {
                    if (doc.Cref is null or "")
                    {
                        var @base = Base(member);
                        if (@base is not null)
                            return Merge(member.Comment.Nodes, @base.Comment.Nodes);
                    }
                    else
                    {
                        var cref = ByCref(member, doc.Cref);
                        if (cref is not null)
                            return Merge(member.Comment.Nodes, cref.Comment.Nodes);
                    }

                    return Enumerable.Empty<DocCommentNode>();

                    IEnumerable<DocCommentNode> Merge(IEnumerable<DocCommentNode> source, IEnumerable<DocCommentNode> inherit) =>
                        source.Where(x => x is not DocCommentInheritDoc).Concat(inherit);
                }
            }
        }

        DocMember? Base(DocMember x)
        {
            if (x is DocTypeDeclaration declaration)
                return declaration.Base.Select(Declaration).FirstOrDefault(x => x is not null);

            if (x is DocMethod method)
            {
                if (method.DeclaringType is not null)
                {
                    var declaring = Declaration(method.DeclaringType);
                    if (declaring is not null)
                    {
                        if (Base(declaring) is DocTypeDeclaration @base)
                            return @base.Members.OfType<DocMethod>().FirstOrDefault(x => x.Name == method.Name);
                    }
                }
            }

            return null;
        }

        // TODO: Should check cases where method tries to inherit type documentation?
        DocMember? ByCref(DocMember x, string cref)
        {
            if (x is DocTypeDeclaration declaration)
                return declaration.Base.Select(Declaration).FirstOrDefault(x => x is not null && x.Cref == cref);

            if (x is DocMethod method)
            {
                // TODO: Search base types.
                if (method.DeclaringType is not null)
                {
                    var declaring = Declaration(method.DeclaringType);
                    if (declaring is not null)
                        return declaring.Members.FirstOrDefault(x => x.Cref == cref);
                }
            }

            return null;
        }

        DocTypeDeclaration? Declaration(DocType type) =>
            members.OfType<DocTypeDeclaration>().FirstOrDefault(x => x.Name == type.Name);
    }
}