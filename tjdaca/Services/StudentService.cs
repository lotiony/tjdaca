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
            var student = _dbContext.AcaStudents.FirstOrDefault(x => x.StuIdx == id);
            if (student != null)
            {
                _dbContext.AcaStudents.Remove(student);
                _dbContext.SaveChanges();
            }
        }

        public AcaStudents GetStudentById(int id)
        {
            var student = _dbContext.AcaStudents.SingleOrDefault(x => x.StuIdx == id);
            return student;
        }

        public List<AcaStudents> GetStudents()
        {
            return _dbContext.AcaStudents.OrderByDescending(x=> x.StuIdx).ToList();
        }

        public async Task<List<AcaStudents>> GetStudentsAsync()
        {
            return await Task.Run(() =>
            {
                return _dbContext.AcaStudents.OrderByDescending(x => x.StuIdx).ToList();
            });
        }

        public void SaveStudent(AcaStudents students)
        {
            if (students.StuIdx == 0) _dbContext.AcaStudents.Add(students);
            else _dbContext.AcaStudents.Update(students);
            _dbContext.SaveChanges();
        }
        
    }
}
