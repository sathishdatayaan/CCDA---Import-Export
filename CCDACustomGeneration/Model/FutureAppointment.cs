using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreateClinicalReport.Model
{
    public class FutureAppointment
    {
        public string DoctorName { get; set; }
        public Nullable<DateTime> AppointmentDate { get; set; }
    }
}
