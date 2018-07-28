namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Contracts;
    using Services.Contracts;
    using Dtos;

    public class RegisterUserCommand : ICommand
    {
        private readonly IUserService userService;

        public RegisterUserCommand(IUserService userService)
        {
            this.userService = userService;
        }

        // RegisterUser <username> <password> <repeat-password> <email>
        public string Execute(string[] data)
        {
            if (data.Length != 4)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            string username = data[0];
            string password = data[1];
            string reapeatPassword = data[2];
            string email = data[3];

            if (this.userService.Exists(username))
            {
                throw new InvalidOperationException(string.Format(OutputMessages.UsernameTakenMessage, username));
            }

            if (password != reapeatPassword)
            {
                throw new ArgumentException(OutputMessages.PasswordDoNotMatchMessage);
            }

            RegisterUserDto registerUserDto = new RegisterUserDto()
            {
                Username = username,
                Password = password,
                Email = email
            };

            if (!this.IsValid(registerUserDto))
            {
                throw new ArgumentException(OutputMessages.InvalidInputDataMessage);
            }

            this.userService.Register(username, password, email);

            return string.Format(OutputMessages.RegisterSuccessMessage, username);
        }

        private bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
