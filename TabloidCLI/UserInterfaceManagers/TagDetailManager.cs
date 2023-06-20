using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    internal class TagDetailManager : IUserInterfaceManager
    {
        private IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;
        private PostRepository _postRepository;
        private BlogRepository _blogRepository;
        private AuthorRepository _authorRepository;

        private string _connectionString;
        private int _tagId;

        public TagDetailManager(IUserInterfaceManager parentUI, string connectionString, int tagId)
        {
            _parentUI = parentUI;
            _tagRepository = new TagRepository(connectionString);
            _postRepository = new PostRepository(connectionString);
            _blogRepository = new BlogRepository(connectionString);
            _authorRepository = new AuthorRepository(connectionString);

            _connectionString = connectionString;
            _tagId = tagId;
        }

        public IUserInterfaceManager Execute()
        {
            Tag tag = _tagRepository.Get(_tagId);
            Console.WriteLine($"{tag.Name} Details");
            Console.WriteLine(" 1) View");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Remove Tag");
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
            Tag tag = _tagRepository.Get(_tagId);
            Console.WriteLine($"Name: {tag.Name}");
            Console.WriteLine("Tags:");

        }
    }
}
        