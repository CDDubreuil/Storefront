using System;
using System.Collections.Generic;

namespace Storefront.DATA.EF.Models
{
    public partial class Artist
    {
        public Artist()
        {
            Records = new HashSet<Record>();
        }

        public string ArtistName { get; set; } = null!;
        public int ArtistId { get; set; }
        public string City { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public string? YearsActive { get; set; }
        public int? NumberOfAlbums { get; set; }
        public string? Members { get; set; }
        public int GenreId { get; set; }
        public string? ArtistImage { get; set; }

        public virtual Genre? Genre { get; set; } = null!;
        public virtual ICollection<Record> Records { get; set; }
    }
}
