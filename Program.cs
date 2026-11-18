// Models: Student, Course, Enrollment gibi veri siniflarini kullanmak icin
using StudentManagementSystem.Models;
// Services: UniversityManager is kurallarini kullanmak icin
using StudentManagementSystem.Services;

// StudentManagementSystem: Uygulamanin ana isim alani
namespace StudentManagementSystem
{
    // Program: Konsol uygulamasının giriş noktası; menü ve kullanıcı etkileşimini yönetir
    public class Program
    {
        // _manager: Tüm iş kurallarını uygulayan paylaşılan servis nesnesi
        private static UniversityManager _manager = new UniversityManager();

        // Main: Programın başladığı metot; menü döngüsünü çalıştırır
        public static void Main(string[] args)
        {
            // Konsol pencere başlığını ayarlar
            Console.Title = "01. Student Management System";

            // Demo için örnek (kurgusal) verileri yükler
            SeedSampleData();

            // Menü döngüsünü kontrol eden bayrak
            bool isRunning = true;

            // Kullanıcı çıkana kadar menüyü tekrar eder
            while (isRunning)
            {
                // Ana menüyü gösterir
                ShowMainMenu();

                // Kullanıcıdan menü seçimini alır
                string choice = ReadRequired("Select an option");

                // Seçime göre ilgili işlemi çalıştırır
                switch (choice)
                {
                    case "1":
                        // Öğrenci kaydı
                        HandleRegisterStudent();
                        break;
                    case "2":
                        // Ders oluşturma
                        HandleCreateCourse();
                        break;
                    case "3":
                        // Derse kayıt (enrollment)
                        HandleEnrollStudent();
                        break;
                    case "4":
                        // Öğrencinin ders sayısını hesaplama/gösterme
                        HandleStudentCourseCount();
                        break;
                    case "5":
                        // Programa göre öğrenci listeleme
                        HandleStudentsByProgram();
                        break;
                    case "6":
                        // Tüm enrollment listesini gösterme
                        HandleListEnrollments();
                        break;
                    case "7":
                        // Tüm öğrencileri polimorfik detayla listeleme
                        HandleListStudents();
                        break;
                    case "0":
                        // Uygulamadan çıkış
                        isRunning = false;
                        PrintSuccess("Goodbye!");
                        break;
                    default:
                        // Geçersiz menü seçimi
                        PrintError("Invalid menu option.");
                        Pause();
                        break;
                }
            }
        }

        // ShowMainMenu: Ana menüyü ve canlı sayaçları ekrana yazar
        private static void ShowMainMenu()
        {
            // Mümkünse ekranı temizler (yönlendirilmiş çıktıda hata vermemesi için try-catch)
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

        // Pause: Kullanıcının sonucu okuyabilmesi için Enter bekler
        private static void Pause()
        {
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }

        // ReadRequired: Boş olamayacak metin girdisi okur
        private static string ReadRequired(string label)
        {
            while (true)
            {
                Console.Write($"  > {label}: ");
                string? input = Console.ReadLine();

                // Boş veya sadece boşluk girilirse tekrar ister
                if (!string.IsNullOrWhiteSpace(input))
                {
                    return input.Trim();
                }

                PrintError("Input cannot be empty.");
            }
        }

        // ReadIntInRange: Verilen aralıkta tam sayı okur (örnek: yaş 15-65)
        private static int ReadIntInRange(string label, int min, int max)
        {
            while (true)
            {
                Console.Write($"  > {label} ({min}-{max}): ");
                string? input = Console.ReadLine();

                // Sayıya çevrilebilen ve aralıkta olan değeri kabul eder
                if (int.TryParse(input, out int value) && value >= min && value <= max)
                {
                    return value;
                }

                PrintError($"Value must be between {min} and {max}.");
            }
        }

        // ReadPositiveInt: Sıfırdan büyük tam sayı okur (örnek: ders kapasitesi)
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

        // PrintSuccess: Başarı mesajı yazar
        private static void PrintSuccess(string message) => Console.WriteLine($"[OK] {message}");

        // PrintError: Hata mesajı yazar
        private static void PrintError(string message) => Console.WriteLine($"[ERROR] {message}");

        // PrintInfo: Bilgi mesajı yazar
        private static void PrintInfo(string message) => Console.WriteLine($"[INFO] {message}");

        // HandleRegisterStudent: Lisans veya yüksek lisans öğrencisi kaydeder
        private static void HandleRegisterStudent()
        {
            Console.WriteLine("--- REGISTER STUDENT ---");
            Console.WriteLine("  B = Bachelor, M = Master");

            // Öğrenci türünü okur
            string type = ReadRequired("Type (B/M)").ToUpperInvariant();

            // Ortak alanları okur
            string id = ReadRequired("Student Id");
            string fullName = ReadRequired("Full Name");
            int age = ReadIntInRange("Age", 15, 65);
            string degreeProgram = ReadRequired("Degree Program");

            try
            {
                if (type == "B")
                {
                    // Lisans yılı ek alanıdır
                    int studyYear = ReadIntInRange("Study Year", 1, 4);
                    BachelorStudent student = _manager.RegisterBachelorStudent(id, fullName, age, degreeProgram, studyYear);
                    PrintSuccess("Bachelor student registered.");
                    PrintInfo(student.GetStudentDetails());
                }
                else if (type == "M")
                {
                    // Tez konusu yüksek lisansın ek alanıdır
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
                // Başarısız işlemde state bozulmaz; sadece hata mesajı gösterilir
                PrintError(ex.Message);
            }

            Pause();
        }

        // HandleCreateCourse: Yeni ders oluşturur
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

        // HandleEnrollStudent: Öğrenciyi bir derse kaydeder
        private static void HandleEnrollStudent()
        {
            Console.WriteLine("--- ENROLL STUDENT ---");
            string studentId = ReadRequired("Student Id");
            string courseId = ReadRequired("Course Code");

            try
            {
                // Servis: unknown Id, duplicate enrollment ve kapasite kurallarını kontrol eder
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

        // HandleStudentCourseCount: Bir öğrencinin kaç derse kayıtlı olduğunu hesaplar ve gösterir
        private static void HandleStudentCourseCount()
        {
            Console.WriteLine("--- COURSES OF A STUDENT ---");
            string studentId = ReadRequired("Student Id");

            // Bilinmeyen öğrenci kontrolü
            Student? student = _manager.FindStudentById(studentId);
            if (student == null)
            {
                PrintError($"Student '{studentId}' was not found.");
                Pause();
                return;
            }

            // Öğrencinin kayıtlarını ve ders sayısını alır
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
                // Kayıtlı dersleri tek tek yazar
                foreach (Enrollment enrollment in enrollments)
                {
                    Console.WriteLine($"  - {enrollment.Course.Id} | {enrollment.Course.Title}");
                }
            }

            Pause();
        }

        // HandleStudentsByProgram: Belirli bir programa ait öğrencileri listeler
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
                        // Polimorfizm: runtime'da Bachelor veya Master GetStudentDetails() çalışır
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

        // HandleListEnrollments: Sistemdeki tüm enrollment kayıtlarını gösterir
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

        // HandleListStudents: Tüm öğrencileri polimorfik detaylarıyla listeler
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
                    // Student referansı üzerinden override edilmiş metot çağrılır
                    Console.WriteLine($"  - {student.GetStudentDetails()}");
                }
            }

            Pause();
        }

        // SeedSampleData: Canlı demo için kurgusal örnek verileri yükler
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
                // Seed hatası uygulamayı düşürmez; sadece uyarı verir
                PrintError($"Sample data could not be loaded: {ex.Message}");
            }
        }
    }
}
