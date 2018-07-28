namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;

    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class AcceptFriendCommand : ICommand
    {
        private readonly IUserService userService;

        public AcceptFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AcceptFriend <username1> <username2>
        public string Execute(string[] data)
        {
            if (data.Length != 2)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            string username = data[0];
            string friendUsername = data[1];

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException($"User{username} not found!");
            }

            if (!this.userService.Exists(friendUsername))
            {
                throw new ArgumentException($"User {friendUsername} not found!");
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username);
            UserFriendsDto friend = this.userService.ByUsername<UserFriendsDto>(friendUsername);

            bool isSentRequestFromUser = user.Friends.Any(f => f.Username == friend.Username);
            bool isSentRequestFromFriend = friend.Friends.Any(f => f.Username == user.Username);

            if (isSentRequestFromFriend && isSentRequestFromUser)
            {
                throw new InvalidOperationException($"{friendUsername} is already a friend to {username}");
            }

            if (!isSentRequestFromFriend)
            {
                throw new InvalidOperationException($"{friendUsername} has not added {username} as a friend");
            }

            this.userService.AcceptFriend(user.Id, friend.Id);

            return $"{username} accepted {friendUsername} as a friend";
        }
    }
}
