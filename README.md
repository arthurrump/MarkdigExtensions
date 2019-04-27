# Markdig.Extensions
Some useful extensions to the [Markdig](https://github.com/lunet-io/markdig) Markdown processor.

## Markdig.Extensions.UrlRewriter
Allows you to rewrite URLs in link and image tags. It's reason for existence was the need to convert local image paths to a path on the webserver, but it's flexible enough to rewrite any link or image URL.

### Example
```fsharp
let pipeline =
    MarkdownPipelineBuilder()
        .UseUrlRewriter(fun link ->
            if link.Url.StartsWith("https://") 
            then "https://example.net"
            else link.Url)
        .Build()

let markdown = "[Test1](https://example.com), [Test2](http://example.com)"
let html = Markdown.ToHtml(markdown, pipeline)
```

```csharp
var pipeline = new MarkdownPipelineBuilder()
    .UseUrlRewriter(link =>
    {
        if (link.Url.StartsWith("https://"))
            return "https://example.net";
        else
            return link.Url;
    })
    .Build();

var markdown = "[Test1](https://example.com), [Test2](http://example.com)";
var html = Markdown.ToHtml(markdown, pipeline);
```

Result: `<p><a href="https://example.net">Test1</a>, <a href="http://example.com">Test2</a></p>`

## Markdig.Extensions.ImageAsFigure
TODO - Renders all images as figures with a figcaption element set to the title of the image.

## Markdig.Extensions.SyntaxHighlighting
TODO - Uses [ColorCode-Universal](https://github.com/WilliamABradley/ColorCode-Universal) to provide syntax highlighting for code blocks.
