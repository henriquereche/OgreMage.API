using System.ComponentModel.DataAnnotations;

namespace OgreMage.API.Models
{
    public class ProcessRSSFeed
    {
        [Required]
        public string RSSFeed { get; set; }
    }
}
