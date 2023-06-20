using System.Linq.Dynamic.Core;
using tjdaca.Data;
using tjdaca.Pages;

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

        public List<AcaStuExamScore> GetExamScore(int id)
        {
            var examScore = _dbContext.AcaStuExamScore.Where(x => x.StuIdx == id).ToList();
            return examScore;
        }

        public AcaStuSchoolScore GetSchoolScore(int id)
        {
            var schoolScore = _dbContext.AcaStuSchoolScore.SingleOrDefault(x => x.StuIdx == id);
            return schoolScore ?? new AcaStuSchoolScore(id);

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

        public void SaveExamScore(AcaStuExamScore examScore)
        {
            if (examScore.Idx == 0) _dbContext.AcaStuExamScore.Add(examScore);
            else _dbContext.AcaStuExamScore.Update(examScore);
            _dbContext.SaveChanges();
        }
        public void SaveSchoolScore(AcaStuSchoolScore schoolScore)
        {
            if (schoolScore.Idx == 0) _dbContext.AcaStuSchoolScore.Add(schoolScore);
            else _dbContext.AcaStuSchoolScore.Update(schoolScore);
            _dbContext.SaveChanges();
        }

        public void DeleteExamScore(AcaStuExamScore examScore)
        {
            _dbContext.AcaStuExamScore.Remove(examScore);
            _dbContext.SaveChanges();
        }

    }
}
