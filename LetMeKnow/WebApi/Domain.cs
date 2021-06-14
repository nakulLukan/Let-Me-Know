using LetMeKnow.Utilities;
using Refit;
using System.Net.Http;

namespace LetMeKnow.WebApi
{
    public static class Domain
    {
        public const string CowinBaseUrl = "https://cdn-api.co-vin.in";
        private static HttpClient httpClient = new HttpClient(new HttpLoggingHandler())
        {
            BaseAddress = new System.Uri(CowinBaseUrl),
        };
        public static IMetadataApi MetaDataBase => RestService.For<IMetadataApi>(httpClient);
        public static IAppointmentAvailabilityApi AppointmentAvailBase => RestService.For<IAppointmentAvailabilityApi>(httpClient);
    }
}
