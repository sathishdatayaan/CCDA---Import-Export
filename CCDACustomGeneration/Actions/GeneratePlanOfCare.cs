using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{
    public class GeneratePlanOfCare
    {

        PlanOfCareModel ptPlanOfCareModel;
        GenerateTableBodyStructure managetable;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocParagraph paragraph;
        private IStrucDocTable tble;
        private IStrucDocThead thead;
        private IStrucDocTbody tbody;
        private IStrucDocTr tr;
        ArrayList DataArr = new ArrayList();
        public string FillPlanOfCare(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptPlanOfCareModel = new PlanOfCareModel();
            CreateComponent(ptPlanOfCareModel, clinicalDoc, hl7III);// Manage Allergy Component
            FillImmunizationContent(patientinfo, hl7factory, hl7III);
            if (patientinfo.ptAppointment != null)
            {
                FillAppointmentContent(patientinfo, hl7factory, hl7III);
            }
            return clinicalDoc.Xml;
        }
        private void FillImmunizationContent(PatientClinicalInformation patientinfo, Factory hl7factory, III hl7III)
        {
            if (patientinfo.ptPlanOfCare != null && patientinfo.ptPlanOfCare.Count > 0)
            {
                if (patientinfo.ptPlanOfCare.Count > 0)
                {
                    managetable = new GenerateTableBodyStructure();

                    DataArr.Add("Planned Activity");
                    DataArr.Add("Planned Date");
                    tble = hl7factory.CreateStrucDocTable();
                    thead = tble.Thead;
                    tbody = tble.Tbody.Append();
                    tr = thead.Tr.Append();
                    managetable.CreateTableHeader(DataArr, hl7factory, tble, thead, tr);
                    //ArrayList alleries = new ArrayList(patientinfo.ptAllergies.ptAllergies);
                    string isExistTbody = "false";
                    int i = 0;

                    foreach (var item in patientinfo.ptPlanOfCare)
                    {
                        DataArr = new ArrayList();
                        DataArr.Add("Goal: " + item.Goal + ", Instructions: " + item.Instructions);
                        DataArr.Add(Convert.ToString(item.PlannedDate));
                        //CreateTableTd(DataArr);
                        isExistTbody = "true";
                        managetable.CreateTableBody(DataArr, hl7factory, tble, tbody, tr);
                        var entry = functionalStatus.Section.Entry.Append();
                        entry.TypeCode = x_ActRelationshipEntry.DRIV;
                        entry.AsAct.ClassCode = 0;
                        entry.AsAct.MoodCode = 0;
                        hl7III = entry.AsAct.TemplateId.Append();
                        hl7III.Init("2.16.840.1.113883.10.20.22.4.20");
                        hl7III = entry.AsAct.Id.Append();
                        hl7III.Init(Guid.NewGuid().ToString());
                        entry.AsAct.Code.Code = "409073007";
                        entry.AsAct.Code.DisplayName = "instruction";
                        entry.AsAct.Code.CodeSystem = "2.16.840.1.113883.6.96";

                        entry.AsAct.Text.Text = "Goal: " + item.Goal + ", Instructions: " + item.Instructions;
                        entry.AsAct.StatusCode.Code = "completed";
                        i++;

                    }
                    functionalStatus.Section.Text.Items.Add(tble);
                    //managetable.CreateTableBody(alleries, hl7factory);
                }
                else
                {
                    managetable.CreateTableBody1("2", hl7factory, tble, tbody, tr); //Empty Body Entry
                }
            }
			
		}
        private void FillAppointmentContent(PatientClinicalInformation patientinfo, Factory hl7factory, III hl7III)
        {
            if (patientinfo.ptAppointment.Count > 0)
            {
                managetable = new GenerateTableBodyStructure();
                paragraph = hl7factory.CreateStrucDocParagraph();
                paragraph.Items.Add("Future Appointment");
                functionalStatus.Section.Text.Items.Add(paragraph);
                DataArr = new ArrayList();
                DataArr.Add("Date");
                DataArr.Add("Provider Name");
                tble = hl7factory.CreateStrucDocTable();
                thead = tble.Thead;
                tbody = tble.Tbody.Append();
                tr = thead.Tr.Append();
                managetable.CreateTableHeader(DataArr, hl7factory, tble, thead, tr);
                //ArrayList alleries = new ArrayList(patientinfo.ptAllergies.ptAllergies);
                string isExistTbody = "false";
                int i = 0;

                foreach (var item in patientinfo.ptAppointment)
                {
                    DataArr = new ArrayList();
                    DataArr.Add(Convert.ToString(item.AppointmentDate));
                    DataArr.Add(item.DoctorName);
                    managetable.CreateTableBody(DataArr, hl7factory, tble, tbody,tr);
                    isExistTbody = "true";
                    i++;

                }
                if(isExistTbody == "false"){
                    managetable.CreateTableBody1("2", hl7factory, tble, tbody, tr); //Empty Body Entry
                }
               
                functionalStatus.Section.Text.Items.Add(tble);
                //managetable.CreateTableBody(alleries, hl7factory);
            }
            else
            {
                paragraph = hl7factory.CreateStrucDocParagraph();
                paragraph.Items.Add("N/A");
                functionalStatus.Section.Text.Items.Add(paragraph);
            }
        }
        private void CreateComponent(PlanOfCareModel ptPlanOfCareModel, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptPlanOfCareModel.root != null)
            {
                hl7III.Init(ptPlanOfCareModel.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptPlanOfCareModel.code != null)
            {
                functionalStatus.Section.Code.Code = ptPlanOfCareModel.code;
            }

            if (ptPlanOfCareModel.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptPlanOfCareModel.codeSystem;
            }

            if (ptPlanOfCareModel.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptPlanOfCareModel.codeSystemName;
            }

            if (ptPlanOfCareModel.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptPlanOfCareModel.displayName;
            }

            if (ptPlanOfCareModel.title != null)
            {
                functionalStatus.Section.Title.Text = ptPlanOfCareModel.title;
            }
        }
    }
}
