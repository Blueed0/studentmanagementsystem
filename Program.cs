using StudentManagementSystem.Models; // Student, Course, Enrollment modellerini kullanmak için
using StudentManagementSystem.Services; // UniversityManager servisini kullanmak için

namespace StudentManagementSystem // Uygulamanın ana isim alanı
{
    // Program: Konsol menüsünü ve kullanıcı etkileşimini yöneten giriş sınıfı
    public class Program // .NET konsol uygulamasının giriş noktası
    {
        // _manager: Tüm iş kurallarını yöneten paylaşılan servis örneği
        private static UniversityManager _manager = new UniversityManager(); // Uygulama boyunca tek registry kullanılır

        // Main: Programın başlangıç metodu; menü döngüsünü çalıştırır
        public static void Main(string[] args) // args: komut satırı argümanları (bu projede kullanılmaz)
        {
            // Konsol başlığını ayarlar
            Console.Title = "01. Student Management System"; // Pencere başlığında proje adını gösterir

            // Demo için örnek verileri yükler (canlı sunumda menüyü hızlı göstermek için)
            SeedSampleData(); // Başlangıçta birkaç öğrenci ve ders ekler

            // Uygulama çalışmaya devam ettiği sürece true kalan bayrak
            bool isRunning = true; // Menü döngüsünü kontrol eder

            // Ana menü döngüsü: kullanıcı çıkana kadar tekrar eder
            while (isRunning) // true olduğu sürece menüyü gösterir
            {
                // Ana menüyü ekrana çizer
                ShowMainMenu(); // Kutu çizimli başlık ve seçenekler

                // Kullanıcıdan menü seçimini okur
                string choice = ReadRequired("Select an option"); // Boş girişleri reddeder

                // Seçime göre ilgili işlemi çalıştırır
                switch (choice) // Menü yönlendirmesi
                {
                    case "1": // Register Student seçeneği
                        HandleRegisterStudent(); // Öğrenci kayıt akışını başlatır
                        break; // switch'ten çıkar

                    case "2": // Create Course seçeneği
                        HandleCreateCourse(); // Ders oluşturma akışını başlatır
                        break; // switch'ten çıkar

                    case "3": // Enroll Student seçeneği
                        HandleEnrollStudent(); // Kayıt (enrollment) akışını başlatır
                        break; // switch'ten çıkar

                    case "4": // Drop Enrollment seçeneği
                        HandleDropEnrollment(); // Kayıt düşürme akışını başlatır
                        break; // switch'ten çıkar

                    case "5": // List Students seçeneği
                        HandleListStudents(); // Tüm öğrencileri listeler
                        break; // switch'ten çıkar

                    case "6": // List Courses seçeneği
                        HandleListCourses(); // Tüm dersleri kalan koltuklarla listeler
                        break; // switch'ten çıkar

                    case "7": // List Enrollments seçeneği
                        HandleListEnrollments(); // Tüm kayıtları listeler
                        break; // switch'ten çıkar

                    case "8": // Search Student seçeneği
                        HandleSearchStudent(); // Öğrenci arama akışını başlatır
                        break; // switch'ten çıkar

                    case "9": // Search Course seçeneği
                        HandleSearchCourse(); // Ders arama akışını başlatır
                        break; // switch'ten çıkar

                    case "10": // View Student Courses seçeneği
                        HandleViewStudentCourses(); // Öğrencinin derslerini gösterir
                        break; // switch'ten çıkar

                    case "11": // View Course Roster seçeneği
                        HandleViewCourseRoster(); // Ders roster'ını gösterir
                        break; // switch'ten çıkar

                    case "12": // Remove Student seçeneği
                        HandleRemoveStudent(); // Öğrenci silme akışını başlatır
                        break; // switch'ten çıkar

                    case "13": // Remove Course seçeneği
                        HandleRemoveCourse(); // Ders silme akışını başlatır
                        break; // switch'ten çıkar

                    case "14": // Display Students by Program seçeneği
                        HandleDisplayStudentsByProgram(); // Programa göre öğrenci listesini gösterir
                        break; // switch'ten çıkar

                    case "0": // Exit seçeneği
                        isRunning = false; // Döngüyü sonlandırmak için bayrağı false yapar
                        PrintSuccess("Exiting Student Management System. Goodbye!"); // Veda mesajı
                        break; // switch'ten çıkar

                    default: // Tanınmayan seçim
                        PrintError("Invalid menu option. Please select a valid number."); // Hata mesajı gösterir
                        Pause(); // Kullanıcının mesajı okuması için bekler
                        break; // switch'ten çıkar
                } // switch bloğunun sonu
            } // while döngüsünün sonu
        } // Main metodunun sonu

        // ========================= MENU & UI HELPERS =========================

        // ShowMainMenu: Ana menüyü kutu çizimleriyle gösterir
        private static void ShowMainMenu() // Her döngü başında çağrılır
        {
            // Ekranı temizleyerek düzenli bir görünüm sağlar
            try // Gerçek konsolda temizler; yönlendirilmiş çıktıda çökmemesi için korunur
            {
                // Önceki menü çıktılarını siler
                Console.Clear(); // Canlı demoda temiz ekran sağlar
            } // try bloğunun sonu
            catch (IOException) // Konsol yoksa / çıktı yönlendirilmişse Clear başarısız olabilir
            {
                // Sessizce devam eder; uygulama yine de çalışır
            } // catch bloğunun sonu

            // Üst kutu çizgisini yazdırır
            PrintHeader("01. STUDENT MANAGEMENT SYSTEM"); // Profesyonel başlık

            // Özet istatistik satırını yazdırır
            Console.WriteLine($"  Students: {_manager.GetStudentCount()}   |   Courses: {_manager.GetCourseCount()}   |   Enrollments: {_manager.GetEnrollmentCount()}"); // Canlı sayaçlar

            // Ayırıcı çizgi çizer
            PrintDivider(); // Görsel düzen için

            // Menü seçeneklerini hizalı şekilde listeler
            Console.WriteLine("  1.  Register Student"); // Öğrenci kaydı
            Console.WriteLine("  2.  Create Course"); // Ders oluşturma
            Console.WriteLine("  3.  Enroll Student in Course"); // Derse kayıt
            Console.WriteLine("  4.  Drop Enrollment"); // Kayıt düşürme
            Console.WriteLine("  5.  List All Students"); // Öğrenci listesi
            Console.WriteLine("  6.  List All Courses"); // Ders listesi
            Console.WriteLine("  7.  List All Enrollments"); // Kayıt listesi
            Console.WriteLine("  8.  Search Student by Id"); // Öğrenci arama
            Console.WriteLine("  9.  Search Course by Code"); // Ders arama
            Console.WriteLine(" 10.  View Courses of a Student"); // Öğrencinin dersleri
            Console.WriteLine(" 11.  View Course Roster"); // Ders roster'ı
            Console.WriteLine(" 12.  Remove Student"); // Öğrenci silme
            Console.WriteLine(" 13.  Remove Course"); // Ders silme
            Console.WriteLine(" 14.  Display Students by Program"); // Programa göre öğrenci listesi
            Console.WriteLine("  0.  Exit"); // Çıkış

            // Alt ayırıcı çizgi çizer
            PrintDivider(); // Menü sonu görseli
        } // ShowMainMenu metodunun sonu

        // PrintHeader: Kutu çizimli başlık basar
        private static void PrintHeader(string title) // title: gösterilecek başlık metni
        {
            // Üst çerçeve çizgisini yazdırır
            Console.WriteLine("+==============================================================+"); // Box-drawing üst kenar

            // Başlığı ortalanmış gibi gösteren satırı yazdırır
            Console.WriteLine($"|  {title.PadRight(58)}|"); // Başlık satırı

            // Alt çerçeve çizgisini yazdırır
            Console.WriteLine("+==============================================================+"); // Box-drawing alt kenar
        } // PrintHeader metodunun sonu

        // PrintSection: Alt bölüm başlığı basar
        private static void PrintSection(string title) // title: bölüm adı
        {
            // Boş satır bırakarak bölümü ayırır
            Console.WriteLine(); // Görsel boşluk

            // Bölüm başlığını çerçeve içinde yazdırır
            Console.WriteLine($"----- {title} -----"); // Alt başlık formatı
        } // PrintSection metodunun sonu

        // PrintDivider: Yatay ayırıcı çizgi basar
        private static void PrintDivider() // Düzen için yardımcı
        {
            // Ayırıcı çizgiyi yazdırır
            Console.WriteLine("--------------------------------------------------------------"); // Görsel ayırıcı
        } // PrintDivider metodunun sonu

        // PrintSuccess: Başarı mesajını [SUCCESS] etiketiyle basar
        private static void PrintSuccess(string message) // message: başarı metni
        {
            // Konsol rengini yeşile çevirir
            Console.ForegroundColor = ConsoleColor.Green; // Başarı rengi

            // Başarı mesajını yazdırır
            Console.WriteLine($"[SUCCESS] {message}"); // Standart başarı etiketi

            // Konsol rengini varsayılana döndürür
            Console.ResetColor(); // Renk sızıntısını önler
        } // PrintSuccess metodunun sonu

        // PrintError: Hata mesajını [ERROR] etiketiyle basar
        private static void PrintError(string message) // message: hata metni
        {
            // Konsol rengini kırmızıya çevirir
            Console.ForegroundColor = ConsoleColor.Red; // Hata rengi

            // Hata mesajını yazdırır
            Console.WriteLine($"[ERROR] {message}"); // Standart hata etiketi

            // Konsol rengini varsayılana döndürür
            Console.ResetColor(); // Renk sızıntısını önler
        } // PrintError metodunun sonu

        // PrintInfo: Bilgi mesajını [INFO] etiketiyle basar
        private static void PrintInfo(string message) // message: bilgi metni
        {
            // Konsol rengini cyan'a çevirir
            Console.ForegroundColor = ConsoleColor.Cyan; // Bilgi rengi

            // Bilgi mesajını yazdırır
            Console.WriteLine($"[INFO] {message}"); // Standart bilgi etiketi

            // Konsol rengini varsayılana döndürür
            Console.ResetColor(); // Renk sızıntısını önler
        } // PrintInfo metodunun sonu

        // Pause: Kullanıcının devam etmek için Enter'a basmasını bekler
        private static void Pause() // Menüye dönmeden önce bekler
        {
            // Kullanıcıya talimat yazar
            Console.WriteLine(); // Boş satır
            Console.Write("Press Enter to continue..."); // Bekleme istemi

            // Enter tuşunu bekler
            Console.ReadLine(); // Kullanıcı girdisini okur ve yok sayar
        } // Pause metodunun sonu

        // ReadRequired: Boş olamayacak metin girdisi okur
        private static string ReadRequired(string label) // label: istem metni
        {
            // Geçerli giriş alınana kadar döner
            while (true) // Doğrulama döngüsü
            {
                // Etiketli istemi yazdırır
                Console.Write($"  > {label}: "); // Hizalı giriş istemi

                // Kullanıcı satırını okur
                string? input = Console.ReadLine(); // null olabilir

                // Giriş boş değilse kabul eder
                if (!string.IsNullOrWhiteSpace(input)) // Zorunlu alan kontrolü
                {
                    // Kırpılmış değeri döndürür
                    return input.Trim(); // Baş/son boşlukları temizler
                } // if bloğunun sonu

                // Boş girişte hata gösterir ve tekrar ister
                PrintError("Input cannot be empty."); // Kullanıcıyı bilgilendirir
            } // while döngüsünün sonu
        } // ReadRequired metodunun sonu

        // ReadIntInRange: Belirli aralıkta tam sayı okur
        private static int ReadIntInRange(string label, int min, int max) // min/max dahil sınırlar
        {
            // Geçerli sayı alınana kadar döner
            while (true) // Doğrulama döngüsü
            {
                // Etiketli istemi yazdırır
                Console.Write($"  > {label} ({min}-{max}): "); // Aralık bilgisini de gösterir

                // Kullanıcı satırını okur
                string? input = Console.ReadLine(); // null olabilir

                // Metni tam sayıya çevirmeyi dener
                if (int.TryParse(input, out int value)) // Geçerli sayı mı?
                {
                    // Değer aralık içindeyse kabul eder
                    if (value >= min && value <= max) // Dahil sınır kontrolü
                    {
                        // Geçerli sayıyı döndürür
                        return value; // Başarılı giriş
                    } // iç if bloğunun sonu

                    // Aralık dışı değer için hata gösterir
                    PrintError($"Value must be between {min} and {max}."); // Aralık hatası
                } // dış if bloğunun sonu
                else // Sayıya çevrilemedi
                {
                    // Geçersiz sayı formatı için hata gösterir
                    PrintError("Please enter a valid integer."); // Format hatası
                } // else bloğunun sonu
            } // while döngüsünün sonu
        } // ReadIntInRange metodunun sonu

        // ReadPositiveInt: Sıkı pozitif (> 0) tam sayı okur
        private static int ReadPositiveInt(string label) // Capacity gibi alanlar için
        {
            // Geçerli sayı alınana kadar döner
            while (true) // Doğrulama döngüsü
            {
                // Etiketli istemi yazdırır
                Console.Write($"  > {label} (> 0): "); // Pozitiflik bilgisini gösterir

                // Kullanıcı satırını okur
                string? input = Console.ReadLine(); // null olabilir

                // Metni tam sayıya çevirmeyi dener
                if (int.TryParse(input, out int value)) // Geçerli sayı mı?
                {
                    // Değer 0'dan büyükse kabul eder
                    if (value > 0) // Sıkı pozitif kontrolü
                    {
                        // Geçerli sayıyı döndürür
                        return value; // Başarılı giriş
                    } // iç if bloğunun sonu

                    // Pozitif olmayan değer için hata gösterir
                    PrintError("Value must be greater than 0."); // İş kuralı hatası
                } // dış if bloğunun sonu
                else // Sayıya çevrilemedi
                {
                    // Geçersiz sayı formatı için hata gösterir
                    PrintError("Please enter a valid integer."); // Format hatası
                } // else bloğunun sonu
            } // while döngüsünün sonu
        } // ReadPositiveInt metodunun sonu

        // ========================= FEATURE HANDLERS =========================

        // HandleRegisterStudent: Lisans veya yüksek lisans öğrenci kaydı yapar
        private static void HandleRegisterStudent() // Menü seçeneği 1
        {
            // Bölüm başlığını yazdırır
            PrintSection("REGISTER STUDENT"); // Kullanıcıya bağlam verir

            // Öğrenci türünü sorar
            Console.WriteLine("  Student Type:"); // Tür seçimi başlığı
            Console.WriteLine("    B - Bachelor Student"); // Lisans seçeneği
            Console.WriteLine("    M - Master Student"); // Yüksek lisans seçeneği

            // Tür seçimini okur
            string type = ReadRequired("Enter type (B/M)").ToUpperInvariant(); // Büyük harfe çevirir

            // Ortak alanları okur
            string id = ReadRequired("Student Id"); // Benzersiz kimlik
            string fullName = ReadRequired("Full Name"); // Ad soyad
            int age = ReadIntInRange("Age", 15, 65); // Yaş aralığı doğrulaması
            string degreeProgram = ReadRequired("Degree Program"); // Bölüm/program

            // İşlemi try-catch ile korur; başarısız işlem state'i bozmaz
            try // Olası ArgumentException / InvalidOperationException yakalanır
            {
                // Lisans öğrencisi seçildiyse ilgili kayıt metodunu çağırır
                if (type == "B") // Bachelor yolu
                {
                    // Lisans yılı bilgisini okur
                    int studyYear = ReadIntInRange("Study Year", 1, 4); // 1–4 aralığı

                    // Servis üzerinden lisans öğrencisini kaydeder
                    BachelorStudent student = _manager.RegisterBachelorStudent(id, fullName, age, degreeProgram, studyYear); // İş kuralları serviste

                    // Başarı mesajı ve polimorfik detay yazdırır
                    PrintSuccess("Bachelor student registered successfully."); // Başarı etiketi
                    PrintInfo(student.GetStudentDetails()); // override edilmiş detay
                } // if bloğunun sonu
                else if (type == "M") // Master yolu
                {
                    // Tez konusu bilgisini okur
                    string thesisTopic = ReadRequired("Thesis Topic"); // Zorunlu alan

                    // Servis üzerinden yüksek lisans öğrencisini kaydeder
                    MasterStudent student = _manager.RegisterMasterStudent(id, fullName, age, degreeProgram, thesisTopic); // İş kuralları serviste

                    // Başarı mesajı ve polimorfik detay yazdırır
                    PrintSuccess("Master student registered successfully."); // Başarı etiketi
                    PrintInfo(student.GetStudentDetails()); // override edilmiş detay
                } // else if bloğunun sonu
                else // Tanınmayan tür
                {
                    // Geçersiz tür için hata gösterir
                    PrintError("Unknown student type. Please enter B or M."); // Kullanıcıyı yönlendirir
                } // else bloğunun sonu
            } // try bloğunun sonu
            catch (Exception ex) // Tüm beklenen doğrulama / iş kuralı hataları
            {
                // Hata mesajını [ERROR] ile gösterir; listeler değişmemiştir
                PrintError(ex.Message); // Servisten gelen açıklayıcı mesaj
            } // catch bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleRegisterStudent metodunun sonu

        // HandleCreateCourse: Yeni ders oluşturur
        private static void HandleCreateCourse() // Menü seçeneği 2
        {
            // Bölüm başlığını yazdırır
            PrintSection("CREATE COURSE"); // Kullanıcıya bağlam verir

            // Ders alanlarını okur
            string id = ReadRequired("Course Code"); // Benzersiz ders kodu
            string title = ReadRequired("Course Title"); // Ders başlığı
            int capacity = ReadPositiveInt("Capacity"); // > 0 kontenjan

            // İşlemi try-catch ile korur
            try // Olası hataları yakalar
            {
                // Servis üzerinden dersi oluşturur
                Course course = _manager.CreateCourse(id, title, capacity); // Yinelenen kod / geçersiz kapasite burada kontrol edilir

                // Başarı mesajı ve ders detayını yazdırır
                PrintSuccess("Course created successfully."); // Başarı etiketi
                PrintInfo(course.GetCourseDetails()); // Ders özeti
            } // try bloğunun sonu
            catch (Exception ex) // Doğrulama / iş kuralı hataları
            {
                // Hata mesajını gösterir; state bozulmamıştır
                PrintError(ex.Message); // Açıklayıcı hata
            } // catch bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleCreateCourse metodunun sonu

        // HandleEnrollStudent: Öğrenciyi derse kaydeder
        private static void HandleEnrollStudent() // Menü seçeneği 3
        {
            // Bölüm başlığını yazdırır
            PrintSection("ENROLL STUDENT IN COURSE"); // Kullanıcıya bağlam verir

            // Gerekli kimlikleri okur
            string studentId = ReadRequired("Student Id"); // Öğrenci kimliği
            string courseId = ReadRequired("Course Code"); // Ders kodu

            // İşlemi try-catch ile korur
            try // Kapasite, duplicate, varlık kontrolleri servistedir
            {
                // Servis üzerinden kaydı gerçekleştirir
                Enrollment enrollment = _manager.EnrollStudent(studentId, courseId); // Tüm iş kuralları burada doğrulanır

                // Başarı mesajı ve kayıt detayını yazdırır
                PrintSuccess("Enrollment completed successfully."); // Başarı etiketi
                PrintInfo(enrollment.GetEnrollmentDetails()); // Kayıt özeti

                // Kalan kontenjanı da gösterir
                int remaining = _manager.GetRemainingSeats(courseId); // Güncel kalan koltuk
                PrintInfo($"Remaining seats in {courseId}: {remaining}"); // Canlı gösterge
            } // try bloğunun sonu
            catch (Exception ex) // Başarısız kayıt
            {
                // Hata mesajını gösterir; hiçbir liste değişmemiştir
                PrintError(ex.Message); // Açıklayıcı hata
            } // catch bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleEnrollStudent metodunun sonu

        // HandleDropEnrollment: Öğrenci-ders kaydını düşürür
        private static void HandleDropEnrollment() // Menü seçeneği 4
        {
            // Bölüm başlığını yazdırır
            PrintSection("DROP ENROLLMENT"); // Kullanıcıya bağlam verir

            // Gerekli kimlikleri okur
            string studentId = ReadRequired("Student Id"); // Öğrenci kimliği
            string courseId = ReadRequired("Course Code"); // Ders kodu

            // İşlemi try-catch ile korur
            try // Bulunamayan kayıt burada yakalanır
            {
                // Servis üzerinden kaydı düşürür
                _manager.DropEnrollment(studentId, courseId); // Eşleşen enrollment silinir

                // Başarı mesajını yazdırır
                PrintSuccess($"Enrollment dropped for student '{studentId}' from course '{courseId}'."); // Başarı etiketi
            } // try bloğunun sonu
            catch (Exception ex) // Başarısız drop
            {
                // Hata mesajını gösterir
                PrintError(ex.Message); // Açıklayıcı hata
            } // catch bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleDropEnrollment metodunun sonu

        // HandleListStudents: Tüm öğrencileri polimorfik detaylarıyla listeler
        private static void HandleListStudents() // Menü seçeneği 5
        {
            // Bölüm başlığını yazdırır
            PrintSection("ALL STUDENTS"); // Kullanıcıya bağlam verir

            // Öğrenci listesinin kopyasını alır
            List<Student> students = _manager.GetAllStudents(); // Servisten güvenli kopya

            // Liste boşsa bilgi mesajı gösterir
            if (students.Count == 0) // Kayıt yok kontrolü
            {
                // Boş liste bilgisini yazdırır
                PrintInfo("No students registered yet."); // Bilgi mesajı
            } // if bloğunun sonu
            else // En az bir öğrenci var
            {
                // Satır numarasını 1'den başlatır
                int index = 1; // Hizalı liste için sayaç

                // Her öğrenciyi sırayla yazdırır
                foreach (Student student in students) // Polimorfik dolaşım
                {
                    // GetStudentDetails runtime'da Bachelor/Master sürümünü çağırır
                    Console.WriteLine($"  {index,2}. {student.GetStudentDetails()}"); // override sayesinde tür-özel detay

                    // Satır numarasını artırır
                    index = index + 1; // Sonraki satır için
                } // foreach döngüsünün sonu
            } // else bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleListStudents metodunun sonu

        // HandleListCourses: Tüm dersleri kalan koltuk göstergesiyle listeler
        private static void HandleListCourses() // Menü seçeneği 6
        {
            // Bölüm başlığını yazdırır
            PrintSection("ALL COURSES"); // Kullanıcıya bağlam verir

            // Ders listesinin kopyasını alır
            List<Course> courses = _manager.GetAllCourses(); // Servisten güvenli kopya

            // Liste boşsa bilgi mesajı gösterir
            if (courses.Count == 0) // Kayıt yok kontrolü
            {
                // Boş liste bilgisini yazdırır
                PrintInfo("No courses created yet."); // Bilgi mesajı
            } // if bloğunun sonu
            else // En az bir ders var
            {
                // Satır numarasını 1'den başlatır
                int index = 1; // Hizalı liste için sayaç

                // Her dersi sırayla yazdırır
                foreach (Course course in courses) // Ders dolaşımı
                {
                    // Kalan kontenjanı hesaplar
                    int remaining = _manager.GetRemainingSeats(course.Id); // Capacity - enrolled

                    // Dolu koltuk sayısını hesaplar
                    int enrolled = _manager.CountEnrollmentsForCourse(course.Id); // Mevcut kayıt sayısı

                    // Ders satırını hizalı ve kalan koltukla yazdırır
                    Console.WriteLine($"  {index,2}. {course.GetCourseDetails()} | Enrolled: {enrolled} | Remaining seats: {remaining}"); // Profesyonel satır

                    // Satır numarasını artırır
                    index = index + 1; // Sonraki satır için
                } // foreach döngüsünün sonu
            } // else bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleListCourses metodunun sonu

        // HandleListEnrollments: Tüm kayıt ilişkilerini listeler
        private static void HandleListEnrollments() // Menü seçeneği 7
        {
            // Bölüm başlığını yazdırır
            PrintSection("ALL ENROLLMENTS"); // Kullanıcıya bağlam verir

            // Kayıt listesinin kopyasını alır
            List<Enrollment> enrollments = _manager.GetAllEnrollments(); // Servisten güvenli kopya

            // Liste boşsa bilgi mesajı gösterir
            if (enrollments.Count == 0) // Kayıt yok kontrolü
            {
                // Boş liste bilgisini yazdırır
                PrintInfo("No enrollments found yet."); // Bilgi mesajı
            } // if bloğunun sonu
            else // En az bir kayıt var
            {
                // Satır numarasını 1'den başlatır
                int index = 1; // Hizalı liste için sayaç

                // Her kaydı sırayla yazdırır
                foreach (Enrollment enrollment in enrollments) // Kayıt dolaşımı
                {
                    // Kayıt detay satırını yazdırır
                    Console.WriteLine($"  {index,2}. {enrollment.GetEnrollmentDetails()}"); // Association özeti

                    // Satır numarasını artırır
                    index = index + 1; // Sonraki satır için
                } // foreach döngüsünün sonu
            } // else bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleListEnrollments metodunun sonu

        // HandleSearchStudent: Kimliğe göre öğrenci arar
        private static void HandleSearchStudent() // Menü seçeneği 8
        {
            // Bölüm başlığını yazdırır
            PrintSection("SEARCH STUDENT"); // Kullanıcıya bağlam verir

            // Aranacak kimliği okur
            string id = ReadRequired("Student Id"); // Arama anahtarı

            // Servis üzerinden öğrenciyi arar
            Student? student = _manager.FindStudentById(id); // Yoksa null döner

            // Öğrenci bulunduysa detayını yazdırır
            if (student != null) // Başarılı arama
            {
                // Başarı mesajı ve polimorfik detay gösterir
                PrintSuccess("Student found."); // Başarı etiketi
                PrintInfo(student.GetStudentDetails()); // Bachelor/Master detayı
            } // if bloğunun sonu
            else // Öğrenci bulunamadı
            {
                // Hata mesajı gösterir
                PrintError($"Student '{id}' was not found."); // Arama başarısız
            } // else bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleSearchStudent metodunun sonu

        // HandleSearchCourse: Koda göre ders arar
        private static void HandleSearchCourse() // Menü seçeneği 9
        {
            // Bölüm başlığını yazdırır
            PrintSection("SEARCH COURSE"); // Kullanıcıya bağlam verir

            // Aranacak ders kodunu okur
            string id = ReadRequired("Course Code"); // Arama anahtarı

            // Servis üzerinden dersi arar
            Course? course = _manager.FindCourseById(id); // Yoksa null döner

            // Ders bulunduysa detayını yazdırır
            if (course != null) // Başarılı arama
            {
                // Kalan kontenjanı hesaplar
                int remaining = _manager.GetRemainingSeats(course.Id); // Güncel kalan koltuk

                // Başarı mesajı ve ders detayını gösterir
                PrintSuccess("Course found."); // Başarı etiketi
                PrintInfo($"{course.GetCourseDetails()} | Remaining seats: {remaining}"); // Kalan koltuklu özet
            } // if bloğunun sonu
            else // Ders bulunamadı
            {
                // Hata mesajı gösterir
                PrintError($"Course '{id}' was not found."); // Arama başarısız
            } // else bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleSearchCourse metodunun sonu

        // HandleViewStudentCourses: Bir öğrencinin kayıtlı olduğu dersleri gösterir
        private static void HandleViewStudentCourses() // Menü seçeneği 10
        {
            // Bölüm başlığını yazdırır
            PrintSection("COURSES OF A STUDENT"); // Kullanıcıya bağlam verir

            // Öğrenci kimliğini okur
            string studentId = ReadRequired("Student Id"); // Hedef öğrenci

            // Önce öğrencinin varlığını kontrol eder
            Student? student = _manager.FindStudentById(studentId); // Varlık kontrolü

            // Öğrenci yoksa hata gösterir ve çıkar
            if (student == null) // Geçersiz kimlik
            {
                // Hata mesajı yazdırır
                PrintError($"Student '{studentId}' was not found."); // Bulunamadı

                // Bekleyip metoda son verir
                Pause(); // Kullanıcı mesajı okusun
                return; // Erken çıkış
            } // if bloğunun sonu

            // Öğrencinin kayıtlarını alır
            List<Enrollment> enrollments = _manager.GetEnrollmentsByStudent(student.Id); // Filtreli liste

            // Öğrenci özetini yazdırır
            PrintInfo(student.GetStudentDetails()); // Polimorfik öğrenci detayı
            PrintInfo($"Enrolled course count: {enrollments.Count}/{UniversityManager.MaxCoursesPerStudent}"); // Yük göstergesi

            // Kayıt yoksa bilgi mesajı gösterir
            if (enrollments.Count == 0) // Boş sonuç
            {
                // Bilgi mesajı yazdırır
                PrintInfo("This student is not enrolled in any courses."); // Boş durum
            } // if bloğunun sonu
            else // En az bir ders var
            {
                // Satır numarasını başlatır
                int index = 1; // Sayaç

                // Her kayıttaki dersi yazdırır
                foreach (Enrollment enrollment in enrollments) // Dolaşım
                {
                    // Ders satırını yazdırır
                    Console.WriteLine($"  {index,2}. {enrollment.Course.GetCourseDetails()} | Enrolled on: {enrollment.EnrollmentDate:yyyy-MM-dd}"); // Ders + tarih

                    // Sayacı artırır
                    index = index + 1; // Sonraki satır
                } // foreach döngüsünün sonu
            } // else bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleViewStudentCourses metodunun sonu

        // HandleViewCourseRoster: Bir derse kayıtlı öğrencileri gösterir
        private static void HandleViewCourseRoster() // Menü seçeneği 11
        {
            // Bölüm başlığını yazdırır
            PrintSection("COURSE ROSTER"); // Kullanıcıya bağlam verir

            // Ders kodunu okur
            string courseId = ReadRequired("Course Code"); // Hedef ders

            // Önce dersin varlığını kontrol eder
            Course? course = _manager.FindCourseById(courseId); // Varlık kontrolü

            // Ders yoksa hata gösterir ve çıkar
            if (course == null) // Geçersiz kod
            {
                // Hata mesajı yazdırır
                PrintError($"Course '{courseId}' was not found."); // Bulunamadı

                // Bekleyip metoda son verir
                Pause(); // Kullanıcı mesajı okusun
                return; // Erken çıkış
            } // if bloğunun sonu

            // Derse ait kayıtları alır
            List<Enrollment> enrollments = _manager.GetEnrollmentsByCourse(course.Id); // Roster listesi

            // Kalan kontenjanı hesaplar
            int remaining = _manager.GetRemainingSeats(course.Id); // Kalan koltuk

            // Ders özetini yazdırır
            PrintInfo(course.GetCourseDetails()); // Ders detayı
            PrintInfo($"Enrolled: {enrollments.Count} | Remaining seats: {remaining}"); // Doluluk göstergesi

            // Roster boşsa bilgi mesajı gösterir
            if (enrollments.Count == 0) // Boş roster
            {
                // Bilgi mesajı yazdırır
                PrintInfo("No students are enrolled in this course yet."); // Boş durum
            } // if bloğunun sonu
            else // En az bir öğrenci var
            {
                // Satır numarasını başlatır
                int index = 1; // Sayaç

                // Her kayıttaki öğrenciyi yazdırır
                foreach (Enrollment enrollment in enrollments) // Dolaşım
                {
                    // Öğrenci satırını polimorfik detayla yazdırır
                    Console.WriteLine($"  {index,2}. {enrollment.Student.GetStudentDetails()}"); // Bachelor/Master detayı

                    // Sayacı artırır
                    index = index + 1; // Sonraki satır
                } // foreach döngüsünün sonu
            } // else bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleViewCourseRoster metodunun sonu

        // HandleRemoveStudent: Öğrenciyi güvenli şekilde siler
        private static void HandleRemoveStudent() // Menü seçeneği 12
        {
            // Bölüm başlığını yazdırır
            PrintSection("REMOVE STUDENT"); // Kullanıcıya bağlam verir

            // Silinecek kimliği okur
            string studentId = ReadRequired("Student Id"); // Hedef öğrenci

            // İşlemi try-catch ile korur
            try // Aktif enrollment varken silme reddedilir
            {
                // Servis üzerinden öğrenciyi siler
                _manager.RemoveStudent(studentId); // İlişki yoksa silinir

                // Başarı mesajını yazdırır
                PrintSuccess($"Student '{studentId}' removed successfully."); // Başarı etiketi
            } // try bloğunun sonu
            catch (Exception ex) // Silme başarısız
            {
                // Hata mesajını gösterir; state bozulmamıştır
                PrintError(ex.Message); // Açıklayıcı hata
            } // catch bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleRemoveStudent metodunun sonu

        // HandleRemoveCourse: Dersi güvenli şekilde siler
        private static void HandleRemoveCourse() // Menü seçeneği 13
        {
            // Bölüm başlığını yazdırır
            PrintSection("REMOVE COURSE"); // Kullanıcıya bağlam verir

            // Silinecek ders kodunu okur
            string courseId = ReadRequired("Course Code"); // Hedef ders

            // İşlemi try-catch ile korur
            try // Aktif enrollment varken silme reddedilir
            {
                // Servis üzerinden dersi siler
                _manager.RemoveCourse(courseId); // İlişki yoksa silinir

                // Başarı mesajını yazdırır
                PrintSuccess($"Course '{courseId}' removed successfully."); // Başarı etiketi
            } // try bloğunun sonu
            catch (Exception ex) // Silme başarısız
            {
                // Hata mesajını gösterir; state bozulmamıştır
                PrintError(ex.Message); // Açıklayıcı hata
            } // catch bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleRemoveCourse metodunun sonu

        // HandleDisplayStudentsByProgram: Belirli bir programa kayıtlı öğrencileri listeler
        private static void HandleDisplayStudentsByProgram() // Menü seçeneği 14
        {
            // Bölüm başlığını yazdırır
            PrintSection("STUDENTS BY PROGRAM"); // Kullanıcıya bağlam verir

            // Filtrelenecek program adını okur
            string degreeProgram = ReadRequired("Degree Program"); // Örn. Computer Science

            // İşlemi try-catch ile korur
            try // Boş program adı ArgumentException üretebilir
            {
                // Servisten programa göre filtrelenmiş öğrencileri alır
                List<Student> students = _manager.GetStudentsByProgram(degreeProgram); // Case-insensitive eşleşme

                // Eşleşme yoksa bilgilendirir
                if (students.Count == 0) // Filtre sonucu boş
                {
                    // Kullanıcıya net bilgi verir
                    PrintInfo($"No students found for program '{degreeProgram}'."); // Boş sonuç
                } // if bloğunun sonu
                else // En az bir eşleşme var
                {
                    // Kaç öğrenci bulunduğunu yazar
                    PrintInfo($"Found {students.Count} student(s) in '{degreeProgram}':"); // Özet

                    // Satır numarasını 1'den başlatır
                    int index = 1; // Hizalı liste için sayaç

                    // Her eşleşen öğrenciyi polimorfik detayla yazdırır
                    foreach (Student student in students) // Bachelor/Master override çalışır
                    {
                        // Öğrenci ayrıntısını numaralı satır olarak basar
                        Console.WriteLine($"  {index,2}. {student.GetStudentDetails()}"); // Tür-özel detay

                        // Satır numarasını artırır
                        index = index + 1; // Sonraki satır
                    } // foreach döngüsünün sonu
                } // else bloğunun sonu
            } // try bloğunun sonu
            catch (Exception ex) // Geçersiz program girişi vb.
            {
                // Hata mesajını gösterir; state bozulmamıştır
                PrintError(ex.Message); // Açıklayıcı hata
            } // catch bloğunun sonu

            // Kullanıcının sonucu okuması için bekler
            Pause(); // Menüye dönmeden önce duraklatır
        } // HandleDisplayStudentsByProgram metodunun sonu

        // ========================= SAMPLE DATA =========================

        // SeedSampleData: Canlı demo için örnek verileri yükler
        private static void SeedSampleData() // Main başında bir kez çağrılır
        {
            // Örnek veri yüklemeyi try-catch ile korur
            try // Seed hataları uygulamayı düşürmesin
            {
                // Örnek lisans öğrencisi ekler
                _manager.RegisterBachelorStudent("S1001", "Ada Lovelace", 20, "Computer Science", 2); // Bachelor örneği

                // Örnek yüksek lisans öğrencisi ekler
                _manager.RegisterMasterStudent("S2001", "Alan Turing", 24, "Computer Science", "Computability Theory"); // Master örneği

                // İkinci lisans öğrencisi ekler
                _manager.RegisterBachelorStudent("S1002", "Grace Hopper", 21, "Software Engineering", 3); // Ek Bachelor

                // Örnek dersler oluşturur
                _manager.CreateCourse("CS101", "Introduction to Programming", 3); // Küçük kontenjanlı ders (kapasite demosu)
                _manager.CreateCourse("CS201", "Object-Oriented Programming", 40); // Ana ders
                _manager.CreateCourse("CS501", "Advanced Algorithms", 25); // Yüksek lisans odaklı ders

                // Birkaç örnek kayıt ekler
                _manager.EnrollStudent("S1001", "CS101"); // Ada -> CS101
                _manager.EnrollStudent("S1001", "CS201"); // Ada -> CS201
                _manager.EnrollStudent("S2001", "CS501"); // Alan -> CS501
                _manager.EnrollStudent("S1002", "CS201"); // Grace -> CS201
            } // try bloğunun sonu
            catch (Exception ex) // Seed sırasında beklenmeyen hata
            {
                // Seed hatasını bilgi olarak gösterir; uygulama yine de açılır
                PrintError($"Sample data could not be loaded: {ex.Message}"); // Demo verisi opsiyoneldir
            } // catch bloğunun sonu
        } // SeedSampleData metodunun sonu
    } // Program sınıfının sonu
} // StudentManagementSystem isim alanının sonu
