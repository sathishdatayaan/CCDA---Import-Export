using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{
    public class GenerateAllergies
    {

        Allergy ptallergy;
        GenerateTableBodyStructure managetable;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocContent content;
        private IStrucDocTable tble;
        private IStrucDocThead thead;
        private IStrucDocTbody tbody;
        private IStrucDocTr tr;
       

        public string FillPatientAllergies(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptallergy = new Allergy();
            CreateComponent(ptallergy, clinicalDoc, hl7III);// Manage Allergy Component
            FillAllergiesContent(patientinfo, hl7factory);
            if (patientinfo.ptAllergies != null && patientinfo.ptAllergies.Count > 0)
            {
                if (patientinfo.ptAllergies.Count > 0)
                {
                    foreach (PatientAllergies item in patientinfo.ptAllergies)
                    {
                        GenerateAllergyEntry(item, hl7III, hl7factory);
                    }
                }
                else
                {
                    GenerateAllergyEntryEmpty(hl7III, hl7factory);
                }
            }
			else
			{
				GenerateAllergyEntryEmpty(hl7III, hl7factory);
			}
			return clinicalDoc.Xml;
        }
        private void FillAllergiesContent(PatientClinicalInformation patientinfo, Factory hl7factory)
        {
            if (patientinfo.ptAllergies != null && patientinfo.ptAllergies.Count > 0)
            {

                if (patientinfo.ptAllergies.Count > 0)
                {
                    managetable = new GenerateTableBodyStructure();
                    ArrayList DataArr = new ArrayList();
                    DataArr.Add("Substance");
                    DataArr.Add("Reaction");
                    DataArr.Add("Status");
                    DataArr.Add("Date");
                    tble = hl7factory.CreateStrucDocTable();
                    thead = tble.Thead;
                    tbody = tble.Tbody.Append();
                    tr = thead.Tr.Append();
                    managetable.CreateTableHeader(DataArr, hl7factory, tble, thead, tr);
                    //ArrayList alleries = new ArrayList(patientinfo.ptAllergies.ptAllergies);
                    int i = 0;
                    foreach (var item in patientinfo.ptAllergies)
                    {
                        DataArr = new ArrayList();
                        DataArr.Add(Convert.ToString(item.substance));
                        content = hl7factory.CreateStrucDocContent();
                        content.XmlId = "reaction" + (i + 1);
                        content.Items.Add(item.reaction);
                        DataArr.Add(content);
                        DataArr.Add(item.status);
                        DataArr.Add(item.reaction);
                        managetable.CreateTableBody(DataArr, hl7factory, tble, tbody, tr);
                        i++;

                    }
                    functionalStatus.Section.Text.Items.Add(tble);
                    //managetable.CreateTableBody(alleries, hl7factory);
                }
                else
                {
                    IStrucDocParagraph paragraph = hl7factory.CreateStrucDocParagraph();
                    paragraph.Items.Add("N/A");
                    functionalStatus.Section.Text.Items.Add(paragraph);
                }
            }
			else
			{
				IStrucDocParagraph paragraph = hl7factory.CreateStrucDocParagraph();
				paragraph.Items.Add("N/A");
				functionalStatus.Section.Text.Items.Add(paragraph);
			}

		}
        private void CreateComponent(Allergy ptallergy, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptallergy.root != null)
            {
                hl7III.Init(ptallergy.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptallergy.code != null)
            {
                functionalStatus.Section.Code.Code = ptallergy.code;
            }

            if (ptallergy.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptallergy.codeSystem;
            }

            if (ptallergy.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptallergy.codeSystemName;
            }

            if (ptallergy.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptallergy.displayName;
            }

            if (ptallergy.title != null)
            {
                functionalStatus.Section.Title.Text = ptallergy.title;
            }
        }
        public void GenerateAllergyEntry(PatientAllergies patientAllergies, III hl7III, Factory hl7factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsAct.ClassCode = x_ActClassDocumentEntryAct.ACT;
            entry.AsAct.MoodCode = x_DocumentActMood.EVN;
            hl7III = entry.AsAct.Id.Append();
            hl7III.Init("36e3e930-7b14-11db-9fe1-0800200c9a66");
            hl7III = entry.AsAct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.30");
            entry.AsAct.Code.Code = "48765-2";
            entry.AsAct.Code.CodeSystem = "2.16.840.1.113883.6.1";
            entry.AsAct.Code.CodeSystemName = "LOINC";

            entry.AsAct.Code.DisplayName = "Allergies, adverse reactions, alerts";

            entry.AsAct.StatusCode.Code = Convert.ToString(patientAllergies.status).ToLower();

            if (Convert.ToString(patientAllergies.status) == "Active")
            {
                entry.AsAct.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                // If Status Active
            }
            else
            {
                entry.AsAct.EffectiveTime = new IVL_TS().Init(high: new IVXB_TS());
                // If Status is Not Active
            }

            IEntryRelationship entryRel = entry.AsAct.EntryRelationship.Append();
            entryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            entryRel.AsObservation.ClassCode = "OBS";
            entryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            III TempId = entryRel.AsObservation.TemplateId.Append();
            TempId.Root = "2.16.840.1.113883.10.20.22.4.7";
            entryRel.AsObservation.StatusCode.Code = "completed";
            hl7III = entryRel.AsObservation.Id.Append();
            hl7III.Root = "4adc1020-7b14-11db-9fe1-0800200c9a66";
            entryRel.AsObservation.Code.Code = "ASSERTION";
            entryRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.5.4";
            ICS CS = hl7factory.CreateCS();
            CS.Init("completed");

            entryRel.AsObservation.EffectiveTime.NullFlavor = "UNK";
            // Because Allergy Start Date is Missing.
            CD obsValueAsCD = new CD();
            CE obsValueAsCE = new CE();
            //obsValueAsCD.Code = patientAllergies.AllergyType == "Medication" ? "416098002" : patientAllergies.AllergyType == "Food" ? "414285001" : patientAllergies.AllergyType == "Environmental" ? "419199007" : "420134006";//(patientAllergies.AllergyType == "Medication" ? "416098002" : (patientAllergies.AllergyType == "Food" ? "414285001" : (patientAllergies.AllergyType == "Environmental" ? "426232007" : "106190000")));
            //// Allergy Type SNOMED Code(Environmental Alergy:426232007, Food Allergy: 414285001, Medication Allergy: 416098002, Other Allergy: 106190000)
            //obsValueAsCD.DisplayName = (patientAllergies.AllergyType == "Medication" ? "Drug allergy" : (patientAllergies.AllergyType == "Food" ? "Food allergy" : (patientAllergies.AllergyType == "Environmental" ? "Environmental allergy" : "Other allergy")));

            obsValueAsCD.CodeSystem = "2.16.840.1.113883.6.96";
            obsValueAsCD.CodeSystemName = "SNOMED CT";
            obsValueAsCD.OriginalText.Reference.Value = "#" + Convert.ToString(patientAllergies.allergyId);
            // "#reaction1" ' Dynamic Value
            entryRel.AsObservation.Value.Add(obsValueAsCD);
            PN pn = new PN();
            pn.Text = patientAllergies.substance;
            IParticipant2 parti = hl7factory.CreateParticipant2();
            parti.TypeCode = "CSM";
            parti.ParticipantRole.ClassCode = "MANU";
            parti.ParticipantRole.AsPlayingEntity.ClassCode = "MMAT";
            parti.ParticipantRole.AsPlayingEntity.Code.Code = Convert.ToString(patientAllergies.rxNorm);
            //"314422" ' Dynamic Value
            parti.ParticipantRole.AsPlayingEntity.Code.DisplayName = patientAllergies.substance;
            //"ALLERGENIC EXTRACT, PENICILLIN" ' Dynamic Value
            parti.ParticipantRole.AsPlayingEntity.Code.CodeSystem = "2.16.840.1.113883.6.88";
            parti.ParticipantRole.AsPlayingEntity.Code.CodeSystemName = "RxNorm";
            parti.ParticipantRole.AsPlayingEntity.Code.OriginalText.Reference.Value = "";
            //"#" & Convert.ToString(dtRow("ReactionID"))
            parti.ParticipantRole.AsPlayingEntity.Name.Add(pn);
            entryRel.AsObservation.Participant.Add(parti);

            ///'' Allergy Status Observation '''''''''''
            IEntryRelationship obs_EntryRel = entryRel.AsObservation.EntryRelationship.Append();
            obs_EntryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            obs_EntryRel.InversionInd = true;
            obs_EntryRel.InversionIndSpecified = true;
            obs_EntryRel.AsObservation.ClassCode = "OBS";
            obs_EntryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = obs_EntryRel.AsObservation.TemplateId.Append();
            hl7III.Root = "2.16.840.1.113883.10.20.22.4.28";
            obs_EntryRel.AsObservation.Code.Code = "33999-4";
            obs_EntryRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.1";
            obs_EntryRel.AsObservation.Code.CodeSystemName = "LOINC";
            obs_EntryRel.AsObservation.Code.DisplayName = "Status";
            obs_EntryRel.AsObservation.StatusCode.Code = "completed";


            obsValueAsCE = new CE();
            obsValueAsCE.Code = Convert.ToString(patientAllergies.status) == "Active" ? "55561003" : patientAllergies.status == "Inactive" ? "73425007" : "413322009";
            obsValueAsCE.DisplayName = Convert.ToString(patientAllergies.status);
            obsValueAsCE.CodeSystem = "2.16.840.1.113883.6.96";

            obs_EntryRel.AsObservation.Value.Add(obsValueAsCE);
            obs_EntryRel.InversionInd = true;
            obs_EntryRel.InversionIndSpecified = true;
            ///' Allergy Reaction Observation
            if (!string.IsNullOrEmpty(patientAllergies.reaction))
            {
                obs_EntryRel = entryRel.AsObservation.EntryRelationship.Append();
                obs_EntryRel.TypeCode = x_ActRelationshipEntryRelationship.MFST;
                obs_EntryRel.InversionInd = true;
                obs_EntryRel.InversionIndSpecified = true;
                obs_EntryRel.AsObservation.ClassCode = "OBS";
                obs_EntryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
                hl7III = obs_EntryRel.AsObservation.TemplateId.Append();
                hl7III.Init("2.16.840.1.113883.10.20.22.4.9");
                hl7III = obs_EntryRel.AsObservation.Id.Append();
                hl7III.Init("4adc1020-7b14-11db-9fe1-0800200c9a64");
                obs_EntryRel.AsObservation.Code.NullFlavor = "UNK";
                //obs_EntryRel.AsObservation.Text.Reference.Value = "#" & Convert.ToString(dtRow("ReactionID")) '"#reaction1"
                obs_EntryRel.AsObservation.StatusCode.Code = "completed";
                obs_EntryRel.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS(), high: new IVXB_TS());
                obsValueAsCD = new CD();
                if (!string.IsNullOrEmpty(GetSNOMED(patientAllergies.reaction)))
                {
                    obsValueAsCD.Code = GetSNOMED(patientAllergies.reaction);
                    // Dynamic Value ''
                }

                obsValueAsCD.DisplayName = patientAllergies.reaction;
                //"Hives"
                obsValueAsCD.CodeSystem = "2.16.840.1.113883.6.96";
                obsValueAsCD.OriginalText.Reference.Value = "#" + Convert.ToString(patientAllergies.allergyId);
                obs_EntryRel.AsObservation.Value.Add(obsValueAsCD);
            }

            ///' Allergy Severity Observation
            if (!string.IsNullOrEmpty(patientAllergies.reaction))
            {
                obs_EntryRel = entryRel.AsObservation.EntryRelationship.Append();
                obs_EntryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
                obs_EntryRel.InversionInd = true;
                obs_EntryRel.InversionIndSpecified = true;
                obs_EntryRel.AsObservation.ClassCode = "OBS";
                obs_EntryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
                hl7III = obs_EntryRel.AsObservation.TemplateId.Append();
                hl7III.Init("2.16.840.1.113883.10.20.22.4.8");
                obs_EntryRel.AsObservation.Code.Code = "SEV";
                obs_EntryRel.AsObservation.Code.DisplayName = "Severity Observation";
                obs_EntryRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.5.4";
                obs_EntryRel.AsObservation.Code.CodeSystemName = "ActCode";
                //obs_EntryRel.AsObservation.Text.Reference.Value = "#severity1"
                obs_EntryRel.AsObservation.StatusCode.Code = "completed";
                obsValueAsCD = new CD();
                obsValueAsCD.Code = (Convert.ToString(patientAllergies.reaction) == "Mild" ? "255604002" : (Convert.ToString(patientAllergies.reaction) == "Moderate" ? "6736007" : "24484000"));
                obsValueAsCD.DisplayName = patientAllergies.reaction;
                // Moderate or Mild or severe"
                obsValueAsCD.CodeSystem = "2.16.840.1.113883.6.96";
                obsValueAsCD.CodeSystemName = "SNOMED CT";
                obsValueAsCD.OriginalText.Reference.Value = "#" + Convert.ToString(patientAllergies.allergyId);
                obs_EntryRel.AsObservation.Value.Add(obsValueAsCD);
            }


        }
        public void GenerateAllergyEntryEmpty(III hl7III, Factory hl7factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsAct.ClassCode = x_ActClassDocumentEntryAct.ACT;
            entry.AsAct.MoodCode = x_DocumentActMood.EVN;
            hl7III = entry.AsAct.Id.Append();
            hl7III.NullFlavor = "UNK";
            hl7III = entry.AsAct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.30");
            entry.AsAct.Code.Code = "48765-2";
            entry.AsAct.Code.CodeSystem = "2.16.840.1.113883.6.1";
            entry.AsAct.Code.CodeSystemName = "LOINC";
            entry.AsAct.Code.DisplayName = "Allergies, adverse reactions, alerts";
            entry.AsAct.StatusCode.Code = "completed";
            entry.AsAct.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS(), high: new IVXB_TS());
            // If Status Complete

            IEntryRelationship entryRel = entry.AsAct.EntryRelationship.Append();
            entryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            entryRel.AsObservation.ClassCode = "OBS";
            entryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            III TempId = entryRel.AsObservation.TemplateId.Append();
            TempId.Root = "2.16.840.1.113883.10.20.22.4.7";
            entryRel.AsObservation.StatusCode.Code = "completed";
            hl7III = entryRel.AsObservation.Id.Append();
            hl7III.Root = "1.3.6.1.4.1.22812.11.0.100610.4.10.2";
            hl7III.Extension = "41700060";
            entryRel.AsObservation.Code.Code = "ASSERTION";
            entryRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.5.4";
            entryRel.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
            CD obsValueAsCD = new CD();
            obsValueAsCD.NullFlavor = "UNK";
            entryRel.AsObservation.Value.Add(obsValueAsCD);
            PN pn = new PN();
            pn.Text = "N/A";
            IParticipant2 parti = hl7factory.CreateParticipant2();
            parti.TypeCode = "CSM";
            parti.ParticipantRole.ClassCode = "MANU";
            parti.ParticipantRole.AsPlayingEntity.ClassCode = "MMAT";
            parti.ParticipantRole.AsPlayingEntity.Code.NullFlavor = "UNK";
            parti.ParticipantRole.AsPlayingEntity.Name.Add(pn);
            entryRel.AsObservation.Participant.Add(parti);
        }
        public string GetSNOMED(string Description)
        {
            //try
            //{
            //    string snomedIDs = string.Empty;
            //    SnApiServiceClient snomedClient = new SnApiServiceClient();
            //    Concept[] SNOMEDID = snomedClient.SearchConcepts(Description, "", "0", "", "", "", "", "", "", "", "0", 100, 0, "W", "ad71930b-b37e-4299-bd87-5dea35b42173");
            //    if (SNOMEDID.Length == 1)
            //    {
            //        snomedIDs = SNOMEDID[0].ConceptId.ToString();
            //        return snomedIDs;
            //    }

            //    foreach (SnAPIService.Concept value in SNOMEDID)
            //    {
            //        if ((value.FullySpecifiedName.ToLower().IndexOf(Description.ToLower()) == 0))
            //        {
            //            snomedIDs = value.ConceptId.ToString();
            //            break; // TODO: might not be correct. Was : Exit For
            //        }
            //    }
            //    return snomedIDs;
            //}
            //catch (Exception ex)
            //{
            //    return string.Empty;
            //}
            return Description;
        }
    }
}
