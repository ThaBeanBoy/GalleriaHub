namespace Routes;

public static class Developers
{
    public static List<TeamMember> Team = new List<TeamMember>()
    {
        new TeamMember(Name: "Trevor", Initials: "N/A", StudentNr: "000000000"),
        new TeamMember(Name: "Michelle", Initials: "N/A", StudentNr: "000000000"),
        new TeamMember(Name: "Morena", Initials: "N/A", StudentNr: "000000000"),
        new TeamMember(Name: "Tiney", Initials: "TG", StudentNr: "220150124"),
    };

    public static string RouterPrefix = "/developers";

    public static RouteGroupBuilder DeveloperRoutes(this RouteGroupBuilder group)
    {
        // Get's all developers
        group.MapGet("/", () => Team);

        // Get's a specific developer
        group.MapGet("/{studentNr}", (string studentNr) => Team.SingleOrDefault(Member => Member.StudentNr == studentNr));

        return group;
    }

    public  class TeamMember {
        // Properties
        private string _name;
        private string _studentNr;
        private string? _initials;

        // Constructors
        public TeamMember(string Name, string StudentNr){
            this._name = Name;
            this._studentNr = StudentNr;
        }

        public TeamMember(string Name, string StudentNr, string Initials): this(Name, StudentNr)  {
            this._initials = Initials;
        }

        // Getters & setters
        public string Name {
            get{
                return this._name;
            }
            set{
                this._name = value;
            }
        }

        public string StudentNr {
            get {
                return this._studentNr;
            }
            set {
                ValidateStudentNr(value);
                this._studentNr = value.Trim();
            }
        }

        public string? Initials {
            get {
                return this._initials;
            }
            set {
                this._initials = value;
            }
        }

        private static void ValidateStudentNr(string StudentNr){
            if(StudentNr.Length != 9){
                throw new InvalidStudentNumber();
            }
        }

        public class InvalidStudentNumber : Exception {
            public InvalidStudentNumber(): base("The student number is invalid"){

            }
        }
    }
}