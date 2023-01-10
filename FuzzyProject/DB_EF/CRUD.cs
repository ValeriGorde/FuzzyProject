using FuzzyProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FuzzyProject.DB_EF
{
    internal class CRUD
    {
        public void Create(ApplicationContext ac, string login, string password, string role)
        {
            var account = new Account { Login = login, Password = password, Role = role };
            ac.Accounts.Add(account);
            ac.SaveChanges();
        }

        //удаление объекта из БД (по id)
        public void Delete(ApplicationContext ac, int id)
        {
            var account = new Account { Id = id };
            ac.Accounts.Remove(account);
            ac.SaveChanges();
        }

        //обновление
        public void Update(ApplicationContext ac, int id, string newLogin)
        {
            var account = ac.Accounts.FirstOrDefault(b => b.Id == id);
            account.Login = newLogin;

            ac.SaveChanges();
        }

        //выбор всех обектов
        public void SelectAllBooks(ApplicationContext ac)
        {
            var accounts = ac.Accounts.ToList();

            foreach (var account in accounts)
            {
                Console.WriteLine(account.Id + " " + account.Login + " " + account.Password + " " + account.Role);
            }
        }
    }
}
