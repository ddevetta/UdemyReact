using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyReact.Model
{
    public class Place
    {
        [ForeignKey("Image")]
        public string Id { get; set; }
        public string Title { get; set; }
        public Image Image { get; set; } = new Image();
        public float Lat { get; set; }
        public float Lon { get; set; }

    }
}