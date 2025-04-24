using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyReact.Model
{
    public class Image
    {
        [Key]
        public string Id { get; set; }
        public string Src { get; set; }
        public string? Alt { get; set; }
    }
}