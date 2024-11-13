﻿using Microsoft.AspNetCore.Mvc;
using RoomExpenseManagerWebApp.Models;
using RoomExpenseManagerWebApp.Services.Interface.IExpense;
using RoomExpenseManagerWebApp.Services.Interface.IUser;

namespace RoomExpenseManagerWebApp.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpense _expense;
        private readonly IUser _user;

        public ExpenseController(IExpense expense, IUser user)
        {
            _expense = expense;
            _user = user;
        }
        
        
        [HttpGet]
        public async Task<IActionResult> Dashboard(int? year = null, int? month = null)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                // Fetch all expenses and users
                var expenses = await _expense.GetAllExpenseAsync();
                var users = await _user.GetAllUsersAsync();

                // Apply filters based on provided parameters
                if (year.HasValue && month.HasValue)
                {
                    // Filter by both year and month
                    expenses = expenses.Where(e => e.CreatedDate.Year == year.Value &&
                                                   e.CreatedDate.Month == month.Value).ToList();
                }
                else if (year.HasValue)
                {
                    // Filter by year only
                    expenses = expenses.Where(e => e.CreatedDate.Year == year.Value).ToList();
                }
                else if (month.HasValue)
                {
                    // Filter by month only (for any year)
                    expenses = expenses.Where(e => e.CreatedDate.Month == month.Value).ToList();
                }

                // Join expenses with users to get the user's name for each expense
                var joinedData = from expense in expenses
                                 join user in users on expense.UserId equals user.UserId
                                 select new
                                 {
                                     UserName = user.Name,
                                     Amount = expense.Amount
                                 };

                // Group expenses by username and calculate total amount per user
                var result = joinedData
                    .GroupBy(j => j.UserName)
                    .Select(g => new ExpenseViewModel
                    {
                        UserName = g.Key,
                        Amount = g.Sum(e => e.Amount)
                    })
                    .ToList();

                // Return the filtered result to the view
                return View(result);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Unauthorized");
            }
        }




        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var expenses = await _expense.GetAllExpenseAsync();
                var users = await _user.GetAllUsersAsync();
                var result = from expense in expenses
                             join user in users on expense.UserId equals user.UserId
                             //where expense.CreatedDate.Year == DateTime.Now.Year // Filter for the year 2024
                             orderby expense.CreatedDate.Year descending // Order by the full CreatedDate in descending order
                             select new ExpenseViewModel
                             {
                                 ExpenseId = expense.ExpenseId,
                                 Item = expense.Item,
                                 Description = expense.Description,
                                 Amount = expense.Amount,
                                 CreatedDate = expense.CreatedDate,
                                 UserName = user.Name // Assuming 'UserName' is a property of user
                             };
                //return View(expenses);

                
                return View(result);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Unauthorized");
            }
        }
        public async Task<IActionResult> MyExpenses()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var userId = HttpContext.Session.GetInt32("UserId");
                var expenses = await _expense.GetAllExpenseAsync();
                var userExpenses = expenses.Where(e => e.UserId == userId).ToList();
                return View(userExpenses);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Unauthorized");
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetExpense(int id)
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var expense = await _expense.GetExpenseByIdAsync(id);
                return Json(expense);
            }
            else
            {
                return Json(string.Empty);
            }
        }
        [HttpPut]
        public async Task<bool> EditExpense(int id,Expense expense)
        {

            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    var res = await _expense.UpdateExpenseAsync(id, expense);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        [HttpDelete]
        public async Task<bool> DeleteExpense(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    var res = await _expense.DeleteExpenseAsync(id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        [HttpGet]
        public IActionResult CreateExpense()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Unauthorized", "Unauthorized");
            }
        }
       
        [HttpPost]
        // Ensures CSRF protection
        public async Task<IActionResult> CreateExpense(Expense expense)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    expense.UserId = (int)HttpContext.Session.GetInt32("UserId");
                    await _expense.CreateExpenseAsync(expense);
                    return Ok(new { message = "User created successfully." }); // 200 OK response
                }
                else
                {
                    return RedirectToAction("Unauthorized", "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the user." }); // 500 Internal Server Error
            }
        }
    }
}
