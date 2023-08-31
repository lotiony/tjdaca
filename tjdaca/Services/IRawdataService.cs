using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IRawdataService
    {
        List<AcaRawdata> GetRawdata();
        List<string> GetTestGradeList(string subject);
        List<AcaRawdata> GetRawdatasByTid(int tid);
        List<AcaRawdata> GetRawdatasBySid(int stuid);
        AcaRawdata GetRawdataById(int rid);
        AcaRawdata GetLastRawdataBySid(int sid);
        List<AcaStudents> GetStudentByTid(int tid);
        void SaveData(AcaRawdata rawdata);
        void DeleteRawdata(int id);
        List<string> GetSubjectListByGrade(string subject, string grade);
        string GetTeacherNameByTid(int tid);
    }
}
