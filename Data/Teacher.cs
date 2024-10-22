namespace VanierAppAPIs.Data
{
    public class Teacher
    {
        public int TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public User User { get; set; }
    }
}
