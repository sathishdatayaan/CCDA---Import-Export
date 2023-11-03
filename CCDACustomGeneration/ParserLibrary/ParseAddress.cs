using CreateClinicalReport.Model;
using HL7SDK;
using HL7SDK.Cda;
using System;
using System.Collections.Generic;
using System.Text;

namespace CreateClinicalReport.ParserPatient
{
    public class ParseAddress
    {
        /// <summary>
        /// Generic Method Get Address
        /// </summary>
        /// <param name="ptaddess"></param>
        /// <returns></returns>
        public AddressModel FillAddress(string pt)
        {
            AddressModel address = new AddressModel();
            if (!string.IsNullOrEmpty(pt))
            {
                address.country = pt;
                address.city = pt;
                address.street = pt;
                address.state = pt;
                address.pinCode = pt;
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
