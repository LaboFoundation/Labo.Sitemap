namespace Labo.Sitemap
{
    using System;

    /// <summary>
    /// The sitemap entry class.
    /// </summary>
    public sealed class SitemapEntry
    {
        /// <summary>
        /// Gets or sets the URL loc.
        /// </summary>
        /// <value>
        /// The URL loc.
        /// </value>
        public string Loc { get; set; }

        /// <summary>
        /// Gets or sets the page last modified date.
        /// </summary>
        /// <value>
        /// The page last modified date.
        /// </value>
        public DateTime? Date { get; set; }

        /// <summary>
        /// Gets or sets the change frequency.
        /// </summary>
        /// <value>
        /// The change frequency.
        /// </value>
        public SitemapBuilder.ChangeFrequency ChangeFrequency { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public double Priority { get; set; }
    }
}