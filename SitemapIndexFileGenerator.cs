namespace Labo.Sitemap
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    /// <summary>
    /// The sitemap index file generator class.
    /// </summary>
    public sealed class SitemapIndexFileGenerator
    {
        /// <summary>
        /// The root URI of the website
        /// </summary>
        private readonly Uri m_RootUri;

        /// <summary>
        /// The sitemap entries collection.
        /// </summary>
        private readonly SitemapEntryCollection m_SitemapEntries;

        /// <summary>
        /// The maximum entries per sitemap
        /// </summary>
        private readonly int m_MaxEntriesPerSitemap;

        /// <summary>
        /// The maximum site map entries per sitemap index
        /// </summary>
        private readonly int m_MaxSiteMapEntriesPerSitemapIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapIndexFileGenerator"/> class.
        /// </summary>
        /// <param name="rootUri">The root URI.</param>
        /// <param name="sitemapEntries">The sitemap entries.</param>
        /// <param name="maxEntriesPerSitemap">The maximum entries per sitemap.</param>
        /// <param name="maxSiteMapEntriesPerSitemapIndex">Maximum index of the site map entries per sitemap.</param>
        /// <exception cref="System.ArgumentNullException">
        /// rootUri
        /// or
        /// sitemapEntries
        /// </exception>
        public SitemapIndexFileGenerator(Uri rootUri, SitemapEntryCollection sitemapEntries, int maxEntriesPerSitemap = 50000, int maxSiteMapEntriesPerSitemapIndex = 1000)
        {
            if (rootUri == null)
            {
                throw new ArgumentNullException("rootUri");
            }

            if (sitemapEntries == null)
            {
                throw new ArgumentNullException("sitemapEntries");
            }

            m_RootUri = rootUri;
            m_SitemapEntries = sitemapEntries;
            m_MaxEntriesPerSitemap = maxEntriesPerSitemap;
            m_MaxSiteMapEntriesPerSitemapIndex = maxSiteMapEntriesPerSitemapIndex;
        }

        /// <summary>
        /// Generates the specified site map XML folder name.
        /// </summary>
        /// <param name="siteMapXmlFolderName">Name of the site map XML folder.</param>
        public void Generate(string siteMapXmlFolderName)
        {
            SitemapIndexBuilder sitemapIndexBuilder = new SitemapIndexBuilder();
            SitemapBuilder sitemapBuilder = new SitemapBuilder();
            int sitemapCounter = 0;
            int sitemapIndexCounter = 0;

            for (int i = 1; i <= m_SitemapEntries.Count; i++)
            {
                SitemapEntry sitemapEntry = m_SitemapEntries[i - 1];

                AddSitemapEntry(sitemapEntry, sitemapBuilder);

                if (i % m_MaxEntriesPerSitemap == 0)
                {
                    WriteSitemapFile(siteMapXmlFolderName, sitemapCounter, sitemapBuilder, sitemapIndexBuilder);
                    sitemapCounter++;

                    if (sitemapCounter % m_MaxSiteMapEntriesPerSitemapIndex == 0)
                    {
                        WriteSitemapIndexFile(siteMapXmlFolderName, sitemapIndexCounter, sitemapIndexBuilder);

                        sitemapIndexBuilder = new SitemapIndexBuilder();

                        sitemapIndexCounter++;
                    }

                    sitemapBuilder = new SitemapBuilder();
                }
            }

            if (!sitemapBuilder.IsEmpty)
            {
                WriteSitemapFile(siteMapXmlFolderName, sitemapCounter, sitemapBuilder, sitemapIndexBuilder);
            }

            if (!sitemapIndexBuilder.IsEmpty)
            {
                WriteSitemapIndexFile(siteMapXmlFolderName, sitemapIndexCounter, sitemapIndexBuilder);                
            }
        }

        /// <summary>
        /// Writes the sitemap index file.
        /// </summary>
        /// <param name="siteMapXmlFolderName">Name of the site map XML folder.</param>
        /// <param name="sitemapIndexCounter">The sitemap index counter.</param>
        /// <param name="sitemapIndexBuilder">The sitemap index builder.</param>
        private static void WriteSitemapIndexFile(string siteMapXmlFolderName, int sitemapIndexCounter, SitemapIndexBuilder sitemapIndexBuilder)
        {
            string sitemapIndexFileName = string.Format(CultureInfo.InvariantCulture, "sitemapindex{0}.xml", sitemapIndexCounter + 1);
            File.WriteAllText(Path.Combine(siteMapXmlFolderName, sitemapIndexFileName), sitemapIndexBuilder.GenerateXmlText());
        }

        /// <summary>
        /// Writes the sitemap file.
        /// </summary>
        /// <param name="siteMapXmlFolderName">Name of the site map XML folder.</param>
        /// <param name="sitemapCounter">The sitemap counter.</param>
        /// <param name="sitemapBuilder">The sitemap builder.</param>
        /// <param name="sitemapIndexBuilder">The sitemap index builder.</param>
        private void WriteSitemapFile(string siteMapXmlFolderName, int sitemapCounter, SitemapBuilder sitemapBuilder, SitemapIndexBuilder sitemapIndexBuilder)
        {
            string sitemapFileName = string.Format(CultureInfo.InvariantCulture, "sitemap{0}.xml.gz", sitemapCounter + 1);
            File.WriteAllBytes(Path.Combine(siteMapXmlFolderName, sitemapFileName), GzipContent(sitemapBuilder.GenerateXmlText()));

            sitemapIndexBuilder.AppendSitemapUrl(new Uri(m_RootUri, sitemapFileName).ToString(), DateTime.Now);
        }

        /// <summary>
        /// Adds the sitemap entry.
        /// </summary>
        /// <param name="sitemapEntry">The sitemap entry.</param>
        /// <param name="sitemapBuilder">The sitemap builder.</param>
        private static void AddSitemapEntry(SitemapEntry sitemapEntry, SitemapBuilder sitemapBuilder)
        {
            if (sitemapEntry.Date.HasValue && sitemapEntry.Priority > 0)
            {
                sitemapBuilder.AppendUrl(
                    sitemapEntry.Loc,
                    sitemapEntry.ChangeFrequency,
                    sitemapEntry.Priority,
                    sitemapEntry.Date.Value);
            }
            else if (sitemapEntry.Date.HasValue)
            {
                sitemapBuilder.AppendUrl(sitemapEntry.Loc, sitemapEntry.ChangeFrequency, 1, sitemapEntry.Date.Value);
            }
            else
            {
                sitemapBuilder.AppendUrl(sitemapEntry.Loc, sitemapEntry.ChangeFrequency, 1);
            }
        }

        /// <summary>
        /// Gzips the content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>Gzipped bytes.</returns>
        private static byte[] GzipContent(string content)
        {
            byte[] contentBytes = Encoding.UTF8.GetBytes(content);

            using (MemoryStream ms = new MemoryStream())
            {
                using (Stream stream = new GZipStream(ms, CompressionMode.Compress))
                {
                    stream.Write(contentBytes, 0, contentBytes.Length);
                    stream.Close();

                    return ms.ToArray();
                }
            }
        }
    }
}