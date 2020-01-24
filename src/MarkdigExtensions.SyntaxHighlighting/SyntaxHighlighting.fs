namespace MarkdigExtensions.SyntaxHighlighting

open ColorCode
open ColorCode.Styling

open Markdig
open Markdig.Renderers
open Markdig.Renderers.Html
open Markdig.Syntax

open System

/// A renderer that will render fenced code blocks with syntax highlighting.
/// It replaces the default <see cref="CodeBlockRenderer"/>, but it will still use
/// that renderer for code blocks with unknown languages.
type HighlightedCodeBlockRenderer(style : StyleDictionary, inlineCss : bool) =
    inherit CodeBlockRenderer()

    let color lang code = 
        if inlineCss
        then HtmlFormatter(style).GetHtmlString(code, lang)
        else HtmlClassFormatter(style).GetHtmlString(code, lang)

    let getContent (block : LeafBlock) = 
        let slice = block.Lines.ToSlice()
        slice.Text.Substring(slice.Start, slice.Length)

    new(style) = HighlightedCodeBlockRenderer(style, true)
    new() = HighlightedCodeBlockRenderer(Styling.StyleDictionary.DefaultLight)

    override __.Write(renderer : HtmlRenderer, cb : CodeBlock) =
        match cb with
        | :? FencedCodeBlock as fcb when not (String.IsNullOrEmpty fcb.Info) ->
            let lang = Languages.FindById(fcb.Info)
            if lang <> null then
                let code = getContent fcb
                renderer.Write(color lang code) |> ignore
            else
                base.Write(renderer, cb)
        | _ -> 
            base.Write(renderer, cb)


/// An extension for Markdig that highlights syntax in fenced code blocks
type SyntaxHighlightingExtension(style : StyleDictionary, inlineCss : bool) =
    new(style) = SyntaxHighlightingExtension(style, true)
    new() = SyntaxHighlightingExtension(StyleDictionary.DefaultLight)

    interface IMarkdownExtension with

        member __.Setup(_) = ()

        member __.Setup(_, renderer) = 
            renderer.ObjectRenderers.ReplaceOrAdd<CodeBlockRenderer>(new HighlightedCodeBlockRenderer(style, inlineCss)) |> ignore
