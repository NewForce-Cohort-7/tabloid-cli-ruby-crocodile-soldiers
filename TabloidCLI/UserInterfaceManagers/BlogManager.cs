﻿using System;
using System.Collections.Generic;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class BlogManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private string _connectionString;

        public BlogManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Blog Menu");
            Console.WriteLine(" 1) Add Blog");
            Console.WriteLine(" 2) See All Blogs");
            Console.WriteLine(" 3) Remove a Blog");
            Console.WriteLine(" 4) Edit a Blog");
            Console.WriteLine(" 5) Blog Details");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    AddBlog();
                    return this;
                case "2":
                    ListBlogs();
                    return this;
                case "3":
                    RemoveBlog();
                    return this;
                case "4":
                    EditBlog();
                    return this;
                case "5":
                    Blog blog = ChooseBlog();
                    if (blog == null)
                    {
                        return this;
                    }
                    else
                    {
                        return new BlogDetailManager(this, _connectionString, blog.Id);
                    }
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void AddBlog()
        {
            Console.WriteLine("New Blog");
            Blog blog = new Blog();

            Console.Write("Title: ");
            blog.Title = Console.ReadLine();

            Console.Write("URL: ");
            blog.Url = Console.ReadLine();

            _blogRepository.Insert(blog);
        }

        private void ListBlogs()
        {
            List<Blog> blogs = _blogRepository.GetAll();
            foreach (Blog blog in blogs)
            {
                Console.WriteLine(blog);
            }
        }
        private Blog ChooseBlog(string prompt = null)
        {
            Console.WriteLine(prompt);
            List<Blog> blogs = _blogRepository.GetAll();
            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.Title} ({blog.Url})");
            }
            Console.Write("> ");
            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void EditBlog()
        {
            Blog blogToEdit = ChooseBlog("Which blog would you like to edit?");
            if (blogToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                blogToEdit.Title = title;
            }
            Console.Write("New URL (blank to leave unchanged: ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                blogToEdit.Url = url;
            }
            _blogRepository.Update(blogToEdit);
        }

        private void RemoveBlog()
        {
           Blog blogToDelete = ChooseBlog("Which blog will you remove?");
            if (blogToDelete != null)
            {
                _blogRepository.Delete(blogToDelete.Id);
            }
        }
    }
}
