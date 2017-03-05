using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a blog application: Tables Needed: (Users, Blogs, Comments)
            // All users to create a blog, read a blog, comment on the blog they are currently reading
            // When reading a blog, comments should show at the bottom

            SqlConnection connection;
            connection = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=C:\Users\hlucas\Google Drive\AcademyPGH\week5\BlogApplication\BlogApplication\Database1.mdf;Integrated Security=True");


            connection.Open();

            SqlCommand selectUsers;
            selectUsers = new SqlCommand("SELECT * FROM Users", connection);

            SqlDataReader selectusersReader;
            selectusersReader = selectUsers.ExecuteReader();


            if (selectusersReader.HasRows)
            {
                while (selectusersReader.Read())
                {
                    Console.WriteLine(selectusersReader["UserID"] + " " + selectusersReader["UserName"]);
                }
            }
            Console.WriteLine();
            Console.WriteLine("Look over the list of users and IDs above, enter your ID and then press enter.");

            selectusersReader.Close();




            string userId = Console.ReadLine();

            SqlCommand findUserId;
            findUserId = new SqlCommand($"SELECT UserName FROM Users WHERE UserID = {userId}", connection);

            SqlDataReader userIdReader;
            userIdReader = findUserId.ExecuteReader();

            if (userIdReader.HasRows)
            {
                userIdReader.Read();
                Console.WriteLine($"Hello {userIdReader["UserName"]}, what would you like to do?");
            }

            Console.Clear();
            Console.WriteLine();

            userIdReader.Close();



            bool looping = true;
            while (looping)
            {
                Console.WriteLine("Enter a) to create your own blog post,");
                Console.WriteLine("      b) to view someone's blog post,"); 
                Console.WriteLine("      c) to comment on someone's blog post,");
                Console.WriteLine("   or x) to exit the system.");
                string choice = Console.ReadLine().ToLower();
                Console.Clear();

                if (choice == "a")
                {
                    Console.WriteLine();
                    Console.WriteLine("Enter the title of your blog post:");
                    string blogTitle = Console.ReadLine();
                    Console.WriteLine("Enter the body of your blog post: ");
                    string blog = Console.ReadLine();

                    SqlCommand createBlog;
                    createBlog = new SqlCommand($"INSERT INTO blogs (BlogTitle, UserID, Blog) VALUES ('{blogTitle}', '{userId}', '{blog}');", connection);

                    Console.Clear();
                    createBlog.ExecuteNonQuery();
                }

                if (choice == "b")
                {

                    SqlCommand pickBlog;
                    pickBlog = new SqlCommand("SELECT blogs.BlogID, users.UserName, blogs.BlogTitle FROM users JOIN blogs ON users.UserID = blogs.UserID", connection);

                    SqlDataReader pickBlogReader;
                    pickBlogReader = pickBlog.ExecuteReader();

                    if (pickBlogReader.HasRows)
                    {
                        while (pickBlogReader.Read())
                        {
                            Console.WriteLine($"{pickBlogReader["BlogID"]}" + "---" + $"{pickBlogReader["UserName"]}" + "---" + $"\"{pickBlogReader["BlogTitle"]}\"");
                        }

                    }

                    Console.WriteLine();
                    Console.WriteLine($"Choose the ID of the blog post that you would like to view.");                    
                    string blogId = Console.ReadLine();
                    Console.Clear();

                    pickBlogReader.Close();

                    SqlCommand viewBlog;
                    viewBlog = new SqlCommand($"SELECT BlogTitle, Blog FROM Blogs WHERE BlogID = {blogId}", connection);

                    SqlDataReader viewBlogReader;
                    viewBlogReader = viewBlog.ExecuteReader();

                    if (viewBlogReader.HasRows)
                    {
                        while (viewBlogReader.Read())
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{viewBlogReader["BlogTitle"]}");
                            Console.WriteLine();
                            Console.WriteLine($"{viewBlogReader["Blog"]}");
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }

                    viewBlogReader.Close();

                    SqlCommand viewComments;
                    viewComments = new SqlCommand($"SELECT Text, UserName FROM comments JOIN users ON users.UserID = comments.UserID WHERE BlogID = {blogId}", connection);

                    SqlDataReader viewCommentsReader;
                    viewCommentsReader = viewComments.ExecuteReader();

                    if (viewCommentsReader.HasRows)
                    {
                        while (viewCommentsReader.Read())
                        {
                            Console.WriteLine();
                            Console.WriteLine($"User comment: {viewCommentsReader["UserName"]}");
                            Console.WriteLine($"{viewCommentsReader["Text"]}");
                            Console.WriteLine();
                            Console.WriteLine("-------------------------------------------------------");
                        }
                    }

                    viewCommentsReader.Close();

                }


                if (choice == "c")
                {
                    SqlCommand pickBlog;
                    pickBlog = new SqlCommand("SELECT blogs.blogID, users.UserName, blogs.BlogTitle FROM users JOIN blogs ON users.UserID = blogs.UserID", connection);

                    SqlDataReader pickBlogReader;
                    pickBlogReader = pickBlog.ExecuteReader();

                    if (pickBlogReader.HasRows)
                    {
                        while (pickBlogReader.Read())
                        {
                            Console.WriteLine($"{pickBlogReader["BlogID"]}" + "---" + $"{pickBlogReader["UserName"]}" + "---" + $"\"{pickBlogReader["BlogTitle"]}\"");
                        }

                    }

                    Console.WriteLine();
                    Console.WriteLine($"Choose the ID of the blog post on which you would like to comment.");
                    string blogId = Console.ReadLine();
                    pickBlogReader.Close();

                    SqlCommand viewBlog;
                    viewBlog = new SqlCommand($"SELECT BlogTitle, Blog FROM Blogs WHERE BlogID = {blogId}", connection);

                    SqlDataReader viewBlogReader;
                    viewBlogReader = viewBlog.ExecuteReader();

                    if (viewBlogReader.HasRows)
                    {
                        while (viewBlogReader.Read())
                        {
                            Console.WriteLine();
                            Console.WriteLine($"{viewBlogReader["BlogTitle"]}");
                            Console.WriteLine();
                            Console.WriteLine($"{viewBlogReader["Blog"]}");
                            Console.WriteLine();
                            Console.WriteLine();
                        }
                    }

                    viewBlogReader.Close();


                    Console.WriteLine("Enter your comment about the blog post you viewed.");
                    string text = Console.ReadLine();

                    SqlCommand comment;
                    comment = new SqlCommand($"INSERT INTO comments (Text, UserID, BlogID) VALUES ('{text}', '{userId}', '{blogId}');", connection);

                    comment.ExecuteNonQuery();
                }


                if (choice == "x")
                {
                    Console.WriteLine("Thanks for blogging! Goodbye!");
                    looping = false;
                }

            }

            Console.ReadLine();
            //connection.Close();


        }
    }
}

