namespace PhotoShare.Client.Core.Commands
{
    using System;

    using Contracts;
    using Models.Enums;
    using Dtos;
    using Services.Contracts;

    public class ShareAlbumCommand : ICommand
    {
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly IAlbumRoleService albumRoleService;

        public ShareAlbumCommand(IAlbumService albumService, IUserService userService, IAlbumRoleService albumRoleService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.albumRoleService = albumRoleService;
        }

        // ShareAlbum <albumId> <username> <permission>
        // For example:
        // ShareAlbum 4 dragon321 Owner
        // ShareAlbum 4 dragon11 Viewer
        public string Execute(string[] data)
        {
            if (data.Length != 3)
            {
                int commandNameLenght = this.GetType().Name.Length - "Command".Length;
                string commandName = this.GetType().Name.Substring(0, commandNameLenght);

                throw new ArgumentException($"Command {commandName} not valid");
            }

            int albumId = int.Parse(data[0]);
            string username = data[1];
            string permission = data[2];

            if (!this.albumService.Exists(albumId))
            {
                throw new ArgumentException($"Album {albumId} not found!");
            }

            if (!this.userService.Exists(username))
            {
                throw new ArgumentException($"User {username} not found!");
            }

            bool validPermission = Enum.TryParse(permission, out Color result);

            if (!validPermission)
            {
                throw new ArgumentException("Permission must be either \"Owner\" or \"Viewer\"!");
            }

            int userId = this.userService.ByUsername<UserDto>(username).Id;

            string albumName = this.albumService.ById<AlbumDto>(albumId).Name;

            this.albumRoleService.PublishAlbumRole(albumId, userId, permission);

            return $"Username {username} added to album {albumName} ({permission})";
        }
    }
}
