using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IStudentService
    {
        List<AcaStudents> GetStudents();
        AcaStudents GetStudentById(int id);
        void SaveStudent(AcaStudents students);
        void DeleteStudent(int id);
    }
}
