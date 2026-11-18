using StudentManagementSystem.Interfaces;

namespace StudentManagementSystem.Models
{
    public class Course : IIdentifiable
    {
        private string _id = string.Empty;
        private string _title = string.Empty;
        private int _capacity;

        public string Id
        {
            get { return _id; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Course Id (code) cannot be empty.");
                }

                _id = value.Trim();
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Course title cannot be empty.");
                }

                _title = value.Trim();
            }
        }

        public int Capacity
        {
            get { return _capacity; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Course capacity must be greater than 0.");
                }

                _capacity = value;
            }
        }

        public Course(string id, string title, int capacity)
        {
            Id = id;
            Title = title;
            Capacity = capacity;
        }

        public string GetCourseDetails()
        {
            return $"Code: {Id} | Title: {Title} | Capacity: {Capacity}";
        }

        public override string ToString()
        {
            return GetCourseDetails();
        }
    }
}
