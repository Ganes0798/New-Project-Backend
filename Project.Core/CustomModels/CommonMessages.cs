using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Core.CustomModels
{
    public class CustomMessages
    {

        public const string TITLE = "Commerce";
        public const string VERSION = "1.0.0.0";
    }

    public enum ErrorCodes
    {
        None = 0,
        EmailDoesNotExist = 1001,
        InvalidCredentials,
        UnableToLogin,
        LoggedOffSuccessfully,
        UnableToLogOff,
        NewUserAddedSuccessFully,
        LoggedInSuccessFully,
        UnableToAddUser,
        EmailAlreadyExists,
        EmptyUserList,
		InvalidEnumRole,
		UserAlreadyExist,
		EmptyUsers,
        ProductsAddedSuccessfully,
		PasswordMismatch,
		PasswordUpdatedSuccessfully
	}
}
