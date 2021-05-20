using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Context;
using Users.Models;
using Users.Repositories;

namespace Users.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserDbContext _dbContext;

        public UserController(IUserRepository userRepository, UserDbContext dbContext)
        {
            _userRepository = userRepository;
            _dbContext = dbContext;
        }


        // POST: apiUsers/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserModel>> CreateNewUserAsync(UserRegisterModel userRegister)
        {
            if (ModelState.IsValid)
            {
                if (userRegister != null)
                {
                    try
                    {
                        var checkExisting = await _userRepository.CheckIfEmailIsRegisteredAsync(userRegister.Email);
                        if (checkExisting == true)
                            return Conflict();

                        var newUser = await _userRepository.CreateUserAsync(userRegister);
                        if (newUser != null)
                            return Ok(newUser);
                    }
                    catch (Exception)
                    {
                        return BadRequest();
                    }
                }
            }

            return BadRequest(ModelState);
        }

        // GET: api/Users/all
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsersAsync()
        {
            var result = await _userRepository.GetAllUsersAsync();
            return Ok(result);
        }

        // GET: api/Users/5
        [Authorize]
        [HttpGet("getuserbyid/{id}")]
        public async Task<ActionResult<UserModel>>GetUserByIdAsync(Guid id)
        {
            var userExistInDb = IsUserExists(id);

            if (!userExistInDb)
                NotFound();

            var result = await _userRepository.GetUserByIdAsync(id);
            if(result != null)
           
                return Ok(result);
            return BadRequest();
        }

        // PUT: api/Users/5
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(Guid id, UserModel user)
        {
            if (id != user.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _userRepository.UpdateUserAsync(id, user);
                    if (result != null)
                        return Ok(result);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IsUserExists(id))
                        return NotFound();
                    else
                        throw;
                }
            }
            return BadRequest();
        }

        // POST: api/Users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseModel>> LoginUserAsync(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var result = await _userRepository.LoginUserAsync(loginModel);

                if (result == null)
                    return Unauthorized();

                return Ok(result);
            }

            return BadRequest();
        }


        // DELETE: api/Users/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserModel>> DeleteUser(Guid id)
        {
            var result = await _userRepository.DeleteUserAsync(id);

            if (result != null)
                return Ok(result);

            return BadRequest();
        }

        [HttpPost("token")]
        public async Task<ActionResult<TokenModel>> RequestNewTokenAsync(NewTokenModel model)
        {
            if (ModelState.IsValid)
            {
                var userExists = IsUserExists(model.UserId);

                if (userExists)
                {
                    var tokenModel = await _userRepository.GenerateNewTokensAsync(model.UserId, model.Token);

                    if (tokenModel != null)
                        return Ok(tokenModel);
                }
                else
                    return NotFound();
            }
            return BadRequest();
        }

        [HttpGet("verifyemail/{email}")]
        public async Task<ActionResult<bool>>CheckEmailAvailableInDbAsync(string email)
        {
            var isRegistered = await _userRepository.CheckIfEmailIsRegisteredAsync(email);
            return Ok(isRegistered);
        }

        private bool IsUserExists(Guid id)
        {
            return _dbContext.Users.Any(e => e.Id == id);
        }
    }
}
