using Lab4.Task1.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4.Task1.DAL
{
    public class BookRepository
    {
        private readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionString_ADO"].ConnectionString;

        public List<Book> GetAllBooks()
        {
            List<Book> books = [];
            using (SqlConnection connection = new(connectionString))
            {
                SqlDataAdapter adapter = new("SELECT ID, ISBN, Title, Authors, Publisher, PublicationYear FROM Books", connection);
                DataTable bookTable = new();
                adapter.Fill(bookTable);

                foreach (DataRow row in bookTable.Rows)
                {
                    Book book = new()
                    {
                        ID = Convert.ToInt32(row["ID"]),
                        ISBN = row["ISBN"].ToString(),
                        Title = row["Title"].ToString(),
                        Authors = row["Authors"].ToString(),
                        Publisher = row["Publisher"].ToString(),
                        PublicationYear = Convert.ToInt32(row["PublicationYear"])
                    };
                    books.Add(book);
                }
            }
            return books;
        }

        public void UpdateBook(Book book)
        {
            using SqlConnection connection = new(connectionString);

            SqlCommand command = new(@"

                    UPDATE Books SET 
                        Title=@Title, 
                        Authors=@Authors, 
                        Publisher=@Publisher, 
                        PublicationYear=@PublicationYear,
                        ISBN=@ISBN 
                        WHERE ID=@ID",

            connection);

            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Authors", book.Authors);
            command.Parameters.AddWithValue("@Publisher", book.Publisher);
            command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
            command.Parameters.AddWithValue("@ID", book.ID);

            connection.Open();
            command.ExecuteNonQuery();
        }

        public void CreateBook(Book book)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new(@"

                    INSERT INTO Books 
                    (ISBN, Title, Authors, Publisher, PublicationYear) VALUES 
                    (@ISBN, @Title, @Authors, @Publisher, @PublicationYear)", 

            connection);

            command.Parameters.AddWithValue("@ISBN", book.ISBN);
            command.Parameters.AddWithValue("@Title", book.Title);
            command.Parameters.AddWithValue("@Authors", book.Authors);
            command.Parameters.AddWithValue("@Publisher", book.Publisher);
            command.Parameters.AddWithValue("@PublicationYear", book.PublicationYear);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void DeleteBook(int id)
        {
            using SqlConnection connection = new(connectionString);
            SqlCommand command = new("DELETE FROM Books WHERE ID=@ID", connection);
            command.Parameters.AddWithValue("@ID", id);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
