using tjdaca.Data;

namespace tjdaca.Services
{
    public interface IQuestionService
    {
        List<AcaQuestions> GetQuestions();
        AcaQuestions GetQuestionById(int qid);
        void SaveQuestion(AcaQuestions question);
        void DeleteQuestion(int qid);
        List<AcaStudents> GetStudentByTid(int tid);
        List<AcaTeachers> GetTeachers();
    }
}
