using StudentManagementSystem.Interfaces; // IIdentifiable arayüzünü kullanabilmek için gerekli using

namespace StudentManagementSystem.Models // Öğrenci ve ilgili model sınıflarının isim alanı
{
    // Student: Tüm öğrenci türlerinin ortak davranışını tanımlayan soyut (abstract) temel sınıf
    public abstract class Student : IIdentifiable // IIdentifiable sözleşmesini uygular; doğrudan örneği oluşturulamaz
    {
        // _id: Öğrencinin benzersiz kimliğini tutan özel (private) alan
        private string _id = string.Empty; // Başlangıç değeri; gerçek değer constructor'da property ile atanır

        // _fullName: Öğrencinin tam adını tutan özel alan
        private string _fullName = string.Empty; // Nullable uyarısını önlemek için güvenli başlangıç

        // _age: Öğrencinin yaşını tutan özel alan
        private int _age; // 15–65 aralığı property setter'da doğrulanır

        // _degreeProgram: Öğrencinin bölüm/program bilgisini tutan özel alan
        private string _degreeProgram = string.Empty; // Boş olamaz; property üzerinden korunur

        // Id: Benzersiz öğrenci kimliğini kapsüllenmiş şekilde dışarı açan özellik
        public string Id // IIdentifiable arayüzünün Id sözleşmesini karşılar
        {
            get { return _id; } // Private alandaki değeri güvenli şekilde okur
            private set // Id yalnızca sınıf içinde (constructor üzerinden) atanabilir
            {
                // Kimlik boş, null veya yalnızca boşluklardan oluşuyorsa reddet
                if (string.IsNullOrWhiteSpace(value)) // Geçersiz kimlik girişini yakalar
                {
                    // Açıklayıcı hata fırlatarak sistem durumunun bozulmasını önler
                    throw new ArgumentException("Student Id cannot be empty."); // İngilizce hata mesajı (UI dili)
                } // if bloğunun sonu

                // Geçerli değeri kırpılmış (trim) halde private alana yazar
                _id = value.Trim(); // Baş/son boşlukları temizleyerek tutarlı kimlik saklar
            } // Id setter'ının sonu
        } // Id özelliğinin sonu

        // FullName: Öğrencinin adını okuma/yazma ile dışarı açan özellik
        public string FullName // Kapsüllenmiş ad bilgisi
        {
            get { return _fullName; } // Private alandaki adı döndürür
            set // Dışarıdan güncellemeye izin verir; ancak doğrulama uygular
            {
                // Ad boş veya yalnızca boşluk ise reddet
                if (string.IsNullOrWhiteSpace(value)) // Geçersiz ad girişini kontrol eder
                {
                    // Ad zorunlu olduğu için ArgumentException fırlatılır
                    throw new ArgumentException("Full name cannot be empty."); // Kullanıcıya net hata mesajı
                } // if bloğunun sonu

                // Geçerli adı kırpılmış halde saklar
                _fullName = value.Trim(); // Boşlukları temizleyerek tutarlı veri sağlar
            } // FullName setter'ının sonu
        } // FullName özelliğinin sonu

        // Age: Öğrenci yaşını doğrulayarak yöneten özellik
        public int Age // 15–65 dahil aralıkta olmalıdır
        {
            get { return _age; } // Private yaş alanını okur
            set // Yaş atamasında iş kuralı doğrulaması yapar
            {
                // Yaş 15'ten küçük veya 65'ten büyükse geçersiz kabul edilir
                if (value < 15 || value > 65) // Dahil sınırlar dışındaki değerleri reddeder
                {
                    // Yaş kuralı ihlalinde açıklayıcı istisna fırlatılır
                    throw new ArgumentException("Age must be between 15 and 65 (inclusive)."); // Sözlü savunmaya uygun mesaj
                } // if bloğunun sonu

                // Geçerli yaşı private alana yazar
                _age = value; // Doğrulanmış değeri saklar
            } // Age setter'ının sonu
        } // Age özelliğinin sonu

        // DegreeProgram: Öğrencinin bölüm/program bilgisini kapsüller
        public string DegreeProgram // Boş olamaz
        {
            get { return _degreeProgram; } // Program adını okur
            set // Program atamasında boş değer kontrolü yapar
            {
                // Program adı boş/null/whitespace ise reddet
                if (string.IsNullOrWhiteSpace(value)) // Zorunlu alan doğrulaması
                {
                    // Geçersiz program girişi için istisna fırlatılır
                    throw new ArgumentException("Degree program cannot be empty."); // Net hata mesajı
                } // if bloğunun sonu

                // Geçerli program adını kırpılmış halde saklar
                _degreeProgram = value.Trim(); // Tutarlı metin saklama
            } // DegreeProgram setter'ının sonu
        } // DegreeProgram özelliğinin sonu

        // Student yapıcı metodu: Ortak alanları başlatır ve doğrulamayı tetikler
        protected Student(string id, string fullName, int age, string degreeProgram) // protected: yalnızca türetilmiş sınıflar çağırabilir
        {
            // Id property setter'ı üzerinden kimliği doğrula ve ata
            Id = id; // Boş kimlikler burada ArgumentException üretir

            // FullName property setter'ı üzerinden adı doğrula ve ata
            FullName = fullName; // Boş adlar burada reddedilir

            // Age property setter'ı üzerinden yaşı doğrula ve ata
            Age = age; // 15–65 dışı değerler burada reddedilir

            // DegreeProgram property setter'ı üzerinden programı doğrula ve ata
            DegreeProgram = degreeProgram; // Boş program adları burada reddedilir
        } // Student yapıcı metodunun sonu

        // GetAcademicLevel: Türetilmiş sınıfların akademik seviyeyi polimorfik olarak bildirmesini zorunlu kılar
        public abstract string GetAcademicLevel(); // abstract: her alt sınıf kendi sürümünü override etmelidir

        // GetStudentDetails: Öğrenci ayrıntılarını polimorfik biçimde üreten sanal metot
        public virtual string GetStudentDetails() // virtual: alt sınıflar genişletebilir / değiştirebilir
        {
            // Temel öğrenci bilgilerini tek satırlık metin olarak döndürür
            return $"Id: {Id} | Name: {FullName} | Age: {Age} | Program: {DegreeProgram} | Level: {GetAcademicLevel()}"; // Ortak özet formatı
        } // GetStudentDetails metodunun sonu

        // ToString: Konsolda nesneyi yazdırırken anlamlı metin gösterir
        public override string ToString() // Object.ToString'i ezer
        {
            // Detay metodunu yeniden kullanarak tutarlı çıktı sağlar
            return GetStudentDetails(); // Polimorfik detay metnini kullanır
        } // ToString metodunun sonu
    } // Student sınıfının sonu
} // StudentManagementSystem.Models isim alanının sonu
