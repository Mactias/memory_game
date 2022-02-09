using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace memory_game
{
	class Program
	{
		static int Main(string[] args)
		{
			Console.WriteLine("Welcome to the memory game!");
			do
			{
				Console.WriteLine("1. Play Game\n2. Exit");
				ConsoleKeyInfo input = Console.ReadKey();
				if (input.KeyChar == '1') //Play game
				{
					Console.WriteLine("\nSelect difficulty level\n1. Easy\n2. Hard");
					ConsoleKeyInfo difficulty = Console.ReadKey();
					if (difficulty.KeyChar == '1') // Easy difficulty
					{
						Console.Clear();
						new GameEngine("easy", GetWords(4));
					}
					else if (difficulty.KeyChar == '2') // Hard difficulty
					{
						Console.Clear();
						new GameEngine("hard", GetWords(8));
					}
					else
					{
						Console.WriteLine("\nEnter 1 or 2");
					}
				} 
				else if(input.KeyChar == '2') //Exit game
				{
					return 0;
				}
				else
				{
					Console.WriteLine("\nEnter 1 or 2");
				}
			} while (true);
		}

		private static string[] GetWords(int count)
		{
			Random rnd = new Random();
			string path = "Words.txt";
			List<string> all_words = File.ReadAllLines(path).ToList();

			string[] words = new string[count];

			for (int i = 0; i < count; i++)
			{
				int index = rnd.Next(0, all_words.Count);
				words[i] = all_words[index];
				all_words.RemoveAt(index);
			}

			string[] words2 = new string[words.Length * 2];
			words.CopyTo(words2, 0);
			words.CopyTo(words2, words.Length);

			return words2;
		}
	}
}
