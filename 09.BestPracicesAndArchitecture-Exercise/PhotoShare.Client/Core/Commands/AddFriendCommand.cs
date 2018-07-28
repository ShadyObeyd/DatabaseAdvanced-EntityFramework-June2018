namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Services.Contracts;
    using Dtos;
    using System.Linq;

    public class AddFriendCommand : ICommand
    {
        private readonly IUserService userService;

        public AddFriendCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // AddFriend <username1> <username2>
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
                throw new ArgumentException($"User {username} not found!");
            }

            if (!this.userService.Exists(friendUsername))
            {
                throw new ArgumentException($"User {friendUsername} not found!");
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username);
            UserFriendsDto friend = this.userService.ByUsername<UserFriendsDto>(friendUsername);

            bool isSentRequestFromUser = user.Friends.Any(f => f.Username == friend.Username);
            bool isSentRequestFromFriend = friend.Friends.Any(f => f.Username == user.Username);

            if (isSentRequestFromUser && isSentRequestFromFriend)
            {
                throw new InvalidOperationException($"{friendUsername} is already a friend to {username}");
            }
            else if (isSentRequestFromUser && !isSentRequestFromFriend)
            {
                throw new InvalidOperationException("Request is already sent!");
            }
            else if (!isSentRequestFromUser && isSentRequestFromFriend)
            {
                throw new InvalidOperationException("Request is already sent!");
            }

            this.userService.AddFriend(user.Id, friend.Id);

            return $"Friend {friendUsername} added to {username}";
        }
    }
}
