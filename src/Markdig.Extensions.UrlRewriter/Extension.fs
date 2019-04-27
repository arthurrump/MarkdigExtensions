namespace Markdig

open Markdig.Extensions.UrlRewriter
open System.Runtime.CompilerServices

[<Extension>]
type MarkdownPipelineBuilderExtensions() =
    [<Extension>]
    /// <summary>Use the urlRewriter function to rewrite URLs in any link in your Markdown</summary>
    /// <param name="pipeline">The Markdig <see cref="MarkdownPipelineBuilder"/> to add the extension to</param>
    /// <param name="urlRewriter">
    ///     A function that gets a <see cref="Markdig.Syntax.Inlines.LinkInline"/> object and returns the new URL for that link
    /// </param>
    /// <example>
    /// Example that rewrites all http urls to https:
    /// <code>
    /// var pipeline = new MarkdownPipelineBuilder()
    ///     .UseUrlRewriter(link => link.Url.Replace("http://", "https://"));
    /// </code>
    /// </example>
    static member UseUrlRewriter(pipeline : MarkdownPipelineBuilder, urlRewriter) =
        pipeline.Extensions.Add(UrlRewriterExtension(urlRewriter))
        pipeline