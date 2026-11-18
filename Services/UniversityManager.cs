using StudentManagementSystem.Models;

namespace StudentManagementSystem.Services
{
    // UniversityManager: Öğrenci, ders ve kayıt işlemlerini yöneten servis sınıfı
    // Amaç: İş kurallarını tek yerde tutmak; konsol UI doğrudan listeleri değiştirmesin
    public class UniversityManager
    {
        // _students: Sistemdeki tüm öğrencileri tutan liste
        private readonly List<Student> _students;

        // _courses: Sistemdeki tüm dersleri tutan liste
        private readonly List<Course> _courses;

        // _enrollments: Öğrenci-ders kayıt ilişkilerini tutan liste
        private readonly List<Enrollment> _enrollments;

        // UniversityManager yapıcısı: Boş koleksiyonlarla sistemi başlatır
        public UniversityManager()
        {
            _students = new List<Student>();
            _courses = new List<Course>();
            _enrollments = new List<Enrollment>();
        }

        // RegisterBachelorStudent: Yeni lisans öğrencisi kaydeder
        public BachelorStudent RegisterBachelorStudent(string id, string fullName, int age, string degreeProgram, int studyYear)
        {
            // Aynı Id ile öğrenci varsa işlemi reddeder (state bozulmaz)
            if (FindStudentById(id) != null)
            {
                throw new InvalidOperationException($"Student Id '{id}' already exists.");
            }

            // Doğrulanmış BachelorStudent nesnesini oluşturur
            BachelorStudent student = new BachelorStudent(id, fullName, age, degreeProgram, studyYear);

            // Listeye ekler ve çağırana döndürür
            _students.Add(student);
            return student;
        }

        // RegisterMasterStudent: Yeni yüksek lisans öğrencisi kaydeder
        public MasterStudent RegisterMasterStudent(string id, string fullName, int age, string degreeProgram, string thesisTopic)
        {
            // Yinelenen Id kontrolü
            if (FindStudentById(id) != null)
            {
                throw new InvalidOperationException($"Student Id '{id}' already exists.");
            }

            // Doğrulanmış MasterStudent nesnesini oluşturur
            MasterStudent student = new MasterStudent(id, fullName, age, degreeProgram, thesisTopic);

            // Listeye ekler ve çağırana döndürür
            _students.Add(student);
            return student;
        }

        // FindStudentById: Id'ye göre öğrenci arar; yoksa null döner
        public Student? FindStudentById(string id)
        {
            // Boş arama anahtarı ile sonuç yok kabul edilir
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            // Listedeki öğrencileri tek tek karşılaştırır
            foreach (Student student in _students)
            {
                if (string.Equals(student.Id, id.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return student;
                }
            }

            // Eşleşme yoksa null döner
            return null;
        }

        // GetAllStudents: Tüm öğrencilerin kopya listesini döndürür
        public List<Student> GetAllStudents()
        {
            // Dışarıdan orijinal listenin bozulmasını önlemek için kopya verir
            return new List<Student>(_students);
        }

        // GetStudentsByProgram: Brief isteği - öğrencileri programa göre filtreler
        public List<Student> GetStudentsByProgram(string degreeProgram)
        {
            // Boş program adı ile filtre yapılmaz
            if (string.IsNullOrWhiteSpace(degreeProgram))
            {
                throw new ArgumentException("Degree program cannot be empty.");
            }

            string target = degreeProgram.Trim();
            List<Student> matches = new List<Student>();

            // Program adı eşleşen öğrencileri toplar
            foreach (Student student in _students)
            {
                if (string.Equals(student.DegreeProgram, target, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(student);
                }
            }

            return matches;
        }

        // GetStudentCount: Kayıtlı öğrenci sayısını döndürür
        public int GetStudentCount()
        {
            return _students.Count;
        }

        // CreateCourse: Benzersiz kod ve pozitif kapasite ile ders oluşturur
        public Course CreateCourse(string id, string title, int capacity)
        {
            // Aynı ders kodu varsa reddeder
            if (FindCourseById(id) != null)
            {
                throw new InvalidOperationException($"Course code '{id}' already exists.");
            }

            // Course property doğrulamaları capacity > 0 kuralını uygular
            Course course = new Course(id, title, capacity);
            _courses.Add(course);
            return course;
        }

        // FindCourseById: Ders koduna göre arar; yoksa null döner
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

        // GetAllCourses: Tüm derslerin kopya listesini döndürür
        public List<Course> GetAllCourses()
        {
            return new List<Course>(_courses);
        }

        // GetCourseCount: Ders sayısını döndürür
        public int GetCourseCount()
        {
            return _courses.Count;
        }

        // EnrollStudent: Öğrenciyi derse kaydeder; brief kurallarını uygular
        public Enrollment EnrollStudent(string studentId, string courseId)
        {
            // Bilinmeyen öğrenci Id'sini reddeder
            Student? student = FindStudentById(studentId);
            if (student == null)
            {
                throw new InvalidOperationException($"Student '{studentId}' was not found.");
            }

            // Bilinmeyen ders kodunu reddeder
            Course? course = FindCourseById(courseId);
            if (course == null)
            {
                throw new InvalidOperationException($"Course '{courseId}' was not found.");
            }

            // Aynı öğrenci aynı derse ikinci kez kayıt olamaz
            if (IsAlreadyEnrolled(student.Id, course.Id))
            {
                throw new InvalidOperationException($"Student '{student.Id}' is already enrolled in course '{course.Id}'.");
            }

            // Kontenjan doluysa kaydı reddeder
            int usedSeats = CountEnrollmentsForCourse(course.Id);
            if (usedSeats >= course.Capacity)
            {
                throw new InvalidOperationException($"Course '{course.Id}' is full. No remaining seats.");
            }

            // Yeni Enrollment association nesnesini oluşturur ve listeye ekler
            string enrollmentId = $"ENR-{_enrollments.Count + 1:D4}";
            Enrollment enrollment = new Enrollment(enrollmentId, student, course, DateTime.Now);
            _enrollments.Add(enrollment);
            return enrollment;
        }

        // IsAlreadyEnrolled: Öğrencinin derse daha önce kayıtlı olup olmadığını kontrol eder
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

        // CountEnrollmentsForStudent: Brief isteği - öğrencinin ders sayısını hesaplar
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

        // CountEnrollmentsForCourse: Bir derse kayıtlı öğrenci sayısını hesaplar (kontenjan için)
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

        // GetAllEnrollments: Brief isteği - tüm enrollment listesini döndürür
        public List<Enrollment> GetAllEnrollments()
        {
            return new List<Enrollment>(_enrollments);
        }

        // GetEnrollmentsByStudent: Bir öğrencinin kayıtlarını listeler
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

        // GetEnrollmentCount: Toplam kayıt sayısını döndürür
        public int GetEnrollmentCount()
        {
            return _enrollments.Count;
        }
    }
}
