namespace MarkdigExtensions.UrlRewriter

open Markdig
open Markdig.Parsers
open Markdig.Syntax
open Markdig.Syntax.Inlines

open System

/// An extension for Markdig that can rewrite urls for any link
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
