using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.Model
{
    public class PatientClinicalInformation
    {
        public PatientDemographicDetail ptDemographicDetail { get; set; }
        public ClinicInformation ptClinicInformation { get; set; }
        /// <summary>
        /// Encounter Information
        /// </summary>
        public string EncounterCode { get; set; }
        public string EncounterDxDate { get; set; }
        public string EncounterNoteDate { get; set; }
        public string EncounterStaffName { get; set; }
        public string EncounterDescription { get; set; }
        /// <summary>
        /// Encounter End
        /// </summary>
        public string reasonforTransfer { get; set; }
        public List<DocumentationOfList> documentationOfInfo { get; set; }
        public List<PatientAllergies> ptAllergies { get; set; }
        public List<PatientProblemes> ptProblemes { get; set; }
        public SocialHistoryModel ptSocialHistory { get; set; }
        public List<VitalSigns> ptVitalSigns { get; set; }
        public List<PatientMedication> ptMedication { get; set; }
        public List<FunctionalStatus> ptFunctionalStatus { get; set; }
        public List<Encounters> ptEncounters { get; set; }
        public List<LabResult> ptLabResults { get; set; }
        public ReasonForVisit ptReason { get; set; }
        public List<Immunization> ptImmunization { get; set; }
        public List<PlanOfCare> ptPlanOfCare { get; set; }
        public List<FutureAppointment> ptAppointment { get; set; }
        public List<ProcedureList> ptProcedure { get; set; }
    }
}
