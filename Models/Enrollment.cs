namespace StudentManagementSystem.Models
{
    // Association object between one Student and one Course.
    public class Enrollment
    {
        private string _enrollmentId = string.Empty;
        private Student _student = null!;
        private Course _course = null!;
        private DateTime _enrollmentDate;

        public string EnrollmentId
        {
            get { return _enrollmentId; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Enrollment Id cannot be empty.");
                }

                _enrollmentId = value.Trim();
            }
        }

        public Student Student
        {
            get { return _student; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Enrollment must reference a student.");
                }

                _student = value;
            }
        }

        public Course Course
        {
            get { return _course; }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Enrollment must reference a course.");
                }

                _course = value;
            }
        }

        public DateTime EnrollmentDate
        {
            get { return _enrollmentDate; }
            private set { _enrollmentDate = value; }
        }

        public Enrollment(string enrollmentId, Student student, Course course, DateTime enrollmentDate)
        {
            EnrollmentId = enrollmentId;
            Student = student;
            Course = course;
            EnrollmentDate = enrollmentDate;
        }

        public string GetEnrollmentDetails()
        {
            return $"Enrollment: {EnrollmentId} | Student: {Student.Id} ({Student.FullName}) | Course: {Course.Id} ({Course.Title}) | Date: {EnrollmentDate:yyyy-MM-dd HH:mm}";
        }

        public override string ToString()
        {
            return GetEnrollmentDetails();
        }
    }
}
