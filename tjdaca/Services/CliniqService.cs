using System.Linq.Dynamic.Core;
using tjdaca.Data;
using tjdaca.Models;
using tjdaca.Pages;

namespace tjdaca.Services
{
    public class CliniqService : ICliniqService
    {
        private readonly tjdacaContext _dbContext;

        public CliniqService(tjdacaContext dbContext)
        {
            _dbContext = dbContext;
        }



        public List<StuCliniq> GetCliniqList(DateTime startDate, DateTime endDate)
        {
            var rawdata = (from r in _dbContext.AcaRawdata
                           where r.BaseDate >= startDate.Date
                             && r.BaseDate < endDate.AddDays(1).Date
                             && r.DailyScore != null
                           select new
                           {
                               rIdx = r.RIdx,
                               date = r.BaseDate,
                               className = r.ClassName,
                               tIdx = r.TIdx,
                               stuIdx = r.StuIdx,
                               stuName = r.StuName,
                               schType = r.SchType,
                               schName = r.SchName,
                               schGrade = r.Grade,
                               testGrade = r.TestGrade,
                               testSubject = r.TestSubject,
                               dailyCount = r.DaliyCount,
                               dailyCorrect = r.DailyCorrect,
                               dailyScore = r.DailyScore
                           }).ToList();

            var stuAvg = (from r in rawdata
                          group r by r.stuIdx into g
                          select (StuIdx: g.Key, AverageScore: g.Average(r => r.dailyScore))
                         ).ToList();

            var sttList = (from p in stuAvg
                          join s in _dbContext.AcaStudents on p.StuIdx equals s.StuIdx
                          join t in _dbContext.AcaTeachers on s.Teacher equals t.TName
                          select new StuCliniq() { 
                              StuIdx = p.StuIdx ?? 0,
                              StuName = s.StuName,
                              School = s.School,
                              SchType = s.SchType,
                              SchGrade = s.SchGrade,
                              Teacher = t.TName,
                              AverageScore = Math.Round(Convert.ToDecimal(p.AverageScore ?? 0), 1),
                              CliniqSubject = string.Empty
                          }).ToList();

            /// 대상 목록에서  클리닉 대상 단원만 추출해서 덧붙인다
            foreach ( var s in sttList)
            {

                try
                {
                    var subjects = string.Join(Environment.NewLine, rawdata.Where(x => x.stuIdx == s.StuIdx && x.dailyScore < 70).Select(x => $"[{x.date.ToString("M월d일")}] {x.testGrade}-{x.testSubject} ({x.dailyScore:N0}점)"));
                    s.CliniqSubject = subjects;
                }
                catch (Exception)
                {
                }
            }

            return sttList;
        }

    }
}
