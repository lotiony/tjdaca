using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IStudentService
    {
        List<AcaStudents> GetStudents();
        Task<List<AcaStudents>> GetStudentsAsync();
        AcaStudents GetStudentById(int id);
        void SaveStudent(AcaStudents students);
        void DeleteStudent(int id);
    }
}
