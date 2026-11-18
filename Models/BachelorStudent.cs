namespace StudentManagementSystem.Models
{
    public class BachelorStudent : Student
    {
        private int _studyYear;

        public int StudyYear
        {
            get { return _studyYear; }
            set
            {
                if (value < 1 || value > 4)
                {
                    throw new ArgumentException("Study year must be between 1 and 4 (inclusive).");
                }

                _studyYear = value;
            }
        }

        public BachelorStudent(string id, string fullName, int age, string degreeProgram, int studyYear)
            : base(id, fullName, age, degreeProgram)
        {
            StudyYear = studyYear;
        }

        public override string GetAcademicLevel()
        {
            return "Bachelor";
        }

        public override string GetStudentDetails()
        {
            return $"{base.GetStudentDetails()} | Study Year: {StudyYear}";
        }
    }
}
