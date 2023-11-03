using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using CreateClinicalReport.Model;
using HL7SDK.Cda;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace CreateClinicalReport.OpenEMRDataSource
{
	public class OpenEMRData
	{
		public (PatientDemographicDetail,EncountersFacility) OpenEMRPatientData(int PatientId)
		{

			string connectionString = "Server=127.0.0.1;Database=openemr;User ID=root;Password=;";

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				
				try
				{

					connection.Open();

					string query = $"SELECT pd.*, enc.* FROM form_encounter enc JOIN patient_data pd " +
									$"ON enc.pid = pd.pid WHERE enc.pid = '{PatientId}';";
					

					MySqlCommand command = new MySqlCommand(query, connection);

					using (MySqlDataReader patient = command.ExecuteReader())
					{
						if(patient.HasRows)
						{
							while (patient.Read())
							{
								PatientDemographicDetail ptDemographic = new PatientDemographicDetail();
								ptDemographic.Street = patient["street"] != null ? patient["street"].ToString() : string.Empty;
								ptDemographic.City = patient["city"] != null ? patient["city"].ToString() : string.Empty;
								ptDemographic.State = patient["state"] != null ? patient["state"].ToString() : string.Empty;
								ptDemographic.Country = patient["county"] != null ? patient["county"].ToString() : string.Empty;
								ptDemographic.Zip = patient["postal_code"] != null ? patient["postal_code"].ToString() : string.Empty;
								///// Patient Name

								ptDemographic.FirstName = patient["fname"] != null ? patient["fname"].ToString() : string.Empty;///Get Patient First Name

								ptDemographic.LastName = patient["lname"] != null ? patient["lname"].ToString() : string.Empty;///Get Patient Last Name
								///// Patient DOB

								ptDemographic.DateofBirth = patient["DOB"] != null ? patient["DOB"].ToString() : string.Empty;
								///// Patient Gender
								ptDemographic.gender = patient["sex"] != null ? patient["sex"].ToString() : string.Empty;
								///// Patient SSN

								ptDemographic.SSN = patient["ss"] != null ? patient["ss"].ToString() : string.Empty;

								///// Patient PHNo.
								ptDemographic.ContactNo = patient["phone_contact"] != null ? patient["phone_contact"].ToString() : string.Empty;
								///// Patient Race
								ptDemographic.Race = patient["race"] != null ? patient["race"].ToString() : "UNK";
								///// Patient Language

								ptDemographic.PreferredLanguage = patient["language"] != null ? patient["language"].ToString() : string.Empty;
								ptDemographic.LanguageCode = patient["language"] != null ? patient["language"].ToString() : string.Empty;

								///// Patient Ethencity
								ptDemographic.Ethnicity = patient["ethnicity"] != null ? patient["ethnicity"].ToString() : string.Empty;

								EncountersFacility encountersFacility = new EncountersFacility();

								encountersFacility.EncounterNoteDate = patient["date"] != null ? patient["date"].ToString() : string.Empty;
								encountersFacility.EncounterCode = patient["encounter_type_code"] != null ? patient["encounter_type_code"].ToString() : string.Empty;
								encountersFacility.EncounterDescription = patient["reason"] != null ? patient["reason"].ToString() : string.Empty;
								encountersFacility.facility_id = int.Parse(patient["facility_id"].ToString());

								return (ptDemographic, encountersFacility);
							}
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

				return (null,null);

			}

			

		}

		public string OpenEMRUserData(int facility_id)
		{

			string connectionString = "Server=127.0.0.1;Database=openemr;User ID=root;Password=;";

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				try
				{

					connection.Open();

					string query = $"SELECT * from users where facility_id = '{facility_id}' LIMIT 1;";


					MySqlCommand command = new MySqlCommand(query, connection);

					using (MySqlDataReader Author = command.ExecuteReader())
					{
						if (Author.HasRows)
						{
							while (Author.Read())
							{
								string author = Author["fname"] != null ? Author["fname"].ToString() : string.Empty;

								return author;
							}
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

				return string.Empty;

			}

		}

		public (ClinicInformation, List<DocumentationOfList>) OpenEMRFacilityData(int facility_id, string docstaffName)
		{

			string connectionString = "Server=127.0.0.1;Database=openemr;User ID=root;Password=;";

			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{

				try
				{

					connection.Open();

					string query = $"SELECT * from facility where id = '{facility_id}' LIMIT 1;";


					MySqlCommand command = new MySqlCommand(query, connection);

					using (MySqlDataReader facility = command.ExecuteReader())
					{
						if (facility.HasRows)
						{
							while (facility.Read())
							{
								ClinicInformation ptClinicInformation = new ClinicInformation();
								List<DocumentationOfList> documentationOfInfo = new List<DocumentationOfList>();

								ptClinicInformation.ClinicCity = facility["city"] != null ? facility["city"].ToString() : string.Empty;
								ptClinicInformation.ClinicState = facility["state"] != null ? facility["state"].ToString() : string.Empty;
								ptClinicInformation.ClinicStreeet = facility["street"] != null ? facility["street"].ToString() : string.Empty;
								ptClinicInformation.ClinicCountry = facility["country_code"] != null ? facility["country_code"].ToString() : string.Empty;
								ptClinicInformation.ClinicZip = facility["postal_code"] != null ? facility["postal_code"].ToString() : string.Empty;
								///// Clinic / Provider PHNo.
								ptClinicInformation.ClinicPhoneNumber = facility["phone"] != null ? facility["phone"].ToString() : string.Empty;

								///// Clinic / Provider Name
								ptClinicInformation.ClinicName = facility["name"] != null ? facility["name"].ToString() : string.Empty;


								DocumentationOfList docof = new DocumentationOfList();
								docof.address = facility["street"] != null ? facility["street"].ToString() : string.Empty;
								docof.city = facility["city"] != null ? facility["city"].ToString() : string.Empty;
								docof.state = facility["state"] != null ? facility["state"].ToString() : string.Empty;
								docof.pinCode = facility["postal_code"] != null ? facility["postal_code"].ToString() : string.Empty;
								docof.staffName = docstaffName;

								documentationOfInfo.Add(docof);


								return (ptClinicInformation, documentationOfInfo);
							}
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

				return (null, null);

			}

		}



	}
}
