using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IStudentService
    {
        List<Students> GetStudents();
        Students GetStudentById(int id);
        void SaveStudent(Students students);
        void DeleteStudent(int id);
    }
}
