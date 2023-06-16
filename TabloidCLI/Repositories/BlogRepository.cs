using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI
{
    public class BlogRepository : DatabaseConnector, IRepository<Blog>
    {
        public BlogRepository(string connectionString) : base(connectionString) { }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Blog Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Blog> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Insert(Blog entry)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                INSERT INTO Blog (Title, URL)
                VALUES (@title, @url);
            ";
                    cmd.Parameters.AddWithValue("@title", entry.Title);
                    cmd.Parameters.AddWithValue("@url", entry.Url);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void Update(Blog entry)
        {
            throw new NotImplementedException();
        }
    }
}