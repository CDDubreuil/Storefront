using System;
using System.Collections.Generic;

namespace Storefront.DATA.EF.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Artists = new HashSet<Artist>();
        }

        public string GenreName { get; set; } = null!;
        public string GenreDescription { get; set; } = null!;
        public int GenreId { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }
    }
}
