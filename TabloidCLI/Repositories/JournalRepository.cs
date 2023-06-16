using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabloidCLI.Repositories
{
    using System;
    using System.Collections.Generic;
    using global::TabloidCLI.Models;
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
                                               FirstName,
                                               LastName,
                                               Bio
                                          FROM Author";

                        List<Author> authors = new List<Author>();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Author author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            };
                            authors.Add(author);
                        }

                        reader.Close();

                        return authors;
                    }
                }
            }

            public Author Get(int id)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"SELECT a.Id AS AuthorId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               t.Id AS TagId,
                                               t.Name
                                          FROM Author a 
                                               LEFT JOIN AuthorTag at on a.Id = at.AuthorId
                                               LEFT JOIN Tag t on t.Id = at.TagId
                                         WHERE a.id = @id";

                        cmd.Parameters.AddWithValue("@id", id);

                        Author author = null;

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (author == null)
                            {
                                author = new Author()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Bio = reader.GetString(reader.GetOrdinal("Bio")),
                                };
                            }

                            if (!reader.IsDBNull(reader.GetOrdinal("TagId")))
                            {
                                author.Tags.Add(new Tag()
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                });
                            }
                        }

                        reader.Close();

                        return author;
                    }
                }
            }

            public void Insert(Author author)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"INSERT INTO Author (FirstName, LastName, Bio )
                                                     VALUES (@firstName, @lastName, @bio)";
                        cmd.Parameters.AddWithValue("@firstName", author.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", author.LastName);
                        cmd.Parameters.AddWithValue("@bio", author.Bio);

                        cmd.ExecuteNonQuery();
                    }
                }
            }

            public void Update(Author author)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Author 
                                           SET FirstName = @firstName,
                                               LastName = @lastName,
                                               bio = @bio
                                         WHERE id = @id";

                        cmd.Parameters.AddWithValue("@firstName", author.FirstName);
                        cmd.Parameters.AddWithValue("@lastName", author.LastName);
                        cmd.Parameters.AddWithValue("@bio", author.Bio);
                        cmd.Parameters.AddWithValue("@id", author.Id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }



            
        }
    }
}
