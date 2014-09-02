using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Data.Layer;

namespace VEDK_Dashboard
{
    public class CustomRoleProvider : RoleProvider
    {
        private string _ApplicationName;

        public CustomRoleProvider()
        {

        }

        public override string ApplicationName
        {
            get
            {
                return _ApplicationName;
            }
            set
            {
                _ApplicationName = value;
            }
        }


        public override string[] GetAllRoles()
        {
            return new string[] { "admin", "user" };
        }

        public override string[] GetRolesForUser(string username)
        {
            return new string[] { UsersRepository.GetRoleForUser(username) };
        }

        public override string[] GetUsersInRole(string roleName)
        {

            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            return UsersRepository.IsUserInRole(username, roleName);
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public void ChangeRole(string username)
        {
            UsersRepository.ChangeRole(username);
        }
        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }
    }
}