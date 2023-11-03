using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Actions
{
    public class GenerateComponent
    {
        GenerateAddressPhNo addressphno;
        NameModel nameinfo;

        public string FillComponentInfo(ClinicalDocument clinicalDoc, Factory hl7factory, III hl7III, PatientClinicalInformation patientinfo)
        {
            string documentationOfdetais = string.Empty;

            hl7III = clinicalDoc.ComponentOf.EncompassingEncounter.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6");
            clinicalDoc.ComponentOf.EncompassingEncounter.Code.Code = patientinfo.EncounterCode;
            clinicalDoc.ComponentOf.EncompassingEncounter.Code.CodeSystem = "2.16.840.1.113883.6.96";
            clinicalDoc.ComponentOf.EncompassingEncounter.Code.DisplayName = patientinfo.EncounterDescription;
            IVXB_TS low = new IVXB_TS();
            if (patientinfo.EncounterNoteDate !=null && patientinfo.EncounterNoteDate !="")
            {
                low.Init(Convert.ToDateTime(patientinfo.EncounterNoteDate));
                clinicalDoc.ComponentOf.EncompassingEncounter.EffectiveTime = new IVL_TS().Init(low: low);
            }else
            {
                var times = hl7factory.CreateIVXB_TS();
                clinicalDoc.ComponentOf.EncompassingEncounter.EffectiveTime.Init(null, times, times);
            }

            hl7III = clinicalDoc.ComponentOf.EncompassingEncounter.ResponsibleParty.AssignedEntity.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6");

            var EnName = clinicalDoc.ComponentOf.EncompassingEncounter.ResponsibleParty.AssignedEntity.AssignedPerson.Name.Append();
            addressphno = new GenerateAddressPhNo();
            nameinfo = new NameModel();
            nameinfo.Createenfamily = patientinfo.EncounterStaffName;
            addressphno.FillName(nameinfo, EnName, hl7factory);

            var EncounterParticipant = clinicalDoc.ComponentOf.EncompassingEncounter.EncounterParticipant.Append();
            hl7III = EncounterParticipant.AssignedEntity.Id.Append();
            hl7III.Init("2.16.840.1.113883.4.6");
            var ParticipantName = EncounterParticipant.AssignedEntity.AssignedPerson.Name.Append();
            addressphno.FillName(nameinfo, ParticipantName, hl7factory);

            clinicalDoc.ComponentOf.EncompassingEncounter.DischargeDispositionCode.CodeSystem = "2.16.840.1.113883.12.112";
            clinicalDoc.ComponentOf.EncompassingEncounter.DischargeDispositionCode.Code = "01";
            clinicalDoc.ComponentOf.EncompassingEncounter.DischargeDispositionCode.DisplayName = "Routine Discharge";
            clinicalDoc.ComponentOf.EncompassingEncounter.DischargeDispositionCode.CodeSystemName = "HL7 Discharge Disposition";
            hl7III = clinicalDoc.ComponentOf.EncompassingEncounter.Location.HealthCareFacility.Id.Append();
            hl7III.Init("2.16.540.1.113883.19.2");

            documentationOfdetais = clinicalDoc.Xml;
            return documentationOfdetais;
        }
    }
}
