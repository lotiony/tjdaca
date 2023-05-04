using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IRawdataService
    {
        List<AcaRawdata> GetRawdata();
        List<AcaRawdata> GetRawdatasByTid(int tid);
        AcaRawdata GetRawdataById(int rid);

        List<AcaStudents> GetStudentByTid(int tid);
        void SaveData(AcaRawdata rawdata);
        void DeleteRawdata(int id);
    }
}
