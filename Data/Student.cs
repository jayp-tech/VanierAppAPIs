namespace VanierAppAPIs.Data
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentEmail { get; set; }
        public int UserID { get; set; }
        public User User { get; set; }
    }
}
