using Markdig;
using Xunit;

namespace MarkdigExtensions.Tests.CSharp
{
    public class UrlRewriterTests
    {
        [Fact]
        public void RewritesUrls()
        {
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
            Assert.Contains("https://example.net", html);
            Assert.Contains("http://example.com", html);
            Assert.DoesNotContain("https://example.com", html);
        }

        [Fact]
        public void RewriteToHttps()
        {
            var pipeline = new MarkdownPipelineBuilder()
                .UseUrlRewriter(link => link.Url.Replace("http://", "https://"))
                .Build();

            var markdown = "[Anchor](http://example.net), ![Image](http://example.com/img.png)";
            var html = Markdown.ToHtml(markdown, pipeline);
            var expected = "<p><a href=\"https://example.net\">Anchor</a>, <img src=\"https://example.com/img.png\" alt=\"Image\" /></p>\n";
            Assert.Equal(expected, html);
        }
    }
}
