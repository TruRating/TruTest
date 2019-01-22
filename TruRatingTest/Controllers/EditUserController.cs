using System.Data;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Collections.Generic;

using TruRatingTest.Entities;
using TruRatingTest.ViewModels;

namespace TruRatingTest.Controllers
{
    public class EditUserController : Controller
    {
        public ActionResult Index(int userId)
        {
            var user = GetUserById(userId);
            return View(user);
        }  

        [HttpPost]
        public ActionResult Post(User user)
        {
            var userObjectToUpdate = GetUserById(user.Id);
            if (userObjectToUpdate != null)
            {
                var errors = new List<string>();
                if (string.IsNullOrEmpty(user.FirstName))
                {
                    errors.Add("First Name is required");
                }

                if (string.IsNullOrEmpty(user.LastName))
                {
                    errors.Add("Last Name is required");
                }

                if (string.IsNullOrEmpty(user.UserName))
                {
                    errors.Add("User Name is required");
                }

                if (string.IsNullOrEmpty(user.EmailAddress))
                {
                    errors.Add("Email Address is required");
                }

                if (errors.Count == 0)
                {
                    UpdateUser(user);
                }
                else
                {
                    throw new System.Exception("Missing Required Fields");
                }
            }

            return Redirect("~/Home/Index");
        }

        private void UpdateUser(User user)
        {
            var connection = new SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnnectionString"].ToString());
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = $"UPDATE dbo.[TruUsers] SET FirstName = '{user.FirstName}' , LastName = '{user.LastName}' , UserName = '{user.UserName}' , EmailAddress = '{user.EmailAddress}' WHERE Id = " + user.Id;

            command.ExecuteNonQuery();
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
    }
}
