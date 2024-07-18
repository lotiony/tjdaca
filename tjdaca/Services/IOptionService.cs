using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IOptionService
    {
        List<AcaOptions> GetOptions();
        List<string> GetTeachers();
        List<AcaTeachers> GetTeachersList();
        List<AcaOptions> GetOptions(string optType);
        AcaOptions GetOptionById(int id);
        void SaveOption(AcaOptions option);
        void DeleteOption(int id);
    }
}
