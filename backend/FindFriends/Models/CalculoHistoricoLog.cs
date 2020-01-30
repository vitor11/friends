using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FindFriends.Models
{
    public class CalculoHistoricoLog
    {
        [Key]
        public int id { get; set; }
        public int FriendId { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public double LatitudeA { get; set; }
        public double LongitudeA { get; set; }
        public double LatitudeB { get; set; }
        public double LongitudeB { get; set; }
        public double Distance { get; set; }
    }
}
