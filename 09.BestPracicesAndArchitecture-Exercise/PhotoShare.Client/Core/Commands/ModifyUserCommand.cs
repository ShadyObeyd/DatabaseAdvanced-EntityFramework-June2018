namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using PhotoShare.Models;
    using Services.Contracts;

    public class ModifyUserCommand : ICommand
    {
        private readonly IUserService userService;
        private readonly ITownService townService;

        public ModifyUserCommand(IUserService userService, ITownService townService)
        {
            this.userService = userService;
            this.townService = townService;
        }

        // ModifyUser <username> <property> <new value>
        // For example:
        // ModifyUser <username> Password <NewPassword>
        // ModifyUser <username> BornTown <newBornTownName>
        // ModifyUser <username> CurrentTown <newCurrentTownName>
        // !!! Cannot change username
        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            string username = data[0];
            string property = data[1];
            string newValue = data[2];

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException(string.Format(OutputMessages.UserDoesNotExistMessage, username));
            }

            int userId = this.userService.ByUsername<User>(username).Id;

            if (property == "Password")
            {
                SetPassword(userId, newValue);
            }
            else if (property == "BornTown")
            {
                SetBornTown(userId, newValue);
            }
            else if (property == "CurrentTown")
            {
                SetCurrentTown(userId, newValue);
            }
            else
            {
                throw new ArgumentException(string.Format(OutputMessages.PropertyNotSupportedMessage, property));
            }

            return string.Format(OutputMessages.UserModifiedSuccessfullyMessage, username, property, newValue);
        }

        private void SetPassword(int userId, string newPassword)
        {
            bool passwordIsValid = newPassword.Any(c => char.IsLower(c) && newPassword.Any(x => char.IsDigit(x)));

            if (!passwordIsValid)
            {
                throw new ArgumentException(string.Format(OutputMessages.InvalidPasswordMessage, newPassword));
            }

            this.userService.ChangePassword(userId, newPassword);
        }

        private void SetBornTown(int userId, string bornTown)
        {
            if (!this.townService.Exists(bornTown))
            {
                throw new ArgumentException(string.Format(OutputMessages.TownDoesNotExistMessage, bornTown, bornTown));
            }

            int townId = this.townService.ByName<Town>(bornTown).Id;

            this.userService.SetBornTown(userId, townId);
        }

        private void SetCurrentTown(int userId, string newCurrentTown)
        {
            if (!this.townService.Exists(newCurrentTown))
            {
                throw new ArgumentException(string.Format(OutputMessages.TownDoesNotExistMessage, newCurrentTown, newCurrentTown));
            }

            int townId = this.townService.ByName<Town>(newCurrentTown).Id;

            this.userService.SetCurrentTown(userId, townId);
        }
    }
}
