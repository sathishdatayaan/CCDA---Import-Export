using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CreateClinicalReport.ParserLibrary
{
    public class ComponentDataCollection
    {
        IEntryRelationshipCollection entryRelationship;
        IAct itemAct;
        IProcedure procedure;
        IEncounter itemEncounter;
        IConsumable consumable;
        IManufacturedProduct manufacturedProduct;
        IEntryRelationshipCollection entity;
        IMaterial material;
        IOrganizer organizer;
        IObservation observation;
        ISubstanceAdministration itemSubstanceAdministration;

        public Dictionary<string, ArrayList> GetDataCollection(ISection sections, PatientClinicalInformation ptInformation)
        {
            Dictionary<string, ArrayList> componententries = new Dictionary<string, ArrayList>();
            IEntryCollection entries = sections.Entry;
            IStrucDocText sectiontext = sections.Text;
            IStrucDocElementCollection textitem = sectiontext.Items;
            if (entries.Count() > 0)
            {
                int count = 0;
                foreach (IEntry singlerecord in entries)
                {
                    itemAct = singlerecord.AsAct;
                    itemEncounter = singlerecord.AsEncounter;
                    itemSubstanceAdministration = singlerecord.AsSubstanceAdministration;
                    observation = singlerecord.AsObservation;
                    organizer = singlerecord.AsOrganizer;
                    procedure = singlerecord.AsProcedure;
                    //if(sections.Code.Code== "18776-5")
                    // {

                    // }
                    ArrayList arrayList = new ArrayList();
                    if (itemAct != null)
                    {

                        entryRelationship = itemAct.EntryRelationship;
                        IIVL_TS efftime = itemAct.EffectiveTime;
                        if (efftime != null && efftime.Low != null)
                        {
                            if (efftime.Low.Value != null)
                            {
                                arrayList.Add(efftime.Low.AsDateTime.ToString());
                            }
                            else
                            {
                                arrayList.Add("null");
                            }
                        }
                        else
                        {
                            arrayList.Add("null");
                        }
                        if (entryRelationship != null && entryRelationship.Count > 0)
                        {
                            observation = entryRelationship.Select(o => o.AsObservation).FirstOrDefault();
                            if (observation != null)
                            {
                                if (observation.Participant.Count() > 0)
                                {
                                    string participent = observation.Participant.Select(p => p.ParticipantRole).FirstOrDefault().AsPlayingEntity.Name.FirstOrDefault().Text;
                                    arrayList.Add(participent);
                                }

                                entity = observation.EntryRelationship;
                                foreach (IEntryRelationship singlentity in entity)
                                {
                                    IObservation entityobservation = singlentity.AsObservation;
                                    IANY observationvalue = entityobservation.Value.FirstOrDefault();
                                    if (observationvalue != null)
                                    {
                                        var obj = observationvalue.GetType();
                                        string objname = obj.Name;
                                        switch (objname)
                                        {
                                            default:
                                                ICD strcd = (ICD)observationvalue;
                                                arrayList.Add(strcd.Code);
                                                arrayList.Add(strcd.DisplayName);
                                                break;
                                            case "PQ":
                                                IPQ strpq = (IPQ)observationvalue;
                                                arrayList.Add(strpq.Value.ToString() + " " + strpq.Unit.ToString());
                                                break;
                                        }

                                    }
                                }

                                componententries.Add(count.ToString(), arrayList);
                                count++;
                            }
                        }
                        else
                        {
                            arrayList.Add(itemAct.Text.Text);
                            componententries.Add(count.ToString(), arrayList);
                            count++;
                        }
                    }
                    else if (itemEncounter != null)
                    {
                        entryRelationship = itemEncounter.EntryRelationship;
                        if ((entryRelationship.Select(t => t.TypeCode).FirstOrDefault().ToString()) == "RSON")
                        {
                            observation = entryRelationship.Select(o => o.AsObservation).FirstOrDefault();
                            IIVL_TS efftime = observation.EffectiveTime;
                            IANY observationvalue = observation.Value.FirstOrDefault();
                            ICD str = (ICD)observationvalue;
                            arrayList.Add(str.DisplayName);
                            arrayList.Add(ptInformation.ptClinicInformation.ClinicName);
                            if (efftime.Low != null)
                            {
                                arrayList.Add(efftime.Low.AsDateTime);
                            }else
                            {
                                arrayList.Add(null);
                            }
                            arrayList.Add(str.Code);
                        }

                        componententries.Add(count.ToString(), arrayList);
                        count++;
                    }
                    else if (itemSubstanceAdministration != null)
                    {
                        consumable = itemSubstanceAdministration.Consumable;
                        manufacturedProduct = consumable.ManufacturedProduct;
                        material = manufacturedProduct.AsMaterial;
                        arrayList.Add(material.Code.Code);
                        arrayList.Add(itemSubstanceAdministration.StatusCode.Code.ToString());
                        ISXCM_TSCollection efftime = itemSubstanceAdministration.EffectiveTime;
                        if (efftime.Count > 1)
                        {
                            foreach (IVL_TS daterange in efftime)
                            {
                                string startdatetime = daterange.Low != null ? daterange.Low.Value != null ? daterange.Low.AsDateTime.ToString() : "null" : "null";
                                string EndDAtetime = daterange.High != null ? daterange.High.Value != null ? daterange.High.AsDateTime.ToString() : "null" : "null";
                                arrayList.Add(startdatetime);
                                arrayList.Add(EndDAtetime);
                                break;
                            }
                        }
                        else
                        {
                            arrayList.Add(efftime[0].AsDateTime.ToString());
                        }

                        arrayList.Add(material.Code.DisplayName);
                        if (itemSubstanceAdministration.DoseQuantity != null)
                        {
                            arrayList.Add(itemSubstanceAdministration.DoseQuantity.Value.ToString() + " " + itemSubstanceAdministration.DoseQuantity.Unit.ToString());
                        }
                        else
                        {
                            arrayList.Add("NA");
                        }
                        if (manufacturedProduct.ManufacturerOrganization != null)
                        {
                            arrayList.Add(manufacturedProduct.ManufacturerOrganization.Name.ToString());
                        }
                        else
                        {
                            arrayList.Add("NA");
                        }

                        componententries.Add(count.ToString(), arrayList);
                        count++;
                    }
                    else if (observation != null)
                    {
                        if (observation.Value.Count > 0)
                        {
                            IANY observationvalue = observation.Value.FirstOrDefault();
                            ICD str = (ICD)observationvalue;
                            arrayList.Add(str.Code);
                            arrayList.Add(str.DisplayName);
                            IIVL_TS efftime = observation.EffectiveTime;
                            if (efftime != null && efftime.Low != null)
                            {
                                if (efftime.Low.Value != null)
                                {
                                    arrayList.Add(efftime.Low.AsDateTime.ToString());
                                }
                                else
                                {
                                    arrayList.Add("null");
                                }
                            }
                            else
                            {
                                arrayList.Add("null");
                            }
                           
                        }else
                        {
                            arrayList.Add(null);
                            arrayList.Add(null);
                            arrayList.Add(null);
                        }

                        componententries.Add(count.ToString(), arrayList);
                        count++;
                    }
                    else if (organizer != null)
                    {
                        IComponent4Collection orgComponent = organizer.Component;

                        foreach (IComponent4 objItem in orgComponent)
                        {
                            IObservation orgObservation = objItem.AsObservation;
                            arrayList.Add(orgObservation.Code.DisplayName);
                            if (orgObservation.Value != null)
                            {
                                IANY observationvalue = orgObservation.Value.FirstOrDefault();
                                var obj = observationvalue.GetType();
                                string objname = obj.Name;

                                switch (objname)
                                {
                                    default:
                                        ICD strcd = (ICD)observationvalue;
                                        arrayList.Add(strcd.Code);
                                        arrayList.Add(strcd.DisplayName);
                                        break;
                                    case "PQ":
                                        IPQ strpq = (IPQ)observationvalue;
                                        arrayList.Add(strpq.Value.ToString() + " " + strpq.Unit.ToString());
                                        break;
                                }
                                //IPQ str = (IPQ)observationvalue;
                                //arrayList.Add(str.Value.ToString() + " " + str.Unit.ToString());
                            }
                            IIVL_TS efftime = orgObservation.EffectiveTime;
                            if (efftime != null && efftime.Low != null)
                            {
                                if (efftime.Low.Value != null)
                                {
                                    arrayList.Add(efftime.Low.AsDateTime.ToString());
                                }
                                else
                                {
                                    arrayList.Add("null");
                                }
                            }
                            else
                            {
                                arrayList.Add("null");
                            }
                            if (orgObservation.ReferenceRange != null)
                            {
                                if (orgObservation.ReferenceRange.Count > 0)
                                {
                                    arrayList.Add(orgObservation.ReferenceRange[0].ObservationRange.Text.Text);
                                }
                                else
                                {
                                    arrayList.Add("NA");
                                }
                            }
                        }
                        arrayList.Add(organizer.Code.Code);
                        componententries.Add(count.ToString(), arrayList);
                        count++;
                    }
                    else if(procedure!=null)
                    {
                        if(procedure.Code!=null)
                        {
                            arrayList.Add(procedure.Code.Code);
                            arrayList.Add(procedure.Code.DisplayName);
                            
                        }else
                        {
                            arrayList.Add(null);
                            arrayList.Add(null);
                        }
                        componententries.Add(count.ToString(), arrayList);
                        count++;
                    }
                }
            }

            return componententries;
        }
    }
}
