using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Models.Users;

public class User : BaseModel {
    public string Login { get; set; }
    
    public string Password { get; set; }
    
    public ICollection<Character> Characters { get; set; }
}