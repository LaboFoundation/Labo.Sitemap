namespace Labo.Sitemap
{
    using System;
    using System.Xml;

    public sealed class SitemapIndexBuilder
    {
        // W3.org (http://www.w3.org/TR/NOTE-datetime) pattern: YYYY-MM-DDThh:mm:ssTZD
        private const string DATETIME_PATTERN = "yyyy-MM-ddTHH:mm:sszzz";

        private readonly XmlDocument m_Doc;

        /// <summary>
        /// Gets Sitemap XmlDocument
        /// </summary>
        public XmlDocument XmlDocument
        {
            get { return m_Doc; }
        }

        /// <summary>
        /// Gets a value indicating whether this sitemap has no entries.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this sitemap has no entries; otherwise, <c>false</c>.
        /// </value>
        public bool IsEmpty
        {
            get
            {
                return m_Doc.DocumentElement.ChildNodes.Count == 0;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapIndexBuilder"/> class.
        /// </summary>
        public SitemapIndexBuilder()
        {
            m_Doc = new XmlDocument();

            // declaration
            XmlDeclaration xmldecl = m_Doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";
            m_Doc.AppendChild(xmldecl);

            // root document node
            XmlElement sitemapindex = m_Doc.CreateElement("sitemapindex");
            sitemapindex.SetAttribute("xmlns", "http://www.sitemaps.org/schemas/sitemap/0.9");
            sitemapindex.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            sitemapindex.SetAttribute("xsi:schemaLocation", "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd");

            m_Doc.AppendChild(sitemapindex);
        }

        /// <summary>
        /// Appends the sitemap URL.
        /// </summary>
        /// <param name="loc">The URL of the sitemap.</param>
        public void AppendSitemapUrl(string loc)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            m_Doc.DocumentElement.AppendChild(CreateSitemapElement(loc));
        }

        /// <summary>
        /// Appends the sitemap URL.
        /// </summary>
        /// <param name="loc">The URL of the sitemap.</param>
        /// <param name="lastmod">The date of last modification of the file.</param>
        public void AppendSitemapUrl(string loc, DateTime lastmod)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateSitemapElement(loc);
            urlNode.AppendChild(CreateLastmodElement(lastmod));

            m_Doc.DocumentElement.AppendChild(urlNode);
        }

        /// <summary>
        /// Generates the XML text.
        /// </summary>
        /// <returns>XML text.</returns>
        public string GenerateXmlText()
        {
            return m_Doc.OuterXml;
        }

        /// <summary>
        /// Creates the sitemap element.
        /// </summary>
        /// <param name="loc">The URL of the sitemap.</param>
        /// <returns>Sitemap xml element.</returns>
        private XmlElement CreateSitemapElement(string loc)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement siteMapNode = m_Doc.CreateElement("sitemap");
            XmlElement locNode = m_Doc.CreateElement("loc");

            locNode.InnerText = loc;
            siteMapNode.AppendChild(locNode);

            return siteMapNode;
        }

        /// <summary>
        /// Creates the lastmod element.
        /// </summary>
        /// <param name="lastmod">The date of last modification of the file..</param>
        /// <returns>The last modified xml element</returns>
        private XmlElement CreateLastmodElement(DateTime lastmod)
        {
            XmlElement lastmodNode = m_Doc.CreateElement("lastmod");
            lastmodNode.InnerText = lastmod.ToString(DATETIME_PATTERN);
            return lastmodNode;
        }
    }
}