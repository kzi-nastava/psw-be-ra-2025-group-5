using Explorer.Encounters.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Encounters.API.Public.Author
{
    public interface IKeyPointChallengeCreationService
    {
        CreateAuthorChallengeDto CreateByAuthor(CreateAuthorChallengeDto dto, long authorId);
    }
}
