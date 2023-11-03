using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Actions
{
    public class GenerateFunctionalStatus
    {

        FunctionalStatusModel ptFunctionalStatus;
        GenerateTableBodyStructure managetable;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocContent content;
        private IStrucDocTable tble;
        private IStrucDocThead thead;
        private IStrucDocTbody tbody;
        private IStrucDocTr tr;
        ArrayList DataArr = new ArrayList();

        public string FillFunctionalStatus(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptFunctionalStatus = new FunctionalStatusModel();
            CreateComponent(ptFunctionalStatus, clinicalDoc, hl7III);// Manage Allergy Component
            FillFunctionalStatusContent(patientinfo, hl7III, hl7factory);
            return clinicalDoc.Xml;
        }
        private void FillFunctionalStatusContent(PatientClinicalInformation patientinfo, III hl7III, Factory hl7factory)
        {
            if (patientinfo.ptFunctionalStatus != null && patientinfo.ptFunctionalStatus.Count > 0)
            {
                if (patientinfo.ptFunctionalStatus.Count > 0)
                {
                    managetable = new GenerateTableBodyStructure();
                    DataArr.Add("Functional Condition");
                    DataArr.Add("Effective Dates");
                    DataArr.Add("Condition Status");
                    tble = hl7factory.CreateStrucDocTable();
                    thead = tble.Thead;
                    tbody = tble.Tbody.Append();
                    tr = thead.Tr.Append();
                    managetable.CreateTableHeader(DataArr, hl7factory, tble, thead, tr);
                    //ArrayList alleries = new ArrayList(patientinfo.ptAllergies.ptAllergies);
                    int i = 0;
                    foreach (var item in patientinfo.ptFunctionalStatus)
                    {
                        DataArr = new ArrayList();
                        content = hl7factory.CreateStrucDocContent();
                        content.XmlId = "functional" + (i + 1);
                        content.Items.Add(item.Description);
                        DataArr.Add(content);
                        DataArr.Add(item.StatusDate.ToString());
                        DataArr.Add("Active");
                        managetable.CreateTableBody(DataArr, hl7factory, tble, tbody, tr);
                        GenerateFunctionalStatusEntry(item, i, hl7III, hl7factory);
                        i++;

                    }
                    functionalStatus.Section.Text.Items.Add(tble);
                }
                else
                {
                    CreateFunctionalStatusEmptyEntry(hl7III, hl7factory);
                }
            }
			else
			{
				CreateFunctionalStatusEmptyEntry(hl7III, hl7factory);
			}

		}
        private void CreateComponent(FunctionalStatusModel ptFunctionalStatus, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptFunctionalStatus.root != null)
            {
                hl7III.Init(ptFunctionalStatus.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptFunctionalStatus.code != null)
            {
                functionalStatus.Section.Code.Code = ptFunctionalStatus.code;
            }

            if (ptFunctionalStatus.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptFunctionalStatus.codeSystem;
            }

            if (ptFunctionalStatus.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptFunctionalStatus.codeSystemName;
            }

            if (ptFunctionalStatus.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptFunctionalStatus.displayName;
            }

            if (ptFunctionalStatus.title != null)
            {
                functionalStatus.Section.Title.Text = ptFunctionalStatus.title;
            }
        }
        public void GenerateFunctionalStatusEntry(FunctionalStatus ptfunctionalStatus,int refid, III hl7III, Factory hl7factory)
        {
            var Entry = functionalStatus.Section.Entry.Append();
            Entry.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            Entry.AsObservation.ClassCode = "OBS";
            hl7III = Entry.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.68");
            hl7III = Entry.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());

            if (Convert.ToString(ptfunctionalStatus.Description).ToLower().Contains("finding"))
            {
                Entry.AsObservation.Code.Code = "404684003";
            }
            else if (Convert.ToString(ptfunctionalStatus.Description).ToLower().Contains("disease"))
            {
                Entry.AsObservation.Code.Code = "282291009";
            }
            else
            {
                Entry.AsObservation.Code.Code = "409586006";
            }
            Entry.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            if (Convert.ToString(ptfunctionalStatus.Description).ToLower().Contains("finding"))
            {
                Entry.AsObservation.Code.DisplayName = "Finding";
            }
            else if (Convert.ToString(ptfunctionalStatus.Description).ToLower().Contains("disease"))
            {
                Entry.AsObservation.Code.DisplayName = "Diagnosis";
            }
            else
            {
                Entry.AsObservation.Code.DisplayName = "Complaint";
            }
            IVXB_TS low = new IVXB_TS();
            low.Init(Convert.ToDateTime(ptfunctionalStatus.StatusDate));
            Entry.AsObservation.Code.OriginalText.Reference.Value = ("#fs"+ (refid + 1));
            Entry.AsObservation.StatusCode.Code = "completed";
            Entry.AsObservation.EffectiveTime= new IVL_TS().Init(low: low);


            //Entry.AsObservation.Code.Code = IIf(InStr(Convert.ToString(ptfunctionalStatus.Description).ToLower(), "finding"), "404684003", IIf(InStr(Convert.ToString(ptfunctionalStatus.Description).ToLower(), "disease"), "282291009", "409586006"));
            //Entry.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            //Entry.AsObservation.Code.DisplayName = IIf(InStr(Convert.ToString(ptfunctionalStatus.Description).ToLower(), "finding"), "Finding", IIf(InStr(Convert.ToString(ptfunctionalStatus.Description).ToLower(), "disease"), "Diagnosis", "Complaint"));
            //Entry.AsObservation.Code.OriginalText.Reference.Value = "#fs" + (refid + 1);
            //Entry.AsObservation.StatusCode.Code = "completed";
            //Entry.AsObservation.EffectiveTime.Init(low: new IVXB_TS().Init(ptfunctionalStatus.StatusDate));

            CD obsValueAsCD = new CD();
            obsValueAsCD.OriginalText.Reference.Value = "#fs" + (refid + 1);
            obsValueAsCD.Code = ptfunctionalStatus.Code;
            obsValueAsCD.CodeSystem = "2.16.840.1.113883.6.96";
            obsValueAsCD.CodeSystemName = "SNOMED CT";
            obsValueAsCD.DisplayName = ptfunctionalStatus.Description;
            Entry.AsObservation.Value.Add(obsValueAsCD);

        }
        public void CreateFunctionalStatusEmptyEntry(III hl7III, Factory hl7factory)
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
    }
}
