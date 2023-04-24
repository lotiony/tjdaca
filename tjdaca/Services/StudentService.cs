using tjdaca.Data;

namespace tjdaca.Services
{
    public class StudentService : IStudentService
    {
        private readonly tjdacaContext _dbContext;

        public StudentService(tjdacaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteStudent(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(x => x.StuIdx == id);
            if (student != null)
            {
                _dbContext.Students.Remove(student);
                _dbContext.SaveChanges();
            }
        }

        public Students GetStudentById(int id)
        {
            var student = _dbContext.Students.SingleOrDefault(x => x.StuIdx == id);
            return student;
        }

        public List<Students> GetStudents()
        {
            return _dbContext.Students.ToList();
        }

        public void SaveStudent(Students students)
        {
            if (students.StuIdx == 0) _dbContext.Students.Add(students);
            else _dbContext.Students.Update(students);
            _dbContext.SaveChanges();
        }
    }
}
