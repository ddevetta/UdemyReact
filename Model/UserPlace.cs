using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UdemyReact.Model
{
    [PrimaryKey(nameof(UserId), nameof(PlaceId))]
    public class UserPlace
    {
        public UserPlace() { }
        public UserPlace(string user, string id, Place place)
        {
            UserId = user;
            PlaceId = id;
            Place = place;
        }
        public string UserId { get; set; } = "AD";

        [ForeignKey("Place")]
        public string PlaceId { get; set; } = string.Empty;
        public Place? Place { get; set; }
    }
}