using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.models
{
    public class t_Rooms
    {
        [Key]
        public int c_RoomId{get;set;}

        public string c_RoomName{get; set;}
    }
}