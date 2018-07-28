namespace PhotoShare.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Models;

    public class TownService : ITownService
    {
        private readonly PhotoShareContext context;

        public TownService(PhotoShareContext context)
        {
            this.context = context;
        }

        public Town Add(string townName, string countryName)
        {
            Town town = new Town
            {
                Name = townName,
                Country = countryName
            };

            this.context.Towns.Add(town);
            this.context.SaveChanges();

            return town;
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
            bool townExists = ById<Town>(id) != null;

            return townExists;
        }

        public bool Exists(string name)
        {
            bool townExists = ByName<Town>(name) != null;

            return townExists;
        }

        private IEnumerable<TModel> By<TModel>(Func<Town, bool> predicate)
        {
            var users = this.context.Towns.Where(predicate).AsQueryable().ProjectTo<TModel>();

            return users;
        }
    }
}
