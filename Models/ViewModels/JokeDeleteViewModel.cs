namespace JokesWebApp.Models.ViewModels
{
    public class JokeDeleteViewModel
    {
        public int Id { get; init; }
        public string JokeQuestion { get; init; } = String.Empty;
        public string JokeAnswer { get; init; } = String.Empty;
    }
}
