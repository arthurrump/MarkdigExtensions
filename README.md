# MarkdigExtensions
[![Build Status](https://dev.azure.com/arthurrump/MarkdigExtensions/_apis/build/status/CI?branchName=master)](https://dev.azure.com/arthurrump/MarkdigExtensions/_build/latest?definitionId=15&branchName=master)

Some useful extensions to the [Markdig](https://github.com/lunet-io/markdig) Markdown processor.

## MarkdigExtensions.UrlRewriter
[![NuGet](https://img.shields.io/nuget/v/MarkdigExtensions.UrlRewriter.svg)](https://www.nuget.org/packages/MarkdigExtensions.UrlRewriter/)

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
[![NuGet](https://img.shields.io/nuget/v/MarkdigExtensions.ImageAsFigure.svg)](https://www.nuget.org/packages/MarkdigExtensions.ImageAsFigure/)

Wraps all images inside a `<figure>` element with a `<figcaption>` set to the title of the image. You can choose to only wrap images where a title is set by providing the `onlyWithTitle` argument set to `true`.

### Example
```csharp
using Markdig;

var pipeline = new MarkdownPipelineBuilder()
    .UseImageAsFigure()
    .Build();

var markdown = "![Alt-text](https://example.com/img.png \"Image title text\")";
var html = Markdown.ToHtml(markdown, pipeline);
```

This will result in the following HTML:

```html
<p>
    <figure>
    	<img src="https://example.com/img.png" alt="Alt-text" title="Image title text" />
        <figcaption>Image title text</figcaption>
    </figure>
</p>
```

You can choose to only surround an image where a title is provided with a `<figure>` tag by using `.UseImageAsFigure(onlyWithTitle: true)`. Here's the output for `![Alt-text](https://example.com/img.png)` with both values:

- `onlyWithTitle = false` (default)

  ```html
  <figure>
      <img src="https://example.com/img.png" alt="Alt-text" />
  </figure>
  ```

- `onlyWithTitle = true`

  ```html
  <img src="https://example.com/img.png" alt="Alt-text" />
  ```

## MarkdigExtensions.SyntaxHighlighting
TODO - Uses [ColorCode-Universal](https://github.com/WilliamABradley/ColorCode-Universal) to provide syntax highlighting for code blocks.
