namespace VanierAppAPIs.Data
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseBlock { get; set; }
        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }
    }
}
