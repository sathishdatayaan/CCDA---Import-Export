using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Actions
{
    public class GeneratePatientProblem
    {
        PatientProblem ptProblem;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocItem listItem;
        private IStrucDocContent contentItem;
        private IStrucDocList list;
        ArrayList DataArr = new ArrayList();
        public string FillPatientProblemes(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptProblem = new PatientProblem();
            CreateComponent(ptProblem, clinicalDoc, hl7III);// Manage Problem Component
            FillProblemContent(patientinfo, hl7factory);
            int i = 0;

            if (patientinfo.ptProblemes != null && patientinfo.ptProblemes.Count > 0)
            {
                if (patientinfo.ptProblemes.Count > 0)
                {
                    foreach (PatientProblemes item in patientinfo.ptProblemes)
                    {
                        GenerateProblemEntry(item, "#Problem" + (i + 1), Convert.ToDateTime(patientinfo.ptDemographicDetail.DateofBirth), hl7III, hl7factory);
                        i++;
                    }
                }
                else
                {
                    GenerateProblemEntryEmpty("#noproblems1", hl7III, hl7factory);
                }
            }
			else
			{
				GenerateProblemEntryEmpty("#noproblems1", hl7III, hl7factory);
			}

			return clinicalDoc.Xml;
        }
        private void FillProblemContent(PatientClinicalInformation patientinfo, Factory hl7factory)
        {
            var arrPatientProblems = patientinfo.ptProblemes;

            if (patientinfo.ptProblemes != null && patientinfo.ptProblemes.Count > 0)
            {
                if (arrPatientProblems.Count > 0)
                {
                    list = hl7factory.CreateStrucDocList();
                    //Creating List Object.


                    int i = 0;
                    foreach (var patientProblem in arrPatientProblems)
                    {
                        listItem = hl7factory.CreateStrucDocItem();
                        contentItem = hl7factory.CreateStrucDocContent();
                        contentItem.XmlId = "Problem" + (i + 1);

                        contentItem.Items.Add(Convert.ToString(patientProblem.Description) + ": Status - " + Convert.ToString(patientProblem.Status) + ", Date Diagnosed - " + Convert.ToString(patientProblem.DateDiagnosed));
                        listItem.Items.Add(contentItem);
                        list.ListType = 0;
                        list.Item.Add(listItem);
                        i++;
                    }
                    functionalStatus.Section.Text.Items.Add(list);
                    // Adding List to Text Tag.
                }
                else
                {
                    contentItem = hl7factory.CreateStrucDocContent();
                    IStrucDocText text = hl7factory.CreateStrucDocText();
                    contentItem.XmlId = "noproblems1";
                    contentItem.Items.Add("No known problems");
                    text.Items.Add(contentItem);
                    functionalStatus.Section.Text.Items.Add(contentItem);
                }
            }
			else
			{
				contentItem = hl7factory.CreateStrucDocContent();
				IStrucDocText text = hl7factory.CreateStrucDocText();
				contentItem.XmlId = "noproblems1";
				contentItem.Items.Add("No known problems");
				text.Items.Add(contentItem);
				functionalStatus.Section.Text.Items.Add(contentItem);
			}
		}
        private void CreateComponent(PatientProblem ptProblem, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptProblem.root != null)
            {
                hl7III.Init(ptProblem.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptProblem.code != null)
            {
                functionalStatus.Section.Code.Code = ptProblem.code;
            }

            if (ptProblem.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptProblem.codeSystem;
            }

            if (ptProblem.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptProblem.codeSystemName;
            }

            if (ptProblem.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptProblem.displayName;
            }

            if (ptProblem.title != null)
            {
                functionalStatus.Section.Title.Text = ptProblem.title;
            }
        }
        public void GenerateProblemEntry(PatientProblemes patientProblem, string refId, DateTime dateofbirth, III hl7III, Factory hl7Factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsAct.ClassCode = x_ActClassDocumentEntryAct.ACT;
            entry.AsAct.MoodCode = x_DocumentActMood.EVN;
            IIVXB_TS TS = hl7Factory.CreateIVXB_TS();
            hl7III = entry.AsAct.Id.Append();

            hl7III.Init(Convert.ToString(patientProblem.PatientGuid));
            hl7III = entry.AsAct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.3");
            entry.AsAct.Code.Code = "CONC";
            entry.AsAct.Code.CodeSystem = "2.16.840.1.113883.5.6";
            entry.AsAct.Code.CodeSystemName = "LOINC";

            entry.AsAct.StatusCode.Init((Convert.ToString(patientProblem.Status) == "Active" ? "active" : "completed"));
            if (patientProblem.DateDiagnosed != null)
            {
                entry.AsAct.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
            }
            else
            {
                entry.AsAct.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                //entry.AsAct.EffectiveTime.NullFlavor = "UNK";
                /* Not Compiled
                entry.AsAct.EffectiveTime = new IVL_TS().Init(Value:null, low:new IVXB_TS().Init(Convert.ToDateTime(patientProblem.DateDiagnosed)),high:null,width:null,center:null);*/
                // Setting the DateTime.
            }

            IEntryRelationship entryRel = entry.AsAct.EntryRelationship.Append();
            entryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            entryRel.AsObservation.ClassCode = "OBS";
            entryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            III TempId = entryRel.AsObservation.TemplateId.Append();
            TempId.Root = "2.16.840.1.113883.10.20.22.4.4";
            hl7III = entryRel.AsObservation.Id.Append();
            hl7III.Root = Guid.NewGuid().ToString();

            entryRel.AsObservation.Code.Code = Convert.ToString(patientProblem.Description).ToLower().Contains("finding") ? "404684003" : Convert.ToString(patientProblem.Description).ToLower().Contains("disease") ? "282291009" : "409586006";
            entryRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            entryRel.AsObservation.Code.DisplayName = Convert.ToString(patientProblem.Description).ToLower().Contains("finding") ? "Finding" : Convert.ToString(patientProblem.Description).ToLower().Contains("disease") ? "Diagnosis" : "Complaint";


            IED ED = hl7Factory.CreateED();
            ED.Reference.Value = refId;
            entryRel.AsObservation.Text = ED;
            ICS CS = hl7Factory.CreateCS();
            CS.Init("completed");
            entryRel.AsObservation.StatusCode = CS;

            if (Convert.ToString(patientProblem.Status) == "Resolved")
            {
                if (object.ReferenceEquals(patientProblem.DateDiagnosed, DBNull.Value))
                {
                    entry.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                }
                else
                {
                    entry.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                    //entryRel.AsObservation.EffectiveTime.NullFlavor = "UNK";
                    /* Not Compiled
                  entryRel.AsObservation.EffectiveTime.Init(low: new IVXB_TS().Init(Convert.ToDateTime(patientProblem.DateDiagnosed)), high: new IVXB_TS());
                    // Because Allergy Start Date is Missing.
                  
                    entryRel.AsObservation.EffectiveTime.High.NullFlavor = "UNK";*/
                }
            }
            else
            {
                if (object.ReferenceEquals(patientProblem.DateDiagnosed, DBNull.Value))
                {
                    entryRel.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                    //entryRel.AsObservation.EffectiveTime.NullFlavor = "UNK";
                }
                else
                {
                    entryRel.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                    //entryRel.AsObservation.EffectiveTime.NullFlavor = "UNK";
                    /* Not Compiled
                  entryRel.AsObservation.EffectiveTime.Init(low: new IVXB_TS().Init(Convert.ToDateTime(patientProblem.DateDiagnosed)), high: new IVXB_TS());
                  entryRel.AsObservation.EffectiveTime.High.NullFlavor = "UNK";*/
                }
            }

            ICD CD = hl7Factory.CreateCD();

            string snomedCode = GetSNOMED(Convert.ToString(patientProblem.Description));
            if (string.IsNullOrEmpty(snomedCode))
            {
                CD.NullFlavor = "UNK";
            }
            else
            {
                CD.Code = snomedCode;
                CD.CodeSystem = "2.16.840.1.113883.6.96";
                CD.CodeSystemName = "SNOMED CT";
                CD.DisplayName = Convert.ToString(patientProblem.Description);
            }

            entryRel.AsObservation.Value.Add(CD);

            ///''''''''''''Problem Observation''''''''''''''''''
            IEntryRelationship entryProblemObs = entryRel.AsObservation.EntryRelationship.Append();
            entryProblemObs.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            entryProblemObs.AsObservation.ClassCode = "OBS";
            entryProblemObs.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            TempId = entryProblemObs.AsObservation.TemplateId.Append();
            TempId.Root = "2.16.840.1.113883.10.20.22.4.4";
            hl7III = entryProblemObs.AsObservation.Id.Append();
            hl7III.Root = Guid.NewGuid().ToString();

            entryProblemObs.AsObservation.Code.Code = Convert.ToString(patientProblem.Description).ToLower().Contains("finding") ? "404684003" : Convert.ToString(patientProblem.Description).ToLower().Contains("disease") ? "282291009" : "409586006";
            entryProblemObs.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            entryProblemObs.AsObservation.Code.DisplayName = Convert.ToString(patientProblem.Description).ToLower().Contains("finding") ? "Finding" : Convert.ToString(patientProblem.Description).ToLower().Contains("disease") ? "Diagnosis" : "Complaint";

            ED = hl7Factory.CreateED();
            ED.Reference.Value = refId;
            entryProblemObs.AsObservation.Text = ED;
            CS = hl7Factory.CreateCS();
            CS.Init("completed");
            entryProblemObs.AsObservation.StatusCode = CS;

            if (Convert.ToString(patientProblem.Status) == "Resolved")
            {
                if (object.ReferenceEquals(patientProblem.DateDiagnosed, DBNull.Value))
                {
                    entryProblemObs.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                    //entryProblemObs.AsObservation.EffectiveTime.NullFlavor = "UNK";
                }
                else
                {
                    entryProblemObs.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                    /* Not Compiled 
                    entryProblemObs.AsObservation.EffectiveTime.Init(low: new IVXB_TS().Init(Convert.ToDateTime(patientProblem.DateDiagnosed)), high: new IVXB_TS());
                    // Because Allergy Start Date is Missing.
                    entryProblemObs.AsObservation.EffectiveTime.High.NullFlavor = "UNK";*/
                }
            }
            else
            {
                if (object.ReferenceEquals(patientProblem.DateDiagnosed, DBNull.Value))
                {
                    entryProblemObs.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());

                }
                else
                {
                    entryProblemObs.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
                    /* Not Compiled 
                    entryProblemObs.AsObservation.EffectiveTime.Init(low: new IVXB_TS().Init(Convert.ToDateTime(patientProblem.DateDiagnosed)), high: new IVXB_TS());
                    entryProblemObs.AsObservation.EffectiveTime.High.NullFlavor = "UNK";*/
                }
            }

            CD = hl7Factory.CreateCD();
            snomedCode = GetSNOMED(Convert.ToString(patientProblem.Description));
            if (string.IsNullOrEmpty(snomedCode))
            {
                CD.NullFlavor = "UNK";
            }
            else
            {
                CD.Code = snomedCode;
                CD.CodeSystem = "2.16.840.1.113883.6.96";
                CD.DisplayName = Convert.ToString(patientProblem.Description);
            }

            entryProblemObs.AsObservation.Value.Add(CD);


            ///'''''''''''' Age Observation''''''''''''''''''

            if ((!object.ReferenceEquals(patientProblem.DateDiagnosed, DBNull.Value)))
            {
                IEntryRelationship age_EntryRel = entryRel.AsObservation.EntryRelationship.Append();
                age_EntryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
                age_EntryRel.InversionInd = true;
                age_EntryRel.InversionIndSpecified = true;
                age_EntryRel.AsObservation.ClassCode = "OBS";
                age_EntryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
                hl7III = age_EntryRel.AsObservation.TemplateId.Append();
                hl7III.Init("2.16.840.1.113883.10.20.22.4.31");
                CD = hl7Factory.CreateCD();
                CD.Init("445518008", "2.16.840.1.113883.6.96", "", "Age");
                age_EntryRel.AsObservation.Code = CD;
                age_EntryRel.AsObservation.StatusCode.Init("completed");
                IPQ PQ = hl7Factory.CreatePQ();
                PQ.Value = calcYear(Convert.ToDateTime(patientProblem.DateDiagnosed), dateofbirth);
                PQ.Unit = "a";
                age_EntryRel.AsObservation.Value.Add(PQ);
            }

            ///'''''''''''' Problem Status Observation'''''''''''''
            IEntryRelationship problemStatus_EntryRel = entryRel.AsObservation.EntryRelationship.Append();
            problemStatus_EntryRel.TypeCode = x_ActRelationshipEntryRelationship.REFR;
            problemStatus_EntryRel.InversionInd = true;
            problemStatus_EntryRel.InversionIndSpecified = true;
            problemStatus_EntryRel.AsObservation.ClassCode = "OBS";
            problemStatus_EntryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = problemStatus_EntryRel.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.6");

            CD = hl7Factory.CreateCD();
            CD.Init("33999-4", "2.16.840.1.113883.6.1", "LOINC", "Status");
            problemStatus_EntryRel.AsObservation.Code = CD;
            problemStatus_EntryRel.AsObservation.StatusCode.Init("completed");
            //55561003-Active, 73425007-Inactive, 413322009- Resolved
            CD = hl7Factory.CreateCD();

            CD.Init((Convert.ToString(patientProblem.Status) == "Active" ? "55561003" : (Convert.ToString(patientProblem.Status) == "Inactive" ? "73425007" : "413322009")), "2.16.840.1.113883.6.96", "SNOMED CT", Convert.ToString(patientProblem.Status));
            problemStatus_EntryRel.AsObservation.Value.Add(CD);


        }
        public int calcYear(DateTime DateDiagnosed, DateTime dateOfBirth)
        {

            DateTime dayStart = Convert.ToDateTime(dateOfBirth);
            DateTime dateEnd = Convert.ToDateTime(DateDiagnosed);
            TimeSpan ts = dateEnd - dayStart;
            int Years = Convert.ToInt32(ts.TotalDays) / 365;
            return Years;


        }
        public void GenerateProblemEntryEmpty(string refId, III hl7III, Factory hl7Factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsAct.ClassCode = x_ActClassDocumentEntryAct.ACT;
            entry.AsAct.MoodCode = x_DocumentActMood.EVN;
            IIVXB_TS TS = hl7Factory.CreateIVXB_TS();
            hl7III = entry.AsAct.Id.Append();
            hl7III.Init(Convert.ToString(Guid.NewGuid()));
            hl7III = entry.AsAct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.3");
            entry.AsAct.Code.Code = "CONC";
            entry.AsAct.Code.CodeSystem = "2.16.840.1.113883.5.6";
            entry.AsAct.Code.CodeSystemName = "HL7ActClass";
            entry.AsAct.Code.DisplayName = "Concern";
            entry.AsAct.StatusCode.Init("completed");
            entry.AsAct.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS());
            IEntryRelationship entryRel = entry.AsAct.EntryRelationship.Append();
            entryRel.TypeCode = x_ActRelationshipEntryRelationship.SUBJ;
            entryRel.AsObservation.ClassCode = "OBS";
            entryRel.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            entryRel.AsObservation.NegationInd = true;
            entryRel.AsObservation.NegationIndSpecified = true;
            III TempId = entryRel.AsObservation.TemplateId.Append();
            TempId.Root = "2.16.840.1.113883.10.20.22.4.4";
            hl7III = entryRel.AsObservation.Id.Append();
            hl7III.Root = Guid.NewGuid().ToString();
            entryRel.AsObservation.Code.Code = "55607006";
            entryRel.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
            entryRel.AsObservation.Code.DisplayName = "Problem";
            entryRel.AsObservation.Code.CodeSystemName = "SNOMED CT";
            IED ED = hl7Factory.CreateED();
            ED.Reference.Value = refId;
            entryRel.AsObservation.Text = ED;
            ICS CS = hl7Factory.CreateCS();
            CS.Init("completed");
            entryRel.AsObservation.StatusCode = CS;
            entryRel.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS(), high: new IVXB_TS());
            ICD CD = hl7Factory.CreateCD();
            CD.Code = "55607006";
            CD.CodeSystem = "2.16.840.1.113883.6.96";
            CD.CodeSystemName = "SNOMED CT";
            CD.DisplayName = Convert.ToString("Problem");
            entryRel.AsObservation.Value.Add(CD);
        }
        public string GetSNOMED(string Description)
        {
            string result = String.Empty;
            switch (Description)
            {
                case "Benign hypertension":
                    result= "10725009";
                    break;
                case "Acquired Brain Injury":
                    result = "702632000";
                    break;
                case "Lowe Syndrome":
                    result = "79385002";
                    break;
            }
            return result;
        }
    }
}
