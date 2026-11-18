using StudentManagementSystem.Models;
using StudentManagementSystem.Services;

namespace StudentManagementSystem
{
    // Console entry point for Case 01: Student Management System.
    public class Program
    {
        private static UniversityManager _manager = new UniversityManager();

        public static void Main(string[] args)
        {
            Console.Title = "01. Student Management System";
            SeedSampleData();

            bool isRunning = true;
            while (isRunning)
            {
                ShowMainMenu();
                string choice = ReadRequired("Select an option");

                switch (choice)
                {
                    case "1":
                        HandleRegisterStudent();
                        break;
                    case "2":
                        HandleCreateCourse();
                        break;
                    case "3":
                        HandleEnrollStudent();
                        break;
                    case "4":
                        HandleStudentCourseCount();
                        break;
                    case "5":
                        HandleStudentsByProgram();
                        break;
                    case "6":
                        HandleListEnrollments();
                        break;
                    case "7":
                        HandleListStudents();
                        break;
                    case "0":
                        isRunning = false;
                        PrintSuccess("Goodbye!");
                        break;
                    default:
                        PrintError("Invalid menu option.");
                        Pause();
                        break;
                }
            }
        }

        private static void ShowMainMenu()
        {
            try { Console.Clear(); } catch (IOException) { }

            Console.WriteLine("========================================");
            Console.WriteLine(" 01. STUDENT MANAGEMENT SYSTEM");
            Console.WriteLine("========================================");
            Console.WriteLine($" Students: {_manager.GetStudentCount()} | Courses: {_manager.GetCourseCount()} | Enrollments: {_manager.GetEnrollmentCount()}");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine(" 1. Register Student");
            Console.WriteLine(" 2. Create Course");
            Console.WriteLine(" 3. Enroll Student in Course");
            Console.WriteLine(" 4. Calculate Courses of a Student");
            Console.WriteLine(" 5. Display Students by Program");
            Console.WriteLine(" 6. Display Enrollment List");
            Console.WriteLine(" 7. List All Students");
            Console.WriteLine(" 0. Exit");
            Console.WriteLine("----------------------------------------");
        }

        private static void Pause()
        {
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }

        private static string ReadRequired(string label)
        {
            while (true)
            {
                Console.Write($"  > {label}: ");
                string? input = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                PrintError("Input cannot be empty.");
            }
        }

        private static int ReadIntInRange(string label, int min, int max)
        {
            while (true)
            {
                Console.Write($"  > {label} ({min}-{max}): ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                PrintError($"Value must be between {min} and {max}.");
            }
        }

        private static int ReadPositiveInt(string label)
        {
            while (true)
            {
                Console.Write($"  > {label} (> 0): ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int value) && value > 0)
                {
                    return value;
                }

                PrintError("Value must be greater than 0.");
            }
        }

        private static void PrintSuccess(string message) => Console.WriteLine($"[OK] {message}");
        private static void PrintError(string message) => Console.WriteLine($"[ERROR] {message}");
        private static void PrintInfo(string message) => Console.WriteLine($"[INFO] {message}");

        private static void HandleRegisterStudent()
        {
            Console.WriteLine("--- REGISTER STUDENT ---");
            Console.WriteLine("  B = Bachelor, M = Master");
            string type = ReadRequired("Type (B/M)").ToUpperInvariant();
            string id = ReadRequired("Student Id");
            string fullName = ReadRequired("Full Name");
            int age = ReadIntInRange("Age", 15, 65);
            string degreeProgram = ReadRequired("Degree Program");

            try
            {
                if (type == "B")
                {
                    int studyYear = ReadIntInRange("Study Year", 1, 4);
                    BachelorStudent student = _manager.RegisterBachelorStudent(id, fullName, age, degreeProgram, studyYear);
                    PrintSuccess("Bachelor student registered.");
                    PrintInfo(student.GetStudentDetails());
                }
                else if (type == "M")
                {
                    string thesisTopic = ReadRequired("Thesis Topic");
                    MasterStudent student = _manager.RegisterMasterStudent(id, fullName, age, degreeProgram, thesisTopic);
                    PrintSuccess("Master student registered.");
                    PrintInfo(student.GetStudentDetails());
                }
                else
                {
                    PrintError("Unknown type. Enter B or M.");
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

            Pause();
        }

        private static void HandleCreateCourse()
        {
            Console.WriteLine("--- CREATE COURSE ---");
            string id = ReadRequired("Course Code");
            string title = ReadRequired("Course Title");
            int capacity = ReadPositiveInt("Capacity");

            try
            {
                Course course = _manager.CreateCourse(id, title, capacity);
                PrintSuccess("Course created.");
                PrintInfo(course.GetCourseDetails());
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

            Pause();
        }

        private static void HandleEnrollStudent()
        {
            Console.WriteLine("--- ENROLL STUDENT ---");
            string studentId = ReadRequired("Student Id");
            string courseId = ReadRequired("Course Code");

            try
            {
                Enrollment enrollment = _manager.EnrollStudent(studentId, courseId);
                PrintSuccess("Enrollment created.");
                PrintInfo(enrollment.GetEnrollmentDetails());
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

            Pause();
        }

        private static void HandleStudentCourseCount()
        {
            Console.WriteLine("--- COURSES OF A STUDENT ---");
            string studentId = ReadRequired("Student Id");

            Student? student = _manager.FindStudentById(studentId);
            if (student == null)
            {
                PrintError($"Student '{studentId}' was not found.");
                Pause();
                return;
            }

            List<Enrollment> enrollments = _manager.GetEnrollmentsByStudent(student.Id);
            int count = _manager.CountEnrollmentsForStudent(student.Id);

            PrintInfo(student.GetStudentDetails());
            PrintInfo($"Number of courses: {count}");

            if (enrollments.Count == 0)
            {
                PrintInfo("No enrollments for this student.");
            }
            else
            {
                foreach (Enrollment enrollment in enrollments)
                {
                    Console.WriteLine($"  - {enrollment.Course.Id} | {enrollment.Course.Title}");
                }
            }

            Pause();
        }

        private static void HandleStudentsByProgram()
        {
            Console.WriteLine("--- STUDENTS BY PROGRAM ---");
            string degreeProgram = ReadRequired("Degree Program");

            try
            {
                List<Student> students = _manager.GetStudentsByProgram(degreeProgram);
                if (students.Count == 0)
                {
                    PrintInfo($"No students found for program '{degreeProgram}'.");
                }
                else
                {
                    PrintInfo($"Found {students.Count} student(s):");
                    foreach (Student student in students)
                    {
                        // Polymorphism: Bachelor/Master override runs here.
                        Console.WriteLine($"  - {student.GetStudentDetails()}");
                    }
                }
            }
            catch (Exception ex)
            {
                PrintError(ex.Message);
            }

            Pause();
        }

        private static void HandleListEnrollments()
        {
            Console.WriteLine("--- ENROLLMENT LIST ---");
            List<Enrollment> enrollments = _manager.GetAllEnrollments();

            if (enrollments.Count == 0)
            {
                PrintInfo("No enrollments yet.");
            }
            else
            {
                foreach (Enrollment enrollment in enrollments)
                {
                    Console.WriteLine($"  - {enrollment.GetEnrollmentDetails()}");
                }
            }

            Pause();
        }

        private static void HandleListStudents()
        {
            Console.WriteLine("--- ALL STUDENTS ---");
            List<Student> students = _manager.GetAllStudents();

            if (students.Count == 0)
            {
                PrintInfo("No students registered yet.");
            }
            else
            {
                foreach (Student student in students)
                {
                    Console.WriteLine($"  - {student.GetStudentDetails()}");
                }
            }

            Pause();
        }

        private static void SeedSampleData()
        {
            try
            {
                _manager.RegisterBachelorStudent("S1001", "Ada Lovelace", 20, "Computer Science", 2);
                _manager.RegisterMasterStudent("S2001", "Alan Turing", 24, "Computer Science", "Computability Theory");
                _manager.RegisterBachelorStudent("S1002", "Grace Hopper", 21, "Software Engineering", 3);
                _manager.CreateCourse("CS101", "Introduction to Programming", 3);
                _manager.CreateCourse("CS201", "Object-Oriented Programming", 40);
                _manager.EnrollStudent("S1001", "CS101");
                _manager.EnrollStudent("S1001", "CS201");
                _manager.EnrollStudent("S2001", "CS201");
            }
            catch (Exception ex)
            {
                PrintError($"Sample data could not be loaded: {ex.Message}");
            }
        }
    }
}
