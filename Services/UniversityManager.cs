using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    // Keeps students, courses and enrollments in memory and applies the business rules.
    public class UniversityManager
    {
        private readonly List<Student> _students;
        private readonly List<Course> _courses;
        private readonly List<Enrollment> _enrollments;

        public UniversityManager()
        {
            _students = new List<Student>();
            _courses = new List<Course>();
            _enrollments = new List<Enrollment>();
        }

        public BachelorStudent RegisterBachelorStudent(string id, string fullName, int age, string degreeProgram, int studyYear)
        {
            if (FindStudentById(id) != null)
            {
                throw new InvalidOperationException($"Student Id '{id}' already exists.");
            }

            BachelorStudent student = new BachelorStudent(id, fullName, age, degreeProgram, studyYear);
            _students.Add(student);
            return student;
        }

        public MasterStudent RegisterMasterStudent(string id, string fullName, int age, string degreeProgram, string thesisTopic)
        {
            if (FindStudentById(id) != null)
            {
                throw new InvalidOperationException($"Student Id '{id}' already exists.");
            }

            MasterStudent student = new MasterStudent(id, fullName, age, degreeProgram, thesisTopic);
            _students.Add(student);
            return student;
        }

        public Student? FindStudentById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            foreach (Student student in _students)
            {
                if (string.Equals(student.Id, id.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return student;
                }
            }

            return null;
        }

        public List<Student> GetAllStudents()
        {
            return new List<Student>(_students);
        }

        public List<Student> GetStudentsByProgram(string degreeProgram)
        {
            if (string.IsNullOrWhiteSpace(degreeProgram))
            {
                throw new ArgumentException("Degree program cannot be empty.");
            }

            string target = degreeProgram.Trim();
            List<Student> matches = new List<Student>();

            foreach (Student student in _students)
            {
                if (string.Equals(student.DegreeProgram, target, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(student);
                }
            }

            return matches;
        }

        public int GetStudentCount()
        {
            return _students.Count;
        }

        public Course CreateCourse(string id, string title, int capacity)
        {
            if (FindCourseById(id) != null)
            {
                throw new InvalidOperationException($"Course code '{id}' already exists.");
            }

            Course course = new Course(id, title, capacity);
            _courses.Add(course);
            return course;
        }

        public Course? FindCourseById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            foreach (Course course in _courses)
            {
                if (string.Equals(course.Id, id.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return course;
                }
            }

            return null;
        }

        public List<Course> GetAllCourses()
        {
            return new List<Course>(_courses);
        }

        public int GetCourseCount()
        {
            return _courses.Count;
        }

        public Enrollment EnrollStudent(string studentId, string courseId)
        {
            Student? student = FindStudentById(studentId);
            if (student == null)
            {
                throw new InvalidOperationException($"Student '{studentId}' was not found.");
            }

            Course? course = FindCourseById(courseId);
            if (course == null)
            {
                throw new InvalidOperationException($"Course '{courseId}' was not found.");
            }

            if (IsAlreadyEnrolled(student.Id, course.Id))
            {
                throw new InvalidOperationException($"Student '{student.Id}' is already enrolled in course '{course.Id}'.");
            }

            int usedSeats = CountEnrollmentsForCourse(course.Id);
            if (usedSeats >= course.Capacity)
            {
                throw new InvalidOperationException($"Course '{course.Id}' is full. No remaining seats.");
            }

            string enrollmentId = $"ENR-{_enrollments.Count + 1:D4}";
            Enrollment enrollment = new Enrollment(enrollmentId, student, course, DateTime.Now);
            _enrollments.Add(enrollment);
            return enrollment;
        }

        public bool IsAlreadyEnrolled(string studentId, string courseId)
        {
            foreach (Enrollment enrollment in _enrollments)
            {
                bool sameStudent = string.Equals(enrollment.Student.Id, studentId, StringComparison.OrdinalIgnoreCase);
                bool sameCourse = string.Equals(enrollment.Course.Id, courseId, StringComparison.OrdinalIgnoreCase);
                if (sameStudent && sameCourse)
                {
                    return true;
                }
            }

            return false;
        }

        public int CountEnrollmentsForStudent(string studentId)
        {
            int count = 0;
            foreach (Enrollment enrollment in _enrollments)
            {
                if (string.Equals(enrollment.Student.Id, studentId, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return count;
        }

        public int CountEnrollmentsForCourse(string courseId)
        {
            int count = 0;
            foreach (Enrollment enrollment in _enrollments)
            {
                if (string.Equals(enrollment.Course.Id, courseId, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            return count;
        }

        public List<Enrollment> GetAllEnrollments()
        {
            return new List<Enrollment>(_enrollments);
        }

        public List<Enrollment> GetEnrollmentsByStudent(string studentId)
        {
            List<Enrollment> matches = new List<Enrollment>();
            foreach (Enrollment enrollment in _enrollments)
            {
                if (string.Equals(enrollment.Student.Id, studentId, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(enrollment);
                }
            }

            return matches;
        }

        public int GetEnrollmentCount()
        {
            return _enrollments.Count;
        }
    }
}
