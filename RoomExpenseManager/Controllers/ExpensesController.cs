using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RoomExpenseManager.Interfaces;
using RoomExpenseManager.Models;
using Serilog;

namespace RoomExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;

        public ExpensesController(IExpenseService expenseService, IMapper mapper)
        {
            _expenseService = expenseService;
            _mapper = mapper;
        }
        /// <summary>
        /// Retrieves a list of expenses.
        /// </summary>
        /// <returns>A list of expenses.</returns>

        [HttpGet]
        [Route("GetExpenses")]
        public async Task<ActionResult<IEnumerable<ExpenseResponse>>> GetExpenses()
        {
            try
            {
                Log.Information("Entering GetExpenses method");
                var expenses = await _expenseService.GetAllExpensesAsync();
                var expenseResponses = _mapper.Map<IEnumerable<ExpenseResponse>>(expenses);
                Log.Information("Exiting GetExpenses method with response: {Response}", expenses);
                return Ok(expenseResponses);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in GetExpenses method");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [Route("GetExpenseById")]
        public async Task<ActionResult<ExpenseResponse>> GetExpense(int id)
        {
            try
            {
                Log.Information("Entering GetExpense method for id: {Id}", id);
                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (expense == null)
                {
                    Log.Warning("Expense not found for id: {Id}", id);
                    return NotFound();
                }

                var expenseResponse = _mapper.Map<ExpenseResponse>(expense);
                Log.Information("Exiting GetExpense method with response: {Response}", expenseResponse);
                return Ok(expenseResponse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in GetExpense method for id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("CreateExpense")]
        public async Task<ActionResult<ExpenseResponse>> CreateExpense([FromBody] ExpenseRequest expenseRequest)
        {
            
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state in CreateExpense method");
                return BadRequest(ModelState);
            }

            try
            {
                Log.Information("Entering CreateExpense method with request: {Request}", expenseRequest);
                var expense = _mapper.Map<Expense>(expenseRequest);
                //expense.CreatedDate = DateTime.Now;
                

                await _expenseService.AddExpenseAsync(expense);
                var expenseResponse = _mapper.Map<ExpenseResponse>(expense);

                Log.Information("Exiting CreateExpense method with response: {Response}", expenseResponse);
                return CreatedAtAction(nameof(GetExpense), new { id = expense.ExpenseId }, expenseResponse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in CreateExpense method");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        [Route("UpdateExpense")]
        public async Task<ActionResult<bool>> UpdateExpense(int id, [FromBody] ExpenseRequest expenseRequest)
        {
            bool res = false;
            if (!ModelState.IsValid)
            {
                Log.Warning("Invalid model state in UpdateExpense method for id: {Id}", id);
                return BadRequest(ModelState);
            }

            try
            {
                Log.Information("Entering UpdateExpense method for id: {Id}", id);
                var expense = await _expenseService.GetExpenseByIdAsync(id);
                
                if (expense == null)
                {
                    Log.Warning("Expense not found for id: {Id}", id);
                    return NotFound(false);
                }
                expenseRequest.UserId = expense.UserId;
                
                
                expenseRequest.UpdatedDate = DateTime.Now;
                _mapper.Map(expenseRequest, expense);
                
                await _expenseService.UpdateExpenseAsync(expense);
                
                Log.Information("Exiting UpdateExpense method for id: {Id}", id);
                return res=true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in UpdateExpense method for id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete]
        [Route("DeleteExpense")]
        public async Task<ActionResult<bool>> DeleteExpense(int id)
        {
            try
            {
                Log.Information("Entering DeleteExpense method for id: {Id}", id);

                var expense = await _expenseService.GetExpenseByIdAsync(id);
                if (expense == null)
                {
                    Log.Warning("Expense not found for id: {Id}", id);
                    return NotFound(false); // Return NotFound with false
                }

                await _expenseService.DeleteExpenseAsync(id);
                Log.Information("Exiting DeleteExpense method for id: {Id}", id);
                return Ok(true); // Return true with OK status
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in DeleteExpense method for id: {Id}", id);
                return StatusCode(500, "Internal server error");
            }
        }

    }
}
