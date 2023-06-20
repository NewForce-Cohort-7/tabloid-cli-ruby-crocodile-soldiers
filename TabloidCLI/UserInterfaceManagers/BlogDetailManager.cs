using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class BlogDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private TagRepository _tagRepository;
        //private NoteRepository _noteRepository;
        private string _connectionString;
        private int _blogId;

        public BlogDetailManager(IUserInterfaceManager parentUI, string connectionString, int postId)
        {
            _parentUI = parentUI;
            _blogRepository = new BlogRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);
            //_noteRepository = new NoteRepository(connectionString);
            _connectionString = connectionString;
            _blogId = blogId;
        }

        public IUserInterfaceManager Execute()
        {
            Post post = _blogRepository.Get(_blogId);
            Console.WriteLine($"{blog.Title} Details");
            Console.WriteLine(" 1) View");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Remove Tag");
            Console.WriteLine(" 4) View Posts");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    View();
                    return this;
                case "2":
                    throw new Exception();
                case "3":
                    throw new Exception();
                case "4":
                    throw new Exception();
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void View()
        {
            Post post = _blogRepository.Get(_blogId);
            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"URL: {post.Url}");
            Console.WriteLine($"Published: {blog.PublishDateTime}");
            Console.WriteLine("Tags:");

        }
    }
}
//View

//Add Tag

//Remove Tag

//View Posts

//Return