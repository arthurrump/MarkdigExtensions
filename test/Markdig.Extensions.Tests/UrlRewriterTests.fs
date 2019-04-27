module UrlRewriterTests

open Expecto

open Markdig
open Markdig.Extensions.UrlRewriter
open Markdig.Syntax.Inlines

[<Tests>]
let tests =
    testList "UrlRewrite" [
        test "Rewrites all urls" {
            let rewriter (link : LinkInline) =
                if link.Url.StartsWith("https://") 
                then "https://example.net"
                else link.Url

            let pipeline =
                MarkdownPipelineBuilder()
                    .UseUrlRewriter(rewriter)
                    .Build()

            let markdown = "[Test1](https://example.com), [Test2](http://example.com)"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.isTrue (html.Contains("https://example.net")) "Contains new https"
            Expect.isTrue (html.Contains("http://example.com")) "Not replaced http"
            Expect.isFalse (html.Contains("https://example.com")) "Replaced old https"
        }
    ]
