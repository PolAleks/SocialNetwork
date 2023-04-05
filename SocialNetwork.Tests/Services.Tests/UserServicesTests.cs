using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Tests.Services.Tests
{
    [TestClass]
    public class UserServicesTests
    {
        [TestMethod]
        public void AddFrientMustReturnArgumentNullException()
        {
            UserAddFriendData userAddFriendData = new UserAddFriendData()
            {
                EmailFutureFriend = "pochta",
                CurrentUserId = 1
            };

            UserService userService = new UserService();

            Assert.ThrowsException<ArgumentNullException>(() => userService.AddFriend(userAddFriendData));
            
        }
    }
}
