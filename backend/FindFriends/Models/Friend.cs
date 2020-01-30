using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FindFriends.Models
{
    public class Friend
    {
        [Key]
        public int FriendId { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public double LatitudeB { get; set; }
        public double LongitudeB { get; set; }
        
    }
}
