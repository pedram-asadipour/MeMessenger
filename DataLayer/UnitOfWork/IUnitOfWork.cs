using System;
using DataLayer.Entities;
using DataLayer.Repository;

namespace DataLayer.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Account> Accounts { get; }
        public IGenericRepository<Chat> Chats { get; }
        public IGenericRepository<Message> Messages { get; }
        public IGenericRepository<UserChat> UserChat { get; }
        public void SaveChange();
    }
}