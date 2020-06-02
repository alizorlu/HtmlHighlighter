using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsuiteNetProxyDemo.HapManager
{
    public interface IHapManager : IDisposable
    {
        string GetPageSource();
        List<string> SourceOfLinks();
        List<string> SourceOfLinks(HtmlAgilityPack.HtmlDocument hdoc);
        HtmlAgilityPack.HtmlDocument GetDocument();
        List<HtmlNode> QueryHtmlNode(string xpath, ref HtmlDocument hdoc);
        List<HtmlNode> QueryHtmlNodeByAttribute(string attribute, ref List<HtmlNode> list);
        List<string> GetHtmlNodeAttributeValue(string attribute, ref List<HtmlNode> list);
        HtmlAgilityPack.HtmlDocument SetHtmlTag(string elementName, string attribute, HtmlAgilityPack.HtmlDocument hdoc);
        HtmlAgilityPack.HtmlDocument SetHtmlTagMultiple(string[] sortedElementNames, string[] sortedAttributes, HtmlAgilityPack.HtmlDocument hdoc);
        HtmlAgilityPack.HtmlDocument SetHighlights(string[] keywords, HtmlAgilityPack.HtmlDocument inputDocument);
        string FindAndReplaceInnerText(string innerText, string[] keywords);
        string FrameHighlight(string input);
        List<HtmlNode> GetChildsElementByNode(HtmlNode node, HtmlAgilityPack.HtmlDocument hdoc);
        void RemoveMultipleTag( ref HtmlAgilityPack.HtmlDocument hdoc, params string[] tagNames);
    }

}
