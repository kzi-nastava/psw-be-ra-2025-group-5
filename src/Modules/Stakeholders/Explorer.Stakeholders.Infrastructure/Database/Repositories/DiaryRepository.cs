using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class DiaryRepository : IDiaryRepository
{
    private readonly StakeholdersContext _context;

    public DiaryRepository(StakeholdersContext context)
    {
        _context = context;
    }

    public PagedResult<Diary> GetByTourist(long touristId, int page, int pageSize)
    {
        var query = _context.Diaries
            .Where(d => d.TouristId == touristId)
            .OrderByDescending(d => d.CreatedAt);

        var totalCount = query.Count();

        var items = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResult<Diary>(items, totalCount);
    }

    public Diary? GetById(long id)
    {
        return _context.Diaries.FirstOrDefault(d => d.Id == id);
    }

    public Diary Create(Diary diary)
    {
        _context.Diaries.Add(diary);
        _context.SaveChanges();
        return diary;
    }

    public Diary Update(Diary diary)
    {
        var trackedEntity = _context.Diaries.Local.FirstOrDefault(e => e.Id == diary.Id);
        if (trackedEntity != null)
        {
            _context.Entry(trackedEntity).State = EntityState.Detached;
        }

        _context.Diaries.Update(diary);
        _context.SaveChanges();
        return diary;
    }

    public void Delete(long id)
    {
        var diary = _context.Diaries.FirstOrDefault(d => d.Id == id);
        if (diary == null) return;

        _context.Diaries.Remove(diary);
        _context.SaveChanges();
    }
}

