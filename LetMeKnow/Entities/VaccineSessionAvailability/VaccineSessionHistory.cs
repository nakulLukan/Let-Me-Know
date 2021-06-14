using LetMeKnow.Models.AppointmentAvailability;
using System;
using System.Collections.Generic;

namespace LetMeKnow.Entities
{
    public class VaccineSessionHistory
    {
        public DateTime CollectedOn { get; set; }
        public List<Center> Appointments { get; set; }
    }
}
