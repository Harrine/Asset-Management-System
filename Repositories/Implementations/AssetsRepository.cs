using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.models;

namespace Repositories.Implementations
{
    public class AssetsRepository : IAssetInterface
    {
        private readonly NpgsqlConnection _npgsqlconnection;

        public AssetsRepository(NpgsqlConnection npgsqlConnection)
        {
            _npgsqlconnection = npgsqlConnection;
        }

        public async Task<int> Create(t_Assets assets)
        {
            try
            {
                if (_npgsqlconnection.State != ConnectionState.Open)
                {
                    _npgsqlconnection.Open();
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand("Insert Into t_Assets(c_assetName,c_Description,c_assetImage,c_userId,c_cupboardId,c_tags) values (@c_assetName,@c_Description,@c_assetImage,@c_userId,@c_cupboardId,@c_tags)", _npgsqlconnection))
                {
                    // insert query logic
                    cmd.Parameters.AddWithValue("@c_assetName", assets.c_assetsName);
                    cmd.Parameters.AddWithValue("@c_Description", assets.c_Description);
                    cmd.Parameters.AddWithValue("@c_assetImage", assets.c_assetImage == null ? DBNull.Value : assets.c_assetImage);
                    cmd.Parameters.AddWithValue("@c_userId", assets.c_userID);
                    cmd.Parameters.AddWithValue("@c_cupboardId", assets.c_cupboardID);
                    cmd.Parameters.AddWithValue("@c_tags", assets.c_tags);
                    // _npgsqlconnection.Open();
                    cmd.ExecuteNonQuery();
                    // _npgsqlconnection.Close();

                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while adding through create in asset respository : " + ex.Message);
            }
            finally
            {
                if (_npgsqlconnection.State == System.Data.ConnectionState.Open)
                {
                    _npgsqlconnection.Close();
                }
            }
            return 0;
        }

        public async Task<int> Delete(string id)
        {
            try
            {
                if (_npgsqlconnection.State != ConnectionState.Open)
                {
                    _npgsqlconnection.Open();
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand("Delete from t_Assets where c_assetId = @c_assetsId", _npgsqlconnection))
                {
                    cmd.Parameters.AddWithValue("@c_assetsId", id);
                    cmd.ExecuteNonQuery();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ocurred while deleting : ", ex.Message);
            }
            finally
            {
                if (_npgsqlconnection.State == ConnectionState.Open)
                {
                    _npgsqlconnection.Close();
                }
            }
            return 0;
        }

        public async Task<List<t_Assets>> GetAll()
        {
            List<t_Assets> assets = new List<t_Assets>();
            try
            {

                using (NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_Assets", _npgsqlconnection))
                {
                    if (_npgsqlconnection.State != ConnectionState.Open)
                    {
                        _npgsqlconnection.Open();
                    }

                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                assets.Add(new t_Assets
                                {
                                    c_assetsId = reader.GetInt32(reader.GetOrdinal("c_assetId")),
                                    c_assetsName = reader.GetString(reader.GetOrdinal("c_assetName")),
                                    c_Description = reader.GetString(reader.GetOrdinal("c_Description")),
                                    c_tags = reader.GetString(reader.GetOrdinal("c_tags")),
                                    c_assetImage = reader.GetString(reader.GetOrdinal("c_assetImage")),
                                });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Ocurred while getting all assets : " + ex.Message);
            }
            finally
            {
                if (_npgsqlconnection.State == ConnectionState.Open)
                {
                    _npgsqlconnection.Close();
                }
            }
            return assets;
        }

        public async Task<List<t_Cupboards>> GetALLCupboards()
        {
            List<t_Cupboards> cupboards = new List<t_Cupboards>();
            try{
                if(_npgsqlconnection.State != ConnectionState.Open){
                    _npgsqlconnection.Open();
                }

                using(NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_Cupboards",_npgsqlconnection)){
                    using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        if(reader.HasRows){
                            while(await reader.ReadAsync()){
                                cupboards.Add(new t_Cupboards{
                                    c_CupboardID = reader.GetInt32(reader.GetOrdinal("c_cupboardId")),
                                    c_CupboardName = reader.GetString(reader.GetOrdinal("c_cupboardName")),
                                    c_c_RoomID = reader.GetInt32(reader.GetOrdinal("c_roomId"))
                                });
                            }
                        }
                    }
                }

            }catch(Exception ex){
                Console.WriteLine("Error While getting all cupboards : ",ex.Message);
            }finally{
                if(_npgsqlconnection.State == ConnectionState.Open){
                    _npgsqlconnection.Close();
                }
            }
            return cupboards;
        }

        public async Task<List<t_Cupboards>> GetALLCupboardsByID(string id)
        {
            List<t_Cupboards> cupboards_by_id = new List<t_Cupboards>();
            try{
                if(_npgsqlconnection.State != ConnectionState.Open){
                    _npgsqlconnection.Open();
                }

                using(NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_Cupboards where c_roomId = @c_roomId;",_npgsqlconnection)){
                    cmd.Parameters.AddWithValue("@c_roomId",Convert.ToInt32(id));

                    using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        if(reader.HasRows){
                            while(await reader.ReadAsync()){
                                cupboards_by_id.Add(new t_Cupboards{
                                    c_CupboardID = reader.GetInt32(reader.GetOrdinal("c_cupboardId")),
                                    c_CupboardName = reader.GetString(reader.GetOrdinal("c_cupboardName")),
                                    c_c_RoomID = reader.GetInt32(reader.GetOrdinal("c_roomId"))
                                });
                            }
                        }
                    }
                }
            }catch(Exception ex){
                Console.WriteLine("Error while getting all cupboards by room id : ",ex.Message);
            }finally{
                if(_npgsqlconnection.State == ConnectionState.Open){
                    _npgsqlconnection.Close();
                }
            }
            return cupboards_by_id;
        }

        public async Task<List<t_Rooms>> GetALLRomms()
        {
            List<t_Rooms> rooms = new List<t_Rooms>();
            try{
                if(_npgsqlconnection.State != ConnectionState.Open){
                    _npgsqlconnection.Open();
                }
                using(NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_Rooms",_npgsqlconnection)){
                    using(NpgsqlDataReader reader = await cmd.ExecuteReaderAsync()){
                        if(reader.HasRows){
                            while(await reader.ReadAsync()){
                                rooms.Add(new t_Rooms{
                                    c_RoomId = reader.GetInt32(reader.GetOrdinal("c_roomId")),
                                    c_RoomName = reader.GetString(reader.GetOrdinal("c_roomName"))
                                });
                            }
                        }
                    }
                }
            }catch(Exception ex){
                Console.WriteLine("Error While getting all Rooms : "+ex.Message);
            }finally{
                if(_npgsqlconnection.State == ConnectionState.Open){
                    _npgsqlconnection.Close();
                }
            }
            return rooms;
        }

        public async Task<t_Assets> GetAssetsById(string id)
        {
            t_Assets asset = null;
            try
            {

                if (_npgsqlconnection.State != ConnectionState.Open)
                {
                    _npgsqlconnection.Open();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_Assets where c_userId = @c_userId", _npgsqlconnection))
                {
                    cmd.Parameters.AddWithValue("@c_userId", Convert.ToInt32(id));
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (await reader.ReadAsync())
                            {
                                asset = new t_Assets
                                {
                                    // c_assetsId = reader.GetInt32(reader.GetOrdinal("c_assetId")),
                                    c_assetsName = reader.GetString(reader.GetOrdinal("c_assetName")),
                                    c_Description = reader.GetString(reader.GetOrdinal("c_Description")),
                                    c_tags = reader.GetString(reader.GetOrdinal("c_tags")),
                                    c_assetImage = reader.GetString(reader.GetOrdinal("c_assetImage")),
                                };
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured while getting specific asset : " + ex.Message);
            }
            finally
            {
                if (_npgsqlconnection.State == ConnectionState.Open)
                {
                    _npgsqlconnection.Close();
                }
            }
            return asset;
        }

        public async Task<List<t_Assets>> GetAssetsByUser(string id)
        {
            List<t_Assets> asset = new List<t_Assets>();
            try
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_Assets where c_userId::text = @c_userID", _npgsqlconnection))
                {
                    cmd.Parameters.AddWithValue("@c_userID", id);
                    if (_npgsqlconnection.State != ConnectionState.Open)
                    {
                        _npgsqlconnection.Open();
                    }
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            asset.Add(new t_Assets
                            {
                                c_assetsId = reader.GetInt32(reader.GetOrdinal("c_assetId")),
                                c_assetsName = reader.GetString(reader.GetOrdinal("c_assetName")),
                                c_Description = reader.GetString(reader.GetOrdinal("c_Description")),
                                c_tags = reader.GetString(reader.GetOrdinal("c_tags")),
                                c_assetImage = reader.GetString(reader.GetOrdinal("c_assetImage")),
                                c_cupboardID = reader.GetInt32(reader.GetOrdinal("c_cupboardId")),
                                c_userID = reader.GetInt32(reader.GetOrdinal("c_userId"))
                            });
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured in Get all assets by user : " + ex.Message);
            }
            finally
            {
                if (_npgsqlconnection.State != ConnectionState.Open)
                {
                    _npgsqlconnection.Close();
                }
            }
            return asset;
        }

        public async Task<int> Update(t_Assets asset)
        {
            try
            {
                if (_npgsqlconnection.State != ConnectionState.Open)
                {
                    _npgsqlconnection.Open();
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand("Update t_Assets set c_assetName=@c_assetname,c_Description=@c_description,c_assetImage=@c_image,c_cupboardId=@c_cupboardID,c_tags=@c_tags where c_assetId = @c_assetID", _npgsqlconnection))
                {
                    cmd.Parameters.AddWithValue("@c_assetID", asset.c_assetsId);
                    cmd.Parameters.AddWithValue("@c_assetname", asset.c_assetsName);
                    cmd.Parameters.AddWithValue("@c_description", asset.c_Description);
                    cmd.Parameters.AddWithValue("@c_image", asset.c_assetImage ?? (object)DBNull.Value); // Handle null values);
                    cmd.Parameters.AddWithValue("@c_cupboardID", asset.c_cupboardID);
                    cmd.Parameters.AddWithValue("@c_tags", asset.c_tags);
                    // _npgsqlconnection.Open();
                    cmd.ExecuteReader();
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error ocurred while updating the asset : " + ex.Message);
            }
            finally
            {
                if (_npgsqlconnection.State == ConnectionState.Open)
                {
                    _npgsqlconnection.Close();
                }
            }
            return 0;
        }

    }
}