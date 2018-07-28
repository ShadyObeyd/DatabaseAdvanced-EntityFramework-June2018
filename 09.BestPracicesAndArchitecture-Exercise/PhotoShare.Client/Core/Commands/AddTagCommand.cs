namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Utilities;
    using Services.Contracts;

    public class AddTagCommand : ICommand
    {
        private readonly ITagService tagService;

        public AddTagCommand(ITagService tagService)
        {
            this.tagService = tagService;
        }

        public string Execute(string[] args)
        {
            if (args.Length != 1)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            string tagName = args[0];

            if (this.tagService.Exists(tagName))
            {
                throw new ArgumentException($"Tag {tagName} exists!");
            }

            tagName = tagName.ValidateOrTransform();

            this.tagService.AddTag(tagName);

            return $"Tag {tagName} was added successfully!";
        }
    }
}
