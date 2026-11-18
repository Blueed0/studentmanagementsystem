// Models isim alani: Veri modellerinin bulundugu katman
namespace StudentManagementSystem.Models
{
    // BachelorStudent: Lisans öğrencisini temsil eder; Student sınıfından kalıtım alır
    public class BachelorStudent : Student
    {
        // _studyYear: Lisans öğrencisinin sınıf yılı (1-4)
        private int _studyYear;

        // StudyYear: Yıl bilgisini doğrulayarak okur/yazar
        public int StudyYear
        {
            get { return _studyYear; }
            set
            {
                // Lisans yılı sadece 1 ile 4 arasında olabilir
                if (value < 1 || value > 4)
                {
                    throw new ArgumentException("Study year must be between 1 and 4 (inclusive).");
                }

                _studyYear = value;
            }
        }

        // BachelorStudent yapıcısı: Ortak alanları base'e, yıl bilgisini kendine atar
        public BachelorStudent(string id, string fullName, int age, string degreeProgram, int studyYear)
            : base(id, fullName, age, degreeProgram)
        {
            StudyYear = studyYear;
        }

        // GetAcademicLevel: Polimorfizm - bu tür için "Bachelor" döner
        public override string GetAcademicLevel()
        {
            return "Bachelor";
        }

        // GetStudentDetails: Ortak detaya lisans yılını ekler
        public override string GetStudentDetails()
        {
            return $"{base.GetStudentDetails()} | Study Year: {StudyYear}";
        }
    }
}
