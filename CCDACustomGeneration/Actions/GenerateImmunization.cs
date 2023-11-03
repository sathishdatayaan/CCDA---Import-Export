using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{
    public class GenerateImmunization
    {

        ImmunizationModel ptImmunizationModel;
        GenerateTableBodyStructure managetable;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocContent content;
        private IStrucDocTable tble;
        private IStrucDocThead thead;
        private IStrucDocTbody tbody;
        private IStrucDocTr tr;
        ArrayList DataArr = new ArrayList();


        public string FillPatientImmunization(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptImmunizationModel = new ImmunizationModel();
            CreateComponent(ptImmunizationModel, clinicalDoc, hl7III);// Manage Allergy Component
            FillImmunizationContent(patientinfo, hl7factory);
            int i = 0;
            if (patientinfo.ptImmunization!=null && patientinfo.ptImmunization.Count > 0)
            {
                foreach (Immunization item in patientinfo.ptImmunization)
                {
                    GenerateImmunizationEntry(item, (i+1), hl7III, hl7factory);
                    i++;
                }
            }
            else
            {
                GenerateImmunizationEntryEmpty(hl7III, hl7factory);
            }
            return clinicalDoc.Xml;
        }
        private void FillImmunizationContent(PatientClinicalInformation patientinfo, Factory hl7factory)
        {
            if (patientinfo.ptImmunization != null && patientinfo.ptImmunization.Count > 0)
            {
                if (patientinfo.ptAllergies.Count > 0)
                {
                    managetable = new GenerateTableBodyStructure();

                    DataArr.Add("Vaccine");
                    DataArr.Add("Date");
                    DataArr.Add("Status");
                    tble = hl7factory.CreateStrucDocTable();
                    thead = tble.Thead;
                    tbody = tble.Tbody.Append();
                    tr = thead.Tr.Append();
                    managetable.CreateTableHeader(DataArr, hl7factory, tble, thead, tr);
                    //ArrayList alleries = new ArrayList(patientinfo.ptAllergies.ptAllergies);
                    int i = 0;
                    foreach (var item in patientinfo.ptImmunization)
                    {
                        DataArr = new ArrayList();
                        //DataArr.Add(Convert.ToString(item.substance));
                        content = hl7factory.CreateStrucDocContent();
                        content.XmlId = "immun" + (i + 1);
                        content.Items.Add(item.Vaccine != null ? item.Vaccine : "");
                        DataArr.Add(content);
                        if (!String.IsNullOrEmpty(item.ApproximateDate.ToString()))
                        {
                            DataArr.Add(string.Format("{0:MMM yyyy}", Convert.ToDateTime(item.ApproximateDate).ToString()));
                        }
                        else
                        {
                            DataArr.Add(string.Format("{0:MMM yyyy}", Convert.ToDateTime(DateTime.Now).ToString()));
                        }
                        DataArr.Add("Completed");
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
        private void CreateComponent(ImmunizationModel ptImmunization, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptImmunization.root != null)
            {
                hl7III.Init(ptImmunization.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptImmunization.code != null)
            {
                functionalStatus.Section.Code.Code = ptImmunization.code;
            }

            if (ptImmunization.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptImmunization.codeSystem;
            }

            if (ptImmunization.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptImmunization.codeSystemName;
            }

            if (ptImmunization.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptImmunization.displayName;
            }

            if (ptImmunization.title != null)
            {
                functionalStatus.Section.Title.Text = ptImmunization.title;
            }
        }
        public void GenerateImmunizationEntry(Immunization patientImmunization, int refValue, III hl7III, Factory hl7Factory)
        {
            IEntry entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsSubstanceAdministration.ClassCode = "SBADM";
            entry.AsSubstanceAdministration.MoodCode = x_DocumentSubstanceMood.EVN;
            entry.AsSubstanceAdministration.NegationInd = false;
            entry.AsSubstanceAdministration.NegationIndSpecified = true;
            hl7III = entry.AsSubstanceAdministration.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.52");
            hl7III = entry.AsSubstanceAdministration.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            entry.AsSubstanceAdministration.Text.Reference.Value = ("#immun" + refValue.ToString());
            entry.AsSubstanceAdministration.StatusCode.Code = "Completed";
            var TS = hl7Factory.CreateSXCM_TS();
            if (!String.IsNullOrEmpty(patientImmunization.ApproximateDate.ToString()))
            {
                TS.Init(Convert.ToDateTime(patientImmunization.ApproximateDate));
            }
            else
            {
                TS.Init(DateTime.Now);
            }

            entry.AsSubstanceAdministration.EffectiveTime.Add(TS);
            entry.AsSubstanceAdministration.DoseQuantity.NullFlavor = "UNK";
            var Consumable = entry.AsSubstanceAdministration.Consumable;
            hl7III = Consumable.ManufacturedProduct.TemplateId.Append();
            Consumable.ManufacturedProduct.ClassCode = RoleClassManufacturedProduct.MANU;
            hl7III.Init("2.16.840.1.113883.10.20.22.4.54");
            Consumable.ManufacturedProduct.AsMaterial.Code.Code = ((patientImmunization.CVX.ToString().Length == 1) ? ("0" + patientImmunization.CVX.ToString()) : patientImmunization.CVX.ToString());
            Consumable.ManufacturedProduct.AsMaterial.Code.CodeSystem = "2.16.840.1.113883.12.292";
            Consumable.ManufacturedProduct.AsMaterial.Code.CodeSystemName = "CVX";
            Consumable.ManufacturedProduct.AsMaterial.Code.DisplayName = Convert.ToString(patientImmunization.Vaccine);
            Consumable.ManufacturedProduct.AsMaterial.Code.OriginalText.Text = Convert.ToString(patientImmunization.Vaccine);
            var ION = Consumable.ManufacturedProduct.ManufacturerOrganization.Name.Append();
            ION.Text = Convert.ToString(patientImmunization.Manufacturer);
        }
        public void GenerateImmunizationEntryEmpty(III hl7III, Factory hl7Factory)
        {
            var entry = functionalStatus.Section.Entry.Append();
            entry.TypeCode = x_ActRelationshipEntry.DRIV;
            entry.AsSubstanceAdministration.ClassCode = "SBADM";
            entry.AsSubstanceAdministration.MoodCode = x_DocumentSubstanceMood.EVN;
            entry.AsSubstanceAdministration.NegationInd = false;
            entry.AsSubstanceAdministration.NegationIndSpecified = true;
            hl7III = entry.AsSubstanceAdministration.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.52");
            hl7III = entry.AsSubstanceAdministration.Id.Append();
            hl7III.NullFlavor = "NA";
            entry.AsSubstanceAdministration.StatusCode.Code = "Completed";
            var TS = hl7Factory.CreateSXCM_TS();
            TS.Init(DateTime.Now);
            entry.AsSubstanceAdministration.EffectiveTime.Add(TS);
            var Consumable = entry.AsSubstanceAdministration.Consumable;
            Consumable.ManufacturedProduct.ClassCode = RoleClassManufacturedProduct.MANU;
            hl7III = Consumable.ManufacturedProduct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.54");
            Consumable.ManufacturedProduct.AsMaterial.Code.NullFlavor = "NA";
        }
    }
}
