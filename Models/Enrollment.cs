namespace StudentManagementSystem.Models // Kayıt (enrollment) modelinin bulunduğu isim alanı
{
    // Enrollment: Bir öğrenci ile bir ders arasındaki ilişkiyi (association) temsil eder
    public class Enrollment // Kompozisyon/ilişki nesnesi: Student ve Course referanslarını tutar
    {
        // _enrollmentId: Kayıt kaydının benzersiz kimliğini tutan özel alan
        private string _enrollmentId = string.Empty; // Başlangıç değeri; gerçek kimlik constructor'da atanır

        // _student: Kayıtlı öğrenci nesnesine referans tutan özel alan
        private Student _student = null!; // Association: Enrollment bir Student'a bağlıdır; constructor'da kesin atanır

        // _course: Kayıtlı ders nesnesine referans tutan özel alan
        private Course _course = null!; // Association: Enrollment bir Course'a bağlıdır; constructor'da kesin atanır

        // _enrollmentDate: Kaydın oluşturulma zamanını tutan özel alan
        private DateTime _enrollmentDate; // Ne zaman kayıt yapıldığını gösterir

        // EnrollmentId: Kayıt kimliğini salt-okunur şekilde dışarı açar
        public string EnrollmentId // Dışarıdan değiştirilemez
        {
            get { return _enrollmentId; } // Private kimliği okur
            private set // Yalnızca sınıf içinde atanabilir
            {
                // Kimlik boş/null/whitespace ise reddet
                if (string.IsNullOrWhiteSpace(value)) // Geçersiz kimlik kontrolü
                {
                    // Boş kayıt kimliği için ArgumentException fırlatılır
                    throw new ArgumentException("Enrollment Id cannot be empty."); // Net hata mesajı
                } // if bloğunun sonu

                // Geçerli kimliği kırpılmış halde saklar
                _enrollmentId = value.Trim(); // Tutarlı kimlik saklama
            } // EnrollmentId setter'ının sonu
        } // EnrollmentId özelliğinin sonu

        // Student: İlişkili öğrenci nesnesini salt-okunur şekilde dışarı açar
        public Student Student // Association ucu: öğrenci
        {
            get { return _student; } // Private öğrenci referansını okur
            private set // Yalnızca yapıcıda atanır
            {
                // Öğrenci referansı null ise reddet
                if (value == null) // Null association engellenir
                {
                    // Null öğrenci için ArgumentNullException fırlatılır
                    throw new ArgumentNullException(nameof(value), "Enrollment must reference a student."); // Net hata mesajı
                } // if bloğunun sonu

                // Geçerli öğrenci referansını saklar
                _student = value; // Association bağlantısını kurar
            } // Student setter'ının sonu
        } // Student özelliğinin sonu

        // Course: İlişkili ders nesnesini salt-okunur şekilde dışarı açar
        public Course Course // Association ucu: ders
        {
            get { return _course; } // Private ders referansını okur
            private set // Yalnızca yapıcıda atanır
            {
                // Ders referansı null ise reddet
                if (value == null) // Null association engellenir
                {
                    // Null ders için ArgumentNullException fırlatılır
                    throw new ArgumentNullException(nameof(value), "Enrollment must reference a course."); // Net hata mesajı
                } // if bloğunun sonu

                // Geçerli ders referansını saklar
                _course = value; // Association bağlantısını kurar
            } // Course setter'ının sonu
        } // Course özelliğinin sonu

        // EnrollmentDate: Kayıt tarihini salt-okunur şekilde dışarı açar
        public DateTime EnrollmentDate // Dışarıdan değiştirilemez
        {
            get { return _enrollmentDate; } // Private tarihi okur
            private set // Yalnızca yapıcıda atanır
            {
                // Tarihi private alana yazar
                _enrollmentDate = value; // Kayıt anını saklar
            } // EnrollmentDate setter'ının sonu
        } // EnrollmentDate özelliğinin sonu

        // Enrollment yapıcı metodu: İlişki nesnesini güvenli şekilde oluşturur
        public Enrollment(string enrollmentId, Student student, Course course, DateTime enrollmentDate) // Tüm zorunlu alanları alır
        {
            // EnrollmentId property setter'ı üzerinden kimliği doğrula ve ata
            EnrollmentId = enrollmentId; // Boş kimlikler burada reddedilir

            // Student property setter'ı üzerinden öğrenci referansını doğrula ve ata
            Student = student; // null öğrenci burada reddedilir

            // Course property setter'ı üzerinden ders referansını doğrula ve ata
            Course = course; // null ders burada reddedilir

            // EnrollmentDate property setter'ı üzerinden tarihi ata
            EnrollmentDate = enrollmentDate; // Kayıt zamanını saklar
        } // Enrollment yapıcı metodunun sonu

        // GetEnrollmentDetails: Kayıt bilgisini okunabilir tek satır olarak üretir
        public string GetEnrollmentDetails() // Listeleme ekranlarında kullanılır
        {
            // Kayıt kimliği, öğrenci, ders ve tarihi biçimlendirilmiş metin olarak döndürür
            return $"Enrollment: {EnrollmentId} | Student: {Student.Id} ({Student.FullName}) | Course: {Course.Id} ({Course.Title}) | Date: {EnrollmentDate:yyyy-MM-dd HH:mm}"; // Tutarlı özet
        } // GetEnrollmentDetails metodunun sonu

        // ToString: Konsolda Enrollment yazdırılırken anlamlı metin gösterir
        public override string ToString() // Object.ToString'i ezer
        {
            // Detay metodunu yeniden kullanarak tutarlı çıktı sağlar
            return GetEnrollmentDetails(); // Aynı formatı kullanır
        } // ToString metodunun sonu
    } // Enrollment sınıfının sonu
} // StudentManagementSystem.Models isim alanının sonu
