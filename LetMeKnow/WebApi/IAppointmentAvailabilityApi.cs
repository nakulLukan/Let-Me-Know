using CaseExtensions;
using LetMeKnow.Models.AppointmentAvailability;
using Refit;
using System.Threading.Tasks;

namespace LetMeKnow.WebApi
{
    public interface IAppointmentAvailabilityApi
    {
        /// <summary>
        /// Get vaccination sessions by districts for 7 days
        /// </summary>
        /// <returns></returns>
        [Get("/api/v2/appointment/sessions/public/calendarByDistrict")]
        Task<VaccSessAvailabilityResponseModel> GetVaccinationSessionsFor7Days([AliasAs("district_id")][Query] int districtId, [AliasAs("date")][Query] string date);
    }
}
