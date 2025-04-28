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

    /// <summary>
    /// This class is used as the TResult of the join between UserPlace and Place, in the GET operations
    /// </summary>
    public class UserPlaces
    {
        public string UserId { get; set; }
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; }
        public Image Image { get; set; } = new Image();
        public float Lat { get; set; }
        public float Lon { get; set; }
    }
}