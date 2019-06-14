using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OgreMage.API.Filters
{
    public class EpisodeFilter : BaseFilter
    {
        public string Title { get; set; }
        public string Summary { get; set; }
    }
}
