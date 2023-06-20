using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IStudentService
    {
        List<AcaStudents> GetStudents();
        Task<List<AcaStudents>> GetStudentsAsync();
        AcaStudents GetStudentById(int id);
        AcaStuSchoolScore GetSchoolScore(int id);
        List<AcaStuExamScore> GetExamScore(int id);
        void SaveStudent(AcaStudents students);
        void DeleteStudent(int id);
        void SaveSchoolScore(AcaStuSchoolScore schoolScore);
        void SaveExamScore(AcaStuExamScore examScore);
        void DeleteExamScore(AcaStuExamScore examScore);
    }
}
