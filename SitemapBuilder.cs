namespace Labo.Sitemap
{
    using System;
    using System.Xml;

    /// <summary>
    /// Xml Sitemap builder.
    /// Attention: Max 50,000 entries and size of 10 MB.
    /// </summary>
    public sealed class SitemapBuilder
    {
        public enum ChangeFrequency
        {
            always, 
            hourly, 
            daily,
            weekly, 
            monthly, 
            yearly, 
            never
        }

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
        /// Initializes a new instance of the <see cref="SitemapBuilder"/> class.
        /// </summary>
        public SitemapBuilder()
        {
            m_Doc = new XmlDocument();

            // declaration
            XmlDeclaration xmldecl = m_Doc.CreateXmlDeclaration("1.0", null, null);
            xmldecl.Encoding = "UTF-8";
            m_Doc.AppendChild(xmldecl);

            // root document node
            XmlElement urlset = m_Doc.CreateElement("urlset");
            XmlAttribute xmlnsAttribute = m_Doc.CreateAttribute("xmlns");
            xmlnsAttribute.Value = "http://www.sitemaps.org/schemas/sitemap/0.9";
            urlset.Attributes.Append(xmlnsAttribute);

            XmlAttribute xmlnsxsiAttribute = m_Doc.CreateAttribute("xmlns:xsi");
            xmlnsxsiAttribute.Value = "http://www.w3.org/2001/XMLSchema-instance";
            urlset.Attributes.Append(xmlnsxsiAttribute);

            XmlAttribute xmlnsschemaLocationAttribute = m_Doc.CreateAttribute("xmlns:schemaLocation");
            xmlnsschemaLocationAttribute.Value = "http://www.sitemaps.org/schemas/sitemap/0.9 http://www.sitemaps.org/schemas/sitemap/0.9/siteindex.xsd";
            urlset.Attributes.Append(xmlnsschemaLocationAttribute);

            m_Doc.AppendChild(urlset);
        }

        /// <summary>
        /// Append new Url location into Sitemap.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        public void AppendUrl(string loc)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            m_Doc.DocumentElement.AppendChild(CreateUrlElement(loc));
        }

        /// <summary>
        /// Append new Url into Sitemap with last modification date.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        /// <param name="lastmod">The date of last modification of the file.</param>
        public void AppendUrl(string loc, DateTime lastmod)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateUrlElement(loc);
            urlNode.AppendChild(CreateLastmodElement(lastmod));

            m_Doc.DocumentElement.AppendChild(urlNode);
        }

        /// <summary>
        /// Append new Url into Sitemap with refresh rate.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        /// <param name="changefreq">How frequently the page is likely to change.</param>
        public void AppendUrl(string loc, ChangeFrequency changefreq)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateUrlElement(loc);
            urlNode.AppendChild(CreateChangefreqElement(changefreq));
 
            m_Doc.DocumentElement.AppendChild(urlNode);
        }

        /// <summary>
        /// Append new Url into Sitemap with priority.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        /// <param name="priority">The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0.</param>
        public void AppendUrl(string loc, double priority)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateUrlElement(loc);
            urlNode.AppendChild(CreatePriorityElement(priority));

            m_Doc.DocumentElement.AppendChild(urlNode);
        }

        /// <summary>
        /// Append new Url into Sitemap with last modification date and refresh rate.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        /// <param name="lastmod">The date of last modification of the file.</param>
        /// <param name="changefreq">How frequently the page is likely to change.</param>
        public void AppendUrl(string loc, DateTime lastmod, ChangeFrequency changefreq)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateUrlElement(loc);
            urlNode.AppendChild(CreateLastmodElement(lastmod));
            urlNode.AppendChild(CreateChangefreqElement(changefreq));

            m_Doc.DocumentElement.AppendChild(urlNode);
        }

        /// <summary>
        /// Append new Url into Sitemap with last modification date and priority.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        /// <param name="lastmod">The date of last modification of the file.</param>
        /// <param name="priority">The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0.</param>
        public void AppendUrl(string loc, DateTime lastmod, double priority)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateUrlElement(loc);
            urlNode.AppendChild(CreateLastmodElement(lastmod));
            urlNode.AppendChild(CreatePriorityElement(priority));

            m_Doc.DocumentElement.AppendChild(urlNode);
        }

        /// <summary>
        /// Append new Url into Sitemap with refresh rate and priority.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        /// <param name="changefreq">How frequently the page is likely to change.</param>
        /// <param name="priority">The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0.</param>
        public void AppendUrl(string loc, ChangeFrequency changefreq, double priority)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateUrlElement(loc);
            urlNode.AppendChild(CreateChangefreqElement(changefreq));
            urlNode.AppendChild(CreatePriorityElement(priority));

            m_Doc.DocumentElement.AppendChild(urlNode);
        }

        /// <summary>
        /// Append new Url into Sitemap with refresh rate, priority and last modification date.
        /// </summary>
        /// <param name="loc">URL of the page.</param>
        /// <param name="changefreq">How frequently the page is likely to change.</param>
        /// <param name="priority">The priority of this URL relative to other URLs on your site. Valid values range from 0.0 to 1.0.</param>
        /// <param name="lastmod">The date of last modification of the file.</param>
        public void AppendUrl(string loc, ChangeFrequency changefreq, double priority, DateTime lastmod)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = CreateUrlElement(loc);
            urlNode.AppendChild(CreateChangefreqElement(changefreq));
            urlNode.AppendChild(CreatePriorityElement(priority));
            urlNode.AppendChild(CreateLastmodElement(lastmod));

            m_Doc.DocumentElement.AppendChild(urlNode);
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
        /// Generates the XML text.
        /// </summary>
        /// <returns>XML text.</returns>
        public string GenerateXmlText()
        {
            return m_Doc.OuterXml;
        }

        /// <summary>
        /// Creates the URL element.
        /// </summary>
        /// <param name="loc">The loc.</param>
        /// <returns>The creaed url element.</returns>
        private XmlElement CreateUrlElement(string loc)
        {
            if (loc == null)
            {
                throw new ArgumentNullException("loc");
            }

            XmlElement urlNode = m_Doc.CreateElement("url");
            XmlElement locNode = m_Doc.CreateElement("loc");

            locNode.InnerText = loc;
            urlNode.AppendChild(locNode);

            return urlNode;
        }

        /// <summary>
        /// Creates the lastmod xml element.
        /// </summary>
        /// <param name="lastmod">The lastmod.</param>
        /// <returns>The lastmod xml element.</returns>
        private XmlElement CreateLastmodElement(DateTime lastmod)
        {
            XmlElement lastmodNode = m_Doc.CreateElement("lastmod");
            lastmodNode.InnerText = lastmod.ToString(DATETIME_PATTERN);
            return lastmodNode;
        }

        /// <summary>
        /// Creates the changefreq xml element.
        /// </summary>
        /// <param name="changefreq">The changefreq.</param>
        /// <returns>The changefreq xml element.</returns>
        private XmlElement CreateChangefreqElement(ChangeFrequency changefreq)
        {
            XmlElement changefreqNode = m_Doc.CreateElement("changefreq");
            changefreqNode.InnerText = changefreq.ToString();
            return changefreqNode;
        }

        /// <summary>
        /// Creates the priority xml element.
        /// </summary>
        /// <param name="priority">The priority.</param>
        /// <returns>The priority xml element</returns>
        private XmlElement CreatePriorityElement(double priority)
        {
            XmlElement changefreqNode = m_Doc.CreateElement("priority");
            changefreqNode.InnerText = priority.ToString(System.Globalization.CultureInfo.InvariantCulture);
            return changefreqNode;
        }
    }
}
