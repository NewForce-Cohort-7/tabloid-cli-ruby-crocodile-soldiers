﻿using System;
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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Blog
                        WHERE Id = @id;
                    ";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Blog Get(int id)
        {
            throw new NotImplementedException();
        }

        public List<Blog> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Title, Url
                        FROM Blog;
                    ";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Blog> blogs = new List<Blog>();
                        while (reader.Read())
                        {
                            Blog blog = new Blog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("Url"))
                            };

                            blogs.Add(blog);
                        }

                        return blogs;
                    }
                }
            }
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
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Blog 
                                           SET Title = @title,
                                               Url = @url
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@title", entry.Title);
                    cmd.Parameters.AddWithValue("@Url", entry.Url);
                    cmd.Parameters.AddWithValue("@id", entry.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void InsertTag(Blog blog, Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO BlogTag (BlogId, TagId)
                                                       VALUES (@blogId, @tagId)";
                    cmd.Parameters.AddWithValue("@blogId", blog.Id);
                    cmd.Parameters.AddWithValue("@tagId", tag.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteTag(int blogId, int tagId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM BlogTAg 
                                         WHERE BlogId = @blogid AND 
                                               TagId = @tagId";
                    cmd.Parameters.AddWithValue("@blogId", blogId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}