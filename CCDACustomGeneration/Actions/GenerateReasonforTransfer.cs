using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateReasonforTransfer
    {
        ReasonForReferralModel ptReasonForReferralModel;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocParagraph paragraph;

        public string FillReasonForReferral(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            ptReasonForReferralModel = new ReasonForReferralModel();
            CreateComponent(ptReasonForReferralModel,clinicalDoc,hl7III);
            paragraph = hl7factory.CreateStrucDocParagraph();
            if (patientinfo.ptReason != null)
            {
                if (patientinfo.reasonforTransfer != null && patientinfo.reasonforTransfer != "")
                {
                    paragraph.Items.Add(patientinfo.reasonforTransfer);
                }
                else
                {
                    paragraph.Items.Add("N/A");
                }
            }
			else
			{
				paragraph.Items.Add("N/A");
			}

			functionalStatus.Section.Text.Items.Add(paragraph);

            return clinicalDoc.Xml;
        }

        private void CreateComponent(ReasonForReferralModel ptReasonForReferral, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptReasonForReferral.root != null)
            {
                hl7III.Init(ptReasonForReferral.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptReasonForReferral.code != null)
            {
                functionalStatus.Section.Code.Code = ptReasonForReferral.code;
            }

            if (ptReasonForReferral.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptReasonForReferral.codeSystem;
            }

            if (ptReasonForReferral.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptReasonForReferral.codeSystemName;
            }

            if (ptReasonForReferral.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptReasonForReferral.displayName;
            }

            if (ptReasonForReferral.title != null)
            {
                functionalStatus.Section.Title.Text = ptReasonForReferral.title;
            }
        }
    }
}
