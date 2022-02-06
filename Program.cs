using System;

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
				if (input.KeyChar == '1') //Start game
				{
					Console.WriteLine("playing game");
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
	}
}
