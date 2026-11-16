using StudentManagementSystem.Models; // Student, Course, Enrollment modellerini kullanmak için

namespace StudentManagementSystem.Services // İş kurallarını yöneten servis katmanı isim alanı
{
    // UniversityManager: Tüm öğrenci/ders/kayıt işlemlerini kapsülleyen servis / kayıt defteri sınıfı
    public class UniversityManager // Console UI doğrudan listelere dokunmaz; tüm işlemler buradan geçer
    {
        // _students: Sistemdeki tüm öğrencileri tutan özel salt-okunur liste
        private readonly List<Student> _students; // readonly: referans yeniden atanamaz; içerik yönetilebilir

        // _courses: Sistemdeki tüm dersleri tutan özel salt-okunur liste
        private readonly List<Course> _courses; // CRUD işlemleri bu liste üzerinden yapılır

        // _enrollments: Öğrenci-ders kayıt ilişkilerini tutan özel salt-okunur liste
        private readonly List<Enrollment> _enrollments; // Association kayıtları burada saklanır

        // MaxCoursesPerStudent: Bir öğrencinin alabileceği maksimum ders sayısı sabiti
        public const int MaxCoursesPerStudent = 5; // İş kuralı: aşırı yüklenmeyi önler

        // UniversityManager yapıcı metodu: Boş koleksiyonlarla sistemi başlatır
        public UniversityManager() // Parametresiz yapıcı: bellek içi (in-memory) sistem
        {
            // Öğrenci listesini boş olarak oluşturur
            _students = new List<Student>(); // Başlangıçta kayıtlı öğrenci yoktur

            // Ders listesini boş olarak oluşturur
            _courses = new List<Course>(); // Başlangıçta tanımlı ders yoktur

            // Kayıt listesini boş olarak oluşturur
            _enrollments = new List<Enrollment>(); // Başlangıçta kayıt ilişkisi yoktur
        } // UniversityManager yapıcı metodunun sonu

        // ========================= STUDENT OPERATIONS =========================

        // RegisterBachelorStudent: Yeni bir lisans öğrencisi kaydeder
        public BachelorStudent RegisterBachelorStudent(string id, string fullName, int age, string degreeProgram, int studyYear) // UI'dan gelen girdileri alır
        {
            // Aynı kimlikle başka öğrenci var mı diye kontrol eder
            if (FindStudentById(id) != null) // Yinelenen kimlikleri reddeder
            {
                // Yinelenen kimlikte listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Student Id '{id}' already exists."); // Başarısız işlem state'i bozmaz
            } // if bloğunun sonu

            // Doğrulanmış BachelorStudent nesnesini oluşturur (property doğrulamaları burada tetiklenir)
            BachelorStudent student = new BachelorStudent(id, fullName, age, degreeProgram, studyYear); // Geçersiz alanlar ArgumentException üretir

            // Başarılı oluşturmadan sonra listeye ekler
            _students.Add(student); // Yalnızca geçerli nesne eklenir

            // Oluşturulan öğrenciyi çağırana döndürür
            return student; // UI başarı mesajı için nesneyi kullanabilir
        } // RegisterBachelorStudent metodunun sonu

        // RegisterMasterStudent: Yeni bir yüksek lisans öğrencisi kaydeder
        public MasterStudent RegisterMasterStudent(string id, string fullName, int age, string degreeProgram, string thesisTopic) // UI'dan gelen girdileri alır
        {
            // Aynı kimlikle başka öğrenci var mı diye kontrol eder
            if (FindStudentById(id) != null) // Yinelenen kimlikleri reddeder
            {
                // Yinelenen kimlikte listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Student Id '{id}' already exists."); // Başarısız işlem state'i bozmaz
            } // if bloğunun sonu

            // Doğrulanmış MasterStudent nesnesini oluşturur
            MasterStudent student = new MasterStudent(id, fullName, age, degreeProgram, thesisTopic); // Geçersiz alanlar ArgumentException üretir

            // Başarılı oluşturmadan sonra listeye ekler
            _students.Add(student); // Yalnızca geçerli nesne eklenir

            // Oluşturulan öğrenciyi çağırana döndürür
            return student; // UI başarı mesajı için nesneyi kullanabilir
        } // RegisterMasterStudent metodunun sonu

        // FindStudentById: Kimliğe göre öğrenciyi arar; yoksa null döner
        public Student? FindStudentById(string id) // Arama yardımcısı
        {
            // Aranan kimlik boşsa arama yapılmaz
            if (string.IsNullOrWhiteSpace(id)) // Geçersiz arama anahtarını erken reddeder
            {
                // Boş anahtar için null döndürür
                return null; // Bulunamadı anlamında
            } // if bloğunun sonu

            // Karşılaştırma için kimliği normalize eder
            string normalizedId = id.Trim(); // Baş/son boşlukları kaldırır

            // Listedeki her öğrenciyi tek tek kontrol eder (basit foreach; karmaşık LINQ yok)
            foreach (Student student in _students) // Okunabilir döngü ile arama
            {
                // Kimlikler büyük/küçük harf duyarsız karşılaştırılır
                if (string.Equals(student.Id, normalizedId, StringComparison.OrdinalIgnoreCase)) // Eşleşme kontrolü
                {
                    // Bulunan öğrenci nesnesini döndürür
                    return student; // Arama başarılı
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Eşleşme yoksa null döner
            return null; // Öğrenci bulunamadı
        } // FindStudentById metodunun sonu

        // GetAllStudents: Tüm öğrencilerin kopya listesini döndürür (dış müdahaleyi azaltır)
        public List<Student> GetAllStudents() // Listeleme ekranı için
        {
            // Orijinal listenin sığ kopyasını döndürür
            return new List<Student>(_students); // Dışarıdan orijinal listenin Clear edilmesini engeller
        } // GetAllStudents metodunun sonu

        // GetStudentCount: Kayıtlı öğrenci sayısını döndürür
        public int GetStudentCount() // Özet bilgi için
        {
            // Listedeki öğrenci adedini döndürür
            return _students.Count; // Count özelliği kullanılır
        } // GetStudentCount metodunun sonu

        // GetStudentsByProgram: Belirtilen derece programına göre öğrencileri filtreler
        public List<Student> GetStudentsByProgram(string degreeProgram) // Programa göre listeleme için
        {
            // Boş program adı ile arama yapılmasını reddeder
            if (string.IsNullOrWhiteSpace(degreeProgram)) // Geçersiz filtre anahtarı
            {
                // Açıklayıcı hata fırlatır; sistem state'i değişmez
                throw new ArgumentException("Degree program cannot be empty."); // UI bu mesajı gösterir
            } // if bloğunun sonu

            // Aranan program adını kırpılmış halde tutar
            string targetProgram = degreeProgram.Trim(); // Tutarlı karşılaştırma için

            // Sonucu biriktirmek için yeni bir liste oluşturur
            List<Student> matches = new List<Student>(); // Yalnızca eşleşen öğrenciler

            // Tüm öğrencileri tek tek kontrol eder
            foreach (Student student in _students) // Basit foreach taraması
            {
                // Program adını büyük/küçük harf duyarsız karşılaştırır
                if (string.Equals(student.DegreeProgram, targetProgram, StringComparison.OrdinalIgnoreCase)) // Eşleşme kontrolü
                {
                    // Eşleşen öğrenciyi sonuç listesine ekler
                    matches.Add(student); // Filtre sonucu büyür
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Filtrelenmiş listeyi çağırana döndürür
            return matches; // Boş liste de geçerli bir sonuçtur
        } // GetStudentsByProgram metodunun sonu

        // RemoveStudent: Öğrenciyi yalnızca aktif kaydı yoksa siler
        public void RemoveStudent(string studentId) // Güvenli silme işlemi
        {
            // Silinecek öğrenciyi bulur
            Student? student = FindStudentById(studentId); // Önce varlık kontrolü

            // Öğrenci yoksa işlemi reddeder
            if (student == null) // Bulunamayan kimlik
            {
                // Listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Student '{studentId}' was not found."); // State korunur
            } // if bloğunun sonu

            // Öğrencinin aktif kaydı var mı diye kontrol eder
            int enrollmentCount = CountEnrollmentsForStudent(student.Id); // İlişkili kayıt sayısı

            // Aktif kayıt varken silmeye izin vermez
            if (enrollmentCount > 0) // Veri bütünlüğü koruması
            {
                // Önce drop edilmesi gerektiğini bildirir
                throw new InvalidOperationException($"Cannot remove student '{student.Id}' while {enrollmentCount} enrollment(s) exist. Drop enrollments first."); // State korunur
            } // if bloğunun sonu

            // Güvenli koşullarda öğrenciyi listeden çıkarır
            _students.Remove(student); // Yalnızca ilişki yoksa silinir
        } // RemoveStudent metodunun sonu

        // ========================= COURSE OPERATIONS =========================

        // CreateCourse: Benzersiz kod, başlık ve pozitif kontenjan ile ders oluşturur
        public Course CreateCourse(string id, string title, int capacity) // UI girdilerini alır
        {
            // Aynı ders kodu zaten var mı diye kontrol eder
            if (FindCourseById(id) != null) // Yinelenen ders kodunu reddeder
            {
                // Yinelenen kodda listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Course code '{id}' already exists."); // State korunur
            } // if bloğunun sonu

            // Doğrulanmış Course nesnesini oluşturur
            Course course = new Course(id, title, capacity); // Capacity <= 0 burada ArgumentException üretir

            // Başarılı oluşturmadan sonra listeye ekler
            _courses.Add(course); // Yalnızca geçerli ders eklenir

            // Oluşturulan dersi çağırana döndürür
            return course; // UI başarı mesajı için
        } // CreateCourse metodunun sonu

        // FindCourseById: Ders koduna göre dersi arar; yoksa null döner
        public Course? FindCourseById(string id) // Arama yardımcısı
        {
            // Aranan kod boşsa arama yapılmaz
            if (string.IsNullOrWhiteSpace(id)) // Geçersiz arama anahtarı
            {
                // Boş anahtar için null döndürür
                return null; // Bulunamadı
            } // if bloğunun sonu

            // Karşılaştırma için kodu normalize eder
            string normalizedId = id.Trim(); // Baş/son boşlukları kaldırır

            // Listedeki her dersi tek tek kontrol eder
            foreach (Course course in _courses) // Basit foreach araması
            {
                // Ders kodları büyük/küçük harf duyarsız karşılaştırılır
                if (string.Equals(course.Id, normalizedId, StringComparison.OrdinalIgnoreCase)) // Eşleşme kontrolü
                {
                    // Bulunan ders nesnesini döndürür
                    return course; // Arama başarılı
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Eşleşme yoksa null döner
            return null; // Ders bulunamadı
        } // FindCourseById metodunun sonu

        // GetAllCourses: Tüm derslerin kopya listesini döndürür
        public List<Course> GetAllCourses() // Listeleme ekranı için
        {
            // Orijinal listenin sığ kopyasını döndürür
            return new List<Course>(_courses); // Dış müdahaleyi azaltır
        } // GetAllCourses metodunun sonu

        // GetCourseCount: Tanımlı ders sayısını döndürür
        public int GetCourseCount() // Özet bilgi için
        {
            // Listedeki ders adedini döndürür
            return _courses.Count; // Count özelliği kullanılır
        } // GetCourseCount metodunun sonu

        // GetRemainingSeats: Bir ders için kalan kontenjanı hesaplar
        public int GetRemainingSeats(string courseId) // UI'da Remaining seats göstermek için
        {
            // Dersi bulur
            Course? course = FindCourseById(courseId); // Varlık kontrolü

            // Ders yoksa hesaplama yapılamaz
            if (course == null) // Geçersiz ders kodu
            {
                // Listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Course '{courseId}' was not found."); // State korunur
            } // if bloğunun sonu

            // Bu derse ait mevcut kayıt sayısını sayar
            int enrolledCount = CountEnrollmentsForCourse(course.Id); // Dolu koltuk sayısı

            // Kalan koltuğu kontenjan - dolu olarak hesaplar
            int remaining = course.Capacity - enrolledCount; // Negatif olmamalı (iş kuralları bunu korur)

            // Hesaplanan kalan koltuğu döndürür
            return remaining; // UI göstergesi için
        } // GetRemainingSeats metodunun sonu

        // RemoveCourse: Dersi yalnızca aktif kaydı yoksa siler
        public void RemoveCourse(string courseId) // Güvenli silme işlemi
        {
            // Silinecek dersi bulur
            Course? course = FindCourseById(courseId); // Önce varlık kontrolü

            // Ders yoksa işlemi reddeder
            if (course == null) // Bulunamayan kod
            {
                // Listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Course '{courseId}' was not found."); // State korunur
            } // if bloğunun sonu

            // Derse ait aktif kayıt var mı diye kontrol eder
            int enrollmentCount = CountEnrollmentsForCourse(course.Id); // İlişkili kayıt sayısı

            // Aktif kayıt varken silmeye izin vermez
            if (enrollmentCount > 0) // Veri bütünlüğü koruması
            {
                // Önce drop edilmesi gerektiğini bildirir
                throw new InvalidOperationException($"Cannot remove course '{course.Id}' while {enrollmentCount} enrollment(s) exist. Drop enrollments first."); // State korunur
            } // if bloğunun sonu

            // Güvenli koşullarda dersi listeden çıkarır
            _courses.Remove(course); // Yalnızca ilişki yoksa silinir
        } // RemoveCourse metodunun sonu

        // ========================= ENROLLMENT OPERATIONS =========================

        // EnrollStudent: Öğrenciyi derse kaydeder; tüm iş kurallarını doğrular
        public Enrollment EnrollStudent(string studentId, string courseId) // UI'dan kimlikleri alır
        {
            // Öğrencinin sistemde var olduğunu doğrular
            Student? student = FindStudentById(studentId); // Öğrenci varlık kontrolü

            // Öğrenci yoksa kaydı reddeder
            if (student == null) // Geçersiz öğrenci kimliği
            {
                // Hiçbir listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Student '{studentId}' was not found."); // State korunur
            } // if bloğunun sonu

            // Dersin sistemde var olduğunu doğrular
            Course? course = FindCourseById(courseId); // Ders varlık kontrolü

            // Ders yoksa kaydı reddeder
            if (course == null) // Geçersiz ders kodu
            {
                // Hiçbir listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Course '{courseId}' was not found."); // State korunur
            } // if bloğunun sonu

            // Aynı öğrenci aynı derse daha önce kayıtlı mı diye kontrol eder
            if (IsAlreadyEnrolled(student.Id, course.Id)) // Çift kayıt engeli
            {
                // Yinelenen kayıtta listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Student '{student.Id}' is already enrolled in course '{course.Id}'."); // State korunur
            } // if bloğunun sonu

            // Dersin kalan kontenjanını hesaplar
            int remainingSeats = course.Capacity - CountEnrollmentsForCourse(course.Id); // Kalan koltuk

            // Kontenjan doluysa kaydı reddeder
            if (remainingSeats <= 0) // Capacity iş kuralı
            {
                // Dolu derste listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Course '{course.Id}' is full. No remaining seats."); // State korunur
            } // if bloğunun sonu

            // Öğrencinin mevcut ders sayısını hesaplar
            int studentCourseCount = CountEnrollmentsForStudent(student.Id); // Mevcut yük

            // Maksimum ders limitine ulaşıldıysa kaydı reddeder
            if (studentCourseCount >= MaxCoursesPerStudent) // Aşırı yüklenme engeli
            {
                // Limit aşıldığında listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"Student '{student.Id}' already reached the maximum of {MaxCoursesPerStudent} courses."); // State korunur
            } // if bloğunun sonu

            // Benzersiz bir kayıt kimliği üretir
            string enrollmentId = $"ENR-{_enrollments.Count + 1:D4}-{DateTime.Now:HHmmss}"; // Basit, okunabilir kimlik

            // Tüm doğrulamalar geçtiği için Enrollment nesnesini oluşturur
            Enrollment enrollment = new Enrollment(enrollmentId, student, course, DateTime.Now); // Association nesnesi

            // Başarılı kaydı listeye ekler
            _enrollments.Add(enrollment); // Yalnızca kurallar geçtiyse state değişir

            // Oluşturulan kaydı çağırana döndürür
            return enrollment; // UI başarı mesajı için
        } // EnrollStudent metodunun sonu

        // DropEnrollment: Belirli öğrenci-ders kaydını düşürür
        public void DropEnrollment(string studentId, string courseId) // UI'dan kimlikleri alır
        {
            // Silinecek kayıt nesnesini tutar
            Enrollment? target = null; // Başlangıçta bulunamadı kabul edilir

            // Tüm kayıtları tarayarak eşleşeni arar
            foreach (Enrollment enrollment in _enrollments) // Basit foreach araması
            {
                // Hem öğrenci hem ders kimliği eşleşiyor mu diye bakar
                bool sameStudent = string.Equals(enrollment.Student.Id, studentId, StringComparison.OrdinalIgnoreCase); // Öğrenci eşleşmesi
                bool sameCourse = string.Equals(enrollment.Course.Id, courseId, StringComparison.OrdinalIgnoreCase); // Ders eşleşmesi

                // Her iki uç da eşleşiyorsa hedef bulundu demektir
                if (sameStudent && sameCourse) // Association eşleşmesi
                {
                    // Hedef kaydı işaretler
                    target = enrollment; // Sonra listeden çıkarılacak

                    // İlk eşleşmede aramayı bitirir
                    break; // Gereksiz taramayı durdurur
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Hedef kayıt yoksa işlemi reddeder
            if (target == null) // Bulunamayan enrollment
            {
                // Listeyi değiştirmeden hata fırlatır
                throw new InvalidOperationException($"No enrollment found for student '{studentId}' in course '{courseId}'."); // State korunur
            } // if bloğunun sonu

            // Bulunan kaydı listeden çıkarır
            _enrollments.Remove(target); // Başarılı drop işlemi
        } // DropEnrollment metodunun sonu

        // IsAlreadyEnrolled: Öğrencinin derse zaten kayıtlı olup olmadığını kontrol eder
        public bool IsAlreadyEnrolled(string studentId, string courseId) // Yardımcı doğrulama
        {
            // Tüm kayıtları tarar
            foreach (Enrollment enrollment in _enrollments) // Basit foreach
            {
                // Öğrenci ve ders kimliklerini karşılaştırır
                bool sameStudent = string.Equals(enrollment.Student.Id, studentId, StringComparison.OrdinalIgnoreCase); // Öğrenci eşleşmesi
                bool sameCourse = string.Equals(enrollment.Course.Id, courseId, StringComparison.OrdinalIgnoreCase); // Ders eşleşmesi

                // Her iki kimlik de eşleşiyorsa zaten kayıtlıdır
                if (sameStudent && sameCourse) // Çift kayıt durumu
                {
                    // Kayıtlı olduğunu bildirir
                    return true; // Duplicate enrollment
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Eşleşme yoksa kayıtlı değildir
            return false; // Yeni kayda izin verilebilir
        } // IsAlreadyEnrolled metodunun sonu

        // CountEnrollmentsForStudent: Bir öğrencinin kayıt sayısını hesaplar
        public int CountEnrollmentsForStudent(string studentId) // İş kuralı ve raporlama için
        {
            // Sayacı sıfırdan başlatır
            int count = 0; // Başlangıç değeri

            // Tüm kayıtları dolaşır
            foreach (Enrollment enrollment in _enrollments) // Basit foreach sayımı
            {
                // Kayıt bu öğrenciye ait mi diye bakar
                if (string.Equals(enrollment.Student.Id, studentId, StringComparison.OrdinalIgnoreCase)) // Öğrenci eşleşmesi
                {
                    // Sayacı bir artırır
                    count = count + 1; // Açık ve savunuya uygun artırma
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Hesaplanan sayıyı döndürür
            return count; // Öğrencinin ders yükü
        } // CountEnrollmentsForStudent metodunun sonu

        // CountEnrollmentsForCourse: Bir dersin dolu koltuk sayısını hesaplar
        public int CountEnrollmentsForCourse(string courseId) // Kontenjan ve raporlama için
        {
            // Sayacı sıfırdan başlatır
            int count = 0; // Başlangıç değeri

            // Tüm kayıtları dolaşır
            foreach (Enrollment enrollment in _enrollments) // Basit foreach sayımı
            {
                // Kayıt bu derse ait mi diye bakar
                if (string.Equals(enrollment.Course.Id, courseId, StringComparison.OrdinalIgnoreCase)) // Ders eşleşmesi
                {
                    // Sayacı bir artırır
                    count = count + 1; // Açık ve savunuya uygun artırma
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Hesaplanan sayıyı döndürür
            return count; // Dolu koltuk sayısı
        } // CountEnrollmentsForCourse metodunun sonu

        // GetAllEnrollments: Tüm kayıtların kopya listesini döndürür
        public List<Enrollment> GetAllEnrollments() // Listeleme ekranı için
        {
            // Orijinal listenin sığ kopyasını döndürür
            return new List<Enrollment>(_enrollments); // Dış müdahaleyi azaltır
        } // GetAllEnrollments metodunun sonu

        // GetEnrollmentsByStudent: Belirli bir öğrencinin kayıtlarını döndürür
        public List<Enrollment> GetEnrollmentsByStudent(string studentId) // Öğrenci ders özeti için
        {
            // Sonuç listesini oluşturur
            List<Enrollment> result = new List<Enrollment>(); // Filtre sonucu

            // Tüm kayıtları tarar
            foreach (Enrollment enrollment in _enrollments) // Basit foreach filtreleme
            {
                // Kayıt hedef öğrenciye aitse sonuca ekler
                if (string.Equals(enrollment.Student.Id, studentId, StringComparison.OrdinalIgnoreCase)) // Öğrenci eşleşmesi
                {
                    // Eşleşen kaydı sonuç listesine ekler
                    result.Add(enrollment); // Rapor satırı
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Filtrelenmiş listeyi döndürür
            return result; // Öğrencinin enrollment listesi
        } // GetEnrollmentsByStudent metodunun sonu

        // GetEnrollmentsByCourse: Belirli bir dersin kayıtlarını (roster) döndürür
        public List<Enrollment> GetEnrollmentsByCourse(string courseId) // Ders roster'ı için
        {
            // Sonuç listesini oluşturur
            List<Enrollment> result = new List<Enrollment>(); // Filtre sonucu

            // Tüm kayıtları tarar
            foreach (Enrollment enrollment in _enrollments) // Basit foreach filtreleme
            {
                // Kayıt hedef derse aitse sonuca ekler
                if (string.Equals(enrollment.Course.Id, courseId, StringComparison.OrdinalIgnoreCase)) // Ders eşleşmesi
                {
                    // Eşleşen kaydı sonuç listesine ekler
                    result.Add(enrollment); // Roster satırı
                } // if bloğunun sonu
            } // foreach döngüsünün sonu

            // Filtrelenmiş listeyi döndürür
            return result; // Ders enrollment listesi
        } // GetEnrollmentsByCourse metodunun sonu

        // GetEnrollmentCount: Toplam kayıt sayısını döndürür
        public int GetEnrollmentCount() // Özet bilgi için
        {
            // Listedeki kayıt adedini döndürür
            return _enrollments.Count; // Count özelliği kullanılır
        } // GetEnrollmentCount metodunun sonu
    } // UniversityManager sınıfının sonu
} // StudentManagementSystem.Services isim alanının sonu
