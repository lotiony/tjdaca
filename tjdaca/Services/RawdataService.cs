using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq.Dynamic.Core;
using tjdaca.Data;

namespace tjdaca.Services
{
    public class RawdataService : IRawdataService
    {
        private readonly tjdacaContext _dbContext;

        public RawdataService(tjdacaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteRawdata(int id)
        {
            var rawdata = _dbContext.AcaRawdata.FirstOrDefault(x => x.RIdx == id);
            if (rawdata != null)
            {
                _dbContext.AcaRawdata.Remove(rawdata);
                _dbContext.SaveChanges();
            }
        }

        public AcaRawdata GetRawdataById(int rid)
        {
            var rawdata = _dbContext.AcaRawdata.SingleOrDefault(x => x.RIdx.Equals(rid));
            return rawdata;
        }

        public List<AcaRawdata> GetRawdata()
        {
            return _dbContext.AcaRawdata.ToList();
        }
        public List<AcaRawdata> GetRawdatasByTid(int tid)
        {
            return _dbContext.AcaRawdata.Where(x=> x.TIdx.Equals(tid)).OrderByDescending(x=> x.RIdx).ToList();
        }
        public List<AcaRawdata> GetRawdatasBySid(int stuid)
        {
            return _dbContext.AcaRawdata.Where(x => x.StuIdx.Equals(stuid)).ToList();
        }
        public AcaRawdata GetLastRawdataBySid(int sid)
        {
            var rawdata = _dbContext.AcaRawdata.Where(x => x.StuIdx.Equals(sid)).OrderByDescending(x => x.RIdx).FirstOrDefault();
            return rawdata ?? new AcaRawdata();
        }
        public void SaveData(AcaRawdata rawdata)
        {
            if (rawdata.RIdx == 0) _dbContext.AcaRawdata.Add(rawdata);
            else _dbContext.AcaRawdata.Update(rawdata);
            _dbContext.SaveChanges();
        }

        public List<AcaStudents> GetStudentByTid(int tid)
        {
            var stuList = (from s in _dbContext.AcaStudents
                          join t in _dbContext.AcaTeachers on s.Teacher equals t.TName
                          where t.TIdx == tid
                          select s).ToList();

            return stuList;
        }
        
        public List<string> GetTestGradeList(string subject)
        {
            var gradeList = _dbContext.AcaSubject
                            .Where(s => s.Subject.Equals(subject))
                            .GroupBy(s => s.Grade)
                            .OrderBy(g => g.Min(s => s.MIdx))
                            .Select(g => g.Key).ToList();

            return gradeList;
        }
        public List<string> GetSubjectListByGrade(string subject, string grade)
        {
            var subjectList = _dbContext.AcaSubject
                              .Where(s => s.Subject.Equals(subject) && ( !string.IsNullOrEmpty(grade) ? s.Grade.Equals(grade) : true ))
                              .OrderBy(g => g.MIdx)
                              .Select(g => g.Value).ToList();

            return subjectList;
        }

        public string GetTeacherNameByTid(int tid)
        {
            return _dbContext.AcaTeachers.Where(x => x.TIdx.Equals(tid)).SingleOrDefault()?.TName ?? "";
        }
    }
}
