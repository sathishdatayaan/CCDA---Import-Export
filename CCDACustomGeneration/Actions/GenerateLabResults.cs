using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateLabResults
    {
        LabResultModel ptLabResultModel;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocParagraph paragraph;
        private IStrucDocContent content;
        private IStrucDocItem listItem;
        private IStrucDocList list;

        public string FillPatientLabResult(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptLabResultModel = new LabResultModel();
            CreateComponent(ptLabResultModel, clinicalDoc, hl7III);// Manage Problem Component
            FillLabResults(patientinfo, hl7factory, hl7III);
            return clinicalDoc.Xml;
        }
        private void FillLabResults(PatientClinicalInformation patientinfo, Factory hl7factory, III hl7III)
        {
            if (patientinfo.ptLabResults != null && patientinfo.ptLabResults.Count > 0)
            {

                var arrptLabResults = patientinfo.ptLabResults;

                if (arrptLabResults.Count > 0)
                {
                    list = hl7factory.CreateStrucDocList();
                    list.ListType = 0;
                    //Creating List Object.
                    int i = 0;
                    foreach (var patientLabResult in arrptLabResults)
                    {
                        string testresult = string.Empty;
                        if (!String.IsNullOrEmpty(patientLabResult.TestResultn))
                        {
                            testresult = patientLabResult.TestResultn;
                        }
                        ListWithItem("result" + (i + 1), patientLabResult.TestPerformed + " - " + testresult + " " + patientLabResult.Units, hl7III, hl7factory, list);
                        GenerateLabResultEntry(patientLabResult, (i + 1), hl7III, hl7factory);
                        i++;
                    }
                    functionalStatus.Section.Text.Items.Add(list);
                    // Adding List to Text Tag.
                }
                else
                {
                    paragraph = hl7factory.CreateStrucDocParagraph();
                    paragraph.Items.Add("N/A");
                    GenerateLabResultEntryEmpty(hl7III, hl7factory);
                    functionalStatus.Section.Text.Items.Add(paragraph);

                }
            }
			else
			{
				paragraph = hl7factory.CreateStrucDocParagraph();
				paragraph.Items.Add("N/A");
				GenerateLabResultEntryEmpty(hl7III, hl7factory);
				functionalStatus.Section.Text.Items.Add(paragraph);

			}

		}
        private void CreateComponent(LabResultModel ptLabResultModel, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptLabResultModel.root != null)
            {
                hl7III.Init(ptLabResultModel.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptLabResultModel.code != null)
            {
                functionalStatus.Section.Code.Code = ptLabResultModel.code;
            }

            if (ptLabResultModel.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptLabResultModel.codeSystem;
            }

            if (ptLabResultModel.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptLabResultModel.codeSystemName;
            }

            if (ptLabResultModel.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptLabResultModel.displayName;
            }

            if (ptLabResultModel.title != null)
            {
                functionalStatus.Section.Title.Text = ptLabResultModel.title;
            }
        }
        public void ListWithItem(string Id, string ContxtInfo, III hl7III, Factory hl7Factory, IStrucDocList list)
        {
            content = hl7Factory.CreateStrucDocContent();
            listItem = hl7Factory.CreateStrucDocItem();
            content.XmlId = Id;
            content.Items.Add(ContxtInfo);
            listItem.Items.Add(content);
            list.Item.Add(listItem);
        }
        public void GenerateLabResultEntry(LabResult patientLabResult, int Index, III hl7III, Factory hl7Factory)
        {
            var Entry = functionalStatus.Section.Entry.Append();
            Entry.AsOrganizer.MoodCode = "EVN";
            Entry.AsOrganizer.ClassCode = 0;
            hl7III = Entry.AsOrganizer.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.1");
            hl7III = Entry.AsOrganizer.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            Entry.AsOrganizer.Code.Code = patientLabResult.LonicCode;
            Entry.AsOrganizer.Code.DisplayName = patientLabResult.TestPerformed;
            Entry.AsOrganizer.Code.CodeSystemName = "LOINC";
            Entry.AsOrganizer.Code.CodeSystem = "2.16.840.1.113883.6.1";
            Entry.AsOrganizer.StatusCode.Code = "completed";
            var Comp = Entry.AsOrganizer.Component.Append();
            Comp.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            Comp.AsObservation.ClassCode = "OBS";
            hl7III = Comp.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.2");
            hl7III = Comp.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            Comp.AsObservation.Code.Code = patientLabResult.LonicCode;
            Comp.AsObservation.Code.DisplayName = patientLabResult.TestPerformed;
            Comp.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.1";
            Comp.AsObservation.Code.CodeSystemName = "LOINC";
            Comp.AsObservation.Text.Reference.Value = "#result" + Index;
            Comp.AsObservation.StatusCode.Code = "completed";
            string month = Convert.ToDateTime(patientLabResult.ReportDate).Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            Comp.AsObservation.EffectiveTime.Value = Convert.ToString(Convert.ToDateTime(patientLabResult.ReportDate).Year.ToString() + month + Convert.ToDateTime(patientLabResult.ReportDate).Day.ToString());
            var PQ = hl7Factory.CreatePQ();
            try
            {
                var value = Convert.ToDouble(patientLabResult.TestResultn);
                PQ.Value = Convert.ToDouble(patientLabResult.TestResultn);
                PQ.Unit = patientLabResult.Units;
            }
            catch (System.Exception)
            {
                PQ.NullFlavor = "UNK";
            }

            Comp.AsObservation.Value.Add(PQ);
            var ice = hl7Factory.CreateCE();
            ice.NullFlavor = "UNK";
            Comp.AsObservation.InterpretationCode.Add(ice);
            var refRange = hl7Factory.CreateReferenceRange();
            refRange.ObservationRange.Text.Text = patientLabResult.NormalFindings;
            Comp.AsObservation.ReferenceRange.Add(refRange);
        }
        public void GenerateLabResultEntryEmpty(III hl7III, Factory hl7Factory)
        {
            var Entry = functionalStatus.Section.Entry.Append();
            Entry.AsOrganizer.MoodCode = "EVN";
            Entry.AsOrganizer.ClassCode = 0;
            hl7III = Entry.AsOrganizer.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.1");
            hl7III = Entry.AsOrganizer.Id.Append();
            hl7III.NullFlavor = "NA";
            Entry.AsOrganizer.Code.NullFlavor = "NA";
            Entry.AsOrganizer.StatusCode.Code = "completed";
            Entry.AsOrganizer.EffectiveTime.Init(DateTime.Now);
            var Comp = Entry.AsOrganizer.Component.Append();
            Comp.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            Comp.AsObservation.ClassCode = "OBS";
            Comp.AsObservation.NegationInd = true;
            Comp.AsObservation.NegationIndSpecified = true;
            hl7III = Comp.AsObservation.Id.Append();
            hl7III.NullFlavor = "NA";
            hl7III = Comp.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.2");
            Comp.AsObservation.Code.Code = "26436-6";
            Comp.AsObservation.Code.DisplayName = "Laboratory Studies";
            Comp.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.1";
            Comp.AsObservation.Code.CodeSystemName = "LOINC";
            Comp.AsObservation.EffectiveTime.Init(DateTime.Now);
            Comp.AsObservation.StatusCode.Code = "completed";
            var PQ = hl7Factory.CreatePQ();
            PQ.Value =0;
            Comp.AsObservation.Value.Add(PQ);
        }
    }
}
