using System;
using System.Collections.Generic;

namespace LetMeKnow.Models
{
    public class VaccSessAvailDto
    {
        public string CentreName { get; set; }

        public string CentreAddress { get; set; }

        public string SessionId { get; set; }

        public DateTime Date { get; set; }

        public int AvailableCapacity { get; set; }

        public int MinAgeLimit { get; set; }

        public string Vaccine { get; set; }

        public List<string> Slots { get; set; }

        public string FeeType { get; set; }

        public int AvailableCapacityDose1 { get; set; }

        public int AvailableCapacityDose2 { get; set; }
    }
}
