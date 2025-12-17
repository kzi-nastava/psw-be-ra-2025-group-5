using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.Exceptions;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;

using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Core.UseCases;

public class DiaryService : IDiaryService
{
    private readonly IDiaryRepository _diaryRepository;
    private readonly IMapper _mapper;

    public DiaryService(IDiaryRepository diaryRepository, IMapper mapper)
    {
        _diaryRepository = diaryRepository;
        _mapper = mapper;
    }

    public PagedResult<DiaryDto> GetByTourist(long touristId, int page, int pageSize)
    {
        var diaries = _diaryRepository.GetByTourist(touristId, page, pageSize);
        var mapped = diaries.Results.Select(_mapper.Map<DiaryDto>).ToList();
        return new PagedResult<DiaryDto>(mapped, diaries.TotalCount);
    }

    public DiaryDto Create(DiaryDto dto)
    {
        var entity = _mapper.Map<Diary>(dto);
        var created = _diaryRepository.Create(entity);
        return _mapper.Map<DiaryDto>(created);
    }

    public DiaryDto Update(DiaryDto dto)
    {
        var existingDiary = _diaryRepository.GetById(dto.Id);
        if (existingDiary == null)
        {
            throw new NotFoundException($"Diary with ID {dto.Id} not found.");
        }

        var entity = _mapper.Map<Diary>(dto);
        typeof(Entity).GetProperty("Id")?.SetValue(entity, dto.Id);
        var updated = _diaryRepository.Update(entity);
        return _mapper.Map<DiaryDto>(updated);
    }

    public void Delete(long id)
    {
        _diaryRepository.Delete(id);
    }

}

