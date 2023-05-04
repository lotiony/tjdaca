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
        
    }
}
