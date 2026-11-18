namespace StudentManagementSystem.Models
{
    public class MasterStudent : Student
    {
        private string _thesisTopic = string.Empty;

        public string ThesisTopic
        {
            get { return _thesisTopic; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Thesis topic cannot be empty.");
                }

                _thesisTopic = value.Trim();
            }
        }

        public MasterStudent(string id, string fullName, int age, string degreeProgram, string thesisTopic)
            : base(id, fullName, age, degreeProgram)
        {
            ThesisTopic = thesisTopic;
        }

        public override string GetAcademicLevel()
        {
            return "Master";
        }

        public override string GetStudentDetails()
        {
            return $"{base.GetStudentDetails()} | Thesis: {ThesisTopic}";
        }
    }
}
