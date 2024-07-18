using tjdaca.Data;

namespace tjdaca.Services
{
    public class OptionService : IOptionService
    {
        private readonly tjdacaContext _dbContext;

        public OptionService(tjdacaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void DeleteOption(int id)
        {
            var option = _dbContext.AcaOptions.FirstOrDefault(x => x.OIdx == id);
            if (option != null)
            {
                _dbContext.AcaOptions.Remove(option);
                _dbContext.SaveChanges();
            }
        }

        public AcaOptions GetOptionById(int id)
        {
            var option = _dbContext.AcaOptions.SingleOrDefault(x => x.OIdx == id);
            return option;
        }

        public List<AcaOptions> GetOptions()
        {
            return _dbContext.AcaOptions.ToList();
        }
        public List<AcaOptions> GetOptions(string optType)
        {
            return _dbContext.AcaOptions.Where(x=> x.OptType.Equals(optType)).OrderBy(x=> x.OptOrder).ToList();
        }

        public void SaveOption(AcaOptions Options)
        {
            if (Options.OIdx == 0) _dbContext.AcaOptions.Add(Options);
            else _dbContext.AcaOptions.Update(Options);
            _dbContext.SaveChanges();
        }

        public List<string> GetTeachers()
        {
            return _dbContext.AcaTeachers.Select(x=> x.TName).ToList();
        }

        public List<AcaTeachers> GetTeachersList()
        {
            return _dbContext.AcaTeachers.OrderBy(x => x.TIdx).ToList();
        }


    }
}
