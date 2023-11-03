using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CCDA___Import_Export.Model;
using MARC.Everest.RMIM.UV.CDAr2.POCD_MT000040UV;
using MySql.Data.MySqlClient;

namespace CCDA___Import_Export.OpenEMRDataSource
{
	public class OpenEMRData
	{
		public PatientData OpenEMRPatientData(string PatientName)
		{

			string connectionString = "Server=127.0.0.1;Database=openemr;User ID=root;Password=;";

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				PatientData patient =  new PatientData();

				try
				{

					connection.Open();

					string query = $"SELECT * FROM patient_data where fname = '{PatientName}'";
					MySqlCommand command = new MySqlCommand(query, connection);

					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{

							patient.GivenName = reader["fname"].ToString();
							patient.FamilyName = reader["fname"].ToString();
							patient.DateOfBirth = Convert.ToDateTime(reader["DOB"]);
							patient.Address = reader["street"].ToString();
							patient.Gender = reader["sex"].ToString();
							patient.Id = reader["id"].ToString();
							patient.MothersId = reader["id"].ToString();
							patient.MothersName = reader["mothersname"].ToString();
							patient.City = reader["city"].ToString();
							patient.OtherIds = new List<KeyValuePair<string, string>>()
							{
								new KeyValuePair<string, string>("2.16.2.3.2.3.2.4", "123-231-435")
							};
							patient.State = "ON";

						}
					}

				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
				finally
				{
					connection.Close();
				}

				return patient;

			}

			return null;

		}


		public PatientData OpenEMRPhysicianData(string PatientName)
		{

			string connectionString = "Server=127.0.0.1;Database=openemr;User ID=root;Password=;";

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				PatientData patient = new PatientData();

				try
				{

					connection.Open();

					string query = $"SELECT * FROM patient_data where fname = '{PatientName}'";
					MySqlCommand command = new MySqlCommand(query, connection);

					using (MySqlDataReader reader = command.ExecuteReader())
					{
						while (reader.Read())
						{

							patient.GivenName = reader["fname"].ToString();
							patient.FamilyName = reader["fname"].ToString();
							patient.DateOfBirth = Convert.ToDateTime(reader["DOB"]);
							patient.Address = reader["street"].ToString();
							patient.Gender = reader["sex"].ToString();
							patient.Id = reader["id"].ToString();
							patient.MothersId = reader["id"].ToString();
							patient.MothersName = reader["mothersname"].ToString();
							patient.City = reader["city"].ToString();
							patient.OtherIds = new List<KeyValuePair<string, string>>()
							{
								new KeyValuePair<string, string>("2.16.2.3.2.3.2.4", "123-231-435")
							};
							patient.State = "ON";

						}
					}

				}
				catch (Exception ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
				finally
				{
					connection.Close();
				}

				return patient;

			}

			return null;

		}
	}
}
