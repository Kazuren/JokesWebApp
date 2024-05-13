using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JokesWebApp.Data;
using JokesWebApp.Models;
using JokesWebApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace JokesWebApp.Controllers;

public class JokesController : Controller
{
    private readonly ILogger<JokesController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public JokesController(ILogger<JokesController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    // GET: Jokes
    public async Task<IActionResult> Index()
    {
          return _context.Joke != null ? 
                      View(await _context.Joke.ToListAsync()) :
                      Problem("Entity set 'ApplicationDbContext.Joke'  is null.");
    }
    // GET: Jokes/ShowSearchForm
    public async Task<IActionResult> ShowSearchForm()
    {
        return View();
    }

    // POST: Jokes/ShowSearchResults
    public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
    {
        return View("Index", await _context.Joke.Where(j => j.JokeQuestion.Contains(SearchPhrase)).ToListAsync());
    }

    // GET: Jokes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null || _context.Joke == null)
        {
            return NotFound();
        }

        var joke = await _context.Joke
            .FirstOrDefaultAsync(m => m.Id == id);
       
        if (joke == null)
        {
            return NotFound();
        }

        var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == joke.UserId);
        
        if (user == null)
        {
            return NotFound();
        }

        return View(new JokeDetailsViewModel() { Id = joke.Id, JokeAnswer = joke.JokeAnswer, JokeQuestion = joke.JokeQuestion, Author = user.Email});
    }

    // GET: Jokes/Create
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Jokes/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Create([Bind("Id,JokeQuestion,JokeAnswer,UserId")] Joke joke)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            return NotFound();
        }

        joke.UserId = user.Id;
        if (ModelState.IsValid)
        {
            _context.Add(joke);
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(new JokeCreateViewModel() {  
            JokeAnswer = joke.JokeAnswer,
            JokeQuestion = joke.JokeQuestion,
        });
    }

    // GET: Jokes/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null || _context.Joke == null)
        {
            return NotFound();
        }

        var joke = await _context.Joke.FindAsync(id);
        if (joke == null)
        {
            return NotFound();
        }
        return View(new JokeEditViewModel() { JokeQuestion = joke.JokeQuestion , JokeAnswer = joke.JokeAnswer });
    }

    // POST: Jokes/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost, ActionName("Edit")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> EditPost(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        Joke? jokeToUpdate = await _context.Joke.FirstOrDefaultAsync(j => j.Id == id);

        bool updated = await TryUpdateModelAsync<Joke>(jokeToUpdate, "", j => j.JokeAnswer, j => j.JokeQuestion);
        if (updated)
        {
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists, " +
                    "see your system administrator.");
            }
        }
        return View(new JokeEditViewModel() { JokeQuestion = jokeToUpdate.JokeQuestion, JokeAnswer = jokeToUpdate.JokeAnswer });
    }

    // GET: Jokes/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null || _context.Joke == null)
        {
            return NotFound();
        }

        var joke = await _context.Joke
            .FirstOrDefaultAsync(m => m.Id == id);
        if (joke == null)
        {
            return NotFound();
        }

        return View(new JokeDeleteViewModel() { JokeQuestion = joke.JokeQuestion, JokeAnswer = joke.JokeAnswer, Id = joke.Id });
    }

    // POST: Jokes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (_context.Joke == null)
        {
            return Problem("Entity set 'ApplicationDbContext.Joke'  is null.");
        }
        var joke = await _context.Joke.FindAsync(id);
        if (joke != null)
        {
            _context.Joke.Remove(joke);
        }
        
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
