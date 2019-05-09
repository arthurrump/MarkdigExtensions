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

Uses [ColorCode-Universal](https://github.com/WilliamABradley/ColorCode-Universal) to add syntax highlighting to fenced code blocks. Check out their [LanguageId.cs](https://github.com/WilliamABradley/ColorCode-Universal/blob/master/ColorCode.Core/Common/LanguageId.cs) file to get a list of all supported languages, code blocks with an unsupported language or no language specified will be rendered using the standard renderer.

### Example

```csharp
using Markdig;

var pipeline = new MarkdownPipelineBuilder()
    .UseSyntaxHighlighting()
    .Build();

var markdown = "```f# \nprintfn \"Hello, %s!\" \"world\" \n``` \n";
var html = Markdown.ToHtml(markdown, pipeline);
```

Instead of a normal code block, this will render a code block with inline CSS to add the colorization:

```html
<div style="color:#000000;background-color:#FFFFFF;"><pre>
	printfn <span style="color:#A31515;">&quot;Hello, %s!&quot;</span> <span style="color:#A31515;">&quot;world&quot;</span>
</pre></div>
```

You can specify a custom color scheme by providing a `StyleDictionary`:

```csharp
using Markdig;
using ColorCode.Styling;

var pipeline = new MarkdownPipelineBuilder()
    .UseSyntaxHighlighting(StyleDictionary.DefaultDark)
    .Build();
```

