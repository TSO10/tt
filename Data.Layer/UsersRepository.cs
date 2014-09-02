using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data.Layer
{
    public class UsersRepository
    {
        public static List<VEDK_User> GetAll()
        {
            List<VEDK_User> users;
            using (VEDKEntities entities = new VEDKEntities())
            {
                users = entities.VEDK_Users.ToList();
            }
            return users;
        }

        public static void Add(VEDK_User customer)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                entities.VEDK_Users.AddObject(customer);
                entities.SaveChanges();
            }
        }

        public static VEDK_User GetByEmail(string userName)
        {
            VEDK_User user = null;
            using (VEDKEntities entities = new VEDKEntities())
            {
                user = entities.VEDK_Users.FirstOrDefault(u => u.UserName ==userName);
            }
            return user;
        }

        public static VEDK_User GetByEmailPassword(string userName, string password)
        {
            VEDK_User user = null;
            using (VEDKEntities entities = new VEDKEntities())
            {
                user = entities.VEDK_Users.FirstOrDefault(u => u.UserName ==userName && u.Password == password);
            }
            return user;
        }

        public static string GetRoleForUser(string userName)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                VEDK_User user = entities.VEDK_Users.FirstOrDefault(u => u.UserName ==userName);
                if (user != null)
                {
                    return user.Administrator ? "admin" : "user";
                }
            }
            return null;
        }

        public static bool IsUserInRole(string userName, string role)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                return entities.VEDK_Users.Count(u => u.UserName == userName && u.Administrator == (role == "admin" ? true : false)) > 0;

            }
        }

        public static bool ChangeRole(string userName)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                var user = entities.VEDK_Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    user.Administrator = !user.Administrator;
                    user.DataUpdated = DateTime.Now;
                    entities.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public static void Update(VEDK_User user)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                var temp = entities.VEDK_Users.FirstOrDefault(u => u.UserId == user.UserId);
                if (temp != null)
                {
                    temp.UserName = user.UserName;
                    temp.Password = user.Password;
                    temp.Administrator = user.Administrator;
                    entities.SaveChanges();
                }
            }
        }
        public static void Update(string userName, string comments, bool isAdmin)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                var user = entities.VEDK_Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    user.Comments = comments;
                    user.Administrator = isAdmin;
                    user.DataUpdated = DateTime.Now;
                    entities.SaveChanges();
                }
            }
        }
        public static bool Delete(string userName)
        {
            using (VEDKEntities entities = new VEDKEntities())
            {
                var user = entities.VEDK_Users.FirstOrDefault(u => u.UserName == userName);
                if (user != null)
                {
                    entities.DeleteObject(user);
                    entities.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
