﻿using Demo.DAL.Entities;
using DemoPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoPL.Controllers
{
    [Authorize(Roles = "Admin")]
    //[Authorize(Roles = "HR")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole identityRole)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(identityRole);
            }
            return View(identityRole);
        }

        public async Task<IActionResult> Details(string id)
        {
            if (id is null)
                return NotFound();

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            return View(role);
        }

        public async Task<IActionResult> Update(string id)
        {

            if (id is null)
                return NotFound();

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return NotFound();

            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, IdentityRole identityRole)
        {
            if (id != identityRole.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);

                    role.Name = identityRole.Name;
                    role.NormalizedName = identityRole.Name.ToUpper();


                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                        return RedirectToAction("Index");

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
                catch (Exception ex)
                {
                    throw;
                }
            }


            return View(identityRole);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id is null)
                return NotFound();

            try
            {
                var role = await _roleManager.FindByIdAsync(id);

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                    return RedirectToAction("Index");

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            catch (Exception ex)
            {
                throw;
            }

            return View("Index");
        }

        public async Task<IActionResult> AddOrRemoveUsers(string RoleId)
        {
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (RoleId is null)
                return NotFound();

            ViewBag.RoleId = RoleId;

            var users = new List<UserInRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userInRole = new UserInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else
                    userInRole.IsSelected = false;

                users.Add(userInRole);
            }

            return View(users);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrRemoveUsers(List<UserInRoleViewModel> models, string RoleId)
        {

            var role = await _roleManager.FindByIdAsync(RoleId);

            if (RoleId is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var item in models)
                {
                    var user = await _userManager.FindByIdAsync(item.UserId);

                    if (user is not null)
                    {
                        if (item.IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                            await _userManager.AddToRoleAsync(user, role.Name);
                        else if (!item.IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                            await _userManager.RemoveFromRoleAsync(user, role.Name);


                    }
                }

                return RedirectToAction("Update", new { id = RoleId });
            }

            return View(models);
        }


    }
}
