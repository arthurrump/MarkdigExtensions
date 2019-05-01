namespace Markdig

open MarkdigExtensions.ImageAsFigure
open System.Runtime.CompilerServices
open System.Runtime.InteropServices

[<Extension>]
type MarkdownPipelineBuilderExtensions() =
    [<Extension>]
    /// <summary>Render any image inside a <c>figure</c> tag with
    /// a <c>figcaption</c> tag containing the title of the image, if set</summary>
    /// <param name="pipeline">The Markdig <see cref="MarkdownPipelineBuilder"/> to add the extension to</param>
    /// <param name="onlyWithTitle">Only wrap images with a title in a figure tag, default is <c>false</c></param>
    static member UseImageAsFigure(pipeline : MarkdownPipelineBuilder, 
                                   [<Optional; DefaultParameterValue(false)>] onlyWithTitle) =
        pipeline.Extensions.Add(ImageAsFigureExtension(onlyWithTitle))
        pipeline
