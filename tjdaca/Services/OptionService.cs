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
            var option = _dbContext.Options.FirstOrDefault(x => x.OIdx == id);
            if (option != null)
            {
                _dbContext.Options.Remove(option);
                _dbContext.SaveChanges();
            }
        }

        public Options GetOptionById(int id)
        {
            var option = _dbContext.Options.SingleOrDefault(x => x.OIdx == id);
            return option;
        }

        public List<Options> GetOptions()
        {
            return _dbContext.Options.ToList();
        }
        public List<Options> GetOptions(string optType)
        {
            return _dbContext.Options.Where(x=> x.OptType.Equals(optType)).ToList();
        }

        public void SaveOption(Options Options)
        {
            if (Options.OIdx == 0) _dbContext.Options.Add(Options);
            else _dbContext.Options.Update(Options);
            _dbContext.SaveChanges();
        }
    }
}
