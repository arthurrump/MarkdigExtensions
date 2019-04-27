using Xunit;

namespace Markdig.Extensions.Tests.CSharp
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
    }
}
