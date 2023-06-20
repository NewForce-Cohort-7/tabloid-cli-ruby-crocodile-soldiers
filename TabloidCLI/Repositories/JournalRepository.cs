﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI
    {
        public class JournalRepository : DatabaseConnector, IRepository<Journal>
        {
            public JournalRepository(string connectionString) : base(connectionString) { }

            public List<Journal> GetAll()
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT id,
                                               Title,
                                               Content,
                                               CreateDateTime
                                          FROM Journal";

                        List<Journal> journals = new List<Journal>();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Journal journal = new Journal()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Content = reader.GetString(reader.GetOrdinal("Content")),
                               CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            };
                            journals.Add(journal);
                        }

                        reader.Close();

                        return journals;
                    }
                }
            }

            public Journal Get(int id)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT a.Id AS JournalId,
                                               a.Title,
                                               a.Content,
                                               a.CreateDateTime,
                                               t.Id AS TagId,
                                               t.Name
                                          FROM Journal a 
                                               LEFT JOIN JournalTag at on a.Id = at.JournalId
                                               LEFT JOIN Tag t on t.Id = at.TagId
                                         WHERE a.id = @id";

                        cmd.Parameters.AddWithValue("@id", id);

                        Journal journal = null;

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (journal == null)
                            {
                                journal = new Journal()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("JournalId")),
                                    Title = reader.GetString(reader.GetOrdinal("Title")),
                                    Content = reader.GetString(reader.GetOrdinal("Content")),
                                    CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                                };
                            }

                            
                        }

                        reader.Close();

                        return journal;
                    }
                }
            }

            public void Insert(Journal journal)
            {
                using (SqlConnection conn = Connection)
                { 
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Journal (Title, Content, CreateDateTime )
                                                     VALUES (@Title, @Content, @CreateDateTime)";
                        cmd.Parameters.AddWithValue("@Title", journal.Title);
                        cmd.Parameters.AddWithValue("@Content", journal.Content);
                        cmd.Parameters.AddWithValue("@CreateDateTime", journal.CreateDateTime);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public void Update(Journal journal)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Journal
                                           SET Title = @Title,
                                               Content = @Content,
                                               CreateDateTime = @CreateDateTime
                                         WHERE id = @id";

                        cmd.Parameters.AddWithValue("@Title", journal.Title);
                        cmd.Parameters.AddWithValue("@Content", journal.Content);
                        cmd.Parameters.AddWithValue("@CreateDateTime", journal.CreateDateTime);
                        cmd.Parameters.AddWithValue("@id", journal.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }



            
        }
    }
