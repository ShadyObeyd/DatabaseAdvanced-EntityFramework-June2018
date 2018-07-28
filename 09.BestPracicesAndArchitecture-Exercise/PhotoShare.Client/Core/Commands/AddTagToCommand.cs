namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Services.Contracts;
    using Utilities;

    public class AddTagToCommand : ICommand
    {
        private readonly ITagService tagService;
        private readonly IAlbumService albumService;

        public AddTagToCommand(ITagService tagService, IAlbumService albumService)
        {
            this.tagService = tagService;
            this.albumService = albumService;
        }

        // AddTagTo <albumName> <tag>
        public string Execute(string[] args)
        {
            if (args.Length != 2)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            string albumName = args[0];
            string tagName = args[1].ValidateOrTransform();

            if (!this.albumService.Exists(albumName) || !this.tagService.Exists(tagName))
            {
                throw new ArgumentException("Either tag or album do not exist!");
            }

            return $"Tag {tagName} added to {albumName}!";
        }
    }
}
