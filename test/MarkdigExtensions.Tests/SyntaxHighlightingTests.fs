module SyntaxHighlightingTests

open Expecto

open Markdig
open ColorCode.Styling

[<Tests>]
let tests =
    let pipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting().Build()

    testSequencedGroup "Prevent ColorCode multithreading issues" <| testList "SyntaxHighlighting" [
        test "Highlights code" {
            let markdown = "```f#\nprintfn \"Hello, %s!\" \"world\"\n```\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div style=\"color:#000000;background-color:#FFFFFF;\"><pre>\r\n" + 
                "printfn <span style=\"color:#A31515;\">&quot;Hello, %s!&quot;</span> " + 
                "<span style=\"color:#A31515;\">&quot;world&quot;</span>\r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }

        test "Doesn't color unknown language" {
            let markdown = "```nonexistent-lang\nTest code\n123\n```\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = "<pre><code class=\"language-nonexistent-lang\">Test code\n123\n</code></pre>\n"
            Expect.equal html expected "Correct html"
        }

        test "Highlights with custom style" {
            let pipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting(StyleDictionary.DefaultDark).Build()
            let markdown = "```f#\nprintfn \"Hello, %s!\" \"world\"\n```\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div style=\"color:#DADADA;background-color:#1E1E1E;\"><pre>\r\n" + 
                "printfn <span style=\"color:#D69D85;\">&quot;Hello, %s!&quot;</span> " + 
                "<span style=\"color:#D69D85;\">&quot;world&quot;</span>\r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }
    ]
