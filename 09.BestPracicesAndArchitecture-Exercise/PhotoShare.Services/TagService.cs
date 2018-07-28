namespace PhotoShare.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;

    public class TagService : ITagService
    {
        private readonly PhotoShareContext context;

        public TagService(PhotoShareContext context)
        {
            this.context = context;
        }

        public Tag AddTag(string name)
        {
            Tag tag = new Tag()
            {
                Name = name
            };

            this.context.Tags.Add(tag);
            this.context.SaveChanges();

            return tag;
        }

        public TModel ById<TModel>(int id)
        {
            TModel model = By<TModel>(t => t.Id == id).SingleOrDefault();

            return model;
        }

        public TModel ByName<TModel>(string name)
        {
            TModel model = By<TModel>(t => t.Name == name).SingleOrDefault();

            return model;
        }

        public bool Exists(int id)
        {
            bool tagExists = ById<Tag>(id) != null;

            return tagExists;
        }

        public bool Exists(string name)
        {
            bool tagExists = ByName<Tag>(name) != null;

            return tagExists;
        }

        private IEnumerable<TModel> By<TModel>(Func<Tag, bool> predicate)
        {
            var tags = this.context.Tags.Where(predicate).AsQueryable().ProjectTo<TModel>();

            return tags;
        }
    }
}
