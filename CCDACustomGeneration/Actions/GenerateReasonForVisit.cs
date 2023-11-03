using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateReasonForVisit
    {
        ReasonForVisitModel ptreason;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;

        public string FillReasonForVisit(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptreason = new ReasonForVisitModel();
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            hl7III.Root = ptreason.root;
            functionalStatus.Section.Code.CodeSystem = ptreason.codeSystem;
            functionalStatus.Section.Code.Code = ptreason.code;
            functionalStatus.Section.Code.CodeSystemName = ptreason.codeSystemName;
            functionalStatus.Section.Code.DisplayName = ptreason.displayName;
            functionalStatus.Section.Title.Text = ptreason.title;
            var paragraph = hl7factory.CreateStrucDocParagraph();
            if (patientinfo.ptReason!=null)
            {
                paragraph.Items.Add(Convert.ToString(patientinfo.ptReason.Description));
            }
            else
            {
                paragraph.Items.Add("N/A");
            }
            
            functionalStatus.Section.Text.Items.Add(paragraph);
            return clinicalDoc.Xml;
        }

    }
}
