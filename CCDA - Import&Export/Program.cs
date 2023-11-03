using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Xml;
using System.Xml.Linq;
using CCDA___Import_Export;
using CCDA___Import_Export.Model;
using CCDA___Import_Export.OpenEMRDataSource;
using MARC.Everest.DataTypes;
using MARC.Everest.RMIM.UV.CDAr2.POCD_MT000040UV;
using static System.Collections.Specialized.BitVector32;

class Program
{
	static void Main()
	{
		//string xmlString = GenerateCCDDocument();
		//Console.WriteLine(xmlString);

		PatientData pat = new PatientData();
		OpenEMRData patient = new OpenEMRData();
		pat = patient.OpenEMRPatientData("Nora");

		PhysicianData aut = new PhysicianData()
		{
			AddressLine = "35 King Street West",
			City = "Hamilton",
			OrgId = "123 - 1221",
			OrgName = "Good Health Clinics",

			PhysicianId = "1023433 - ON",

			PhysicianName = new string[] { " Dr.", " Francis ", " F ", " Family "},
			Postal = "L0R2A0"
		};

		

		// Create the CDA
		CCDAGenerator cCDA = new CCDAGenerator();
		cCDA.CreateCCDAXML();
	}

	


	static string GenerateCCDDocument()
	{
		XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";
		XNamespace ns = "urn:hl7-org:v3";
		XNamespace voc = "urn:hl7-org:v3/voc";
		XNamespace sdtc = "urn:hl7-org:sdtc";

		XDocument ccdDocument = new XDocument(
			new XDeclaration("1.0", "utf-8", "yes"),
			new XElement(ns + "ClinicalDocument",
				new XAttribute(XNamespace.Xmlns + "xsi", xsi),
				new XAttribute(XNamespace.Xmlns + "voc", voc),
				new XAttribute(XNamespace.Xmlns + "sdtc", sdtc),
				new XElement(ns + "realmCode", new XAttribute("code", "US")),
				new XElement(ns + "typeId", new XAttribute("root", "2.16.840.1.113883.1.3"), new XAttribute("extension", "POCD_HD000040")),
				new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.1.1"), new XAttribute("extension", "2015-08-01")),
				new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.1.1")),
				new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.1.2"), new XAttribute("extension", "2015-08-01")),
				new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.1.2")),
				new XElement(ns + "id", new XAttribute("root", "2.16.840.1.113883.19.5.99999.1"), new XAttribute("extension", "TT988")),
				new XElement(ns + "code", new XAttribute("codeSystem", "2.16.840.1.113883.6.1"), new XAttribute("codeSystemName", "LOINC"), new XAttribute("code", "34133-9"), new XAttribute("displayName", "Summarization of Episode Note")),
				new XElement(ns + "title", "OpenEMR Transitions of Care"),
				new XElement(ns + "effectiveTime", new XAttribute("value", "20220404112552+0000")),
				new XElement(ns + "confidentialityCode", new XAttribute("displayName", "Normal"), new XAttribute("code", "N"), new XAttribute("codeSystem", "2.16.840.1.113883.5.25"), new XAttribute("codeSystemName", "Confidentiality Code")),
				new XElement(ns + "languageCode", new XAttribute("code", "en-US")),
				new XElement(ns + "setId", new XAttribute("root", "2.16.840.1.113883.19.5.99999.1"), new XAttribute("extension", "sTT988")),
				new XElement(ns + "versionNumber", new XAttribute("value", "1")),
				new XElement(ns + "recordTarget",
					new XElement(ns + "patientRole",
						new XElement(ns + "id", new XAttribute("root", "2.16.840.1.113883.19.5.99999.1"), new XAttribute("extension", "PT-3")),
						new XElement(ns + "addr", new XAttribute("use", "HP"),
							new XElement(ns + "country", "US"),
							new XElement(ns + "state"),
							new XElement(ns + "city"),
							new XElement(ns + "postalCode"),
							new XElement(ns + "streetAddressLine")
						),
						new XElement(ns + "patient",
							new XElement(ns + "name", new XAttribute("use", "L"),
								new XElement(ns + "family", "Moore"),
								new XElement(ns + "given", "Wanda"),
								new XElement(ns + "prefix"),
								new XElement(ns + "suffix")
							),
							new XElement(ns + "administrativeGenderCode", new XAttribute("code", "F"), new XAttribute("codeSystem", "2.16.840.1.113883.5.1"), new XAttribute("codeSystemName", "HL7 AdministrativeGender"), new XAttribute("displayName", "FEMALE")),
							new XElement(ns + "birthTime", new XAttribute("value", "20070218")),
							new XElement(ns + "maritalStatusCode", new XAttribute("code", "S"), new XAttribute("displayName", "SINGLE"), new XAttribute("codeSystem", "2.16.840.1.113883.5.2"), new XAttribute("codeSystemName", "HL7 Marital Status")),
							new XElement(ns + "raceCode", new XAttribute("nullFlavor", "UNK")),
							new XElement(sdtc + "raceCode", new XAttribute("nullFlavor", "UNK")),
							new XElement(ns + "ethnicGroupCode", new XAttribute("nullFlavor", "UNK")),
							new XElement(ns + "languageCommunication",
								new XElement(ns + "languageCode", new XAttribute("code", "en-US")),
								new XElement(ns + "modeCode", new XAttribute("displayName", "Expressed spoken"), new XAttribute("code", "ESP"), new XAttribute("codeSystem", "2.16.840.1.113883.5.60"), new XAttribute("codeSystemName", "LanguageAbilityMode")),
								new XElement(ns + "proficiencyLevelCode", new XAttribute("code", "G"), new XAttribute("displayName", "Good"), new XAttribute("codeSystem", "2.16.840.1.113883.5.61"), new XAttribute("codeSystemName", "LanguageAbilityProficiency")),
								new XElement(ns + "preferenceInd", new XAttribute("value", "true"))
							)
						),
						new XElement(ns + "providerOrganization",
							new XElement(ns + "id", new XAttribute("root", "2.16.840.1.113883.4.6"), new XAttribute("extension", "")),
							new XElement(ns + "name", "Great Clinic"),
							new XElement(ns + "telecom", new XAttribute("use", "WP"), new XAttribute("value", "000-000-0000")),
							new XElement(ns + "addr", new XAttribute("use", "WP"),
								new XElement(ns + "country", "USA"),
								new XElement(ns + "state", "FL"),
								new XElement(ns + "city", "Longview"),
								new XElement(ns + "postalCode", "333222"),
								new XElement(ns + "streetAddressLine", "665 Roadsby Road")
							)
						)
					)
				),
				new XElement(ns + "author",
					new XElement(ns + "time", new XAttribute("value", "20140201")),
					new XElement(ns + "assignedAuthor",
						new XElement(ns + "id", new XAttribute("root", "2.16.840.1.113883.4.6"), new XAttribute("extension", "")),
						new XElement(ns + "addr", new XAttribute("use", "WP"),
							new XElement(ns + "country", "US"),
							new XElement(ns + "state"),
							new XElement(ns + "city"),
							new XElement(ns + "postalCode"),
							new XElement(ns + "streetAddressLine")
						),
						new XElement(ns + "telecom", new XAttribute("value", "0"), new XAttribute("use", "WP")),
						new XElement(ns + "assignedPerson",
							new XElement(ns + "name",
								new XElement(ns + "family", "Smith"),
								new XElement(ns + "given", "Billy")
							)
						),
						new XElement(ns + "representedOrganization",
							new XElement(ns + "id", new XAttribute("root", "2.16.840.1.113883.19.5.99999.1")),
							new XElement(ns + "name", "Great Clinic"),
							new XElement(ns + "telecom", new XAttribute("value", "tel:000-000-0000"), new XAttribute("use", "WP")),
							new XElement(ns + "addr", new XAttribute("use", "WP"),
								new XElement(ns + "country", "USA"),
								new XElement(ns + "state", "FL"),
								new XElement(ns + "city", "Longview"),
								new XElement(ns + "postalCode", "333222"),
								new XElement(ns + "streetAddressLine", "665 Roadsby Road")
							)
						)
					)
				),
								new XElement(ns + "component",
					new XElement(ns + "structuredBody",
						new XElement(ns + "component",
							new XElement(ns + "section",
								new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.2.6.1"), new XAttribute("extension", "2015-08-01")),
								new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.2.6.1")),
								new XElement(ns + "code", new XAttribute("code", "48765-2"), new XAttribute("displayName", "Allergies, adverse reactions, alerts"), new XAttribute("codeSystem", "2.16.840.1.113883.6.1"), new XAttribute("codeSystemName", "LOINC")),
								new XElement(ns + "title", "Allergies, adverse reactions, alerts"),
								new XElement(ns + "text", "No known Allergies and Intolerances"),
								new XElement(ns + "entry", new XAttribute("typeCode", "DRIV"),
									new XElement(ns + "act", new XAttribute("classCode", "ACT"), new XAttribute("moodCode", "EVN"),
										new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.4.30"), new XAttribute("extension", "2015-08-01")),
										new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.4.30")),
										new XElement(ns + "id", new XAttribute("nullFlavor", "UNK")),
										new XElement(ns + "code", new XAttribute("code", "CONC"), new XAttribute("displayName", "Concerns"), new XAttribute("codeSystem", "2.16.840.1.113883.5.6")),
										new XElement(ns + "statusCode", new XAttribute("code", "active")),
										new XElement(ns + "effectiveTime",
											new XElement(ns + "low", new XAttribute("value", "20220404"))
										),
										new XElement(ns + "entryRelationship", new XAttribute("typeCode", "SUBJ"), new XAttribute("inversionInd", "true"),
											new XElement(ns + "observation", new XAttribute("classCode", "OBS"), new XAttribute("moodCode", "EVN"), new XAttribute("negationInd", "true"),
												new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.4.7"), new XAttribute("extension", "2014-06-09")),
												new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.4.7")),
												new XElement(ns + "id", new XAttribute("nullFlavor", "UNK")),
												new XElement(ns + "code", new XAttribute("code", "ASSERTION"), new XAttribute("displayName", "Assertion"), new XAttribute("codeSystem", "2.16.840.1.113883.5.4"), new XAttribute("codeSystemName", "ActCode")),
												new XElement(ns + "statusCode", new XAttribute("code", "completed")),
												new XElement(ns + "effectiveTime",
													new XElement(ns + "low", new XAttribute("value", "20220404"))
												),
												new XElement(ns + "value", new XAttribute(xsi + "type", "CD"), new XAttribute("code", "419199007"), new XAttribute("codeSystem", "2.16.840.1.113883.6.96"), new XAttribute("codeSystemName", "SNOMED-CT"), new XAttribute("displayName", "Allergy to substance (disorder)"),
													new XElement(ns + "originalText",
														new XElement(ns + "reference", new XAttribute("value", "#reaction1"))
													)
												),
												new XElement(ns + "participant", new XAttribute("typeCode", "CSM"),
													new XElement(ns + "participantRole", new XAttribute("classCode", "MANU"),
														new XElement(ns + "playingEntity", new XAttribute("classCode", "MMAT"),
															new XElement(ns + "code", new XAttribute("nullFlavor", "NA"))
														)
													)
												)
											)
										)
									)
								)
							)
						),
						new XElement(ns + "section", new XAttribute("nullFlavor", "NI"),
							new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.2.23"), new XAttribute("extension", "2014-06-09")),
							new XElement(ns + "templateId", new XAttribute("root", "2.16.840.1.113883.10.20.22.2.23")),
							new XElement(ns + "code", new XAttribute("code", "46264-8"), new XAttribute("displayName", "Medical Equipment"), new XAttribute("codeSystem", "2.16.840.1.113883.6.1"), new XAttribute("codeSystemName", "LOINC")),
							new XElement(ns + "title", "Medical Equipment"),
							new XElement(ns + "text", "Not Available")
						)
					)
				)
			)
		);

		return ccdDocument.ToString();
	}
}
