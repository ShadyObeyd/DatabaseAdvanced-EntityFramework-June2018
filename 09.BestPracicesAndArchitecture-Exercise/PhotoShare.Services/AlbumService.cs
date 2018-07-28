namespace PhotoShare.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;
    using Models.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AlbumService : IAlbumService
    {
        private readonly PhotoShareContext context;

        public AlbumService(PhotoShareContext context)
        {
            this.context = context;
        }

        public TModel ById<TModel>(int id)
        {
            TModel model = By<TModel>(a => a.Id == id).SingleOrDefault();

            return model;
        }

        public TModel ByName<TModel>(string name)
        {
            TModel model = By<TModel>(a => a.Name == name).SingleOrDefault();

            return model;
        }

        public Album Create(int userId, string albumTitle, string bgColor, string[] tags)
        {
            Album album = new Album
            {
                Name = albumTitle,
                BackgroundColor = Enum.Parse<Color>(bgColor, true)
            };

            this.context.Albums.Add(album);
            this.context.SaveChanges();

            AlbumRole albumRole = new AlbumRole
            {
                UserId = userId,
                Album = album
            };

            this.context.AlbumRoles.Add(albumRole);
            this.context.SaveChanges();

            foreach (string tagName in tags)
            {
                int tagId = this.context.Tags.FirstOrDefault(t => t.Name == tagName).Id;

                AlbumTag albumTag = new AlbumTag
                {
                    TagId = tagId,
                    Album = album
                };

                this.context.AlbumTags.Add(albumTag);
            }

            this.context.SaveChanges();

            return album;
        }

        public bool Exists(int id)
        {
            bool albumExists = ById<Album>(id) != null;

            return albumExists;
        }

        public bool Exists(string name)
        {
            bool albumExists = ByName<Album>(name) != null;

            return albumExists;
        }

        private IEnumerable<TModel> By<TModel>(Func<Album, bool> predicate)
        {
            var albums = this.context.Albums.Where(predicate).AsQueryable().ProjectTo<TModel>();

            return albums;
        }
    }
}
