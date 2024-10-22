using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VanierAppAPIs.Data;
using System.Data.SqlClient;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    private readonly VanierDBContext _context;
    private string _connectionString;
    public StudentsController(VanierDBContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection");  // Get connection string from appsettings.json
    }

    // GET: api/Students/Names
    [HttpGet("Names")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        // Fetch all student names
        var studentNames = await _context.Students
                                         .Select(s => s.StudentName)
                                         .ToListAsync();

        return Ok(studentNames);                     
    }

    // GET: api/Students/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetStudentById(int id)
    {
        // Query the student by ID
        var student = await _context.Students
                                    .Where(s => s.StudentID == id)
                                    .Select(s => new { s.StudentName, s.StudentEmail })
                                    .FirstOrDefaultAsync();

        // If no student is found, return 404 NotFound
        if (student == null)
        {
            return NotFound();
        }

        // Return the student name and email
        return Ok(student);
    }

    [HttpGet("student-by-username/{username}")]
    public async Task<ActionResult<string>> GetStudentNameByUsername(string username)
    {
        // Query to join Students and Users table and get the student name by username
        var studentName = await _context.Students
                                        .Join(_context.Users,
                                              student => student.UserID,
                                              user => user.Id,
                                              (student, user) => new { student, user })
                                        .Where(su => su.user.Username == username)
                                        .Select(su => su.student.StudentName)
                                        .FirstOrDefaultAsync();

        // If no student is found, return 404
        if (studentName == null)
        {
            return NotFound();
        }

        // Return the student name
        return Ok(studentName);
    }

    //private string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=VanierDB;Integrated Security=True";


    // GET: api/Students/student-by-username/{username}
    [HttpGet("student-by-username-by-rawSQL/{username}")]
    public ActionResult<string> GetStudentNameByUsernameByRawSQL(string username)
    {
        string studentName = null;

        // Use ADO.NET to execute raw SQL
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Define the SQL query to join Users and Students
            string sql = @"SELECT s.StudentName 
                               FROM Students s
                               INNER JOIN Users u ON s.UserID = u.Id
                               WHERE u.Username = @Username";

            // Use SqlCommand to execute the SQL query
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Username", username);

                // Use SqlDataReader to read the result
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Get the StudentName from the result
                        studentName = reader["StudentName"].ToString();
                    }
                }
            }
        }

        // If no student is found, return 404
        if (studentName == null)
        {
            return NotFound();
        }

        // Return the student name
        return Ok(studentName);
    }

    // GET: api/Students/{studentID}/courses
    [HttpGet("{studentID}/courses")]
    public ActionResult<IEnumerable<object>> GetCoursesByStudentID(int studentID)
    {
        List<object> courses = new List<object>();

        // Use ADO.NET to execute raw SQL
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();

            // Define the SQL query to get courses by StudentID
            string sql = @"SELECT Courses.CourseName, Courses.CourseBlock
                               FROM Courses
                               JOIN StudentCourses ON Courses.CourseID = StudentCourses.CourseID
                               WHERE StudentCourses.StudentID = @StudentID";

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                // Add the parameter to the SQL query
                command.Parameters.AddWithValue("@StudentID", studentID);

                // Use SqlDataReader to read the result
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Create an anonymous object and populate it with data from the database
                        var course = new
                        {
                            CourseName = reader["CourseName"].ToString(),
                            CourseBlock = reader["CourseBlock"].ToString()
                        };

                        // Add the course to the list of courses
                        courses.Add(course);
                    }
                }
            }
        }

        // If no courses are found, return 404
        if (courses.Count == 0)
        {
            return NotFound("No courses found for this student.");
        }

        // Return the list of courses
        return Ok(courses);
    }
}
