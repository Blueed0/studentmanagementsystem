// Models isim alani: Veri modellerinin bulundugu katman
namespace StudentManagementSystem.Models
{
    // Enrollment: Bir öğrenci ile bir ders arasındaki ilişkiyi (association) tutan sınıf
    public class Enrollment
    {
        // _enrollmentId: Kayıt kaydının benzersiz kimliği
        private string _enrollmentId = string.Empty;

        // _student: Bu kayda bağlı öğrenci nesnesi
        private Student _student = null!;

        // _course: Bu kayda bağlı ders nesnesi
        private Course _course = null!;

        // _enrollmentDate: Kaydın oluşturulma zamanı
        private DateTime _enrollmentDate;

        // EnrollmentId: Kayıt kimliğini salt-okunur açar
        public string EnrollmentId
        {
            get { return _enrollmentId; }
            private set
            {
                // Kimlik boş olamaz
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Enrollment Id cannot be empty.");
                }

                _enrollmentId = value.Trim();
            }
        }

        // Student: İlişkili öğrenci referansını salt-okunur açar
        public Student Student
        {
            get { return _student; }
            private set
            {
                // Null öğrenci referansı kabul edilmez
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Enrollment must reference a student.");
                }

                _student = value;
            }
        }

        // Course: İlişkili ders referansını salt-okunur açar
        public Course Course
        {
            get { return _course; }
            private set
            {
                // Null ders referansı kabul edilmez
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value), "Enrollment must reference a course.");
                }

                _course = value;
            }
        }

        // EnrollmentDate: Kayıt tarihini salt-okunur açar
        public DateTime EnrollmentDate
        {
            get { return _enrollmentDate; }
            private set { _enrollmentDate = value; }
        }

        // Enrollment yapıcısı: Öğrenci-ders ilişkisini güvenli şekilde kurar
        public Enrollment(string enrollmentId, Student student, Course course, DateTime enrollmentDate)
        {
            EnrollmentId = enrollmentId;
            Student = student;
            Course = course;
            EnrollmentDate = enrollmentDate;
        }

        // GetEnrollmentDetails: Kayıt bilgisini okunabilir metin olarak üretir
        public string GetEnrollmentDetails()
        {
            return $"Enrollment: {EnrollmentId} | Student: {Student.Id} ({Student.FullName}) | Course: {Course.Id} ({Course.Title}) | Date: {EnrollmentDate:yyyy-MM-dd HH:mm}";
        }

        // ToString: Konsolda Enrollment yazdırılırken detay metnini kullanır
        public override string ToString()
        {
            return GetEnrollmentDetails();
        }
    }
}
