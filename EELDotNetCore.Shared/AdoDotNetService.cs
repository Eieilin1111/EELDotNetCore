﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EELDotNetCore.Shared
{
    public class AdoDotNetService
    {
        private readonly string _connectionstring;
        public AdoDotNetService(string connectionstring)
        {
            _connectionstring = connectionstring;
        }

        public List<T> Query<T>(string query, params AdoDotNetParameter[]? parameters) 
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);

            if (parameters is not null &&  parameters.Length > 0)
            {
                //foreach (var item in parameters) 
                //{ 
                //    cmd.Parameters.AddWithValue(item.Name, item.Value);
                //}

                //cmd.Parameters.AddRange(parameters.Select(item =>new SqlParameter(item.Name, item.Value)).ToArray());

                var parametersArray = parameters.Select(item => new SqlParameter(item.Name, item.Value)).ToArray();
                cmd.Parameters.AddRange(parametersArray);
            }
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            string json = JsonConvert.SerializeObject(dt);//C# to Json
            List<T> lst = JsonConvert.DeserializeObject<List<T>>(json)!;

            connection.Close();
            return lst;
        }

        public T QueryFirstOrDefault<T>(string query, params AdoDotNetParameter[]? parameters)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);

            if (parameters is not null && parameters.Length > 0)
            {
                //foreach (var item in parameters) 
                //{ 
                //    cmd.Parameters.AddWithValue(item.Name, item.Value);
                //}

                //cmd.Parameters.AddRange(parameters.Select(item =>new SqlParameter(item.Name, item.Value)).ToArray());

                var parametersArray = parameters.Select(item => new SqlParameter(item.Name, item.Value)).ToArray();
                cmd.Parameters.AddRange(parametersArray);
            }
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            sqlDataAdapter.Fill(dt);

            connection.Close();


            string json = JsonConvert.SerializeObject(dt);//C# to Json
            List<T> lst = JsonConvert.DeserializeObject<List<T>>(json)!;

            
            return lst[0];
        }

        public int Execute(string query, params AdoDotNetParameter[]? parameters)
        {
            SqlConnection connection = new SqlConnection(_connectionstring);
            connection.Open();

            SqlCommand cmd = new SqlCommand(query, connection);

            if (parameters is not null && parameters.Length > 0)
            {
     
                cmd.Parameters.AddRange(parameters.Select(item =>new SqlParameter(item.Name, item.Value)).ToArray());

                //var parametersArray = parameters.Select(item => new SqlParameter(item.Name, item.Value)).ToArray();
                //cmd.Parameters.AddRange(parametersArray);
            }
           var result = cmd.ExecuteNonQuery();

            connection.Close();
            return result;
        }


        public class AdoDotNetParameter
        {
            public AdoDotNetParameter() { }
            public AdoDotNetParameter(string name, object value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; set; }
            public object Value { get; set; }
        }
    }
}
