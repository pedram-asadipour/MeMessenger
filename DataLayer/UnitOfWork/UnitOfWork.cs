using DataLayer.Entities;
using DataLayer.Repository;

namespace DataLayer.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MeMessengerContext _context;

        public UnitOfWork(MeMessengerContext context)
        {
            _context = context;
        }

        private IGenericRepository<Account> _account;
        private IGenericRepository<Chat> _chat;
        private IGenericRepository<Message> _message;
        private IGenericRepository<UserChat> _userChat;


        public IGenericRepository<Account> Accounts => _account ??= new GenericRepository<Account>(_context);
        public IGenericRepository<Chat> Chats => _chat ??= new GenericRepository<Chat>(_context);
        public IGenericRepository<Message> Messages => _message ??= new GenericRepository<Message>(_context);
        public IGenericRepository<UserChat> UserChat => _userChat ??= new GenericRepository<UserChat>(_context);


        public void SaveChange()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}