﻿using System;
using System.Collections.Generic;
using System.Globalization;
using TabloidCLI.Models;

namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Journal");
            Console.WriteLine(" 2) Add Journal");
            Console.WriteLine(" 3) Edit Journal");
            Console.WriteLine(" 4) Delete Journal Entry");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                //case "2":
                    //Journal journal = Choose();
                    //if (journal == null)
                    //{
                    //    return this;
                    //}
                    //else
                    //{
                    //    return new JournalDetailManager(this, _connectionString, journal.Id);
                    //}
                case "2":
                    Add();
                    return this;
                case "3":
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;

                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private void List()
        {
            List<Journal> journals = _journalRepository.GetAll();
            foreach (Journal journal in journals)
            {
                Console.WriteLine($"{journal.Title}");
            }
        }

        private Journal Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Journal:";
            }

            Console.WriteLine(prompt);

            List<Journal> journals = _journalRepository.GetAll();

            for (int i = 0; i < journals.Count; i++)
            {
                Journal journal = journals[i];
                Console.WriteLine($" {i + 1}) {journal.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return journals[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        private void Add()
        {
            Console.WriteLine("New Journal");
            Journal journal = new Journal();

            Console.Write("Title: ");
            journal.Title = Console.ReadLine();

            Console.Write("Content: ");
            journal.Content = Console.ReadLine();

    
          
           journal.CreateDateTime = DateTime.Now;





            _journalRepository.Insert(journal);
        }

        private void Edit()
        {
            Journal journalToEdit = Choose("Which journal would you like to edit?");
            if (journalToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New title (blank to leave unchanged: ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                journalToEdit.Title = title;
            }
            Console.Write("New entry (blank to leave unchanged: ");
            string entry = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(entry))
            {
                journalToEdit.Content = entry;
            }
         

            _journalRepository.Update(journalToEdit);
        }
        private void Remove()
        {
            Journal journalToDelete = Choose("Which entry would you like to remove?");
            if (journalToDelete != null)
            {
                _journalRepository.Delete(journalToDelete.Id);
            }
        }

    }

}

