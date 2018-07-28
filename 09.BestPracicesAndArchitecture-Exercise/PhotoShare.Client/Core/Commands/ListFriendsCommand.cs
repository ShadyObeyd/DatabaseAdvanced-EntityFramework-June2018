namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using System.Text;
    using Contracts;
    using Dtos;
    using Services.Contracts;

    public class ListFriendsCommand : ICommand
    {
        private readonly IUserService userService;

        public ListFriendsCommand(IUserService userService)
        {
            this.userService = userService;
        }

        public string Execute(string[] args)
        {
            if (args.Length != 1)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            string username = args[0];

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            UserFriendsDto user = this.userService.ByUsername<UserFriendsDto>(username);

            if (!user.Friends.Any())
            {
                return "No friends for this user. :(";
            }

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Friends:");

            foreach (FriendDto friend in user.Friends)
            {
                sb.AppendLine($"-{friend.Username}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
