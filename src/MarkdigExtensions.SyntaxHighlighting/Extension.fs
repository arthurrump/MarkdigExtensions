namespace Markdig

open MarkdigExtensions.SyntaxHighlighting
open System.Runtime.CompilerServices

[<Extension>]
type MarkdownPipelineBuilderExtensions() =
    [<Extension>]
    /// <summary>TODO</summary>
    /// <param name="pipeline">The Markdig <see cref="MarkdownPipelineBuilder"/> to add the extension to</param>
    static member UseSyntaxHighlighting(pipeline : MarkdownPipelineBuilder) =
        pipeline.Extensions.Add(SyntaxHighlightingExtension())
        pipeline
