module ImageAsFigureTests

open Expecto

open Markdig

[<Tests>]
let tests =
    let pipeline = MarkdownPipelineBuilder().UseImageAsFigure().Build()

    testList "ImageAsFigure" [
        test "Renders image inside figure" {
            let markdown = """![Image](https://example.com/img.png)"""
            let expected = """<p><figure><img src="https://example.com/img.png" alt="Image" /></figure></p>""" + "\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.equal html expected "Correct html"
        }

        test "Renders image inside figure with title as caption" {
            let markdown = """![Image](https://example.com/img.png "Caption")"""
            let expected = """<p><figure><img src="https://example.com/img.png" alt="Image" title="Caption" /><figcaption>Caption</figcaption></figure></p>""" + "\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.equal html expected "Correct html"
        }

        test "Leaves non-image links as they are" {
            let markdown = """[Normal link](https://example.com)"""
            let expected = """<p><a href="https://example.com">Normal link</a></p>""" + "\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.equal html expected "Correct html"
        }

        test "onlyWithTitle=true skips image without title" {
            let pipeline = MarkdownPipelineBuilder().UseImageAsFigure(onlyWithTitle = true).Build()
            let markdown = """![Image](https://example.com/img.png)"""
            let expected = """<p><img src="https://example.com/img.png" alt="Image" /></p>""" + "\n"
            let html = Markdown.ToHtml(markdown, pipeline)
            Expect.equal html expected "Correct html"
        }
    ]
