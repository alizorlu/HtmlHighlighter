using HtmlAgilityPack;
using MsuiteNetProxyDemo.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MsuiteNetProxyDemo.HapManager
{
    public class HapManager : IHapManager
    {
        private readonly string[] bgColors = new string[] { "#d92027", "#0D0628", "#DA627D", "#FE6847", "#2176AE", "#153243" };
        private readonly string _uri = default;
        private static readonly object _uriLock = new object();
        public HapManager(string uri)
        {
            lock (_uriLock)
            {
                if (_uri == default || _uri == null) _uri = uri;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<string> GetHtmlNodeAttributeValue(string attribute, ref List<HtmlNode> list)
        {
            var result = list.Select(f => f.Attributes[attribute].Value).ToList();
            return result;
        }

        public string GetPageSource()
        {
            HtmlAgilityPack.HtmlWeb hweb = new HtmlWeb();
            //hweb.OverrideEncoding = Encoding.GetEncoding("windows-1254");
            hweb.AutoDetectEncoding = true;
            HtmlAgilityPack.HtmlDocument hdoc = hweb.Load(this._uri);
            return hdoc.DocumentNode.InnerHtml;
        }

        public List<HtmlNode> QueryHtmlNode(string xpath, ref HtmlAgilityPack.HtmlDocument hdoc)
        {
            return hdoc.DocumentNode.SelectNodes(xpath).ToList();
        }

        public List<HtmlNode> QueryHtmlNodeByAttribute(string attribute, ref List<HtmlNode> list)
        {
            return list.Where(f => f.Attributes[attribute] != null && f.Attributes[attribute] != default).ToList();
        }

        public HtmlAgilityPack.HtmlDocument SetHtmlTag(string elementName, string attribute, HtmlAgilityPack.HtmlDocument hdoc)
        {
            var _tempDocument = hdoc;
            //a,link,script,img
            foreach (var item in hdoc.DocumentNode.SelectNodes($"//{elementName}").ToList())
            {
                var attr = item.Attributes[attribute];
                if (attr == default || attr == null) continue;
                else
                {
                    attr.Value = attr.Value.EditSourceLink(this._uri);
                }
            }
            return hdoc;
        }

        public List<string> SourceOfLinks()
        {
            HtmlAgilityPack.HtmlWeb hweb = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument hdoc = hweb.Load(this._uri);

            var aQuery = QueryHtmlNode("//a", ref hdoc);
            aQuery = QueryHtmlNodeByAttribute("href", ref aQuery);
            var imgQuery = QueryHtmlNode("//img", ref hdoc);
            imgQuery = QueryHtmlNodeByAttribute("src", ref imgQuery);
            var scriptQuery = QueryHtmlNode("//script", ref hdoc);
            scriptQuery = QueryHtmlNodeByAttribute("src", ref scriptQuery);
            var linkQuery = QueryHtmlNode("//link", ref hdoc);
            linkQuery = QueryHtmlNodeByAttribute("href", ref linkQuery);
            List<string> result = new List<string>();
            return result.Concat(GetHtmlNodeAttributeValue("href", ref aQuery))
                  .Concat(GetHtmlNodeAttributeValue("src", ref imgQuery))
                  .Concat(GetHtmlNodeAttributeValue("src", ref scriptQuery))
                  .Concat(GetHtmlNodeAttributeValue("href", ref linkQuery))
                  .ToList();
        }

        public HtmlAgilityPack.HtmlDocument SetHtmlTagMultiple(string[] sortedElementNames, string[] sortedAttributes, HtmlAgilityPack.HtmlDocument hdoc)
        {
            try
            {
                int index = -1;
                HtmlAgilityPack.HtmlDocument _swapHdoc = hdoc;
                foreach (var item in sortedElementNames)
                {
                    index++;
                    _swapHdoc = SetHtmlTag(item, sortedAttributes[index], _swapHdoc);
                }
                return _swapHdoc;
            }
            catch (Exception)
            {
                return hdoc;
            }
        }


        public HtmlAgilityPack.HtmlDocument GetDocument()
        {
            HtmlAgilityPack.HtmlWeb hweb = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument hdoc = hweb.Load(this._uri);
            return hdoc;
        }

        public List<string> SourceOfLinks(HtmlAgilityPack.HtmlDocument hdoc)
        {
            var aQuery = QueryHtmlNode("//a", ref hdoc);
            aQuery = QueryHtmlNodeByAttribute("href", ref aQuery);
            var imgQuery = QueryHtmlNode("//img", ref hdoc);
            imgQuery = QueryHtmlNodeByAttribute("src", ref imgQuery);
            var scriptQuery = QueryHtmlNode("//script", ref hdoc);
            scriptQuery = QueryHtmlNodeByAttribute("src", ref scriptQuery);
            var linkQuery = QueryHtmlNode("//link", ref hdoc);
            linkQuery = QueryHtmlNodeByAttribute("href", ref linkQuery);
            List<string> result = new List<string>();
            result = result.Concat(GetHtmlNodeAttributeValue("href", ref aQuery))
                  .Concat(GetHtmlNodeAttributeValue("src", ref imgQuery))
                  .Concat(GetHtmlNodeAttributeValue("src", ref scriptQuery))
                  .Concat(GetHtmlNodeAttributeValue("href", ref linkQuery))
                  .ToList();
            return result;
        }

        public HtmlAgilityPack.HtmlDocument SetHighlights(string[] keywords, HtmlAgilityPack.HtmlDocument inputDocument)
        {
            HtmlAgilityPack.HtmlDocument _swapHdoc = inputDocument;
            List<HtmlNode> allItems = new List<HtmlNode>();
            foreach (var item in _swapHdoc.DocumentNode.SelectNodes("//div").ToList())
            {
                allItems.AddRange(GetChildsElementByNode(item, _swapHdoc).ToList());


            }
            foreach (HtmlNode item in allItems)
            {
                HtmlNode itemSwap = item.LastChild;
                while (itemSwap != null)
                {
                    if (itemSwap.NodeType == HtmlNodeType.Text)
                    {
                        //Text burası
                        if (itemSwap.InnerHtml.ReplaceSpecialChars().Contains("AREA"))
                        {
                            var sad = itemSwap;
                        }
                        itemSwap.InnerHtml = FindAndReplaceInnerText(itemSwap.InnerText.ReplaceSpecialChars(), keywords);
                        break;
                    }
                    else
                    {
                        itemSwap = itemSwap.LastChild;
                    }
                }
            }
            var aldsa = allItems;
            return _swapHdoc;
        }

        public string FrameHighlight(string input)
        {
            if (input.Contains("<span") || input.Contains("</span>")) return input; // tekrar tekrar highligth eklemeyelim.
            string randomBg = this.bgColors[new Random().Next(0, bgColors.Count() - 1)];
            return $"<span class='ap_tech_highlighter' style='background-color:{randomBg} !important;color:white !important;'>" +
                $"{input}" +
                $"</span>";
        }

        public string FindAndReplaceInnerText(string innerText, string[] keywords)
        {

            if (string.IsNullOrEmpty(innerText) || innerText == default) return innerText;
            foreach (var item in keywords)
            {
                if (innerText.Length < item.Length) continue;
                int startIndex = innerText.ReplaceSpecialChars().ToLower().IndexOf(item.ToLower());
                if (startIndex > -1)
                {
                    //Buldu
                    //----------------------------------------------
                    //Case:İlk kelime olabilir
                    //Case: Ortada olabilir
                    //Case:Son kelime olabilir.
                    //----------------------------------------------
                    string value = WebUtility.HtmlDecode(innerText);
                    if (startIndex == 0 || startIndex <= item.Length)
                    {
                        value = value.Remove(0, item.Length);
                        innerText = $"{FrameHighlight(item)}{value}";
                    }
                    else if (startIndex > item.Length)
                    {
                        string beforeText = value.Substring(0, startIndex);
                        string afterText = value.Remove(0, beforeText.Length + (item.Length));
                        innerText = $"{beforeText}{FrameHighlight(item)}{afterText}";
                    }
                }
                else continue;
            }
            return innerText;
        }

        public List<HtmlNode> GetChildsElementByNode(HtmlNode node, HtmlAgilityPack.HtmlDocument hdoc)
        {
            var htmlNodeList = node.ChildNodes.Where(f => f.NodeType == HtmlNodeType.Element
              &&
              (f.InnerText != null && f.InnerText != default && f.InnerText.Length > 3)
              &&
              (f.OriginalName != "script")
              ).ToList();
            return htmlNodeList;

        }

        public void RemoveMultipleTag(ref HtmlAgilityPack.HtmlDocument hdoc, params string[] tagNames)
        {
            foreach (var tag in tagNames)
            {
                foreach (var findedItems in hdoc.DocumentNode.SelectNodes($"//{tag}"))
                {
                    findedItems.Remove();
                }
            }
        }
    }
}
