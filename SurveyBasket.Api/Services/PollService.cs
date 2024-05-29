﻿using SurveyBasket.Api.Entities;

namespace SurveyBasket.Api.Services
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

        public async Task<Poll?> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Polls.FindAsync(id, cancellationToken);
        }

        public async Task<Poll> AddAsync(Poll poll, CancellationToken cancellationToken = default)
        {
            await _context.AddAsync(poll, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return poll;
        }

        public async Task<bool> UpdateAsync(int id, Poll poll, CancellationToken cancellationToken = default)
        {
            var currentPoll = await GetAsync(id, cancellationToken);

            if (currentPoll is null)
                return false;

            currentPoll.Title = poll.Title;
            currentPoll.Summary = poll.Summary;
            currentPoll.StartsAt = poll.StartsAt;
            currentPoll.EndsAt = poll.EndsAt;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task <bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var poll = await GetAsync(id, cancellationToken);

            if (poll is null)
                return false;

            _context.Remove(poll);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> TogglePublishStatusAsync(int id, CancellationToken cancellationToken = default)
        {
            var Poll = await GetAsync(id, cancellationToken);

            if (Poll is null)
                return false;

            Poll.IsPublished = !Poll.IsPublished;

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

