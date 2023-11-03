using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{
    public class GenerateVitalSigns
    {
        VitalSignsCode ptvitalSigns;
        GenerateTableBodyStructure managetable;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocContent content;
        private IStrucDocTable tble;
        private IStrucDocThead thead;
        private IStrucDocTbody tbody;
        private IStrucDocTr tr;
        private IStrucDocTh th;
        private IStrucDocTd td;
        ArrayList DataArr = new ArrayList();

        public string FillVitalSigns(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptvitalSigns = new VitalSignsCode();
            CreateComponent(ptvitalSigns, clinicalDoc, hl7III);// Manage Allergy Component
            FillVitalSignsContent(patientinfo, hl7factory);
            int count = 1;

            if (patientinfo.ptVitalSigns != null && patientinfo.ptVitalSigns.Count > 0)
            {
                if (patientinfo.ptAllergies.Count > 0)
                {
                    foreach (VitalSigns item in patientinfo.ptVitalSigns)
                    {
                        GenerateVitalSignsEntry(item, hl7III, hl7factory, ref count);
                    }
                }
                else
                {
                    GenerateVitalSignsEntryEmpty(hl7III, hl7factory);
                }
            }
			else
			{
				GenerateVitalSignsEntryEmpty(hl7III, hl7factory);
			}
			return clinicalDoc.Xml;
        }
        private void FillVitalSignsContent(PatientClinicalInformation patientinfo, Factory hl7Factory)
        {
            if(patientinfo.ptVitalSigns != null && patientinfo.ptVitalSigns.Count > 0) {
                if (patientinfo.ptVitalSigns.Count > 0)
                {
                    managetable = new GenerateTableBodyStructure();

                    DataArr = new ArrayList();
                    DataArr.Add("Date / Time:");
                    var arrVitalSign = patientinfo.ptVitalSigns;
                    foreach (var vitalSign in arrVitalSign)
                    {
                        DataArr.Add(Convert.ToString(vitalSign.Entrydate));
                    }
                    tble = hl7Factory.CreateStrucDocTable();
                    thead = tble.Thead;
                    tbody = tble.Tbody.Append();
                    tr = thead.Tr.Append();
                    managetable.CreateTableHeader(DataArr, hl7Factory, tble, thead, tr);

                    IStrucDocTr tr2 = hl7Factory.CreateStrucDocTr();
                    IStrucDocTr tr3 = hl7Factory.CreateStrucDocTr();
                    tr = hl7Factory.CreateStrucDocTr();
                    th = hl7Factory.CreateStrucDocTh();

                    th.Items.Add("Height");
                    tr.Items.Add(th);

                    th = hl7Factory.CreateStrucDocTh();
                    th.Items.Add("Weight");
                    tr2.Items.Add(th);

                    th = hl7Factory.CreateStrucDocTh();
                    th.Items.Add("Blood Pressure");
                    tr3.Items.Add(th);


                    // Dim content
                    int i = 1;
                    foreach (var vitalSign in arrVitalSign)
                    {
                        td = hl7Factory.CreateStrucDocTd();
                        content = hl7Factory.CreateStrucDocContent();
                        content.XmlId = "vit" + (i);

                        content.Items.Add(Convert.ToString(vitalSign.Height) + " inch");
                        td.Items.Add(content);
                        tr.Items.Add(td);
                        i = i + 1;

                        td = hl7Factory.CreateStrucDocTd();
                        content = hl7Factory.CreateStrucDocContent();
                        content.XmlId = "vit" + (i);

                        content.Items.Add(Convert.ToString(vitalSign.WEIGHT) + " Kg");
                        td.Items.Add(content);
                        tr2.Items.Add(td);
                        i = i + 1;

                        td = hl7Factory.CreateStrucDocTd();
                        content = hl7Factory.CreateStrucDocContent();
                        content.XmlId = "vit" + (i);

                        content.Items.Add(Convert.ToString(vitalSign.BloodPressure) + " mmHg");
                        td.Items.Add(content);
                        tr3.Items.Add(td);
                        i = i + 1;
                    }

                    tbody.Tr.Add(tr);
                    tbody.Tr.Add(tr2);
                    tbody.Tr.Add(tr3);
                    if (arrVitalSign.Count > 0)
                    {
                        functionalStatus.Section.Text.Items.Add(tble);
                    }
                    else
                    {
                        IStrucDocParagraph paragraph = hl7Factory.CreateStrucDocParagraph();
                        paragraph.Items.Add("N/A");
                        functionalStatus.Section.Text.Items.Add(paragraph);
                    }
                }
				else
				{
					IStrucDocParagraph paragraph = hl7Factory.CreateStrucDocParagraph();
					paragraph.Items.Add("N/A");
					functionalStatus.Section.Text.Items.Add(paragraph);
				}
			}
        }
        private void CreateComponent(VitalSignsCode ptSignsCode, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptSignsCode.root != null)
            {
                hl7III.Init(ptSignsCode.root);
            }

            if (ptSignsCode.code != null)
            {
                functionalStatus.Section.Code.Code = ptSignsCode.code;
            }

            if (ptSignsCode.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptSignsCode.codeSystem;
            }

            if (ptSignsCode.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptSignsCode.codeSystemName;
            }

            if (ptSignsCode.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptSignsCode.displayName;
            }

            if (ptSignsCode.title != null)
            {
                functionalStatus.Section.Title.Text = ptSignsCode.title;
            }
        }
        public void GenerateVitalSignsEntry(VitalSigns vitalSign, III hl7III, Factory hl7Factory, ref int refId)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsOrganizer.ClassCode = x_ActClassDocumentEntryOrganizer.CLUSTER;
            entry.AsOrganizer.MoodCode = "EVN";
            hl7III = entry.AsOrganizer.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.26");
            hl7III = entry.AsOrganizer.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            entry.AsOrganizer.Code.Code = "46680005";
            entry.AsOrganizer.Code.CodeSystem = "2.16.840.1.113883.6.96";
            entry.AsOrganizer.Code.CodeSystemName = "SNOMED CT";
            entry.AsOrganizer.Code.DisplayName = "Vital signs";
            entry.AsOrganizer.StatusCode.Init("completed");

            IVXB_TS low = new IVXB_TS();
            low.Init(Convert.ToDateTime(vitalSign.Entrydate));
            entry.AsOrganizer.EffectiveTime = new IVL_TS().Init(low: low);
            //entry.AsOrganizer.EffectiveTime.Init(Convert.ToDateTime(vitalSign.Entrydate));

            ///'''' Height Component '''''''
            IComponent4 component = entry.AsOrganizer.Component.Append();
            // Height Component 
            component.AsObservation.ClassCode = "OBS";
            component.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = component.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.27");
            hl7III = component.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            component.AsObservation.Code.Code = "8302-2";
            component.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.1";
            component.AsObservation.Code.CodeSystemName = "LOINC";
            component.AsObservation.Code.DisplayName = "Height";
            component.AsObservation.Text.Reference.Value = "#vit" + Convert.ToString(refId);
            component.AsObservation.StatusCode.Init("completed");
            low.Init(Convert.ToDateTime(vitalSign.Entrydate));
            component.AsObservation.EffectiveTime = new IVL_TS().Init(low: low);
            //component.AsObservation.EffectiveTime.Init(Convert.ToDateTime(vitalSign.Entrydate));
            IPQ PQ = hl7Factory.CreatePQ();
            if (!string.IsNullOrEmpty(Convert.ToString(vitalSign.Height)))
            {
                PQ.Value = Convert.ToDouble(vitalSign.Height);
                PQ.Unit = "inch";
            }
            else
            {
                PQ.NullFlavor = "UNK";
            }
            component.AsObservation.Value.Add(PQ);
            ICE CE = hl7Factory.CreateCE();
            CE.Code = "N";
            CE.CodeSystem = "2.16.840.1.113883.5.83";
            component.AsObservation.InterpretationCode.Add(CE);

            ///'''' Weight Component '''''''''
            component = entry.AsOrganizer.Component.Append();
            // Weight Component 
            component.AsObservation.ClassCode = "OBS";
            component.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = component.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.27");
            hl7III = component.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            component.AsObservation.Code.Code = "3141-9";
            component.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.1";
            component.AsObservation.Code.CodeSystemName = "LOINC";
            component.AsObservation.Code.DisplayName = "Patient Body Weight - Measured";
            component.AsObservation.Text.Reference.Value = "#vit" + Convert.ToString(refId + 1);
            component.AsObservation.StatusCode.Init("completed");
            low.Init(Convert.ToDateTime(vitalSign.Entrydate));
            component.AsObservation.EffectiveTime = new IVL_TS().Init(low: low);
            //component.AsObservation.EffectiveTime.Init(Convert.ToDateTime(vitalSign.Entrydate));
            if (!string.IsNullOrEmpty(Convert.ToString(vitalSign.WEIGHT)))
            {
                PQ = hl7Factory.CreatePQ();
                PQ.Value = Convert.ToDouble(vitalSign.WEIGHT);
                PQ.Unit = "Kg";
            }
            else
            {
                PQ.NullFlavor = "UNK";
            }
            component.AsObservation.Value.Add(PQ);
            CE = hl7Factory.CreateCE();
            CE.Code = "N";
            CE.CodeSystem = "2.16.840.1.113883.5.83";
            component.AsObservation.InterpretationCode.Add(CE);

            ///'''' BloodPressure Component '''''''''
            component = entry.AsOrganizer.Component.Append();
            // Blood Pressure 
            component.AsObservation.ClassCode = "OBS";
            component.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = component.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.27");
            hl7III = component.AsObservation.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            component.AsObservation.Code.Code = "3141-9";
            component.AsObservation.Code.CodeSystem = "2.16.840.1.113883.6.1";
            component.AsObservation.Code.CodeSystemName = "LOINC";
            component.AsObservation.Code.DisplayName = "Intravascular Systolic";
            component.AsObservation.Text.Reference.Value = "#vit" + Convert.ToString(refId + 2);
            component.AsObservation.StatusCode.Init("completed");
            low.Init(Convert.ToDateTime(vitalSign.Entrydate));
            component.AsObservation.EffectiveTime = new IVL_TS().Init(low: low);
            // component.AsObservation.EffectiveTime.Init(Convert.ToDateTime(vitalSign.Entrydate));
            PQ = hl7Factory.CreatePQ();
            if (string.IsNullOrEmpty(Convert.ToString(vitalSign.BloodPressure)))
            {
                PQ.NullFlavor = "UNK";
            }
            else
            {
                PQ.Value = Convert.ToDouble(vitalSign.BloodPressure.Split('/')[0]);
                PQ.Unit = "mm[Hg]";
            }
            component.AsObservation.Value.Add(PQ);
            CE = hl7Factory.CreateCE();
            CE.Code = "N";
            CE.CodeSystem = "2.16.840.1.113883.5.83";
            component.AsObservation.InterpretationCode.Add(CE);
            refId = refId + 3;

        }
        public void GenerateVitalSignsEntryEmpty(III hl7III, Factory hl7Factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsOrganizer.ClassCode = x_ActClassDocumentEntryOrganizer.CLUSTER;
            entry.AsOrganizer.MoodCode = "EVN";
            hl7III = entry.AsOrganizer.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.26");
            hl7III = entry.AsOrganizer.Id.Append();
            hl7III.NullFlavor = "UNK";
            entry.AsOrganizer.Code.Code = "46680005";
            entry.AsOrganizer.Code.CodeSystem = "2.16.840.1.113883.6.96";
            entry.AsOrganizer.Code.CodeSystemName = "SNOMED CT";
            entry.AsOrganizer.Code.DisplayName = "Vital signs";
            entry.AsOrganizer.StatusCode.Init("completed");

            entry.AsOrganizer.EffectiveTime.Init(DateTime.Now);

            IComponent4 component = entry.AsOrganizer.Component.Append();
            // Height Component 
            component.AsObservation.ClassCode = "OBS";
            component.AsObservation.MoodCode = x_ActMoodDocumentObservation.EVN;
            hl7III = component.AsObservation.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.27");
            hl7III = component.AsObservation.Id.Append();
            hl7III.NullFlavor = "UNK";
            component.AsObservation.Code.NullFlavor = "UNK";
            component.AsObservation.Text.Reference.NullFlavor = "UNK";
            component.AsObservation.StatusCode.Init("completed");
            component.AsObservation.EffectiveTime.NullFlavor = "NA";
            IPQ PQ = hl7Factory.CreatePQ();
            PQ.NullFlavor = "UNK";
            component.AsObservation.Value.Add(PQ);
            ICE CE = hl7Factory.CreateCE();
            CE.NullFlavor = "UNK";
            component.AsObservation.InterpretationCode.Add(CE);
            IReferenceRange refRange = hl7Factory.CreateReferenceRange();
            refRange.ObservationRange.Text.Text = "";
            component.AsObservation.ReferenceRange.Add(refRange);
        }
    }
}
