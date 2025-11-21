using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.Core.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Administrator.Administration
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
    }
}
