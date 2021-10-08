using System;
using _Framework;
using _Framework.Auth;
using CoreLayer.AccountAgg.Contract;
using DataLayer.Entities;
using DataLayer.UnitOfWork;

namespace CoreLayer.AccountAgg.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthHelper _authHelper;
        private readonly OperationResult _result;

        public AccountServices(IUnitOfWork unitOfWork, IAuthHelper authHelper)
        {
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
            _result = new OperationResult();
        }

        public OperationResult Signin(SigninViewModel command)
        {
            try
            {
                if (command == null)
                    return _result.Failed(OperationMessage.AllRequired);

                var account = _unitOfWork.Accounts.GetBy(x => x.Username == command.Username &&
                                                              x.Password == command.Password);

                if (account == null)
                    return _result.Failed(OperationMessage.ExistAccount);

                var auth = new AuthViewModel(account.Id, account.Username);
                _authHelper.Signin(auth);

                return _result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public OperationResult Signup(SignupViewModel command)
        {
            try
            {
                if (command == null)
                    return _result.Failed(OperationMessage.AllRequired);

                if (_unitOfWork.Accounts.Exists(x => x.Username == command.Username))
                    return _result.Failed(OperationMessage.ExistUsername);

                if (command.Password != command.RePassword)
                    return _result.Failed(OperationMessage.PasswordNotCompare);

                var account = new Account(command.Username, command.Password);
                _unitOfWork.Accounts.Create(account);
                _unitOfWork.SaveChange();

                return _result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public OperationResult Signout()
        {
            try
            {
                _authHelper.Signout();

                return _result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}