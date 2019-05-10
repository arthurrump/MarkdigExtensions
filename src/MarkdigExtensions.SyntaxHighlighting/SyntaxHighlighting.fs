namespace MarkdigExtensions.SyntaxHighlighting

open ColorCode
open ColorCode.Styling

open Markdig
open Markdig.Renderers
open Markdig.Renderers.Html
open Markdig.Syntax

open System

/// A renderer that will render fenced code blocks with syntax highlighting.
/// This renderer only accepts fenced code blocks with a language specified for which
/// a specification is included, so it does not replace the normal <see cref="CodeBlockRenderer"/>, 
/// but should be included before it.
type HighlightedCodeBlockRenderer(style : StyleDictionary) =
    inherit CodeBlockRenderer()
    
    let formatter = HtmlFormatter(style)

    let getContent (block : LeafBlock) = 
        let slice = block.Lines.ToSlice()
        slice.Text.Substring(slice.Start, slice.Length)

    let color language code =
        formatter.GetHtmlString(code, ColorCode.Languages.FindById(language))

    new() = HighlightedCodeBlockRenderer(Styling.StyleDictionary.DefaultLight)

    override __.Accept(_, mo : MarkdownObject) = 
        match mo with
        | :? FencedCodeBlock as fcb ->
            let lang = fcb.Info.ToLowerInvariant()
            ColorCode.Languages.All 
            |> Seq.tryFind (fun l -> l.Id.ToLowerInvariant() = lang || l.HasAlias(lang))
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


/// An extension for Markdig that highlights syntax in fenced code blocks
type SyntaxHighlightingExtension(style : StyleDictionary) =
    new() = SyntaxHighlightingExtension(StyleDictionary.DefaultLight)

    interface IMarkdownExtension with

        member __.Setup(_) = ()

        member __.Setup(_, renderer) = 
            renderer.ObjectRenderers.InsertBefore<CodeBlockRenderer>(new HighlightedCodeBlockRenderer(style)) |> ignore
