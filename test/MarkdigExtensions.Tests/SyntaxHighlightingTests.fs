module SyntaxHighlightingTests

open Expecto

open Markdig
open ColorCode.Styling

[<Tests>]
let tests =
    let concatn = String.concat "\n"
    let pipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting().Build()

    testSequencedGroup "Prevent ColorCode multithreading issues" <| testList "SyntaxHighlighting" [
        test "Highlights code" {
            let markdown = 
                [ "```f#"
                  """printfn "Hello, %s!" "world" """
                  "```" ] |> concatn
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div style=\"color:#000000;background-color:#FFFFFF;\"><pre>\r\n" + 
                "printfn <span style=\"color:#A31515;\">&quot;Hello, %s!&quot;</span> " + 
                "<span style=\"color:#A31515;\">&quot;world&quot;</span> \r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }

        test "Doesn't color unknown language" {
            let markdown = 
                [ "```nonexistent-lang"
                  "Test code"
                  "123"
                  "```" ] |> concatn
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = "<pre><code class=\"language-nonexistent-lang\">Test code\n123\n</code></pre>\n"
            Expect.equal html expected "Correct html"
        }

        test "Highlights with custom style" {
            let pipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting(StyleDictionary.DefaultDark).Build()
            let markdown = 
                [ "```f#"
                  """printfn "Hello, %s!" "world" """
                  "```" ] |> concatn
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div style=\"color:#DADADA;background-color:#1E1E1E;\"><pre>\r\n" + 
                "printfn <span style=\"color:#D69D85;\">&quot;Hello, %s!&quot;</span> " + 
                "<span style=\"color:#D69D85;\">&quot;world&quot;</span> \r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }

        test "Highlights multiline xml" {
            let markdown = 
                [ "```xml"
                  """<MainTag item="value">"""
                  "\t<SubTag something=\"\"/>"
                  "</MainTag>"
                  "```" ] |> concatn
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div style=\"color:#000000;background-color:#FFFFFF;\"><pre>\r\n" + 
                "<span style=\"color:#0000FF;\">&lt;</span><span style=\"color:#A31515;\">MainTag</span> " + 
                "<span style=\"color:#FF0000;\">item</span><span style=\"color:#0000FF;\">=</span>" + 
                "<span style=\"color:#000000;\">&quot;</span><span style=\"color:#0000FF;\">value</span>" + 
                "<span style=\"color:#000000;\">&quot;</span><span style=\"color:#0000FF;\">&gt;</span>\n" +
                "\t&lt;SubTag something=&quot;&quot;/&gt;\n" + 
                "<span style=\"color:#0000FF;\">&lt;/</span><span style=\"color:#A31515;\">MainTag</span>" + 
                "<span style=\"color:#0000FF;\">&gt;</span>\r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }

        test "Multiple code blocks with unrecognized first" {
            let markdown =
                [ "```unknown-lang"
                  "Unknown test code"
                  "```"
                  ""
                  "```csharp"
                  """Console.WriteLine("Hello, world!");"""
                  "```" ] |> concatn
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<pre><code class=\"language-unknown-lang\">Unknown test code\n</code></pre>\n" + 
                "<div style=\"color:#000000;background-color:#FFFFFF;\"><pre>\r\n" + 
                "Console.WriteLine(<span style=\"color:#A31515;\">&quot;Hello, world!&quot;</span>);\r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }
        
        test "Multiple code blocks with recognized first" {
            let markdown =
                [ "```csharp"
                  """Console.WriteLine("Hello, world!");"""
                  "```"
                  ""
                  "```unknown-lang"
                  "Unknown test code"
                  "```" ] |> concatn
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div style=\"color:#000000;background-color:#FFFFFF;\"><pre>\r\n" + 
                "Console.WriteLine(<span style=\"color:#A31515;\">&quot;Hello, world!&quot;</span>);\r\n" + 
                "</pre></div>\n<pre><code class=\"language-unknown-lang\">Unknown test code\n</code></pre>\n"
            Expect.equal html expected "Correct html"
        }

        test "Code block with no specified language" {
            let markdown =
                [ "```"
                  "This is a code block with no language"
                  "It will not be colored"
                  "```" ] |> concatn
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = "<pre><code>This is a code block with no language\nIt will not be colored\n</code></pre>\n"
            Expect.equal html expected "Correct html"
        }

        test "Highlights code with classes when inlineCss = false" {
            let markdown = 
                [ "```f#"
                  """printfn "Hello, %s!" "world" """
                  "```" ] |> concatn
            let pipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting(StyleDictionary.DefaultLight, false).Build()
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
               "<div class=\"FSharp\"><pre>\r\n" +
               "printfn <span class=\"string\">&quot;Hello, %s!&quot;</span> " + 
               "<span class=\"string\">&quot;world&quot;</span> \r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }

        test "Highlights multiline xml with inlineCss = false" {
            let markdown = 
                [ "```xml"
                  """<MainTag item="value">"""
                  "\t<SubTag something=\"\"/>"
                  "</MainTag>"
                  "```" ] |> concatn
            let pipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting(StyleDictionary.DefaultLight, false).Build()
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div class=\"xml\"><pre>\r\n" + 
                "<span class=\"xmlDelimiter\">&lt;</span><span class=\"xmlName\">MainTag</span> " + 
                "<span class=\"xmlAttribute\">item</span><span class=\"xmlDelimiter\">=</span>" + 
                "<span class=\"xmlAttributeQuotes\">&quot;</span><span class=\"xmlAttributeValue\">value</span>" + 
                "<span class=\"xmlAttributeQuotes\">&quot;</span><span class=\"xmlDelimiter\">&gt;</span>\n" +
                "\t&lt;SubTag something=&quot;&quot;/&gt;\n<span class=\"xmlDelimiter\">&lt;/</span>" + 
                "<span class=\"xmlName\">MainTag</span><span class=\"xmlDelimiter\">&gt;</span>\r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }

        test "Highlighted with inlineCss = false should give same result for different themes" {
            let markdown = 
                [ "```xml"
                  """<MainTag item="value">"""
                  "\t<SubTag something=\"\"/>"
                  "</MainTag>"
                  "```" ] |> concatn
            let lightPipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting(StyleDictionary.DefaultLight, false).Build()
            let lightHtml = Markdown.ToHtml(markdown, lightPipeline)
            let darkPipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting(StyleDictionary.DefaultDark, false).Build()
            let darkHtml = Markdown.ToHtml(markdown, darkPipeline)
            Expect.equal lightHtml darkHtml "Light and dark html are the same"
        }
    ]
