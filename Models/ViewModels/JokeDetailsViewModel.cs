using Microsoft.AspNetCore.Identity;

namespace JokesWebApp.Models.ViewModels;

public class JokeDetailsViewModel
{
    public int Id { get; init; }
    public string JokeQuestion { get; init; } = String.Empty;
    public string JokeAnswer { get; init; } = String.Empty;
    public string? Author { get; init; } = String.Empty;
    
    public JokeDetailsViewModel()
    {
        
    }
}
