using AutoMapper;
using MongoDB.Driver;
using ProjectTravelin.Dtos.CommentDtos;
using ProjectTravelin.Entities;
using ProjectTravelin.Settings;

namespace ProjectTravelin.Services.CommentServices
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Comment> _commentCollection;

        public CommentService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _commentCollection = database.GetCollection<Comment>(
                databaseSettings.CommentCollectionName
            );

            _mapper = mapper;
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            var values = await _commentCollection
                .Find(x => true)
                .SortByDescending(x => x.CommentDate)
                .ToListAsync();

            return _mapper.Map<List<ResultCommentDto>>(values);
        }

        public async Task<List<ResultCommentDto>> GetCommentsByTourIdAsync(string tourId)
        {
            var values = await _commentCollection
                .Find(x => x.TourId == tourId)
                .SortByDescending(x => x.CommentDate)
                .ToListAsync();

            return _mapper.Map<List<ResultCommentDto>>(values);
        }

        public async Task<List<ResultCommentDto>> GetApprovedCommentsByTourIdAsync(string tourId)
        {
            var values = await _commentCollection
                .Find(x => x.TourId == tourId && x.IsStatus == true)
                .SortByDescending(x => x.CommentDate)
                .ToListAsync();

            return _mapper.Map<List<ResultCommentDto>>(values);
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            var value = _mapper.Map<Comment>(createCommentDto);

            value.Headline = value.Headline ?? "";
            value.CommentDetail = value.CommentDetail ?? "";
            value.TourId = value.TourId ?? "";

            if (value.CommentDate == default)
            {
                value.CommentDate = DateTime.Now;
            }

            if (value.Score < 1)
            {
                value.Score = 1;
            }

            if (value.Score > 5)
            {
                value.Score = 5;
            }

            value.IsStatus = false;

            await _commentCollection.InsertOneAsync(value);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _commentCollection.DeleteOneAsync(x => x.CommentId == id);
        }

        public async Task ChangeCommentStatusAsync(string id, bool status)
        {
            var filter = Builders<Comment>.Filter.Eq(x => x.CommentId, id);

            var update = Builders<Comment>.Update
                .Set(x => x.IsStatus, status);

            await _commentCollection.UpdateOneAsync(filter, update);
        }

        public async Task<GetCommentByIdDto> GetCommentByIdAsync(string id)
        {
            var value = await _commentCollection
                .Find(x => x.CommentId == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<GetCommentByIdDto>(value);
        }
    }
}