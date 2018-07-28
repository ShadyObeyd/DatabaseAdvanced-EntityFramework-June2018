namespace PhotoShare.Services
{
    using Contracts;
    using Models;
    using Data;

    public class AlbumTagService : IAlbumTagService
    {
        private readonly PhotoShareContext context;

        public AlbumTagService(PhotoShareContext context)
        {
            this.context = context;
        }

        public AlbumTag AddTagTo(int albumId, int tagId)
        {
            AlbumTag albumTag = new AlbumTag
            {
                AlbumId = albumId,
                TagId = tagId
            };

            this.context.AlbumTags.Add(albumTag);
            this.context.SaveChanges();

            return albumTag;
        }
    }
}
