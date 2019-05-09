namespace Markdig

open ColorCode.Styling
open MarkdigExtensions.SyntaxHighlighting
open System.Runtime.CompilerServices

[<Extension>]
type MarkdownPipelineBuilderExtensions() =
    [<Extension>]
    /// <summary>Highlight code in fenced code blocks</summary>
    /// <param name="pipeline">The Markdig <see cref="MarkdownPipelineBuilder"/> to add the extension to</param>
    static member UseSyntaxHighlighting(pipeline : MarkdownPipelineBuilder) =
        pipeline.Extensions.Add(SyntaxHighlightingExtension())
        pipeline

    [<Extension>]
    /// <summary>Highlight code in fenced code blocks</summary>
    /// <param name="pipeline">The Markdig <see cref="MarkdownPipelineBuilder"/> to add the extension to</param>
    /// <param name="style">A custom style to use to highlight your code</param>
    static member UseSyntaxHighlighting(pipeline : MarkdownPipelineBuilder, style : StyleDictionary) =
        pipeline.Extensions.Add(SyntaxHighlightingExtension(style))
        pipeline