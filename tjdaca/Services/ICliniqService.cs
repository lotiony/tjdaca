using tjdaca.Data;
using tjdaca.Models;

namespace tjdaca.Services
{
    public interface ICliniqService
    {
        List<StuCliniq> GetCliniqList(DateTime startDate, DateTime endDate);
      
    }
}
