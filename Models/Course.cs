using StudentManagementSystem.Interfaces; // IIdentifiable arayüzünü kullanmak için gerekli using

namespace StudentManagementSystem.Models // Ders modelinin bulunduğu isim alanı
{
    // Course: Üniversitedeki bir dersi temsil eden sınıf
    public class Course : IIdentifiable // IIdentifiable sözleşmesini uygular
    {
        // _id: Ders kodunu (benzersiz kimlik) tutan özel alan
        private string _id = string.Empty; // Başlangıç değeri; gerçek kod constructor'da atanır

        // _title: Ders başlığını tutan özel alan
        private string _title = string.Empty; // Nullable uyarısını önlemek için güvenli başlangıç

        // _capacity: Dersin maksimum kontenjanını tutan özel alan
        private int _capacity; // Sıkı pozitif olmalıdır (> 0)

        // Id: Ders kodunu kapsüllenmiş şekilde dışarı açan özellik
        public string Id // IIdentifiable.Id sözleşmesini karşılar
        {
            get { return _id; } // Private ders kodunu okur
            private set // Kod yalnızca yapıcı üzerinden atanabilir
            {
                // Ders kodu boş/null/whitespace ise reddet
                if (string.IsNullOrWhiteSpace(value)) // Geçersiz kod kontrolü
                {
                    // Boş kod için ArgumentException fırlatılır
                    throw new ArgumentException("Course Id (code) cannot be empty."); // Net hata mesajı
                } // if bloğunun sonu

                // Geçerli kodu kırpılmış halde saklar
                _id = value.Trim(); // Tutarlı kimlik saklama
            } // Id setter'ının sonu
        } // Id özelliğinin sonu

        // Title: Ders başlığını doğrulayarak yöneten özellik
        public string Title // Boş olamaz
        {
            get { return _title; } // Private başlığı okur
            set // Atamada boş değer kontrolü yapar
            {
                // Başlık boş/null/whitespace ise reddet
                if (string.IsNullOrWhiteSpace(value)) // Zorunlu alan doğrulaması
                {
                    // Geçersiz başlık için ArgumentException fırlatılır
                    throw new ArgumentException("Course title cannot be empty."); // Net hata mesajı
                } // if bloğunun sonu

                // Geçerli başlığı kırpılmış halde saklar
                _title = value.Trim(); // Tutarlı metin saklama
            } // Title setter'ının sonu
        } // Title özelliğinin sonu

        // Capacity: Ders kontenjanını doğrulayarak yöneten özellik
        public int Capacity // Sıkı pozitif (> 0) olmalıdır
        {
            get { return _capacity; } // Private kontenjanı okur
            set // Atamada pozitiflik kontrolü yapar
            {
                // Kontenjan 0 veya negatifse geçersizdir
                if (value <= 0) // > 0 kuralını uygular
                {
                    // Geçersiz kontenjan için ArgumentException fırlatılır
                    throw new ArgumentException("Course capacity must be greater than 0."); // Net iş kuralı mesajı
                } // if bloğunun sonu

                // Geçerli kontenjanı private alana yazar
                _capacity = value; // Doğrulanmış değeri saklar
            } // Capacity setter'ının sonu
        } // Capacity özelliğinin sonu

        // Course yapıcı metodu: Ders alanlarını doğrulayarak başlatır
        public Course(string id, string title, int capacity) // public: dışarıdan örnek oluşturulabilir
        {
            // Id property setter'ı üzerinden ders kodunu doğrula ve ata
            Id = id; // Boş kodlar burada reddedilir

            // Title property setter'ı üzerinden başlığı doğrula ve ata
            Title = title; // Boş başlıklar burada reddedilir

            // Capacity property setter'ı üzerinden kontenjanı doğrula ve ata
            Capacity = capacity; // <= 0 değerler burada reddedilir
        } // Course yapıcı metodunun sonu

        // GetCourseDetails: Ders bilgilerini tek satırlık metin olarak döndürür
        public string GetCourseDetails() // Listeleme ve arama ekranlarında kullanılır
        {
            // Ders kodu, başlık ve kontenjanı biçimlendirilmiş metin olarak üretir
            return $"Code: {Id} | Title: {Title} | Capacity: {Capacity}"; // Tutarlı özet formatı
        } // GetCourseDetails metodunun sonu

        // ToString: Konsolda Course yazdırılırken anlamlı metin gösterir
        public override string ToString() // Object.ToString'i ezer
        {
            // Detay metodunu yeniden kullanarak tutarlı çıktı sağlar
            return GetCourseDetails(); // Aynı formatı kullanır
        } // ToString metodunun sonu
    } // Course sınıfının sonu
} // StudentManagementSystem.Models isim alanının sonu
