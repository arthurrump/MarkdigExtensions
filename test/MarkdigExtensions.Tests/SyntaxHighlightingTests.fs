module SyntaxHighlightingTests

open Expecto

open Markdig

[<Tests>]
let tests =
    let pipeline = MarkdownPipelineBuilder().UseSyntaxHighlighting().Build()

    testList "SyntaxHighlighting" [
        test "Colors" {
            let markdown = "```f#\nprintfn \"Hello, %s!\" \"world\"\n```\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = 
                "<div style=\"color:#000000;background-color:#FFFFFF;\"><pre>\r\n" + 
                "printfn <span style=\"color:#A31515;\">&quot;Hello, %s!&quot;</span> " + 
                "<span style=\"color:#A31515;\">&quot;world&quot;</span>\r\n</pre></div>"
            Expect.equal html expected "Correct html"
        }

        test "Doesn't color unknown" {
            let markdown = "```nonexistent-lang\nTest code\n123\n```\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            let expected = "<pre><code class=\"language-nonexistent-lang\">Test code\n123\n</code></pre>\n"
            Expect.equal html expected "Correct html"
        }
    ]
