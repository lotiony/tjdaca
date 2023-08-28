using tjdaca.Data;

namespace tjdaca.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly tjdacaContext _dbContext;

        public QuestionService(tjdacaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteQuestion(int qid)
        {
            var Question = _dbContext.AcaQuestions.FirstOrDefault(x => x.QIdx == qid);
            if (Question != null)
            {
                _dbContext.AcaQuestions.Remove(Question);
                _dbContext.SaveChanges();
            }
        }

        public AcaQuestions GetQuestionById(int qid)
        {
            var Question = _dbContext.AcaQuestions.SingleOrDefault(x => x.QIdx == qid);
            return Question;
        }

        public List<AcaQuestions> GetQuestions()
        {
            return _dbContext.AcaQuestions.OrderByDescending(x=> x.QIdx).ToList();
        }

        public void SaveQuestion(AcaQuestions Questions)
        {
            if (Questions.QIdx == 0) _dbContext.AcaQuestions.Add(Questions);
            else _dbContext.AcaQuestions.Update(Questions);
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

        public List<AcaTeachers> GetTeachers()
        {
            return _dbContext.AcaTeachers.OrderBy(x=> x.TIdx).ToList();
        }
    }
}
