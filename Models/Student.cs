using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Models
{
    // Abstract base for all student types.
    public abstract class Student : IIdentifiable
    {
        private string _id = string.Empty;
        private string _fullName = string.Empty;
        private int _age;
        private string _degreeProgram = string.Empty;

        public string Id
        {
            get { return _id; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Student Id cannot be empty.");
                }

                _id = value.Trim();
            }
        }

        public string FullName
        {
            get { return _fullName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Full name cannot be empty.");
                }

                _fullName = value.Trim();
            }
        }

        public int Age
        {
            get { return _age; }
            set
            {
                if (value < 15 || value > 65)
                {
                    throw new ArgumentException("Age must be between 15 and 65 (inclusive).");
                }

                _age = value;
            }
        }

        public string DegreeProgram
        {
            get { return _degreeProgram; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Degree program cannot be empty.");
                }

                _degreeProgram = value.Trim();
            }
        }

        protected Student(string id, string fullName, int age, string degreeProgram)
        {
            Id = id;
            FullName = fullName;
            Age = age;
            DegreeProgram = degreeProgram;
        }

        public abstract string GetAcademicLevel();

        public virtual string GetStudentDetails()
        {
            return $"Id: {Id} | Name: {FullName} | Age: {Age} | Program: {DegreeProgram} | Level: {GetAcademicLevel()}";
        }

        public override string ToString()
        {
            return GetStudentDetails();
        }
    }
}
