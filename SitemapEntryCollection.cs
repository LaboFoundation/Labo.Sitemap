namespace Labo.Sitemap
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The sitemap entry collection class.
    /// </summary>
    [Serializable]
    public sealed class SitemapEntryCollection : List<SitemapEntry>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapEntryCollection"/> class.
        /// </summary>
        public SitemapEntryCollection()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapEntryCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public SitemapEntryCollection(IEnumerable<SitemapEntry> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SitemapEntryCollection"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public SitemapEntryCollection(int capacity)
            : base(capacity)
        {
        }
    }
}