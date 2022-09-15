using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Linq;

namespace PS.WPF.Extensions
{
    public static class FlowDocumentExtensions
    {
        public static FlowDocument CreateDocument(this object content, FontFamily fontFamily, double fontSize)
        {
            var document = content as FlowDocument;

            if (content is string formattedString) document = formattedString.ToFlowDocument();
            if (content is IEnumerable<Block> blocks) document = blocks.ToFlowDocument();
            if (content is Block block) document = block.ToFlowDocument();
            if (content is IEnumerable<Inline> inlines) document = inlines.ToFlowDocument();
            if (content is Inline inline) document = inline.ToFlowDocument();

            document = document ?? new FlowDocument();
            document.FontFamily = fontFamily;
            document.FontSize = fontSize;
            return document;
        }

        public static ICollection<Inline> GetInlines(this FlowDocument document)
        {
            return GetInlines(document.Blocks);
        }

        public static ICollection<Inline> GetInlines(TextElementCollection<Block> blocks)
        {
            var inlines = new List<Inline>();
            var sourceBlocks = blocks.ToList();
            var index = 0;
            var count = sourceBlocks.Count;
            foreach (var block in sourceBlocks)
            {
                if (block is Paragraph paragraph)
                {
                    var paragraphSpan = new Span();

                    var properties = new[]
                    {
                        TextElement.FontFamilyProperty,
                        TextElement.FontSizeProperty,
                        TextElement.FontStretchProperty,
                        TextElement.FontStyleProperty,
                        TextElement.FontWeightProperty,
                        TextElement.ForegroundProperty,
                        TextElement.BackgroundProperty
                    };

                    block.CopySimilarValuesTo(paragraphSpan, properties);

                    foreach (var inline in paragraph.Inlines.ToArray())
                    {
                        paragraphSpan.Inlines.Add(inline);
                    }

                    inlines.Add(paragraphSpan);
                }
                else if (block is Section section)
                {
                    inlines.AddRange(GetInlines(section.Blocks));
                }

                if (index + 1 < count)
                {
                    inlines.Add(new LineBreak());
                    inlines.Add(new LineBreak());
                }

                index++;
            }

            return inlines;
        }

        public static FlowDocument ToFlowDocument(this string s)
        {
            s = WebUtility.HtmlEncode(s ?? string.Empty);
            s = s.Replace("\n", "<LineBreak/>");
            var builder = new StringBuilder();
            builder.AppendLine("<FlowDocument " +
                               "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" " +
                               "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" " +
                               "TextAlignment=\"Left\">");
            try
            {
                var xml = $"<Root>{s}</Root>";
                var document = XElement.Parse(xml);
                var paragraphs = document.Descendants(XName.Get("Paragraph")).ToList();
                if (paragraphs.Any())
                {
                    foreach (var paragraph in paragraphs)
                    {
                        builder.AppendLine(paragraph.ToString());
                    }
                }
                else
                {
                    builder.AppendLine($"<Paragraph LineHeight=\"0.1\">{s}</Paragraph>");
                }
            }
            catch (Exception)
            {
                builder.AppendLine($"<Paragraph>{s}</Paragraph>");
            }

            builder.AppendLine("</FlowDocument>");

            try
            {
                return XamlReader.Parse(builder.ToString()) as FlowDocument;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static FlowDocument ToFlowDocument(this IEnumerable<Inline> inlines)
        {
            var document = new FlowDocument
            {
                TextAlignment = TextAlignment.Left
            };
            var paragraph = new Paragraph
            {
                LineHeight = 0.1
            };
            paragraph.Inlines.AddRange(inlines.ToArray());
            document.Blocks.Add(paragraph);
            return document;
        }

        public static FlowDocument ToFlowDocument(this IEnumerable<Block> blocks)
        {
            var document = new FlowDocument
            {
                TextAlignment = TextAlignment.Left
            };
            document.Blocks.AddRange(blocks);
            return document;
        }

        public static FlowDocument ToFlowDocument(this Inline inline)
        {
            var document = new FlowDocument
            {
                TextAlignment = TextAlignment.Left
            };
            var paragraph = new Paragraph
            {
                LineHeight = 0.1
            };
            paragraph.Inlines.Add(inline);
            document.Blocks.Add(paragraph);
            return document;
        }

        public static FlowDocument ToFlowDocument(this Block block)
        {
            var document = new FlowDocument
            {
                TextAlignment = TextAlignment.Left
            };
            document.Blocks.Add(block);
            return document;
        }
    }
}
