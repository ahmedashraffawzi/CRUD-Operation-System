using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using P.DataAccessLayer.Models;
using P.PresentationLayer.Helpers;
using P.PresentationLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace P.PresentationLayer.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;

		public UserController(UserManager<ApplicationUser> userManager, IMapper mapper)
		{
            _userManager = userManager;
			_mapper = mapper;
		}
		public async Task<IActionResult> Index(string SearchValue)
		{
			if (string.IsNullOrEmpty(SearchValue))
			{
				var users = await _userManager.Users.Select(
					U => new UserViewModel()

				{
					Id = U.Id,
					FName = U.FName,
					LName = U.LName,
					Email = U.Email,
					PhoneNumber = U.PhoneNumber,
					Roles = _userManager.GetRolesAsync(U).Result

				}).ToListAsync();

				return View(users);
			}
			else
			{
				var user = await _userManager.FindByEmailAsync(SearchValue);
				var MappedUser = new UserViewModel()
				{
					Id = user.Id,
					FName = user.FName,
					LName = user.LName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					Roles = _userManager.GetRolesAsync(user).Result
				};
				return View(new List<UserViewModel> { MappedUser });

			}

		}
		public async Task<IActionResult> Details(string id, string ViewName = "Details")
		{
			if (id is null)
				return BadRequest();
			var user = await _userManager.FindByIdAsync(id);
			if (user is null)
				return NotFound();
			var MappedUser = _mapper.Map<ApplicationUser, UserViewModel>(user);
			return View(ViewName, MappedUser);
		}
		[HttpGet]
		public async Task<IActionResult> Edit(string id)
		{

			return await Details(id, "Edit");

		}
		[HttpPost]
		public async Task<IActionResult> Edit(UserViewModel UserVM, [FromRoute] string id)
		{
            if (id != UserVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
					var User = await _userManager.FindByIdAsync(id);
					User.PhoneNumber= UserVM.PhoneNumber;
					User.FName= UserVM.FName;
					User.LName= UserVM.LName;
                    //var Mappeduser = _mapper.Map<UserViewModel, ApplicationUser>(UserVM);
                    await _userManager.UpdateAsync(User);
                    return RedirectToAction(nameof(Index));

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }
            return View(UserVM);
        }
		public async Task<IActionResult> Delete(string id)
		{
            return await Details(id, "Delete");
        }
		[HttpPost]
		public async Task<IActionResult> ConfirmDelete(string id)
		{
			try
			{
				var user=await _userManager.FindByIdAsync(id);
				await _userManager.DeleteAsync(user);
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
                ModelState.AddModelError(string.Empty, ex.Message);
				return RedirectToAction("Error", "Home");
            }
		}
		

	}
}
