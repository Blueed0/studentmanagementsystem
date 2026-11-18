using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Models
{
    // Course: Üniversitedeki bir dersi temsil eder
    public class Course : IIdentifiable
    {
        // _id: Ders kodunu saklayan private alan (örnek: CS101)
        private string _id = string.Empty;

        // _title: Ders başlığını saklayan private alan
        private string _title = string.Empty;

        // _capacity: Dersin maksimum kontenjanını saklayan private alan
        private int _capacity;

        // Id: Ders kodunu dışarıya açar; sadece yapıcıda atanır
        public string Id
        {
            get { return _id; }
            private set
            {
                // Ders kodu boş olamaz
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Course Id (code) cannot be empty.");
                }

                _id = value.Trim();
            }
        }

        // Title: Ders başlığını doğrular
        public string Title
        {
            get { return _title; }
            set
            {
                // Başlık zorunludur
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Course title cannot be empty.");
                }

                _title = value.Trim();
            }
        }

        // Capacity: Kontenjanı doğrular; brief kuralı: sıfırdan büyük olmalı
        public int Capacity
        {
            get { return _capacity; }
            set
            {
                // 0 veya negatif kapasite kabul edilmez
                if (value <= 0)
                {
                    throw new ArgumentException("Course capacity must be greater than 0.");
                }

                _capacity = value;
            }
        }

        // Course yapıcısı: Kod, başlık ve kapasiteyi doğrulayarak oluşturur
        public Course(string id, string title, int capacity)
        {
            Id = id;
            Title = title;
            Capacity = capacity;
        }

        // GetCourseDetails: Ders bilgisini tek satır halinde döndürür
        public string GetCourseDetails()
        {
            return $"Code: {Id} | Title: {Title} | Capacity: {Capacity}";
        }

        // ToString: Konsolda Course yazdırılırken detay metnini kullanır
        public override string ToString()
        {
            return GetCourseDetails();
        }
    }
}
