
using AI.Chat.Copilot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace AI.Chat.Copilot.Application
{
    public class AIApplicationAppService
    {
        private IRepository _repository;
      
        public AIApplicationAppService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<List<AIApps>> QueryAsync(string keyword)
        {
            return _repository.GetListAsync<AIApps>(u => string.IsNullOrWhiteSpace(keyword) || u.Name.Contains(keyword), asNoTracking:true);
        }

        public async Task InsertAsync(AIApps apps)
        {
            await _repository.AddAsync<AIApps>(apps);
            await _repository.SaveChangesAsync();
        }
        public async Task UpdateAsync(AIApps apps)
        {
            _repository.Update<AIApps>(apps);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.ExecuteDeleteAsync<AIApps>(u => u.Id == id);
        }

    }
}
