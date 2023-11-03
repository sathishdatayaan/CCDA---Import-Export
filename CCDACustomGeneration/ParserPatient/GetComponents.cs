using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CreateClinicalReport.ParserLibrary
{
    public class GetComponents
    {

        IIVL_TS datetime;
        IPIVL_TS timeduration;
        ICD meterialCode;
        IPQ valueCode;        

        /// <summary>
        /// Get Patient Alleries
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<PatientAllergies> GetAllergies(Dictionary<string, ArrayList> dataArr)
        {

            List<PatientAllergies> alleryies = new List<PatientAllergies>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    PatientAllergies ptall = new PatientAllergies();
                    ArrayList itemalleryies = dataArr[i.ToString()];
                    ptall.substance = itemalleryies[1].ToString();
                    ptall.reaction = itemalleryies[5].ToString();
                    ptall.status = itemalleryies[3].ToString();
                    ptall.allergyDate = itemalleryies[0].ToString();

                    alleryies.Add(ptall);
                }
            }

            return alleryies;
        }
        public List<PatientAllergies> FillAllergies(IEntryCollection entryCollection)
        {
            List<PatientAllergies> alleryies = new List<PatientAllergies>();
            foreach (IEntry entryitem in entryCollection)
            {
                IAct entryact = entryitem.AsAct;
                IEntryRelationship entryRelationship = entryact.EntryRelationship[0];
                IObservation entryobservation = entryRelationship.AsObservation;
                IIVL_TS effectivetime = entryact.EffectiveTime;
                IParticipant2 allergyParticipant = entryobservation.Participant[0];
                IParticipantRole participantRole = allergyParticipant.ParticipantRole;
                IPlayingEntity playingEntity = participantRole.AsPlayingEntity;
                ICE code = playingEntity.Code;
                IPNCollection name = playingEntity.Name;
                string substance = name != null && name.Count() > 0 ? name[0].Text : code.DisplayName;
                PatientAllergies ptallergies = new PatientAllergies();
                ptallergies.substance = substance;
                IEntryRelationship entryRelationshipMFST = entryobservation.EntryRelationship.Where(r => r.TypeCode.ToString() == "MFST").FirstOrDefault();
                if (entryRelationshipMFST != null && entryRelationshipMFST.AsObservation.Value != null)
                {
                    IANY Reactionvaluecollection = entryRelationshipMFST.AsObservation.Value.FirstOrDefault();
                    CD valuesReaction = (CD)Reactionvaluecollection;
                    ptallergies.reaction = valuesReaction.DisplayName;
                }
                ptallergies.rxNorm = code.Code;
                IEntryRelationship entryRelationshipSUBJ = entryobservation.EntryRelationship.Where(r => r.TypeCode.ToString() == "SUBJ").FirstOrDefault();
                if (entryRelationshipSUBJ != null && entryRelationshipSUBJ.AsObservation.Value != null)
                {
                    IANY Statusvaluecollection = entryRelationshipSUBJ.AsObservation.Value.FirstOrDefault();
                    ICE values = (ICE)Statusvaluecollection;
                    ptallergies.status = values.DisplayName;
                }
                if (effectivetime != null && effectivetime.Value != null)
                {
                    ptallergies.allergyDate = effectivetime.AsDateTime.ToString();
                }
                alleryies.Add(ptallergies);

            }

            return alleryies;
        }
        /// <summary>
        /// Get Patient Problems
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<PatientProblemes> GetProblems(Dictionary<string, ArrayList> dataArr)
        {
            List<PatientProblemes> Problemes = new List<PatientProblemes>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    PatientProblemes ptproblem = new PatientProblemes();
                    ArrayList itemproblem = dataArr[i.ToString()];
                    ptproblem.ProblemCode = itemproblem[1].ToString();
                    ptproblem.Status = itemproblem[4].ToString();
                    ptproblem.DateDiagnosed = itemproblem[0].ToString();
                    ptproblem.Description = itemproblem[2].ToString();
                    Problemes.Add(ptproblem);
                }
            }
            return Problemes;
        }

        public List<PatientProblemes> FillProblems(IEntryCollection entryCollection)
        {
            List<PatientProblemes> Problemes = new List<PatientProblemes>();
            foreach (IEntry singleentry in entryCollection)
            {
                IObservation probObservation = singleentry.AsAct.EntryRelationship.FirstOrDefault().AsObservation;
                IIVL_TS effectivetime = probObservation.EffectiveTime;
                IANY probValue = probObservation.Value[0];
                ICD itemVlues = (ICD)probValue;
                PatientProblemes ptproblem = new PatientProblemes();
                ptproblem.ProblemCode = itemVlues.Code != null ? itemVlues.Code : null;
                IEntryRelationship ObserentryRelation = probObservation.EntryRelationship.Where(e => e.AsObservation.Code.Code.ToString() == "33999-4").FirstOrDefault();
                if (ObserentryRelation != null)
                {
                    IANY probStatusVal = ObserentryRelation.AsObservation.Value.FirstOrDefault();
                    ICD StatusVlues = (ICD)probStatusVal;
                    ptproblem.Status = StatusVlues.DisplayName;
                }
                else
                {
                    ptproblem.Status = null;
                }
                ptproblem.DateDiagnosed = effectivetime.Low != null ? effectivetime.Low.Value != null ? effectivetime.Low.AsDateTime.ToString() : null : null;
                ptproblem.Description = itemVlues.DisplayName != null ? itemVlues.DisplayName : null;
                Problemes.Add(ptproblem);
            }
            return Problemes;
        }
        /// <summary>
        /// Patient Social History Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public SocialHistoryModel GetSocialHistory(Dictionary<string, ArrayList> dataArr)
        {
            SocialHistoryModel ptSocialHistory = new SocialHistoryModel();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {

                    ArrayList itemSocialHistory = dataArr[i.ToString()];
                    ptSocialHistory.Smoker = itemSocialHistory[1].ToString();
                    ptSocialHistory.EntryDate = null;
                }
            }
            return ptSocialHistory;
        }
        public SocialHistoryModel FillSocialHistory(IEntryCollection entryCollection)
        {
            SocialHistoryModel ptSocialHistory = new SocialHistoryModel();
            foreach (IEntry singleentry in entryCollection)
            {
                IObservation socialObservation = singleentry.AsObservation;
                if (socialObservation.TemplateId.Select(s => s.Root.ToString()).FirstOrDefault() == "2.16.840.1.113883.10.20.22.4.38")
                {
                    ICD socialCode = socialObservation.Code;
                    IIVL_TS effectiveTime = socialObservation.EffectiveTime;
                    if (socialCode.Code == "230056004" || socialCode.Code == "229819007")
                    {

                        ptSocialHistory.Smoker = socialCode.Code != null ? socialCode.Code : null;
                    }
                    if (socialCode.Code == "160573003")
                    {
                        //ptSocialHistory.EntryDate = effectiveTime != null ? effectiveTime.Low != null ? effectiveTime.Low.Value != null ? effectiveTime.Low.Value.ToString() : null : null : null;
                        ptSocialHistory.Alcohol = socialCode.Code != null ? socialCode.Code : null;
                    }
                    if (socialCode.Code == "363908000")
                    {
                        //ptSocialHistory.EntryDate = effectiveTime != null ? effectiveTime.Low != null ? effectiveTime.Low.Value != null ? effectiveTime.Low.Value.ToString() : null : null : null;
                        ptSocialHistory.Drugs = socialCode.Code != null ? socialCode.Code : null;
                    }
                    if (socialCode.Code == "81703003")
                    {
                        //ptSocialHistory.EntryDate = effectiveTime != null ? effectiveTime.Low != null ? effectiveTime.Low.Value != null ? effectiveTime.Low.Value.ToString() : null : null : null;
                        ptSocialHistory.Tobacoo = socialCode.Code != null ? socialCode.Code : null;
                    }
                    ptSocialHistory.EntryDate = effectiveTime != null ? effectiveTime.Low != null ? effectiveTime.Low.Value != null ? effectiveTime.Low.Value.ToString() : null : null : null;

                }


            }
            return ptSocialHistory;
        }
        /// <summary>
        /// Patient ENCOUNTERS Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<VitalSigns> GetVitalSigns(Dictionary<string, ArrayList> dataArr)
        {
            List<VitalSigns> vitalSigns = new List<VitalSigns>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    VitalSigns ptvitalSigns = new VitalSigns();
                    ArrayList itemvitalSigns = dataArr[i.ToString()];
                    ptvitalSigns.Height = Convert.ToInt16(itemvitalSigns[1].ToString().Split(" ")[0]);
                    ptvitalSigns.WEIGHT = Convert.ToInt16(itemvitalSigns[5].ToString().Split(" ")[0]);
                    ptvitalSigns.BloodPressure = itemvitalSigns[9].ToString();
                    ptvitalSigns.Entrydate = itemvitalSigns[2].ToString() == "null" ? null : new DateTime?(Convert.ToDateTime(itemvitalSigns[2].ToString()));
                    vitalSigns.Add(ptvitalSigns);
                }
            }
            return vitalSigns;
        }
        public List<VitalSigns> FillVitalSigns(IEntryCollection entryCollection)
        {
            List<VitalSigns> vitalSigns = new List<VitalSigns>();
            foreach (IEntry singleentry in entryCollection)
            {
                IOrganizer organizer = singleentry.AsOrganizer;
                IComponent4Collection component = organizer.Component;
                IIVL_TS effectivetime = organizer.EffectiveTime;
                if (effectivetime == null) throw new InvalidOperationException();
                VitalSigns ptvitalSigns = new VitalSigns();
                try
                {
                    ptvitalSigns.VitalDate = effectivetime.AsDateTime;
                }
                catch (Exception)
                {

                    ptvitalSigns.VitalDate = effectivetime != null ? effectivetime.Low != null ? effectivetime.Low.Value != null ? new DateTime?(effectivetime.Low.AsDateTime) : null : null : effectivetime.Value != null ? new DateTime?(effectivetime.AsDateTime) : new DateTime?(effectivetime.AsDateTime);
                }


                foreach (IComponent4 orgComponent in component)
                {
                    IObservation orgObservation = orgComponent.AsObservation;
                    ICD itemCode = orgObservation.Code;
                    IANY vitalSignsObservationValue = orgObservation.Value[0];
                    IPQ itemVlues = (IPQ)vitalSignsObservationValue;

                    if (itemCode.Code != null)
                    {

                        if (itemCode.Code.ToString() == "8302-2")
                        {
                            ptvitalSigns.Height = Convert.ToInt16(itemVlues.Value);
                            ptvitalSigns.HeightUnit = Convert.ToString(itemVlues.Unit);

                        }
                        if (itemCode.Code.ToString() == "3141-9")
                        {
                            ptvitalSigns.WEIGHT = Convert.ToInt16(itemVlues.Value);
                            ptvitalSigns.WeightUnit = Convert.ToString(itemVlues.Unit);

                        }
                        if (itemCode.Code.ToString() == "8480-6")
                        {
                            ptvitalSigns.BloodPressure = itemVlues.Value.ToString() + " " + itemVlues.Unit.ToString();
                            ptvitalSigns.BloodPressureSystolic = itemVlues.Value.ToString();

                        }

                        if (itemCode.Code.ToString() == "8462-4")
                        {
                            ptvitalSigns.BloodPressureDiastolic = itemVlues.Value.ToString();

                        }
                    }

                }
                vitalSigns.Add(ptvitalSigns);
            }
            return vitalSigns;
        }
        /// <summary>
        /// Patient Medication Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<PatientMedication> GetMedication(Dictionary<string, ArrayList> dataArr)
        {
            List<PatientMedication> Medication = new List<PatientMedication>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    PatientMedication ptMedication = new PatientMedication();
                    ArrayList itemMedication = dataArr[i.ToString()];
                    ptMedication.Medication = itemMedication[4].ToString();
                    ptMedication.doseUnit = itemMedication[5].ToString();
                    ptMedication.RxNorm = itemMedication[0].ToString();
                    ptMedication.TakingCurrent = itemMedication[1].ToString() == "Active" ? true : false;
                    ptMedication.StartDate = itemMedication[2].ToString() == "null" ? null : new DateTime?(Convert.ToDateTime(itemMedication[2].ToString())); ;
                    ptMedication.EndDate = itemMedication[3].ToString() == "null" ? null : new DateTime?(Convert.ToDateTime(itemMedication[3].ToString()));
                    Medication.Add(ptMedication);
                }
            }
            return Medication;
        }

        public List<PatientMedication> FillMedication(IEntryCollection entryCollection)
        {
            List<PatientMedication> Medication = new List<PatientMedication>();
            foreach (IEntry entryitem in entryCollection)
            {
                ISubstanceAdministration entrySubstanceAdministration = entryitem.AsSubstanceAdministration;
                foreach (var timeitem in entrySubstanceAdministration.EffectiveTime)
                {
                    var obj = timeitem.GetType();
                    string objname = obj.Name;
                    switch (objname)
                    {
                        case "IVL_TS":
                            datetime = (IIVL_TS)timeitem;
                            break;
                        case "PIVL_TS":
                            timeduration = (IPIVL_TS)timeitem;
                            break;
                    }
                }
                IIVL_PQ doseQuantity = entrySubstanceAdministration.DoseQuantity;
                IIVL_PQ rateQuantity = entrySubstanceAdministration.RateQuantity;
                ICE meterialCode = entrySubstanceAdministration.Consumable.ManufacturedProduct.AsMaterial.Code;
                IEntryRelationship entryRelationShip = entrySubstanceAdministration.EntryRelationship.Where(e => e.TypeCode.ToString() == "RSON").FirstOrDefault();
                if (entryRelationShip != null)
                {
                    IANY entryvalue = entryRelationShip.AsObservation.Value.FirstOrDefault();
                    ICE valueCollection = (ICE)entryvalue;
                }
                PatientMedication ptMedication = new PatientMedication();
                ptMedication.Medication = meterialCode.Translation.FirstOrDefault() != null ? meterialCode.Translation.FirstOrDefault().DisplayName : meterialCode.DisplayName != null ? meterialCode.DisplayName : null;
                ptMedication.RxNorm = meterialCode.Code != null ? meterialCode.Code : null;
                ptMedication.Frequency = timeduration != null ? timeduration.Value != null ? timeduration.Value.ToString() : null : null;
                ptMedication.doseUnit = doseQuantity.Value.ToString() + " " + doseQuantity.Unit != null ? doseQuantity.Unit.ToString() : "";
                ptMedication.StartDate = datetime.Low != null ? datetime.Low.Value != null ? new DateTime?(datetime.Low.AsDateTime) : null : null;
                ptMedication.EndDate = datetime.High != null ? datetime.High.Value != null ? new DateTime?(datetime.High.AsDateTime) : null : null;
                Medication.Add(ptMedication);
            }

            return Medication;
        }
        /// <summary>
        /// Patient ENCOUNTERS Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<Encounters> GetEncounters(Dictionary<string, ArrayList> dataArr)
        {
            List<Encounters> encounters = new List<Encounters>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    Encounters ptEncounters = new Encounters();
                    ArrayList itemencounter = dataArr[i.ToString()];
                    ptEncounters.EncounterDescription = itemencounter[0].ToString();
                    ptEncounters.PerformerName = "";
                    ptEncounters.Location = itemencounter[1].ToString();
                    ptEncounters.EncounterDate = itemencounter[2] == null ? null : new DateTime?(Convert.ToDateTime(itemencounter[2].ToString()));
                    encounters.Add(ptEncounters);
                }
            }
            return encounters;
        }

        public List<Encounters> FillEncounters(IEntryCollection entryCollection, string name)
        {
            List<Encounters> encounters = new List<Encounters>();
            foreach (IEntry entryitem in entryCollection)
            {
                Encounters ptEncounters = new Encounters();
                IEncounter entryEncounter = entryitem.AsEncounter;
                IEntryRelationship entryRelationItem = entryEncounter.EntryRelationship.FirstOrDefault();
                if (entryRelationItem != null)
                {
                    IObservation observation = entryRelationItem.AsObservation;
                    IIVL_TS efftime = observation.EffectiveTime;
                    if (efftime == null) throw new InvalidOperationException();
                    IANY observationvalue = observation.Value.FirstOrDefault();
                    ICD str = (ICD)observationvalue;
                    string location = observation.Participant.Count > 0 ? observation.Participant[0].ParticipantRole.AsPlayingEntity.Name[0].Text : null;
                    ptEncounters.Code = str.Code;
                    ptEncounters.EncounterDescription = str.DisplayName;
                    ptEncounters.PerformerName = name;
                    //ptEncounters.EncounterDate = efftime != null ? efftime.Low != null ? efftime.Low.Value != null ? new DateTime?(Convert.ToDateTime(efftime.Low.AsDateTime)) : null : null : efftime.Value != null ? new DateTime?(Convert.ToDateTime(efftime.AsDateTime)) : null;
                }
                encounters.Add(ptEncounters);
            }

            return encounters;
        }
        /// <summary>
        /// Patient Lab Results Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<LabResult> GetLabResults(Dictionary<string, ArrayList> dataArr)
        {
            List<LabResult> labResult = new List<LabResult>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    LabResult ptLabResult = new LabResult();
                    ArrayList itemlabResult = dataArr[i.ToString()];
                    ptLabResult.LonicCode = itemlabResult[4] == null ? "" : itemlabResult[4].ToString();
                    ptLabResult.TestPerformed = itemlabResult[0].ToString();
                    ptLabResult.ReportDate = itemlabResult[2].ToString() == "null" ? null : new DateTime?(Convert.ToDateTime(itemlabResult[2].ToString()));
                    ptLabResult.TestResultn = itemlabResult[1].ToString().Split(" ")[0].ToString();
                    ptLabResult.Units = itemlabResult[1].ToString().Split(" ")[1].ToString();
                    ptLabResult.NormalFindings = itemlabResult[3].ToString();
                    labResult.Add(ptLabResult);
                }
            }
            return labResult;
        }
        public List<LabResult> FillLabResults(IEntryCollection entryCollection)
        {
            List<LabResult> labResult = new List<LabResult>();
            foreach (IEntry entryitem in entryCollection)
            {
                IOrganizer entryOrganizer = entryitem.AsOrganizer;
                IComponent4Collection entryComponent = entryOrganizer.Component;
                LabResult ptLabResult = new LabResult();
                foreach (IComponent4 obserComponent in entryComponent)
                {
                    IObservation entryObservation = obserComponent.AsObservation;
                    IReferenceRange referenceRange = entryObservation.ReferenceRange.FirstOrDefault();
                    meterialCode = entryObservation.Code;
                    try{valueCode = (IPQ)entryObservation.Value[0]; }catch(Exception){}
                    ptLabResult.TestPerformed = meterialCode.DisplayName;
                    ptLabResult.ReportDate = entryObservation.EffectiveTime == null ? null : new DateTime?(Convert.ToDateTime(entryObservation.EffectiveTime.AsDateTime));
                    ptLabResult.LonicCode = meterialCode.Code;
                    ptLabResult.Units = valueCode != null ? valueCode.Unit.ToString() : string.Empty;
                    ptLabResult.TestResultn = valueCode != null ? valueCode.Value.ToString():string.Empty;
                    ptLabResult.NormalFindings = referenceRange != null ? referenceRange.ObservationRange.Text != null ? referenceRange.ObservationRange.Text.Text : null : null;
                }
                labResult.Add(ptLabResult);
            }

            return labResult;
        }
        /// <summary>
        /// Patient Reason For Visit Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public ReasonForVisit GetReason(Dictionary<string, ArrayList> dataArr)
        {
            ReasonForVisit ptReasonForVisit = new ReasonForVisit();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {

                    ArrayList itemReasonForVisit = dataArr[i.ToString()];
                    ptReasonForVisit.Description = itemReasonForVisit[0].ToString();
                    ptReasonForVisit.VisitDate = null;//Convert.ToDateTime(item[2].ToString());
                }
            }
            return ptReasonForVisit;
        }
        public ReasonForVisit FillReason(ISection section)
        {
            ReasonForVisit ptReasonForVisit = new ReasonForVisit();
            IStrucDocText text = section.Text;
            foreach (var item in text.Items)
            {
                var test = item.GetType();
                ptReasonForVisit.Description = item.ToString();
            }

            return ptReasonForVisit;
        }
        /// <summary>
        /// Patient Immunizations Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<Immunization> GetImmunization(Dictionary<string, ArrayList> dataArr)
        {
            List<Immunization> immunization = new List<Immunization>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    Immunization ptImmunization = new Immunization();
                    ArrayList itemimmunization = dataArr[i.ToString()];
                    ptImmunization.Vaccine = itemimmunization[3].ToString();
                    ptImmunization.ApproximateDate = itemimmunization[2].ToString() == "null" ? null : new DateTime?(Convert.ToDateTime(itemimmunization[2].ToString()));
                    ptImmunization.CVX = Convert.ToInt32(itemimmunization[0].ToString());//Convert.ToDateTime(item[2].ToString());
                    ptImmunization.Manufacturer = itemimmunization[4].ToString();
                    immunization.Add(ptImmunization);
                }
            }
            return immunization;
        }
        public List<Immunization> FillImmunization(IEntryCollection entryCollection)
        {
            List<Immunization> immunization = new List<Immunization>();
            foreach (IEntry singleRecord in entryCollection)
            {
                ISubstanceAdministration entrySubstanceAdministration = singleRecord.AsSubstanceAdministration;
                ISXCM_TS efftime = entrySubstanceAdministration.EffectiveTime[0];
                IConsumable substanseConsumable = entrySubstanceAdministration.Consumable;
                ICE manufacturedProduct = entrySubstanceAdministration.Consumable.ManufacturedProduct.AsMaterial.Code;
                if (manufacturedProduct != null)
                {
                    Immunization ptImmunization = new Immunization();
                    ptImmunization.ApproximateDate = efftime != null ? efftime.Value != null ? new DateTime?(efftime.AsDateTime) : null : null;
                    ptImmunization.CVX = Convert.ToInt32(manufacturedProduct.Code);
                    ptImmunization.Vaccine = manufacturedProduct.OriginalText != null ? manufacturedProduct.OriginalText.Text : null;
                    if (entrySubstanceAdministration.Consumable.ManufacturedProduct.ManufacturerOrganization != null)
                    {
                        ptImmunization.Manufacturer = entrySubstanceAdministration.Consumable.ManufacturedProduct.ManufacturerOrganization.Name.ToString();
                    }
                    else
                    {
                        ptImmunization.Manufacturer = null;
                    }
                    immunization.Add(ptImmunization);
                }
            }

            return immunization;
        }
        /// <summary>
        /// Patient Plan Of Care Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<PlanOfCare> GetPlanOfCare(Dictionary<string, ArrayList> dataArr)
        {
            List<PlanOfCare> planOfCare = new List<PlanOfCare>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    PlanOfCare ptPlanOfCare = new PlanOfCare();
                    ArrayList itemplanOfCare = dataArr[i.ToString()];
                    string goal = string.Empty;
                    string instruction = string.Empty;
                    if (itemplanOfCare[1] != null)
                    {
                        var goalInsturction = itemplanOfCare[1].ToString().Split(",");

                        if (goalInsturction.Length >= 2)
                        {
                            var goalval = goalInsturction[0].Split(":");
                            if (goalval.Length >= 2)
                            {
                                goal = goalval[1].ToString();
                            }
                            var instructionval = goalInsturction[1].Split(":");
                            if (instructionval.Length >= 2)
                            {
                                instruction = instructionval[1].ToString();
                            }
                        }
                        ptPlanOfCare.Goal = goal;
                        ptPlanOfCare.PlannedDate = itemplanOfCare[0].ToString() == "null" ? null : new DateTime?(Convert.ToDateTime(itemplanOfCare[0].ToString()));
                        ptPlanOfCare.Instructions = instruction;//Convert.ToDateTime(item[2].ToString());
                        planOfCare.Add(ptPlanOfCare);
                    }
                }

            }
            return planOfCare;
        }
        public List<PlanOfCare> FillPlanOfCare(IEntryCollection entryCollection)
        {
            List<PlanOfCare> planOfCare = new List<PlanOfCare>();
            foreach (IEntry singleRecord in entryCollection)
            {
                IObservation observation = singleRecord.AsObservation;
                IAct entryAct = singleRecord.AsAct;
                string goal = null;
                string instructions = null;
                if (observation != null)
                {
                    meterialCode = observation.Code;
                    goal = meterialCode.DisplayName;
                    datetime = observation.EffectiveTime;
                }
                if (entryAct != null)
                {
                    instructions = entryAct.Text.Text;
                }
                PlanOfCare ptPlanOfCare = new PlanOfCare();
                ptPlanOfCare.Goal = goal;
                ptPlanOfCare.PlannedDate = datetime != null ? datetime.Center != null ? new DateTime?(datetime.Center.AsDateTime) : null : null;
                ptPlanOfCare.Instructions = instructions;
                planOfCare.Add(ptPlanOfCare);
            }


            return planOfCare;
        }
        /// <summary>
        /// Patient Reason For Transfer Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public string GetReasonForTransfer(Dictionary<string, ArrayList> dataArr)
        {
            string reasonforTransfer = string.Empty;
            for (int i = 0; i < dataArr.Count; i++)
            {
                ArrayList itemreason = dataArr[i.ToString()];
                reasonforTransfer = itemreason[0].ToString();
            }
            return reasonforTransfer;
        }
        public string FillReasonForTransfer(ISection section)
        {
            string reasonforTransfer = string.Empty;
            IStrucDocText text = section.Text;
            foreach (var item in text.Items)
            {
                var test = item.GetType();
                reasonforTransfer = item.ToString();
            }

            return reasonforTransfer;
        }
        /// <summary>
        /// Patient Procedure Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<ProcedureList> GetProcedure(Dictionary<string, ArrayList> dataArr)
        {
            List<ProcedureList> procedureList = new List<ProcedureList>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    ProcedureList ptprocedureList = new ProcedureList();
                    ArrayList itemproc = dataArr[i.ToString()];
                    if (itemproc[0] != null && itemproc[0].ToString() != "null" && itemproc[0].ToString() != string.Empty)
                    {
                        ptprocedureList.CPTCodes = itemproc[0] == null ? "" : itemproc[0].ToString();
                        ptprocedureList.Description = itemproc[1].ToString() == null ? null : itemproc[1].ToString();
                        procedureList.Add(ptprocedureList);
                    }
                }
            }
            return procedureList;
        }
        public List<ProcedureList> FillProcedure(IEntryCollection entryCollection)
        {
            List<ProcedureList> procedureList = new List<ProcedureList>();
            foreach (IEntry entryitem in entryCollection)
            {
                IProcedure entryProcedure = entryitem.AsProcedure;
                if (entryProcedure != null)
                {
                    IObservation entryObservatio = entryitem.AsObservation;
                    ICD meterialCode;
                    //if (entryProcedure != null)
                    //{
                    //    meterialCode = entryProcedure.Code;
                    //}
                    //else
                    //{
                    //    meterialCode = entryObservatio.Code;
                    //}
                    meterialCode = entryProcedure.Code;
                    ProcedureList ptprocedureList = new ProcedureList();
                    ptprocedureList.CPTCodes = meterialCode.Code;
                    ptprocedureList.Description = meterialCode.DisplayName;
                    procedureList.Add(ptprocedureList);
                }
            }

            return procedureList;
        }
        /// <summary>
        /// Patient Functional Status Information
        /// </summary>
        /// <param name="dataArr"></param>
        /// <returns></returns>
        public List<FunctionalStatus> GetFunctionalStatus(Dictionary<string, ArrayList> dataArr)
        {
            List<FunctionalStatus> functionalStatus = new List<FunctionalStatus>();
            if (dataArr.Count > 0)
            {
                for (int i = 0; i < dataArr.Count; i++)
                {
                    FunctionalStatus ptfunctionalStatus = new FunctionalStatus();
                    ArrayList itemfunctional = dataArr[i.ToString()];
                    ptfunctionalStatus.StatusDate = itemfunctional[2].ToString() == "null" ? null : new DateTime?(Convert.ToDateTime(itemfunctional[2].ToString())); ;
                    ptfunctionalStatus.Description = itemfunctional[1].ToString();
                    functionalStatus.Add(ptfunctionalStatus);
                }
            }
            return functionalStatus;
        }

        public List<FunctionalStatus> FillFunctionalStatus(IEntryCollection entryCollection)
        {
            List<FunctionalStatus> functionalStatus = new List<FunctionalStatus>();
            foreach (IEntry entryitem in entryCollection)
            {
                IObservation entryObservation = entryitem.AsObservation;
                IIVL_TS effectiveDate = entryObservation.EffectiveTime;
                IANY functionalValue = entryObservation.Value[0];
                ICD cD = (ICD)functionalValue;
                FunctionalStatus ptfunctionalStatus = new FunctionalStatus();
                ptfunctionalStatus.StatusDate = effectiveDate != null ? effectiveDate.Low != null ? effectiveDate.Low.Value != null ? new DateTime?(effectiveDate.Low.AsDateTime) : null : null : null;
                ptfunctionalStatus.Description = cD.DisplayName;
                ptfunctionalStatus.Code = cD.Code;
                functionalStatus.Add(ptfunctionalStatus);
            }

            return functionalStatus;
        }

    }
}
