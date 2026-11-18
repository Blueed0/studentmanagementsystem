namespace StudentManagementSystem.Models
{
    // MasterStudent: Yüksek lisans öğrencisini temsil eder; Student sınıfından kalıtım alır
    public class MasterStudent : Student
    {
        // _thesisTopic: Yüksek lisans tez konusunu saklar
        private string _thesisTopic = string.Empty;

        // ThesisTopic: Tez konusunu doğrulayarak okur/yazar
        public string ThesisTopic
        {
            get { return _thesisTopic; }
            set
            {
                // Tez konusu boş olamaz
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Thesis topic cannot be empty.");
                }

                _thesisTopic = value.Trim();
            }
        }

        // MasterStudent yapıcısı: Ortak alanları base'e, tez konusunu kendine atar
        public MasterStudent(string id, string fullName, int age, string degreeProgram, string thesisTopic)
            : base(id, fullName, age, degreeProgram)
        {
            ThesisTopic = thesisTopic;
        }

        // GetAcademicLevel: Polimorfizm - bu tür için "Master" döner
        public override string GetAcademicLevel()
        {
            return "Master";
        }

        // GetStudentDetails: Ortak detaya tez konusunu ekler
        public override string GetStudentDetails()
        {
            return $"{base.GetStudentDetails()} | Thesis: {ThesisTopic}";
        }
    }
}
