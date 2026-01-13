using Explorer.Stakeholders.API.Dtos.Users;
using Explorer.Stakeholders.Core.UseCases.Administration.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator
{
    [Authorize(Policy = "administratorPolicy")]
    [Route("api/administration/users")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdministrationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<List<UserDto>> GetAllUsers()
        {
            return Ok(_userService.GetAll());
        }

        [HttpPost("create")]
        public ActionResult<UserDto> Create(CreateUserDto dto)
        {
            return Ok(_userService.Create(dto));
        }

        [HttpPost("{id}/block")]
        public ActionResult<UserDto> BlockUser(long id)
        {
            return Ok(_userService.Block(id));
        }

        [HttpPost("{id}/unblock")]
        public ActionResult<UserDto> UnblockUser(long id)
        {
            return Ok(_userService.Unblock(id));
        }
    }
}
