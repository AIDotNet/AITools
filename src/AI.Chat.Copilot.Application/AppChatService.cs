using AI.Chat.Copilot.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;

namespace AI.Chat.Copilot.Application
{
    public class AppChatService
    {
        private IRepository _repository;

        public AppChatService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task InsertAsync(AppChat chat)
        {
            await _repository.AddAsync(chat);
            await _repository.SaveChangesAsync();
        }
        public Task<List<AppChat>> GetListAsync()
        {
            return _repository.GetListAsync<AppChat>(false);
        }

        public Task<List<AppChatMessage>> GetChatHistoriesAsync(string chatId)
        {
           return  _repository.GetListAsync<AppChatMessage>(u=>u.ChatId == chatId,asNoTracking:false);
        }

        public async Task InsertChatMessageAsync(IEnumerable<AppChatMessage> messages)
        {
            await _repository.AddAsync<AppChatMessage>(messages);
            await _repository.SaveChangesAsync();
        }
    }
}
