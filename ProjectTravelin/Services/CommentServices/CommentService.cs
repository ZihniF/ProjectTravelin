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
        public CommentService(IMapper mapper,IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _commentCollection = database.GetCollection<Comment>(_databaseSettings.CommentCollectionName);
            _mapper = mapper;
        }

        public async Task CreateCommentAsync(CreateCommentDto createCommentDto)
        {
            var values =_mapper.Map<Comment>(createCommentDto);
            await _commentCollection.InsertOneAsync(values);
        }

        public async Task DeleteCommentAsync(string id)
        {
            await _commentCollection.DeleteOneAsync(x => x.CommentId == id);
        }

        public async Task<List<ResultCommentDto>> GetAllCommentAsync()
        {
            var values=await _commentCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultCommentDto>>(values);

        }

        public async Task<GetCommentByIdDto> GetCommentByIdAsync(string id)
        {
            var value=await _commentCollection.Find(x => x.CommentId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetCommentByIdDto>(value);
        }

        public async Task UpdateCommentAsync(UpdateCommentDto updateCommentDto)
        {
            var values = _mapper.Map<Comment>(updateCommentDto);
            await _commentCollection.FindOneAndReplaceAsync(x => x.CommentId == updateCommentDto.CommentId, values);
        }
    }
}
