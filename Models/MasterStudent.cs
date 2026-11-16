namespace StudentManagementSystem.Models // Yüksek lisans öğrencisi modelinin bulunduğu isim alanı
{
    // MasterStudent: Student soyut sınıfından türeyen yüksek lisans öğrencisi sınıfı
    public class MasterStudent : Student // Kalıtım ile ortak öğrenci davranışını yeniden kullanır
    {
        // _thesisTopic: Yüksek lisans tez konusunu tutan özel alan
        private string _thesisTopic = string.Empty; // Nullable uyarısını önlemek için güvenli başlangıç

        // ThesisTopic: Tez konusunu doğrulayarak dışarı açan özellik
        public string ThesisTopic // Zorunlu metin alanı
        {
            get { return _thesisTopic; } // Private alandaki tez konusunu okur
            set // Atama sırasında boş değer kontrolü yapar
            {
                // Tez konusu boş/null/whitespace ise reddet
                if (string.IsNullOrWhiteSpace(value)) // Zorunlu alan doğrulaması
                {
                    // Geçersiz tez konusu için ArgumentException fırlatılır
                    throw new ArgumentException("Thesis topic cannot be empty."); // Net hata mesajı
                } // if bloğunun sonu

                // Geçerli tez konusunu kırpılmış halde saklar
                _thesisTopic = value.Trim(); // Tutarlı metin saklama
            } // ThesisTopic setter'ının sonu
        } // ThesisTopic özelliğinin sonu

        // MasterStudent yapıcı metodu: Temel Student alanlarını ve ThesisTopic'i başlatır
        public MasterStudent(string id, string fullName, int age, string degreeProgram, string thesisTopic) // public: dışarıdan örnek oluşturulabilir
            : base(id, fullName, age, degreeProgram) // base(...): üst sınıf yapıcısını çağırarak ortak alanları kurar
        {
            // ThesisTopic property setter'ı üzerinden konuyu doğrula ve ata
            ThesisTopic = thesisTopic; // Boş konular burada reddedilir
        } // MasterStudent yapıcı metodunun sonu

        // GetAcademicLevel: Yüksek lisans seviyesini polimorfik olarak döndürür
        public override string GetAcademicLevel() // abstract metodun zorunlu uygulaması
        {
            // Bu öğrenci türünün akademik seviyesini sabit metin olarak bildirir
            return "Master"; // Polimorfizm örneği: runtime'da bu sürüm çağrılır
        } // GetAcademicLevel metodunun sonu

        // GetStudentDetails: Yüksek lisansa özgü tez konusunu da ekleyerek ayrıntı üretir
        public override string GetStudentDetails() // virtual metodu ezer (override)
        {
            // Üst sınıf özetine tez konusu bilgisini ekler
            return $"{base.GetStudentDetails()} | Thesis: {ThesisTopic}"; // base. ile ortak detayı yeniden kullanır
        } // GetStudentDetails metodunun sonu
    } // MasterStudent sınıfının sonu
} // StudentManagementSystem.Models isim alanının sonu
