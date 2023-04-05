using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.PLL.Views
{
    public class UserListFriendsView
    {
        public void Show(User user)
        {
            if (user.DearFriends.Count() == 0) 
            {
                Console.WriteLine("Увы, но у Вас нет друзей");
                return;
            }

            Console.WriteLine(new string('-',50));
            Console.WriteLine("Список ваших друзей:");

            foreach (var friend in user.DearFriends)
            {
                Console.WriteLine($"{friend.FirstName} {friend.LastName}");
            }

            Console.WriteLine(new string('-', 50));
        }
    }
}
