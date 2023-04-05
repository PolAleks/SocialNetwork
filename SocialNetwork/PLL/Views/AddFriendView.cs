using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.PLL.Views
{
    public class AddFriendView
    {
        UserService userService;
        public AddFriendView(UserService userService)
        {
            this.userService = userService;
        }
        public void Show(User user)
        {
            UserAddFriendData newFriendData = new UserAddFriendData();

            Console.WriteLine("Введите почтовый адрес пользователя которого вы хотите добавить в друзья: ");
            newFriendData.EmailFutureFriend = Console.ReadLine();
            newFriendData.CurrentUserId = user.Id;

            try
            {
                userService.AddFriend(newFriendData);

                SuccessMessage.Show("Поздравляю! Вы не одинок, у вас появился новый друг!");
            }
            catch(ArgumentNullException)
            {
                AlertMessage.Show("Вы допустили ошибку в email адресе");
            }
            catch(UserNotFoundException)
            {
                AlertMessage.Show("Такой пользователь у нас не зарегистрирован!");
            }
            catch(AddedHimselfAsFriendException)
            {
                AlertMessage.Show("Попытка подружиться с самим собой, вызывает обеспокоиность за ваше психическое состояние!");
            }
            catch(AlreadyFriendsException)
            {
                AlertMessage.Show("Пользователь ранее уже был довбавлен к вам в друзья");
            }
            catch(Exception) 
            {
                AlertMessage.Show("Что-то пошло не так...");
            }
        }
    }
}
