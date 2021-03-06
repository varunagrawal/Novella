﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Novella
{
    class BookModel
    {
        public static ObservableCollection<Book> Books { get; set; }

        public static async Task<ObservableCollection<Book>> GetBooksList()
        {
			try
			{
				Books = await GetBooks();

				return Books;
			}
			catch (Exception ex)
			{
				throw ex;
			}
            
        }

        public static async Task<ObservableCollection<Book>> GetBooks()
        {
            ObservableCollection<Book> books = new ObservableCollection<Book>();

			try
			{
				// Get the file.
				StorageFolder booksFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
				var file = await booksFolder.GetFileAsync("BooksList.txt");

				IList<string> lines = await Windows.Storage.FileIO.ReadLinesAsync(file);

				foreach (var l in lines)
				{
					Book book = new Book();

					var details = l.Split(':');

					book.Name = details[0];
					book.FileName = details[1];
					book.Cover = @"/BookCovers/" + details[2];

					books.Add(book);
				}

				return books;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				throw ex;
			}
			
        }

		public static async void SetBooksList(ObservableCollection<Book> list)
		{
			try
			{
				// Get the file.
				StorageFolder booksFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
				var file = await booksFolder.GetFileAsync("BooksList.txt");

				IList<string> lines = new List<string>();

				foreach (Book book in list)
				{
					string s = string.Format("{0}:{1}:{2}", book.Name, book.FileName, book.Cover.Substring(11));					
					lines.Add(s);
				}

				await Windows.Storage.FileIO.WriteLinesAsync(file, lines);

			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				throw ex;
			}
		}
    }
}
