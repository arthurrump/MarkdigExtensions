namespace Markdig.Extensions.UrlRewriter

open Markdig
open Markdig.Parsers
open Markdig.Syntax
open Markdig.Syntax.Inlines

open System
open System.Runtime.CompilerServices

/// An extension for Markdig will rewrite URLs using the urlRewriter function, which takes
/// the LinkInline object and returns a new URL for the link or image to follow.
/// To get the old URL of the link, use the link.Url property. To check if the link is an
/// image, use the link.IsImage property.
type UrlRewriterExtension(urlRewriter: Func<LinkInline, string>) =

    let documentProcessed (document : MarkdownDocument) =
        for node in (document :> MarkdownObject).Descendants() do
            match node with
            | :? LinkInline as link -> link.Url <- urlRewriter.Invoke(link)
            | _ -> ()

    let deleg = ProcessDocumentDelegate(documentProcessed)
    interface IMarkdownExtension with

        member __.Setup(pipeline : MarkdownPipelineBuilder) =
            pipeline.remove_DocumentProcessed deleg
            pipeline.add_DocumentProcessed deleg

        member __.Setup(_, _) = ()

[<Extension>]
type MarkdownPipelineBuilderExtensions() =
    [<Extension>]
    /// Use the LinkUrlRewriteExtension to rewrite URLs for links and images
    static member UseUrlRewriter(pipeline : MarkdownPipelineBuilder, urlRewriter) =
        pipeline.Extensions.Add(UrlRewriterExtension(urlRewriter))
        pipeline
