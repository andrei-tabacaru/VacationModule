﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VacationModule.Core.Domain.IdentityEntities;
using VacationModule.Core.DTO;
using VacationModule.Core.Enums;

namespace VacationModule.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            // Check for validation errors
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = new ApplicationUser()
            {
                UserName = registerDTO.UserName,
                Email = registerDTO.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password!);

            if (result.Succeeded)
            {
                // Check role
                if (registerDTO.Role == UserRoleOptions.Admin)
                {
                    // Create Admin role
                    if (await _roleManager.FindByNameAsync(UserRoleOptions.Admin.ToString()) is null) 
                    {
                        ApplicationRole applicationRole = new ApplicationRole()
                        {
                            Name = UserRoleOptions.Admin.ToString()
                        };

                        await _roleManager.CreateAsync(applicationRole);
                    }

                    // Add user to role
                    await _userManager.AddToRoleAsync(user, UserRoleOptions.Admin.ToString());
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, UserRoleOptions.Employee.ToString());
                }

                // Sign in
                await _signInManager.SignInAsync(user,
                    // the user will be logged out when the browser is closed
                    isPersistent: false);

                return Ok(result);
            }
            else
            {
                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("Register", error.Description);
                }

                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            // Check for validation errors
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(
                loginDTO.Email!,
                loginDTO.Password!,
                isPersistent: false,
                // if more than 3 attemps fail the account will be locked out for login for a while
                lockoutOnFailure: false
                );

            if (result.Succeeded)
            {
                return Ok(result);
            }
            ModelState.AddModelError("Login", "Invalid email or password");
            
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }
    }
}
