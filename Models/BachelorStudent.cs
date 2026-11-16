namespace StudentManagementSystem.Models // Lisans öğrencisi modelinin bulunduğu isim alanı
{
    // BachelorStudent: Student soyut sınıfından türeyen lisans öğrencisi sınıfı
    public class BachelorStudent : Student // Kalıtım ile ortak öğrenci davranışını yeniden kullanır
    {
        // _studyYear: Lisans öğrencisinin bulunduğu sınıf yılını tutan özel alan
        private int _studyYear; // 1–4 aralığında olmalıdır; gerçek değer constructor'da atanır

        // StudyYear: Lisans yılı bilgisini doğrulayarak dışarı açan özellik
        public int StudyYear // 1, 2, 3 veya 4 olabilir
        {
            get { return _studyYear; } // Private alandaki yılı okur
            set // Atama sırasında aralık doğrulaması yapar
            {
                // Yıl 1'den küçük veya 4'ten büyükse geçersizdir
                if (value < 1 || value > 4) // Dahil sınırlar dışını reddeder
                {
                    // Geçersiz yıl için ArgumentException fırlatılır
                    throw new ArgumentException("Study year must be between 1 and 4 (inclusive)."); // Net iş kuralı mesajı
                } // if bloğunun sonu

                // Geçerli yılı private alana yazar
                _studyYear = value; // Doğrulanmış değeri saklar
            } // StudyYear setter'ının sonu
        } // StudyYear özelliğinin sonu

        // BachelorStudent yapıcı metodu: Temel Student alanlarını ve StudyYear'ı başlatır
        public BachelorStudent(string id, string fullName, int age, string degreeProgram, int studyYear) // public: dışarıdan örnek oluşturulabilir
            : base(id, fullName, age, degreeProgram) // base(...): üst sınıf yapıcısını çağırarak ortak alanları kurar
        {
            // StudyYear property setter'ı üzerinden yılı doğrula ve ata
            StudyYear = studyYear; // 1–4 dışı değerler burada reddedilir
        } // BachelorStudent yapıcı metodunun sonu

        // GetAcademicLevel: Lisans seviyesini polimorfik olarak döndürür
        public override string GetAcademicLevel() // abstract metodun zorunlu uygulaması
        {
            // Bu öğrenci türünün akademik seviyesini sabit metin olarak bildirir
            return "Bachelor"; // Polimorfizm örneği: runtime'da bu sürüm çağrılır
        } // GetAcademicLevel metodunun sonu

        // GetStudentDetails: Lisansa özgü StudyYear bilgisini de ekleyerek ayrıntı üretir
        public override string GetStudentDetails() // virtual metodu ezer (override)
        {
            // Üst sınıf özetine lisans yılı bilgisini ekler
            return $"{base.GetStudentDetails()} | Study Year: {StudyYear}"; // base. ile ortak detayı yeniden kullanır
        } // GetStudentDetails metodunun sonu
    } // BachelorStudent sınıfının sonu
} // StudentManagementSystem.Models isim alanının sonu
