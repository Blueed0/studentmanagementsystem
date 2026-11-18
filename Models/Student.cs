using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Models
{
    // Student: Tüm öğrenci türlerinin ortak özelliklerini tutan soyut (abstract) temel sınıf
    // abstract olduğu için doğrudan new Student() yapılamaz; Bachelor/Master üzerinden kullanılır
    public abstract class Student : IIdentifiable
    {
        // _id: Öğrencinin benzersiz kimliğini saklayan private alan (encapsulation)
        private string _id = string.Empty;

        // _fullName: Öğrencinin ad-soyad bilgisini saklayan private alan
        private string _fullName = string.Empty;

        // _age: Öğrencinin yaşını saklayan private alan
        private int _age;

        // _degreeProgram: Öğrencinin bölüm/program bilgisini saklayan private alan
        private string _degreeProgram = string.Empty;

        // Id: Kimliği dışarıya okunabilir açar; sadece sınıf içinde atanabilir (private set)
        public string Id
        {
            get { return _id; }
            private set
            {
                // Boş kimlik kabul edilmez
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Student Id cannot be empty.");
                }

                // Baş/son boşlukları temizleyerek saklar
                _id = value.Trim();
            }
        }

        // FullName: Öğrenci adını okur/yazar; boş değerleri reddeder
        public string FullName
        {
            get { return _fullName; }
            set
            {
                // Ad zorunlu alandır
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Full name cannot be empty.");
                }

                _fullName = value.Trim();
            }
        }

        // Age: Yaşı doğrular; brief kuralı: 15-65 (dahil)
        public int Age
        {
            get { return _age; }
            set
            {
                // Aralık dışı yaşları reddeder
                if (value < 15 || value > 65)
                {
                    throw new ArgumentException("Age must be between 15 and 65 (inclusive).");
                }

                _age = value;
            }
        }

        // DegreeProgram: Bölüm bilgisini doğrular; boş olamaz
        public string DegreeProgram
        {
            get { return _degreeProgram; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Degree program cannot be empty.");
                }

                _degreeProgram = value.Trim();
            }
        }

        // Student yapıcısı: Ortak alanları property'ler üzerinden başlatır (doğrulama tetiklenir)
        // protected: Sadece türetilmiş sınıflar (Bachelor/Master) çağırabilir
        protected Student(string id, string fullName, int age, string degreeProgram)
        {
            Id = id;
            FullName = fullName;
            Age = age;
            DegreeProgram = degreeProgram;
        }

        // GetAcademicLevel: Alt sınıfların akademik seviyeyi bildirmesini zorunlu kılar (abstract)
        public abstract string GetAcademicLevel();

        // GetStudentDetails: Ortak öğrenci özetini üretir; alt sınıflar override edebilir (virtual/polimorfizm)
        public virtual string GetStudentDetails()
        {
            return $"Id: {Id} | Name: {FullName} | Age: {Age} | Program: {DegreeProgram} | Level: {GetAcademicLevel()}";
        }

        // ToString: Nesne yazdırılırken anlamlı metin göstermek için
        public override string ToString()
        {
            return GetStudentDetails();
        }
    }
}
