namespace PhotoShare.Client.Core.Commands
{
    using System;
    using System.Linq;
    using Contracts;
    using Models.Enums;
    using Utilities;
    using Services.Contracts;
    using PhotoShare.Models;

    public class CreateAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ITagService tagService;

        public CreateAlbumCommand(IAlbumService albumService, IUserService userService, ITagService tagService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.tagService = tagService;
        }

        // CreateAlbum <username> <albumTitle> <BgColor> <tag1> <tag2>...<tagN>
        public string Execute(string[] data)
        {
            if (data.Length < 4)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            string username = data[0];
            string albumTitle = data[1];
            string bgcolor = data[2];
            string[] tags = data.Skip(3).ToArray();

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            if (this.albumService.Exists(albumTitle))
            {
                throw new ArgumentException($"Album {albumTitle} exists!");
            }

            bool validColor = Enum.TryParse(bgcolor, out Color result);

            if (!validColor)
            {
                throw new ArgumentException($"Color {bgcolor} not found");
            }

            for (int i = 0; i < tags.Length; i++)
            {
                tags[i] = tags[i].ValidateOrTransform();

                if (!this.tagService.Exists(tags[i]))
                {
                    throw new ArgumentException("Invalid tags!");
                }
            }

            int userId = this.userService.ByUsername<User>(username).Id;

            this.albumService.Create(userId, albumTitle, bgcolor, tags);

            return $"Album {albumTitle} successfully created!";
        }
    }
}
