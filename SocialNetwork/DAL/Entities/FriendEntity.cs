using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.DAL.Entities
{
    public class FriendEntity
    {
        public int Id { get; set; }
        public int User_id { get; set; }
        public int Friend_id { get; set; }
    }
}
