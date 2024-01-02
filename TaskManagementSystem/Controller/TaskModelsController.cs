using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Models;
using TaskManagementSystem.Data;
using System.Security.Claims;
using OpenAI_API;
using OpenAI_API.Completions;
using System.Threading.Tasks;
using System;

namespace TaskManagementSystem.Controllers
{
    public class TaskModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly OpenAIAPI _openAI;

        public TaskModelsController(ApplicationDbContext context)
        {
            _context = context;
            _openAI = new OpenAIAPI(Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        private async Task<string> GenerateDescription(string taskTitle)
        {
            try
            {
                var completionRequest = new CompletionRequest
                {
                    Prompt = $"For a todo list management system, Generate a task description for: {taskTitle}, " +
                        $"brief without too many details.",
                    Model = OpenAI_API.Models.Model.DavinciText,
                    MaxTokens = 100
                };

                var completions = await _openAI.Completions.CreateCompletionAsync(completionRequest);
                return completions.Completions.FirstOrDefault()?.Text ?? string.Empty;
            }
            catch (Exception ex)
            {
                return $"There was an error when trying to connect with OpenAI: {ex.Message}";
            }
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetCurrentUserId();
            var todos = await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
            return View(todos);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = GetCurrentUserId();
            var taskModel = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskModel == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(taskModel);
            await _context.SaveChangesAsync();

            return Redirect("/TaskModels");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await _context.Todos.FirstOrDefaultAsync(m => m.Id == id);

            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId();
                var taskModel = new TaskModel
                {
                    Title = viewModel.Title,
                    UserId = userId,
                    DueDate = viewModel.DueDate,
                    Status = viewModel.Status,
                    Description = await GenerateDescription(viewModel.Title)
                };

                _context.Add(taskModel);
                await _context.SaveChangesAsync();
                return Redirect("/TaskModels");
            }

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();
            var taskModel = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,DueDate,Status")] TaskModel taskModel)
        {
            if (id != taskModel.Id)
            {
                return NotFound();
            }

            var userId = GetCurrentUserId();
            var existingTask = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (existingTask == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    existingTask.Title = taskModel.Title;
                    existingTask.Description = taskModel.Description;
                    existingTask.DueDate = taskModel.DueDate;
                    existingTask.Status = taskModel.Status;

                    _context.Update(existingTask);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskModelExists(taskModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Redirect("/TaskModels");
            }
            return View(taskModel);
        }

        private bool TaskModelExists(int id)
        {
            return _context.Todos?.Any(e => e.Id == id) ?? false;
        }
    }
}
