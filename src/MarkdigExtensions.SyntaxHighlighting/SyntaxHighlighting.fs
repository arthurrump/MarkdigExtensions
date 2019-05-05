namespace MarkdigExtensions.SyntaxHighlighting

open ColorCode
open ColorCode.HTML

open Markdig
open Markdig.Parsers
open Markdig.Renderers
open Markdig.Renderers.Html
open Markdig.Syntax
open Markdig.Syntax.Inlines

open System

type HighlightedCodeBlockRenderer() =
    inherit CodeBlockRenderer()

    let getContent (block : LeafBlock) = 
        let slice = block.Lines.ToSlice()
        slice.Text.Substring(slice.Start, slice.Length)

    let color language code =
        let formatter = HtmlFormatter()
        formatter.GetHtmlString(code, ColorCode.Languages.FindById(language))

    override __.Accept(_, mo : MarkdownObject) = 
        match mo with
        | :? FencedCodeBlock as fcb ->
            ColorCode.Languages.All 
            |> Seq.tryFind (fun l -> l.HasAlias(fcb.Info))
            |> Option.isSome
        | _ -> false

    override __.Write(renderer : HtmlRenderer, cb : CodeBlock) =
        match cb with
        | :? FencedCodeBlock as fcb when not (String.IsNullOrEmpty fcb.Info) ->
            renderer
                .Write(getContent fcb |> color fcb.Info)
                |> ignore
        | _ -> 
            let r = CodeBlockRenderer()
            base.Write(renderer, cb)


/// An extension for Markdig that can rewrite urls for any link
type SyntaxHighlightingExtension() =
    interface IMarkdownExtension with

        member __.Setup(_) = ()

        member __.Setup(_, renderer) = 
            renderer.ObjectRenderers.InsertBefore<CodeBlockRenderer>(new HighlightedCodeBlockRenderer()) |> ignore
