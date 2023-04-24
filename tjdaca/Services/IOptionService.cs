using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IOptionService
    {
        List<Options> GetOptions();
        List<Options> GetOptions(string optType);
        Options GetOptionById(int id);
        void SaveOption(Options option);
        void DeleteOption(int id);
    }
}
