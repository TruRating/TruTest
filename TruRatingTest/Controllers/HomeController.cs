using System.Data;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

using TruRatingTest.Entities;
using TruRatingTest.ViewModels;

namespace TruRatingTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            var usersViewModel = new UsersListModel { Users = GetUsers() };
            return View(usersViewModel);
        }

        [HttpGet]
        public ActionResult Delete(User user)
        {
            var userToDelete = GetUserById(user.Id);
            if (userToDelete != null)
            {
                DeleteUser(userToDelete.Id);
            }
            else
            {
                throw new System.Exception("User cannot be found");
            }

            return Redirect("~/Home/Index");
        }

        private List<User> GetUsers()
        {
            var allUsers = new List<User>();

            var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnnectionString"].ToString());
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT * FROM dbo.[TruUsers] WHERE Active = 1";

            var dataAdapter = new SqlDataAdapter(command);
            var dataSet = new DataSet();

            dataAdapter.Fill(dataSet);

            var dataReader = dataSet.CreateDataReader();

            while (dataReader.Read())
            {
                var user = new User();
                user.Id = (int)dataReader["Id"];
                user.FirstName = (string)dataReader["FirstName"];
                user.LastName = (string)dataReader["LastName"];
                user.UserName = (string)dataReader["UserName"];
                user.EmailAddress = (string)dataReader["EmailAddress"];
                user.Active = (bool)dataReader["Active"];

                allUsers.Add(user);
            }

            return allUsers;
        }

        private User GetUserById(int id)
        {
            User user = null;

            var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnnectionString"].ToString());
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = "SELECT * FROM dbo.[TruUsers] WHERE Id = " + id;

            var dataAdapter = new SqlDataAdapter(command);
            var dataSet = new DataSet();

            dataAdapter.Fill(dataSet);

            var dataReader = dataSet.CreateDataReader();

            while (dataReader.Read())
            {
                user = new User();
                user.Id = (int)dataReader["Id"];
                user.FirstName = (string)dataReader["FirstName"];
                user.LastName = (string)dataReader["LastName"];
                user.UserName = (string)dataReader["UserName"];
                user.EmailAddress = (string)dataReader["EmailAddress"];
                user.Active = (bool)dataReader["Active"];
            }

            return user;
        }

        private void DeleteUser(int id)
        {
            var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnnectionString"].ToString());
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = $"UPDATE dbo.[TruUsers] SET Active = 0 WHERE Id = " + id;

            command.ExecuteNonQuery();
        }
    }
}
