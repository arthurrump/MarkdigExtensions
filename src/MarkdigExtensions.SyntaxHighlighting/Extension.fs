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

    [<Extension>]
    /// <summary>Highlight code in fenced code blocks</summary>
    /// <param name="pipeline">The Markdig <see cref="MarkdownPipelineBuilder"/> to add the extension to</param>
    /// <param name="style">A custom style to use to highlight your code</param>
    /// <param name="inlineCss">
    ///     If set to true this will use inline styling to set the colors of your code.
    ///     If set to false the code will be styled with classes and you'll have to use the 
    ///     <see cref="ColorCode.HtmlClassFormatter"/> to get the CSS rules for the chosen style.
    /// </param>
    static member UseSyntaxHighlighting(pipeline: MarkdownPipelineBuilder, style : StyleDictionary, inlineCss : bool) =
        pipeline.Extensions.Add(SyntaxHighlightingExtension(style, inlineCss))
        pipeline