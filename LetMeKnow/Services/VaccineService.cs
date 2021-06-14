using LetMeKnow.Entities;
using LetMeKnow.Models;
using LetMeKnow.Models.Metadata;
using LetMeKnow.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LetMeKnow.Services
{
    public class VaccineService
    {
        private readonly AppDbContext _dbContext;

        public static VaccineDto[] Vaccines { get; set; } = new VaccineDto[]
        {
            new VaccineDto
            {
                Id = 0,
                Name = "All"
            },
            new VaccineDto
            {
                Id = 1,
                Name = "COVAXIN"
            },
            new VaccineDto
            {
                Id = 2,
                Name = "COVISHIELD"
            }
        };

        public VaccineService()
        {
            _dbContext = Registry.Container.Resolve<AppDbContext>();
        }

        private async Task<List<VaccSessAvailDto>> GetVaccineSessions(int districtId)
        {
            try
            {
                var appointments = await Domain.AppointmentAvailBase.GetVaccinationSessionsFor7Days(districtId, DateTime.Now.ToString("dd-MM-yyyy"));
                _dbContext.SessionHistories.Insert(new VaccineSessionHistory
                {
                    CollectedOn = DateTime.Now,
                    Appointments = appointments.Centers
                });
                var sessions = appointments.Centers.SelectMany(x => x.Sessions)
                    .Select(x =>
                    {
                        var centre = appointments.Centers.FirstOrDefault(y => y.Sessions.Any(z => z.SessionId == x.SessionId));
                        var item = new VaccSessAvailDto
                        {
                            AvailableCapacity = x.AvailableCapacity,
                            AvailableCapacityDose1 = x.AvailableCapacityDose1,
                            AvailableCapacityDose2 = x.AvailableCapacityDose2,
                            Date = Convert.ToDateTime(x.Date),
                            MinAgeLimit = x.MinAgeLimit,
                            SessionId = x.SessionId,
                            Slots = x.Slots,
                            Vaccine = x.Vaccine,
                            CentreName = centre.Name,
                            FeeType = centre.FeeType,
                            CentreAddress = string.Concat(centre.Address, "\n", centre.DistrictName,", ", centre.StateName, ", ",centre.Pincode) 
                        };
                        return item;
                    })
                    .OrderByDescending(x => x.AvailableCapacity)
                    .ThenByDescending(x => x.AvailableCapacityDose1)
                    .ThenByDescending(x => x.AvailableCapacityDose2)
                    .ThenBy(x => x.Date)
                    .ThenBy(x => x.MinAgeLimit)
                    .ToList();
                return sessions;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return new List<VaccSessAvailDto>();
            }
        }

        public async Task<List<VaccSessAvailDto>> PopulateAppointmentsAsPerSettings(Setting setting)
        {
            var sessions = await GetVaccineSessions(setting.District.Id);

            IEnumerable<VaccSessAvailDto> whereClause = sessions;
            if(setting.Is18Plus && setting.Is45Plus && setting.Is40Plus)
            {
                whereClause = whereClause.Where(x => x.MinAgeLimit >= 18);
            }
            else if (setting.Is18Plus)
            {
                whereClause = whereClause.Where(x => x.MinAgeLimit >= 18 && x.MinAgeLimit < 40);
            }
            else if (setting.Is40Plus)
            {
                whereClause = whereClause.Where(x => x.MinAgeLimit >= 40 && x.MinAgeLimit < 45);
            }
            else if (setting.Is45Plus)
            {
                whereClause = whereClause.Where(x => x.MinAgeLimit >= 45);
            }

            if(setting.Dose1Enabled && setting.Dose2Enabled)
            {
                whereClause = whereClause.Where(x => x.AvailableCapacityDose1 > 0 && x.AvailableCapacityDose2 > 0);
            }
            else if (setting.Dose1Enabled)
            {
                whereClause = whereClause.Where(x => x.AvailableCapacityDose1 > 0);
            }
            else if (setting.Dose2Enabled)
            {
                whereClause = whereClause.Where(x => x.AvailableCapacityDose2 > 0);
            }
            else
            {
                whereClause = whereClause.Where(x => x.AvailableCapacityDose1 > 0 || x.AvailableCapacityDose2 > 0);
            }

            if (setting.OnlyFree)
            {
                whereClause = whereClause.Where(x => x.FeeType == "Free");
            }

            if (setting.Vaccine != null && setting.Vaccine.Id > 0)
            {
                whereClause = whereClause.Where(x => x.Vaccine == setting.Vaccine.Name);
            }

            sessions = whereClause.ToList();
            return sessions;
        }
    }
}
