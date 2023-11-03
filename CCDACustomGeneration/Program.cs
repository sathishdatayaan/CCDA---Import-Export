
using System;
using CreateClinicalReport;
using CreateClinicalReport.Model;

class Program
{
	static void Main()
	{
		RecordParser patientClinical = new RecordParser();
		ClinicalReportFile file = new ClinicalReportFile();
		var xmlStream = file.GenerateCCDA(patientClinical.ParsePatientDetails(22));
		Console.WriteLine(xmlStream);
	}
}
