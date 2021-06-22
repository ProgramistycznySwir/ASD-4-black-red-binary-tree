

// Author: FreeDOOM#4231 on Discord


using System;
using System.IO;
using System.Collections.Generic;

namespace ASD___4
{
	class Program
	{
		public const int N = 1_000_000;
		public const int k = 30_000;

		readonly static string[] _fileName_ = new string[] {
			"_test.tr33",		    // 0
			"_generatedTest.txt",  // 1
			"_1_Pietrzeniuk.txt", // 2
			"Abonents.3"         // 3
			};

		static void Main(string[] args)
		{
			//Generate3From("Abonents.txt").SaveToFile_ByLevel(_fileName_[3]);
			KsiążkaTelefoniczna.MainMenu();
			//BRTree.Testing();
		}

		/// <summary>
		/// Generates Tree from file.
		/// </summary>
		static BRTree.Tree Generate3From(string fileName, bool writeToConsole = false)
        {
			var newTree = new BRTree.Tree();

			Console.WriteLine($">>>Started generating 3 from file \"{fileName}\"...");

			if (!File.Exists(fileName))
				throw new FileNotFoundException($"There is no file {fileName} in program directory.");

			string[] lines = File.ReadAllLines(fileName);

			var rng = new Random();
			int newNumbersCount;
			int[] newNumbers;

			string[] line;
			Data_FreeAccess newData = new Data_FreeAccess();
			for (int i = 0; i < lines.Length; i++)
			{
				line = lines[i].Split(';');
				newData.surname = line[0];
				newData.name = line[1];
				newData.address = line[2];
				newNumbersCount = rng.Next(1, 4);
				newNumbers = new int[newNumbersCount];
				for (int ii = 0; ii < newNumbersCount; ii++)
					newNumbers[ii] = rng.Next(0, 1_000_000_000);

				newData.phoneNumbers = newNumbers;

				if(writeToConsole)
					Console.WriteLine(newData);

				newTree.Insert(new Data(newData));
			}

			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($">>>Compleated generating 3 from file \"{fileName}\"!");
			Console.ForegroundColor = ConsoleColor.Gray;

			return newTree;
		}
	}

	/// <summary>
	/// Stores data... duh...
	/// </summary>
	public struct Data
	{
		public readonly string surname;
		public readonly string name;
		public readonly string address;
		// Limit: 256
		public int[] phoneNumbers;

		public static Data Clear => new Data("Not found", "", "", null);

		public Data(string surname, string name, string address, int[] phoneNumbers)
		{
			this.surname = surname;
			this.name = name;
			this.address = address;
			this.phoneNumbers = phoneNumbers;
		}
		public Data(Data_FreeAccess data)
		{
			this.surname = data.surname;
			this.name = data.name;
			this.address = data.address;
			this.phoneNumbers = data.phoneNumbers;
		}

		public static int Compare(Data a, Data b)
		{
			int result = a.surname.CompareTo(b.surname);
			if (result != 0)
				return result;

			result = a.name.CompareTo(b.name);
			if (result != 0)
				return result;
			return a.address.CompareTo(b.address);
		}
		// "b" to "a" => 1
		public int CompareTo(Data a)
			=> Compare(this, a);
		// "b" > "a"
		public static bool operator >(Data a, Data b)
			=> Compare(a, b) > 0;
		public static bool operator <(Data a, Data b)
			=> Compare(a, b) < 0;

        public override string ToString()
			=> $"{surname}; {name}; {address}; {{{Useful.ArrayToString<int>(phoneNumbers)}}}";
		public void WriteToConsole()
        {
			Console.WriteLine($"{surname} {name}");
			Console.WriteLine($"{address}");
			Console.WriteLine($"{{{Useful.ArrayToString<int>(phoneNumbers, true)}}}");
        }
    }

	public struct Data_FreeAccess
    {
		// Limit: 256
		public string surname;
		// Limit: 256
		public string name;
		// Limit: 256
		public string address;
		// Limit: 16
		public int[] phoneNumbers;

		public Data_FreeAccess(string surname, string name, string address, int[] phoneNumbers)
		{
			this.surname = surname;
			this.name = name;
			this.address = address;
			this.phoneNumbers = phoneNumbers;
		}

		//public Data Lock()
		//      {
		//	return new Data(surname, name, address, phoneNumbers);
		//      }
		public override string ToString()
			=> $"{surname}; {name}; {address}; {{{Useful.ArrayToString<int>(phoneNumbers)}}}";
		public void WriteToConsole()
		{
			Console.WriteLine($"{surname} <> {name}");
			Console.WriteLine($"{address}");
			Console.WriteLine($"{{{Useful.ArrayToString<int>(phoneNumbers, true)}}}");
		}
	}

	public static class Useful
	{
		public static string ArrayToString<T>(T[] array, bool newLines = false)
		{
			if (array == null || array.Length < 1)
				return "";
			var builder = new System.Text.StringBuilder();
			builder.Append(array[0]);
			for (int i = 1; i < array.Length; i++)
				builder.Append($", {array[i]}{(newLines ? "\n" : "")}");
			return builder.ToString();
		}
	}
}
