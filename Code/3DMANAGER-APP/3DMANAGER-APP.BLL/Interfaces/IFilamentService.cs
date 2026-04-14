using _3DMANAGER_APP.BLL.Models.Base;
using _3DMANAGER_APP.BLL.Models.Filament;
using Microsoft.AspNetCore.Http;

namespace _3DMANAGER_APP.BLL.Interfaces
{
    public interface IFilamentService
    {
        public List<FilamentListResponse> GetFilamentList(int group, out BaseError? error);
        public Task<CommonResponse<int>> PostFilament(FilamentRequest filament);
        public bool UpdateFilament(FilamentUpdateRequest request);
        FilamentDetailObject GetFilamentDetail(int groupId, int filamentId, out BaseError? error);
        public Task<CommonResponse<bool>> DeleteFilament(int filamentId, int groupId);
        public Task<CommonResponse<bool>> DeleteFilamentImage(int filamentId, int groupId);
        public Task<CommonResponse<bool>> UpdateFilamentImage(int filamentId, int groupId, IFormFile imageFile);
        Task CheckFilamentLevelsAsync();
    }
}
