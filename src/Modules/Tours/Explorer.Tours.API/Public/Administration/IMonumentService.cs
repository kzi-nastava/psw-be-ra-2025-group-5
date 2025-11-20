using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface IMonumentService
    {
        public PagedResult<MonumentDto> GetPaged(int  page, int pageSize);
        public MonumentDto Create(MonumentDto monumentDto);
        public MonumentDto Update(MonumentDto monumentDto);
        public void Delete(long id);
    }
}
