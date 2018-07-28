namespace PhotoShare.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;
    using Models;
    using Data;
    using AutoMapper.QueryableExtensions;

    public class UserService : IUserService
    {
        private readonly PhotoShareContext context;

        public UserService(PhotoShareContext context)
        {
            this.context = context;
        }

        public Friendship AcceptFriend(int userId, int friendId)
        {
            Friendship friendship = new Friendship
            {
                UserId = userId,
                FriendId = friendId
            };

            this.context.Friendships.Add(friendship);
            this.context.SaveChanges();

            return friendship;
        }

        public Friendship AddFriend(int userId, int friendId)
        {
            Friendship friendship = new Friendship
            {
                UserId = userId,
                FriendId = friendId
            };

            this.context.Friendships.Add(friendship);
            this.context.SaveChanges();

            return friendship;
        }

        public TModel ById<TModel>(int id)
        {
            TModel model = By<TModel>(u => u.Id == id).SingleOrDefault();

            return model;
        }

        public TModel ByUsername<TModel>(string username)
        {
            TModel model = By<TModel>(u => u.Username == username).SingleOrDefault();

            return model;
        }

        public void ChangePassword(int userId, string password)
        {
            User user = this.context.Users.FirstOrDefault(u => u.Id == userId);

            user.Password = password;

            this.context.SaveChanges();
        }

        public void Delete(string username)
        {
            User user = this.context.Users.FirstOrDefault(u => u.Username == username);

            user.IsDeleted = true;

            this.context.SaveChanges();
        }

        public bool Exists(int id)
        {
            bool userExists = ById<User>(id) != null;

            return userExists;
        }

        public bool Exists(string name)
        {
            bool userExists = ByUsername<User>(name) != null;

            return userExists;
        }

        public User Register(string username, string password, string email)
        {
            User user = new User
            {
                Username = username,
                Password = password,
                Email = email,
                IsDeleted = false
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();

            return user;
        }

        public void SetBornTown(int userId, int townId)
        {
            User user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            Town town = this.context.Towns.FirstOrDefault(t => t.Id == townId);

            user.BornTown = town;
            this.context.SaveChanges();
        }

        public void SetCurrentTown(int userId, int townId)
        {
            User user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            Town town = this.context.Towns.FirstOrDefault(t => t.Id == townId);

            user.CurrentTown = town;
            this.context.SaveChanges();
        }

        private IEnumerable<TModel> By<TModel>(Func<User, bool> predicate)
        {
            var users = this.context.Users.Where(predicate).AsQueryable().ProjectTo<TModel>();

            return users;
        }
    }
}