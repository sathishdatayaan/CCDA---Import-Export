using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Actions
{
    public class GenerateMedication
    {
        MedicationInformation ptMedication;
        private IStructuredBody hl7Body;
        private IComponent3 functionalStatus;
        private IStrucDocParagraph paragraph;
        private IStrucDocContent content;
        private IStrucDocItem listItem;
        private IStrucDocList list;
        ArrayList DataArr = new ArrayList();
        public string FillPatientMedication(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string clinicdetais = string.Empty;
            ptMedication = new MedicationInformation();
            CreateComponent(ptMedication, clinicalDoc, hl7III);// Manage Problem Component
            FillMedicationContent(patientinfo, hl7factory, hl7III);
            return clinicalDoc.Xml;
        }
        private void FillMedicationContent(PatientClinicalInformation patientinfo, Factory hl7factory, III hl7III)
        {
            if (patientinfo.ptMedication != null && patientinfo.ptMedication.Count > 0)
            {
                var arrPatientProblems = patientinfo.ptMedication;
                if (arrPatientProblems.Count > 0)
                {
                    list = hl7factory.CreateStrucDocList();
                    //Creating List Object.
                    int i = 0;
                    foreach (var patientmedication in arrPatientProblems)
                    {
                        ListWithItem("medication" + Convert.ToString((i + 1)), Convert.ToString(patientmedication.Medication) + " - " + Convert.ToString(patientmedication.Dosage) + " " + Convert.ToString(patientmedication.Frequency), hl7III, hl7factory);
                        GenerateMedicationEntry(patientmedication, i, hl7III, hl7factory);
                        i++;
                    }
                    functionalStatus.Section.Text.Items.Add(list);
                    // Adding List to Text Tag.
                }
                else
                {
                    paragraph = hl7factory.CreateStrucDocParagraph();
                    paragraph.Items.Add("N/A");
                    functionalStatus.Section.Text.Items.Add(paragraph);
                    GenerateMedicationEntryEmpty(hl7III, hl7factory);
                }
            }
			else
			{
				paragraph = hl7factory.CreateStrucDocParagraph();
				paragraph.Items.Add("N/A");
				functionalStatus.Section.Text.Items.Add(paragraph);
				GenerateMedicationEntryEmpty(hl7III, hl7factory);
			}

		}
        private void CreateComponent(MedicationInformation ptMedication, ClinicalDocument clinicalDoc, III hl7III)
        {
            hl7Body = clinicalDoc.Component.AsStructuredBody;
            functionalStatus = hl7Body.Component.Append();
            hl7III = functionalStatus.Section.TemplateId.Append();
            if (ptMedication.root != null)
            {
                hl7III.Init(ptMedication.root);
            }

            //if (dictionary.ContainsKey(Root2))
            //{
            //    hl7III = functionalStatus.Section.TemplateId.Append;
            //    hl7III.Init(dictionary.Item(Root2));
            //}

            if (ptMedication.code != null)
            {
                functionalStatus.Section.Code.Code = ptMedication.code;
            }

            if (ptMedication.codeSystem != null)
            {
                functionalStatus.Section.Code.CodeSystem = ptMedication.codeSystem;
            }

            if (ptMedication.codeSystemName != null)
            {
                functionalStatus.Section.Code.CodeSystemName = ptMedication.codeSystemName;
            }

            if (ptMedication.displayName != null)
            {
                functionalStatus.Section.Code.DisplayName = ptMedication.displayName;
            }

            if (ptMedication.title != null)
            {
                functionalStatus.Section.Title.Text = ptMedication.title;
            }
        }
        public void ListWithItem(string Id, string ContxtInfo, III hl7III, Factory hl7Factory)
        {
            content = hl7Factory.CreateStrucDocContent();
            listItem = hl7Factory.CreateStrucDocItem();
            content.XmlId = Id;
            content.Items.Add(ContxtInfo);
            listItem.Items.Add(content);
            list.ListType = 0;
            list.Item.Add(listItem);
        }
        public void GenerateMedicationEntry(PatientMedication patientMedication, int Index, III hl7III, Factory hl7Factory)
        {
            IEntry Entry = functionalStatus.Section.Entry.Append();
            var  substance = hl7Factory.CreateSubstanceAdministration();
            substance.ClassCode = "SBADM";
            //substance.MoodCode = "1";
            hl7III = substance.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.16");
            hl7III = substance.Id.Append();
            hl7III.Init(Guid.NewGuid().ToString());
            substance.Text.Reference.Value = "#" + "medication" + Index;
            if (patientMedication.TakingCurrent)
            {
                substance.StatusCode.Code = "Active";
            }
            else
            {
                substance.StatusCode.Code = "Inactive";
            }

            var low = new IVXB_TS();
            var high = new IVXB_TS();
            if (!string.IsNullOrEmpty((patientMedication.StartDate).ToString()))
            {
                low.Init(Convert.ToDateTime(patientMedication.StartDate));
            }
            else
            {
                low.NullFlavor = "UNK";
            }

            if (!string.IsNullOrEmpty((patientMedication.StartDate).ToString()))
            {
                high.Init(Convert.ToDateTime(patientMedication.StartDate));
            }
            else
            {
                high.NullFlavor = "UNK";
            }

            substance.EffectiveTime.Add(new IVL_TS().Init(low: low, high: high));
            var et = new PIVL_TS();
            et.Operator = 0;
            et.InstitutionSpecified = true;
            et.Period.Value = 24;
            et.Period.Unit = "h";
            substance.EffectiveTime.Add(et);
            if (!string.IsNullOrEmpty(patientMedication.doseUnit) && !string.IsNullOrEmpty(patientMedication.Dose))
            {
                substance.DoseQuantity.Unit = patientMedication.doseUnit.Replace(" ", "").Replace(" ", "").Replace(" ", "");
                substance.DoseQuantity.Value = Convert.ToDouble(patientMedication.Dose.ToString().Replace(" ", "").Replace(" ", "").Replace(" ", ""));
            }
            else
            {
                substance.DoseQuantity.NullFlavor = "UNK";
            }

            substance.RateQuantity.NullFlavor = "UNK";
            var Consumable = hl7Factory.CreateConsumable();
            var manufacturedProduct = hl7Factory.CreateManufacturedProduct();
            manufacturedProduct.ClassCode = 0;
            hl7III = manufacturedProduct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.23");
            var material = hl7Factory.CreateMaterial();
            material.Code.Code = patientMedication.RxNorm;
            material.Code.CodeSystem = "2.16.840.1.113883.6.88";
            material.Code.CodeSystemName = "RxNorm";
            material.Code.DisplayName = patientMedication.Medication;
            material.Code.OriginalText.Reference.Value = "#" + "medication" + Index;
            manufacturedProduct.AsMaterial = material;
            Consumable.ManufacturedProduct = manufacturedProduct;
            substance.Consumable = Consumable;
            Entry.AsSubstanceAdministration = substance;


        }
        public void GenerateMedicationEntryEmpty( III hl7III, Factory hl7Factory)
        {
            var Entry = functionalStatus.Section.Entry.Append();
            var substance = hl7Factory.CreateSubstanceAdministration();
            substance.ClassCode = "SBADM";
           // substance.MoodCode = 1;
            substance.NullFlavor = "NI";
            hl7III = substance.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.16");
            hl7III = substance.Id.Append();
            hl7III.Init("2.16.840.1.113883.3.3208.9999");
            substance.StatusCode.Code = "completed";
            var low = new IVXB_TS();
            var high = new IVXB_TS();
            low.NullFlavor = "NA";
            high.NullFlavor = "NA";
            substance.EffectiveTime.Add(new IVL_TS().Init(low: low, high: high));
            var Consumable = hl7Factory.CreateConsumable();
            var manufacturedProduct = hl7Factory.CreateManufacturedProduct();
            manufacturedProduct.ClassCode = 0;
            manufacturedProduct.NullFlavor = "NI";
            hl7III = manufacturedProduct.TemplateId.Append();
            hl7III.Init("2.16.840.1.113883.10.20.22.4.23");
            var material = hl7Factory.CreateMaterial();
            material.Code.NullFlavor = "NI";
            manufacturedProduct.AsMaterial = material;
            Consumable.ManufacturedProduct = manufacturedProduct;
            substance.Consumable = Consumable;
            Entry.AsSubstanceAdministration = substance;
        }
       
    }
}
