# MarkdigExtensions
[![Build Status](https://dev.azure.com/arthurrump/MarkdigExtensions/_apis/build/status/CI?branchName=master)](https://dev.azure.com/arthurrump/MarkdigExtensions/_build/latest?definitionId=15&branchName=master)

Some useful extensions to the [Markdig](https://github.com/lunet-io/markdig) Markdown processor.

## MarkdigExtensions.UrlRewriter
Allows you to rewrite URLs in link and image tags. It's reason for existence was the need to convert local image paths to a path on the webserver, but it's flexible enough to rewrite any link or image URL.

### Example
```csharp
using Markdig;

var pipeline = new MarkdownPipelineBuilder()
    .UseUrlRewriter(link => link.Url.Replace("http://", "https://"))
    .Build();

var markdown = "[Anchor](http://example.net), ![Image](http://example.com/img.png)";
var html = Markdown.ToHtml(markdown, pipeline);
```

Result: `<p><a href="https://example.net">Anchor</a>, <img src="https://example.com/img.png" alt="Image" /></p>`

## MarkdigExtensions.ImageAsFigure
TODO - Renders all images as figures with a figcaption element set to the title of the image.

## MarkdigExtensions.SyntaxHighlighting
TODO - Uses [ColorCode-Universal](https://github.com/WilliamABradley/ColorCode-Universal) to provide syntax highlighting for code blocks.
