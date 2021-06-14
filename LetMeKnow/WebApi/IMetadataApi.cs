using LetMeKnow.Models.Metadata;
using Refit;
using System.Threading.Tasks;

namespace LetMeKnow.WebApi
{
    public interface IMetadataApi
    {
        [Get("/api/v2/admin/location/states")]
        Task<StatesResponseModel> GetStates();

        [Get("/api/v2/admin/location/districts/{state_id}")]
        Task<DistrictResponseModel> GetDistricts([AliasAs("state_id")]int stateId);
    }
}
