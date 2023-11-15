using Livestock_Supervisor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace Livestock_Supervisor.Controllers
{
    public class HomeController : Controller
    {
        SqlCommand command = new SqlCommand();
        SqlConnection connection = new SqlConnection();
        SqlDataReader dataReader;
        List<Livestock> livestocks = new List<Livestock>();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            connection.ConnectionString = Livestock_Supervisor.Properties.Resources.ConnectionString;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult MyLivestock()
        {
            FetchData();
            return View(livestocks);
        }

        private void FetchData()
        {
            if(livestocks.Count > 0)
            {
                livestocks.Clear();
            }
            try
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM [LivestockDb].[dbo].[LivestockTable]";
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    livestocks.Add(new Livestock()
                    {
                        Id = (int)dataReader["ID"],
                        Name = dataReader["NAME"].ToString(),
                        BirthDate = dataReader["BIRTH_DATE"].ToString(),
                        FertilizationDate = dataReader["FERTILIZATION_DATE"].ToString(),
                        StepCount = dataReader["STEP_COUNT"].ToString(),
                        LastBodyTemp = dataReader["LAST_BODY_TEMP"].ToString(),
                    });
                }
                connection.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}