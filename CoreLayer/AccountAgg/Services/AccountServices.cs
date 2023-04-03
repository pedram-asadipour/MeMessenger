using System;
using System.Collections.Generic;
using System.Linq;
using _Framework;
using _Framework.Auth;
using _Framework.FileManager;
using CoreLayer.AccountAgg.Contract;
using CoreLayer.UserChatAgg.Contract;
using DataLayer.Entities;
using DataLayer.UnitOfWork;

namespace CoreLayer.AccountAgg.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthHelper _authHelper;
        private readonly IFileManager _fileManager;
        private readonly List<UserStatus> _userStatus;
        private readonly OperationResult _result;

        public AccountServices(IUnitOfWork unitOfWork, IAuthHelper authHelper, List<UserStatus> userStatus, IFileManager fileManager)
        {
            _unitOfWork = unitOfWork;
            _authHelper = authHelper;
            _userStatus = userStatus;
            _fileManager = fileManager;
            _result = new OperationResult();
        }

        public ProfileViewModel GetCurrentAccount()
        {
            var accountId = _authHelper.GetAuthAccount().Id;

            return _unitOfWork.Accounts.Get()
                .Select(x => new ProfileViewModel
                {
                    Id = x.Id,
                    Username = x.Username,
                    ProfileImagePath = x.ProfileImage
                })
                .SingleOrDefault(x => x.Id == accountId);
        }

        public string GetProfileImage()
        {
            var accountId = _authHelper.GetAuthAccount().Id;

            return _unitOfWork.Accounts.Get()
                .Where(x => x.Id == accountId)
                .Select(x => x.ProfileImage)
                .SingleOrDefault();
        }

        public OperationResult EditAccount(ProfileViewModel command)
        {
            if (command == null)
                return _result.Failed(OperationMessage.AllRequired);

            var account = _unitOfWork.Accounts.GetBy(command.Id);

            if (account == null)
                return _result.Failed(OperationMessage.ExistAccount);

            if (_unitOfWork.Accounts.Exists(x => x.Username == command.Username && x.Id != command.Id))
                return _result.Failed(OperationMessage.ExistUsername);

            var profileImage = "";

            if (command.ProfileImage != null)
            {
                _fileManager.Remove(account.ProfileImage);
                profileImage = _fileManager.Uploader(command.ProfileImage, "account");
            }

            account.Edit(command.Username,account.Password,profileImage);
            _unitOfWork.Accounts.Edit(account);
            _unitOfWork.SaveChange();

            return _result.Success();
        }

        public OperationResult EditPassword(EditPassword command)
        {
            if (command == null)
                return _result.Failed(OperationMessage.AllRequired);

            var account = _unitOfWork.Accounts.GetBy(command.Id);

            if (account == null)
                return _result.Failed(OperationMessage.ExistAccount);

            if (account.Password != command.CurrentPassword)
                return _result.Failed(OperationMessage.IncorrectPassword);

            if(command.NewPassword != command.RePassword)
                return _result.Failed(OperationMessage.PasswordNotCompare);

            account.Edit(account.Username, command.NewPassword, "");
            _unitOfWork.Accounts.Edit(account);
            _unitOfWork.SaveChange();

            return _result.Success();
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