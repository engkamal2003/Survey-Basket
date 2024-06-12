//namespace SurveyBasket.Api.Services
//{
//    public class PollService : IPollService
//    {
//        private readonly ApplicationDbContext _context;

//        public PollService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default)
//        {
//            return await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
//        }

//        public async Task<Poll?> GetAsync(int id, CancellationToken cancellationToken = default)
//        {
//            return await _context.Polls.FindAsync(id, cancellationToken);
//        }

//        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
//        {
//            await _context.AddAsync(poll, cancellationToken);
//            await _context.SaveChangesAsync(cancellationToken);
//            return poll;
//        }

//        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
//        {
//            var currentPoll = await GetAsync(id, cancellationToken);

//            if (currentPoll is null)
//                return false;

//            currentPoll.Title = poll.Title;
//            currentPoll.Summary = poll.Summary;
//            currentPoll.StartsAt = poll.StartsAt;
//            currentPoll.EndsAt = poll.EndsAt;

//            await _context.SaveChangesAsync(cancellationToken);

//            return true;
//        }

//        public async Task <bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
//        {
//            var poll = await GetAsync(id, cancellationToken);

//            if (poll is null)
//                return false;

//            _context.Remove(poll);
//            await _context.SaveChangesAsync(cancellationToken);

//            return true;
//        }

//        public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
//        {
//            var Poll = await GetAsync(id, cancellationToken);

//            if (Poll is null)
//                return false;

//            Poll.IsPublished = !Poll.IsPublished;

//            await _context.SaveChangesAsync(cancellationToken);

//            return true;
//        }
//    }
//}







using SurveyBasket.Api.Errors;

namespace SurveyBasket.Services
{
    public class PollService : IPollService
    {
        private readonly ApplicationDbContext _context;

        public PollService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Poll>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Polls.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<Result<PollResponse>> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(new object[] { id }, cancellationToken);

            if (poll != null)
            {
                return Result.Success(poll.Adapt<PollResponse>());
            }
            else
            {
                return Result.Failure<PollResponse>(PollErrors.PollNotFound);
            }
        }

        public async Task<PollResponse> AddAsync(PollRequest request, CancellationToken cancellationToken = default)
        {
            var poll = request.Adapt<Poll>();

            await _context.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return poll.Adapt<PollResponse>();
        }

        public async Task<Result> UpdateAsync(int id, PollRequest poll, CancellationToken cancellationToken = default)
        {
            var currentPoll = await _context.Polls.FindAsync(new object[] { id }, cancellationToken);

            if (currentPoll == null)
            {
                return Result.Failure(PollErrors.PollNotFound);
            }

            currentPoll.Title = poll.Title;
            currentPoll.Summary = poll.Summary;
            currentPoll.StartsAt = poll.StartsAt;
            currentPoll.EndsAt = poll.EndsAt;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(new object[] { id }, cancellationToken);

            if (poll == null)
            {
                return Result.Failure(PollErrors.PollNotFound);
            }

            _context.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await _context.Polls.FindAsync(new object[] { id }, cancellationToken);

            if (poll == null)
            {
                return Result.Failure(PollErrors.PollNotFound);
            }

            poll.IsPublished = !poll.IsPublished;
            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
