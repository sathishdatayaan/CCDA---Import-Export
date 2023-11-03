using CreateClinicalReport.Model;
using HL7SDK;
using HL7SDK.Cda;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.ParserLibrary
{
    public class ParseAddress
    {
        /// <summary>
        /// Generic Method Get Address
        /// </summary>
        /// <param name="ptaddess"></param>
        /// <returns></returns>
        public AddressModel FillAddress(IADCollection ptaddess)
        {
            AddressModel address = new AddressModel();
            if (ptaddess.Count > 0)
            {
                IHL73ObjectCollection objectCollection = ptaddess[0].ChildObjects;
                foreach (var item in objectCollection)
                {
                    var test = item.GetType();
                    string name = test.Name;

                    switch (name)
                    {
                        case "adxpdeliveryAddressLine":
                            adxpdeliveryAddressLine strt = (adxpdeliveryAddressLine)item;
                            address.street = strt.Text;
                            break;
                        case "adxpstreetAddressLine":
                            adxpstreetAddressLine str = (adxpstreetAddressLine)item;
                            address.street = str.Text;
                            break;
                        case "adxpcity":
                            adxpcity cty = (adxpcity)item;
                            address.city = cty.Text;
                            break;
                        case "adxpstate":
                            adxpstate stt = (adxpstate)item;
                            address.state = stt.Text;
                            break;
                        case "adxpcountry":
                            adxpcountry ctry = (adxpcountry)item;
                            address.country = ctry.Text;
                            break;
                        case "adxppostalCode":
                            adxppostalCode zip = (adxppostalCode)item;
                            address.pinCode = zip.Text;
                            break;
                    }
                }
            }
            return address;
        }
        /// <summary>
        /// Generic Method To Get Name
        /// </summary>
        /// <param name="namecollection"></param>
        /// <returns></returns>
        public NameModel FillName(IPNCollection namecollection)
        {
            NameModel name = new NameModel();
            if (namecollection.Count > 0)
            {
                IHL73ObjectCollection objectCollection = namecollection[0].ChildObjects;
                foreach (var item in objectCollection)
                {
                    var test = item.GetType();
                    string name1 = test.Name;

                    switch (name1)
                    {
                        case "adxpdeliveryAddressLine":
                            adxpdeliveryAddressLine strt = (adxpdeliveryAddressLine)item;
                            name.Createengiven = strt.Text;
                            break;
                        case "adxpstreetAddressLine":
                            adxpstreetAddressLine str = (adxpstreetAddressLine)item;
                            name.Createenfamily = str.Text;
                            break;
                        case "adxpcity":
                            adxpcity cty = (adxpcity)item;
                            name.CreateenSuffix = cty.Text;
                            break;

                    }
                }
            }
            return name;
        }
    }
}
