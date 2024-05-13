using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JokesWebApp.Models;

public class Joke
{
    public int Id { get; set; }
    public string JokeQuestion { get; set; }
    public string JokeAnswer { get; set; }
    public string? UserId { get; set; }

    public Joke()
    {
        
    }
}
