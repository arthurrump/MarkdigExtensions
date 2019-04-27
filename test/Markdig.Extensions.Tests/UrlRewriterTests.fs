module UrlRewriterTests

open Expecto

open Markdig
open Markdig.Extensions.UrlRewriter

[<Tests>]
let tests =
    testList "UrlRewrite" [
        test "Rewrites all urls" {
            let pipeline =
                MarkdownPipelineBuilder()
                    .UseUrlRewriter(fun link ->
                        if link.Url.StartsWith("https://") 
                        then "https://example.net"
                        else link.Url)
                    .Build()

            let markdown = "[Test1](https://example.com), [Test2](http://example.com)"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.isTrue (html.Contains("https://example.net")) "Contains new https"
            Expect.isTrue (html.Contains("http://example.com")) "Not replaced http"
            Expect.isFalse (html.Contains("https://example.com")) "Replaced old https"
        }

        test "Rewrites only images" {
            let pipeline =
                MarkdownPipelineBuilder()
                    .UseUrlRewriter(fun link ->
                        if link.IsImage
                        then "http://newImageUrl.net"
                        else link.Url)
                    .Build()

            let markdown = "[Test1](https://example.com), ![Image](http://example.net)"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.isTrue (html.Contains("https://example.com")) "Not replaced anchor link"
            Expect.isTrue (html.Contains("http://newImageUrl.net")) "Contains new image url"
            Expect.isFalse (html.Contains("http://example.net")) "Replaced old image url"
        }

        test "No rewrite" {
            let pipeline =
                MarkdownPipelineBuilder()
                    .UseUrlRewriter(fun link -> link.Url)
                    .Build()

            let markdown = "[Test1](https://example.com), ![Image](http://example.net)"
            let expected = """<p><a href="https://example.com">Test1</a>, <img src="http://example.net" alt="Image" /></p>""" + "\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.equal html expected "Correct HTML"
        }
    ]
