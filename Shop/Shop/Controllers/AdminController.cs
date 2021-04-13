﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.DataAccess;
using Shop.Extensions;
using Shop.Models;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly OrderService orderService;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(OrderService orderService, UserManager<ApplicationUser> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
        }

        public IActionResult GetOrders() => View(orderService.GetAll());

        public IActionResult GetOrder(Guid id) 
        {
            return View(orderService.GetOrder(id));
        }

        public IActionResult ChangeOrderStatus(string status, Guid id)
        {
            orderService.ChangeStatus(status, id);
            return RedirectToAction("GetOrder", new { id = id });
        }

        public IActionResult GetUsers() => View(userManager.Users.ToList());

        public IActionResult CreateUser() => View();

        [HttpPost]
        public async Task<IActionResult> CreateUser(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { Email = model.Email, UserName = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("GetUsers");
                }
                else
                {
                    result.AddErrorsTo(ModelState);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> GetUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var model = new UserViewModel { Id = user.Id, Email = user.Email, Name = user.FirstName, Surname = user.Surname, PhoneNumber = user.PhoneNumber };
            return View(model);
        }

        public async Task<IActionResult> ChangeUserPassword(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserViewModel { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    var result = await userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetUser", new { id = model.Id });
                    }
                    else
                    {
                        result.AddErrorsTo(ModelState);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var model = new UserViewModel { Id = user.Id, Email = user.Email, Name = user.FirstName, Surname = user.Surname, PhoneNumber = user.PhoneNumber };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Email = model.Email;
                    user.UserName = model.Email;
                    user.FirstName = model.Name;
                    user.Surname = model.Surname;
                    user.PhoneNumber = model.PhoneNumber;
                    var result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetUser", new { id = model.Id });
                    }
                    else
                    {
                        result.AddErrorsTo(ModelState);
                    }
                }
            }
            return View(model);
        }

        public async Task<ActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    result.AddErrorsTo(ModelState);
                }
            }
            return RedirectToAction("GetUsers");
        }
    }
}
