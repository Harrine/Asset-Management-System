using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.models;

namespace Repositories.Interfaces
{
    public interface IAssetInterface
    {
        public Task<int> Create(t_Assets assets);

        public Task<int> Delete(string id);

        public Task<List<t_Assets>> GetAll();

        public Task<t_Assets> GetAssetsById(string id);

        public Task<t_Assets> GetAssetsByAssetID(string id);

        public Task<List<t_Assets>> GetAssetsByUser(string id);

        public Task<int> Update(t_Assets asset);

        public Task<List<t_Rooms>> GetALLRomms();

        public Task<List<t_Cupboards>> GetALLCupboards();

        public Task<List<t_Cupboards>> GetALLCupboardsByID(string id);
    }
}