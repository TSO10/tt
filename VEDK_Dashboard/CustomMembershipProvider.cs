using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Collections.Specialized;
using Data.Layer;
using System.Security.Cryptography;
using System.Text;

namespace VEDK_Dashboard
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public int _minRequiredPasswordLength;
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public  CustomMembershipProvider()
        {
        }
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            VEDK_User user = UsersRepository.GetByEmailPassword(username, GetMD5Hash(oldPassword));
            if (user != null)
            {
                user.Password = GetMD5Hash(newPassword);
                UsersRepository.Update(user);
                return true;
            }
            return false;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            { throw new ArgumentNullException("config"); }
            base.Initialize(name, config);
            _minRequiredPasswordLength = Convert.ToInt32(config["minRequiredPasswordLength"]);
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public MembershipUser CreateUser(string userName, string password, string comments, bool isAdmin)
        {
            MembershipUser user = GetUser(userName, true);

            if (user == null)
            {
                VEDK_User userObj = new VEDK_User();
                userObj.UserName = userName;
                userObj.Comments = comments;
                userObj.Password=GetMD5Hash(password);
                userObj.Administrator = isAdmin;
                userObj.DateCreated = DateTime.Now;

                UsersRepository.Add(userObj);

                return GetUser(userName, true);
            }

            return user;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(string username)
        {
            return UsersRepository.Delete(username);
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            VEDK_User user = UsersRepository.GetByEmail(username);
            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("CustomMembershipProvider", username, user.UserId, user.UserName,
                                                            string.Empty, string.Empty,
                                                            true, false, DateTime.MinValue,
                                                            DateTime.MinValue,
                                                            DateTime.MinValue,
                                                            DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { return false; }
        }

        public override string ResetPassword(string username, string answer)
        {
            string strResetPassword = string.Empty;
            VEDK_User user = UsersRepository.GetByEmail(username);
            if (user != null)
            {
                strResetPassword = Guid.NewGuid().ToString("N").Substring(0, _minRequiredPasswordLength);
                user.Password = GetMD5Hash(strResetPassword);
                UsersRepository.Update(user);
            }
            return strResetPassword;
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(string userName, string comments, bool isAdmin)
        {
            UsersRepository.Update(userName, comments, isAdmin);
        }

        public override bool ValidateUser(string username, string password)
        {
            string sha1Pswd = GetMD5Hash(password);
            VEDK_User userObj = UsersRepository.GetByEmailPassword(username, sha1Pswd);
            if (userObj != null)
                return true;
            return false;
        }

        private static string GetMD5Hash(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}