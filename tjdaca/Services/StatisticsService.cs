using System.Linq.Dynamic.Core;
using tjdaca.Data;
using tjdaca.Models;
using tjdaca.Pages;

namespace tjdaca.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly tjdacaContext _dbContext;

        public StatisticsService(tjdacaContext dbContext)
        {
            _dbContext = dbContext;
        }



        public List<StuStatistics> GetStatisticsList()
        {
            var stuList = (from sss in _dbContext.AcaStuSchoolScore
                           join s in _dbContext.AcaStudents on sss.StuIdx equals s.StuIdx
                           join t in _dbContext.AcaTeachers on s.Teacher equals t.TName
                           where s.SchType != "초등"
                           orderby sss.StuIdx descending
                           select new { StudentScore= sss, Student=s, Teacher=t }).ToList();

            var sttList = new List<StuStatistics>();
            foreach ( var s in stuList )
            {
                StuStatistics stt = new StuStatistics();
                stt.StuIdx = s.StudentScore.StuIdx;
                stt.StuName = s.Student.StuName;
                stt.Teacher = s.Teacher.TName;
                stt.School = s.Student.School;
                stt.SchType = s.Student.SchType;
                stt.SchGrade = s.Student.SchGrade;
                switch (stt.SchType)
                {
                    case "중등":
                        switch (stt.SchGrade)
                        {
                            case "1":
                                stt.Score1_1 = s.StudentScore.M111;
                                stt.Score1_2 = s.StudentScore.M112;
                                stt.Score1_3 = s.StudentScore.M113;
                                stt.Score2_1 = s.StudentScore.M121;
                                stt.Score2_2 = s.StudentScore.M122;
                                stt.Score2_3 = s.StudentScore.M123;
                                break;
                            case "2":
                                stt.Score1_1 = s.StudentScore.M211;
                                stt.Score1_2 = s.StudentScore.M212;
                                stt.Score1_3 = s.StudentScore.M213;
                                stt.Score2_1 = s.StudentScore.M221;
                                stt.Score2_2 = s.StudentScore.M222;
                                stt.Score2_3 = s.StudentScore.M223;
                                break;
                            case "3":
                                stt.Score1_1 = s.StudentScore.M311;
                                stt.Score1_2 = s.StudentScore.M312;
                                stt.Score1_3 = s.StudentScore.M313;
                                stt.Score2_1 = s.StudentScore.M321;
                                stt.Score2_2 = s.StudentScore.M322;
                                stt.Score2_3 = s.StudentScore.M323;
                                break;
                        }
                        break;

                    case "고등":
                        switch (stt.SchGrade)
                        {
                            case "1":
                                stt.Score1_1 = s.StudentScore.H111;
                                stt.Score1_2 = s.StudentScore.H112;
                                stt.Score1_3 = s.StudentScore.H113;
                                stt.Score2_1 = s.StudentScore.H121;
                                stt.Score2_2 = s.StudentScore.H122;
                                stt.Score2_3 = s.StudentScore.H123;
                                break;
                            case "2":
                                stt.Score1_1 = s.StudentScore.H211;
                                stt.Score1_2 = s.StudentScore.H212;
                                stt.Score1_3 = s.StudentScore.H213;
                                stt.Score2_1 = s.StudentScore.H221;
                                stt.Score2_2 = s.StudentScore.H222;
                                stt.Score2_3 = s.StudentScore.H223;
                                break;
                            case "3":
                                stt.Score1_1 = s.StudentScore.H311;
                                stt.Score1_2 = s.StudentScore.H312;
                                stt.Score1_3 = 0;
                                stt.Score2_1 = s.StudentScore.H321;
                                stt.Score2_2 = s.StudentScore.H322;
                                stt.Score2_3 = 0;
                                break;
                        }
                        break;
                }

                try
                {
                    var d10 = _dbContext.AcaRawdata.Where(x => x.StuIdx.Equals(s.Student.StuIdx) && x.DailyScore != null).OrderByDescending(x => x.RIdx).Select(x=> x.DailyScore).Take(10).Average() ?? 0;
                    stt.DailyScore10 = Math.Round((decimal)d10, 1);

                    var d20 = _dbContext.AcaRawdata.Where(x => x.StuIdx.Equals(s.Student.StuIdx) && x.DailyScore != null).OrderByDescending(x => x.RIdx).Select(x=> x.DailyScore).Take(20).Average() ?? 0;
                    stt.DailyScore20 = Math.Round((decimal)d20, 1);
                }
                catch (Exception)
                {
                }

                sttList.Add( stt );
            }


            return sttList;
        }

    }
}
