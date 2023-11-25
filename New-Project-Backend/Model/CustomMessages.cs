namespace New_Project_Backend.Model
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
        UnableToAddUser
    }
}
