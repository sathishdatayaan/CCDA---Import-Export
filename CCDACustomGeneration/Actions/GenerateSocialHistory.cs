using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{

    public class GenerateSocialHistory
    {
        SocialHistory ptSocialHistory;
        GenerateTableBodyStructure managetable;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocContent content;
        private IStrucDocTable tble;
        private IStrucDocContent contentItem;
        private IStrucDocThead thead;
        private IStrucDocTbody tbody;
        private IStrucDocTr tr;
        private IStrucDocList list;
        ArrayList DataArr = new ArrayList();
        public string FillSocialHistory(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptSocialHistory = new SocialHistory();
            CreateComponent(ptSocialHistory, clinicalDoc, hl7III);// Manage Problem Component
            FillSocialHistoryContent(patientinfo, hl7factory);

            //CreateSocialHistoryBody(patientinfo.ptSocialHistory);
            // Creating Social History Table Body with Table Structure
            int idCount = 1;
            ///''' For Smoking Status ''''''

            if (patientinfo.ptSocialHistory != null)
            {
                GenerateSocialHistoryEntry(patientinfo.ptSocialHistory.Smoker, ref idCount, "Smoking", hl7III, hl7factory);
            }
            else
            {
                GenerateSocialHistoryEmpty(hl7III, hl7factory);
            }
            ///''' For Alcohol Status '''''
            if (patientinfo.ptSocialHistory != null)
            {
                if (!string.IsNullOrEmpty(patientinfo.ptSocialHistory.Alcohol))
                {
                    GenerateSocialHistoryEntry(patientinfo.ptSocialHistory.Alcohol, ref idCount, "Alcohol", hl7III, hl7factory);
                }

                if (!string.IsNullOrEmpty(patientinfo.ptSocialHistory.Drugs))
                {
                    GenerateSocialHistoryEntry(patientinfo.ptSocialHistory.Drugs, ref idCount, "Drugs", hl7III, hl7factory);
                }

                if (!string.IsNullOrEmpty(patientinfo.ptSocialHistory.Tobacoo))
                {
                    GenerateSocialHistoryEntry(patientinfo.ptSocialHistory.Tobacoo, ref idCount, "Tobacoo", hl7III, hl7factory);
                }
            }
            return clinicalDoc.Xml;
        }
        private void FillSocialHistoryContent(PatientClinicalInformation patientinfo, Factory hl7factory)
        {
            
            if (patientinfo.ptSocialHistory != null)
            {
                managetable = new GenerateTableBodyStructure();
                DataArr = new ArrayList();
                DataArr.Add("Social History Element");
                DataArr.Add("Description");
                DataArr.Add("Effective Dates");
                tble = hl7factory.CreateStrucDocTable();
                thead = tble.Thead;
                tbody = tble.Tbody.Append();
                tr = thead.Tr.Append();
                managetable.CreateTableHeader(DataArr, hl7factory, tble, thead, tr);
                //isExistTbody = "false";
                int i = 1;
                ///' Smoking Status ''''

                if (patientinfo.ptSocialHistory != null && !string.IsNullOrEmpty(patientinfo.ptSocialHistory.Smoker))
                {
                    SocialHistoryTRFill(ref i, patientinfo.ptSocialHistory.Smoker, "Smoking", Convert.ToDateTime(patientinfo.ptSocialHistory.EntryDate), hl7factory, tble, tbody, tr);
                    //isExistTbody = "true";
                }

                if (patientinfo != null)
                {
                    if (!string.IsNullOrEmpty(patientinfo.ptSocialHistory.Alcohol))
                    {
                        SocialHistoryTRFill(ref i, patientinfo.ptSocialHistory.Alcohol, "Alcohol consumption", Convert.ToDateTime(patientinfo.ptSocialHistory.EntryDate), hl7factory, tble, tbody, tr);
                        //isExistTbody = "true";
                    }

                    if (!string.IsNullOrEmpty(patientinfo.ptSocialHistory.Drugs))
                    {
                        SocialHistoryTRFill(ref i, patientinfo.ptSocialHistory.Drugs, "Drug consumption", Convert.ToDateTime(patientinfo.ptSocialHistory.EntryDate), hl7factory, tble, tbody, tr);
                        //isExistTbody = "true";
                    }

                    if (!string.IsNullOrEmpty(patientinfo.ptSocialHistory.Tobacoo))
                    {
                        SocialHistoryTRFill(ref i, patientinfo.ptSocialHistory.Tobacoo, "Tobacoo consumption", Convert.ToDateTime(patientinfo.ptSocialHistory.EntryDate), hl7factory, tble, tbody, tr);
                        //isExistTbody = "true";
                    }
                }
                functionalStatus.Section.Text.Items.Add(tble);
            }
            else
            {
                contentItem = hl7factory.CreateStrucDocContent();
                contentItem.Items.Add("Social History N/A");
                functionalStatus.Section.Text.Items.Add(contentItem);
            }
        }
        public void SocialHistoryTRFill(ref int Id, string value, string description, DateTime EffectiveDate, Factory hl7factory, IStrucDocTable tble, IStrucDocTbody tbody, IStrucDocTr tr)
        {
            content = hl7factory.CreateStrucDocContent();
            content.XmlId = "Sec" + Id;
            content.Items.Add(description);
            DataArr = new ArrayList();
            DataArr.Add(content);
            DataArr.Add(value);
            DataArr.Add(Convert.ToString(EffectiveDate));
            managetable.CreateTableBody(DataArr, hl7factory, tble, tbody, tr);
            Id = Id + 1;
        }
        private void CreateComponent(SocialHistory ptSocialHistory, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptSocialHistory.root != null)
            {
                hl7III.Init(ptSocialHistory.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptSocialHistory.code != null)
            {
                functionalStatus.Section.Code.Code = ptSocialHistory.code;
            }

            if (ptSocialHistory.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptSocialHistory.codeSystem;
            }

            if (ptSocialHistory.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptSocialHistory.codeSystemName;
            }

            if (ptSocialHistory.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptSocialHistory.displayName;
            }

            if (ptSocialHistory.title != null)
            {
                functionalStatus.Section.Title.Text = ptSocialHistory.title;
            }
        }
        public void GenerateSocialHistoryEntry(string observerValue, ref int refId, string obsrvType, III hl7III, Factory hl7Factory)
        {
            if (obsrvType == "Smoking")
            {
                IEntry entry = functionalStatus.Section.Entry.Append();
                //GetSNOMED(obsrvType)
                entry.AsObservation.ClassCode = "OBS";
                entry.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
                IIVXB_TS TS = hl7Factory.CreateIVXB_TS();
                hl7III = entry.AsObservation.Id.Append();
                hl7III.Init(Guid.NewGuid().ToString());
                hl7III = entry.AsObservation.TemplateId.Append();
                hl7III.Init("2.16.840.1.113883.10.20.22.4.78");
                //entry.AsObservation.Code.Code = IIf(obsrvType = "Smoking", "230056004", IIf(obsrvType = "Alcohol", "160573003", IIf(obsrvType = "Drugs", "228423003", "229819007")))
                IED ED = hl7Factory.CreateED();
                entry.AsObservation.Code.Code = "ASSERTION";
                entry.AsObservation.Code.DisplayName = "Assertion";
                entry.AsObservation.Code.CodeSystem = "2.16.840.1.113883.5.4";
                entry.AsObservation.Code.CodeSystemName = "ActCode";
                //ED.Reference.Value = "#Sec" + refId
                //entry.AsObservation.Code.OriginalText = ED
                //entry.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96"
                //entry.AsObservation.Code.DisplayName = IIf(obsrvType = "Smoking", "Cigarette smoking", IIf(obsrvType = "Alcohol", "Alcohol consumption", IIf(obsrvType = "Drugs", "Drugs consumption", "Tobacoo consumption")))

                entry.AsObservation.StatusCode.Init("completed");
                entry.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS(), high: new IVXB_TS());
                //entry.AsObservation.EffectiveTime.High.Value = "UNK"
                ICD CD = hl7Factory.CreateCD();
                CD.CodeSystemName = "SNOMED CT";
                //= observerValue
                CD.CodeSystem = "2.16.840.1.113883.6.96";
                CD.Code = "77176002";
                CD.DisplayName = "Cigarette smoking";
                CD.OriginalText.Reference.Value = "#Sec" + refId;
                entry.AsObservation.Value.Add(CD);
                refId = refId + 1;
            }
            else
            {
                IEntry entry = functionalStatus.Section.Entry.Append();
                GetSNOMED(obsrvType);
                entry.TypeCode = x_ActRelationshipEntry.DRIV;
                entry.AsObservation.ClassCode = "OBS";
                entry.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
                IIVXB_TS TS = hl7Factory.CreateIVXB_TS();
                hl7III = entry.AsObservation.Id.Append();
                hl7III.Init(Guid.NewGuid().ToString());
                hl7III = entry.AsObservation.TemplateId.Append();
                hl7III.Init("2.16.840.1.113883.10.20.22.4.38");
                entry.AsObservation.Code.Code = (obsrvType == "Smoking" ? "230056004" : (obsrvType == "Alcohol" ? "160573003" : (obsrvType == "Drugs" ? "228423003" : "229819007")));
                IED ED = hl7Factory.CreateED();
                ED.Reference.Value = "#Sec" + refId;
                entry.AsObservation.Code.OriginalText = ED;
                entry.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.96";
                entry.AsObservation.Code.DisplayName = (obsrvType == "Smoking" ? "Cigarette smoking" : (obsrvType == "Alcohol" ? "Alcohol consumption" : (obsrvType == "Drugs" ? "Drugs consumption" : "Tobacoo consumption")));

                entry.AsObservation.StatusCode.Init("completed");
                entry.AsObservation.EffectiveTime.NullFlavor = "UNK";
                IST ST = hl7Factory.CreateST();
                ST.Text = observerValue;
                entry.AsObservation.Value.Add(ST);
                refId = refId + 1;

            }


        }
        public int calcYear(DateTime DateDiagnosed, DateTime dateOfBirth)
        {

            DateTime dayStart = Convert.ToDateTime(dateOfBirth);
            DateTime dateEnd = Convert.ToDateTime(DateDiagnosed);
            TimeSpan ts = dateEnd - dayStart;
            int Years = Convert.ToInt32(ts.TotalDays) / 365;
            return Years;


        }
        public void GenerateSocialHistoryEmpty(III hl7III, Factory hl7Factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.AsObservation.ClassCode = "OBS";
            entry.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            IIVXB_TS TS = hl7Factory.CreateIVXB_TS();
            hl7III = entry.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            hl7III = entry.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.78");
            IED ED = hl7Factory.CreateED();
            entry.AsObservation.Code.Code = "ASSERTION";
            entry.AsObservation.Code.DisplayName = "Assertion";
            entry.AsObservation.Code.CodeSystem = "2.16.840.1.113883.5.4";
            entry.AsObservation.Code.CodeSystemName = "ActCode";
            entry.AsObservation.StatusCode.Init("completed");
            entry.AsObservation.EffectiveTime = new IVL_TS().Init(low: new IVXB_TS(), high: new IVXB_TS());
            ICD CD = hl7Factory.CreateCD();
            CD.NullFlavor = "UNK";
            entry.AsObservation.Value.Add(CD);
        }
        public string GetSNOMED(string Description)
        {   
            return Description;
        }

    }
}
