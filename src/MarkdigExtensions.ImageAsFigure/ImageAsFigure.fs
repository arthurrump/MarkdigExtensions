namespace MarkdigExtensions.ImageAsFigure

open Markdig
open Markdig.Renderers
open Markdig.Syntax
open Markdig.Syntax.Inlines

open System

/// <summary>
/// An extension for Markdig that renders any image inside a <c>figure</c> tag with
/// a <c>figcaption</c> tag containing the title of the image, if set
/// </summary>
/// <param name="onlyWithTitle">Only wrap images with a title in a figure tag, default is <c>false</c></param>
type ImageAsFigureExtension(onlyWithTitle) =
    new() = ImageAsFigureExtension(onlyWithTitle = false)

    interface IMarkdownExtension with

        member __.Setup(_) = ()

        member __.Setup(_, renderer) = 
            let onlyImages (renderer : IMarkdownRenderer) (object : MarkdownObject) f =
                match renderer with
                | :? HtmlRenderer as r ->
                    match object with
                    | :? LinkInline as l when l.IsImage ->
                        f r l
                    | _ -> ()
                | _ -> ()

            let hasTitle (link : LinkInline) = not <| String.IsNullOrEmpty link.Title

            renderer.add_ObjectWriteBefore(fun r o -> onlyImages r o (fun renderer link ->
                if (hasTitle link || not onlyWithTitle) then
                    renderer.Write("<figure>") |> ignore
            ))

            renderer.add_ObjectWriteAfter(fun r o -> onlyImages r o (fun renderer link ->
                if hasTitle link then
                    renderer
                        .Write("<figcaption>")
                        .WriteEscape(link.Title)
                        .Write("</figcaption>")
                        |> ignore

                if (hasTitle link || not onlyWithTitle) then
                    renderer.Write("</figure>") |> ignore
            ))
