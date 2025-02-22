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
    public class UserRepository : IUserInterface
    {
        private readonly NpgsqlConnection _npgsqlConnection;
        public UserRepository(NpgsqlConnection npgsqlConnection)
        {
            _npgsqlConnection = npgsqlConnection;
        }

        public async Task<t_User> Login(Login user)
        {
            t_User user_details = new t_User();

            try
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_Users_Api where c_Email = @c_email and c_Password = @c_password", _npgsqlConnection))
                {
                    cmd.Parameters.AddWithValue("@c_email", user.c_Email);
                    cmd.Parameters.AddWithValue("@c_password", user.c_Password);
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                user_details.c_UserName = (string)reader["c_Name"];
                                user_details.c_UserId = (int)reader["c_Id"];
                                user_details.c_Email = (string)reader["c_Email"];
                                user_details.c_Gender = (string)reader["c_Gender"];
                                user_details.c_Image = (string)reader["c_ProfileImage"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured in Login : " + ex.Message);
            }
            finally
            {
                if (_npgsqlConnection.State == ConnectionState.Open)
                {
                    _npgsqlConnection.Close();
                }
            }
            return user_details;
        }

        public async Task<int> Register(t_User register)
        {
            int status = 0;
            try
            {
                if (_npgsqlConnection.State != ConnectionState.Open)
                {
                    _npgsqlConnection.Open();
                }
                using (NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_users_api where c_Email = @c_email;", _npgsqlConnection))
                {
                    cmd.Parameters.AddWithValue("c_Email", register.c_Email);
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            return 0;
                        }
                    }
                }


                // {
                string query = "INSERT INTO t_Users_Api (c_Name, c_Email, c_Password, c_Gender, c_ProfileImage) VALUES (@c_name, @c_email, @c_password, @c_gender, @c_image); ";
                using (NpgsqlCommand cm = new NpgsqlCommand(query, _npgsqlConnection))
                {
                    // cm.Parameters.AddWithValue("@c_id", register.c_UserId);
                    cm.Parameters.AddWithValue("@c_name", register.c_UserName);
                    cm.Parameters.AddWithValue("@c_email", register.c_Email);
                    cm.Parameters.AddWithValue("@c_password", register.c_Password);
                    cm.Parameters.AddWithValue("@c_gender", register.c_Gender == null ? DBNull.Value : register.c_Gender);
                    cm.Parameters.AddWithValue("@c_image", register.c_Image == null ? DBNull.Value : register.c_Image);
                    // await _npgsqlConnection.OpenAsync();
                    await cm.ExecuteNonQueryAsync();
                    // await _npgsqlConnection.CloseAsync();
                    return 1;
                }
                // }
                // }
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured qhile registering user : " + ex.Message);
            }
            finally
            {
                if (_npgsqlConnection.State == ConnectionState.Open)
                {
                    _npgsqlConnection.Close();
                }
            }
            return -1;
        }
    
        
    }
}