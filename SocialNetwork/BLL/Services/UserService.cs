using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.BLL.Services
{
    public class UserService
    {
        MessageService messageService;
        IUserRepository userRepository;
        IFriendRepository friendRepository; // Репозиторий с друзьями
        public UserService()
        {
            userRepository = new UserRepository();
            messageService = new MessageService();
            friendRepository = new FriendRepository();
        }

        public void Register(UserRegistrationData userRegistrationData)
        {
            if (String.IsNullOrEmpty(userRegistrationData.FirstName))
                throw new ArgumentNullException();

            if (String.IsNullOrEmpty(userRegistrationData.LastName))
                throw new ArgumentNullException();

            if (String.IsNullOrEmpty(userRegistrationData.Password))
                throw new ArgumentNullException();

            if (String.IsNullOrEmpty(userRegistrationData.Email))
                throw new ArgumentNullException();

            if (userRegistrationData.Password.Length < 8)
                throw new ArgumentNullException();

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new ArgumentNullException();

            if (userRepository.FindByEmail(userRegistrationData.Email) != null)
                throw new ArgumentNullException();

            var userEntity = new UserEntity()
            {
                Firstname = userRegistrationData.FirstName,
                Lastname = userRegistrationData.LastName,
                Password = userRegistrationData.Password,
                Email = userRegistrationData.Email
            };

            if (this.userRepository.Create(userEntity) == 0)
                throw new Exception();

        }

        public User Authenticate(UserAuthenticationData userAuthenticationData)
        {
            var findUserEntity = userRepository.FindByEmail(userAuthenticationData.Email);
            if (findUserEntity is null) throw new UserNotFoundException();

            if (findUserEntity.Password != userAuthenticationData.Password)
                throw new WrongPasswordException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindByEmail(string email)
        {
            var findUserEntity = userRepository.FindByEmail(email);
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public User FindById(int id)
        {
            var findUserEntity = userRepository.FindById(id);
            if (findUserEntity is null) throw new UserNotFoundException();

            return ConstructUserModel(findUserEntity);
        }

        public void Update(User user)
        {
            var updatableUserEntity = new UserEntity()
            {
                Id = user.Id,
                Firstname = user.FirstName,
                Lastname = user.LastName,
                Password = user.Password,
                Email = user.Email,
                Photo = user.Photo,
                Favorite_movie = user.FavoriteMovie,
                Favorite_book = user.FavoriteBook
            };

            if (this.userRepository.Update(updatableUserEntity) == 0)
                throw new Exception();
        }
        /// <summary>
        /// Метод запроса в друзья по email адресу 
        /// </summary>
        /// <param name="userFriendData">Объект содержащий Id текущего пользователя и email потенциального друга</param>
        /// <exception cref="ArgumentNullException">Ошибка ввода email адреса</exception>
        /// <exception cref="UserNotFoundException">Пользователь не найден в социалной сети</exception>
        /// <exception cref="AddedHimselfAsFriendException">Попытка подружится с самим собой</exception>
        /// <exception cref="AlreadyFriendsException">Попытка добавления в друзья, пользователя из списка твоих друзей</exception>
        /// <exception cref="Exception">Ошибка добавления записи о новом друге в БД</exception>
        public void AddFriend(UserAddFriendData userFriendData)
        {
            if (!new EmailAddressAttribute().IsValid(userFriendData.EmailFutureFriend))
                throw new ArgumentNullException();

            UserEntity findNewFriend = userRepository.FindByEmail(userFriendData.EmailFutureFriend);
            if (findNewFriend is null) throw new UserNotFoundException(); 

            if(userFriendData.CurrentUserId == findNewFriend.Id)
                throw new AddedHimselfAsFriendException();

            if(friendRepository.FindAllByUserId(userFriendData.CurrentUserId).Any(friend => friend.Friend_id == findNewFriend.Id)) 
                throw new AlreadyFriendsException();

            FriendEntity friendEntity = new FriendEntity()
            {
                User_id = userFriendData.CurrentUserId,
                Friend_id = findNewFriend.Id
            };

            if(friendRepository.Create(friendEntity) == 0) throw new Exception();

        }
        /// <summary>
        /// Метод возвращает список друзей по Id пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        /// <returns></returns>
        public IEnumerable<User> GetFriendsByUserId(int userId)
        {
            return friendRepository.FindAllByUserId(userId).Select(friend => FindById(friend.Friend_id));

        }

        private User ConstructUserModel(UserEntity userEntity)
        {
            var incomingMessages = messageService.GetIncomingMessagesByUserId(userEntity.Id);

            var outgoingMessages = messageService.GetOutcomingMessagesByUserId(userEntity.Id);

            IEnumerable<User> friends = GetFriendsByUserId(userEntity.Id);
            
            return new User(userEntity.Id,
                          userEntity.Firstname,
                          userEntity.Lastname,
                          userEntity.Password,
                          userEntity.Email,
                          userEntity.Photo,
                          userEntity.Favorite_movie,
                          userEntity.Favorite_book,
                          incomingMessages,
                          outgoingMessages,
                          friends
                          );
        }
    }
}
