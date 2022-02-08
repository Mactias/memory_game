using System;
using System.Collections.Generic;
using System.Linq;

namespace memory_game
{
	public class GameEngine
	{
		private Dictionary<string, string> coordinates = new Dictionary<string, string>();
		private Dictionary<string, string> guessedCoordinates = new Dictionary<string, string>();
		private List<string> availabeCoordinates;
		private Dictionary<string, string> boardDic;
		private string[] words;
		private readonly string difficulty;
		private int guessChances;
		public GameEngine(string difficulty, string[] words)
		{
			this.difficulty = difficulty;
			this.words = words;

			if (difficulty == "easy")
			{
				guessChances = 10;

				//InitializeBoard(4);
				BeginEasyGame(4);
			}
			else
			{
				guessChances = 15;
				//boardDic = new Dictionary<string, string>()
				//{
				//	{ "A1", "X" }, { "A2", "X" }, { "A3", "X" }, { "A4", "X" },
				//	{ "A5", "X" }, { "A6", "X" }, { "A7", "X" }, { "A8", "X" },
				//	{ "B1", "X" }, { "B2", "X" }, { "B3", "X" }, { "B4", "X" },
				//	{ "B5", "X" }, { "B6", "X" }, { "B7", "X" }, { "B8", "X" },
				//};
				//availabeCoordinates = new List<string>() { "A1", "A2", "A3", "A4", "A5", "A6", "A7", "A8",
				//										   "B1", "B2", "B3", "B4", "B5", "B6", "B7", "B8" };
				BeginEasyGame(8);
			}
		}

		private void InitializeBoard(int length)
		{
			boardDic = new Dictionary<string, string>();
			availabeCoordinates = new List<string>();

			for (int i = 0; i < 2; i++)
			{
				for (int j = 1; j <= length; j++)
				{
					if (i == 0)
					{
						boardDic.Add("A" + j, "X");
						availabeCoordinates.Add("A" + j);
					}
					else
					{
						boardDic.Add("B" + j, "X");
						availabeCoordinates.Add("B" + j);
					}
				}
			}
		}
		private void BeginEasyGame(int length)
		{
			InitializeBoard(length);
			AssignCoordiates(length);
			PrintBoard(length);

			bool endgame = false;
			do
			{
				Console.WriteLine("Enter first coordinates: ");
				string input1 = Console.ReadLine().ToUpper();
				Console.WriteLine();
				if (availabeCoordinates.Contains(input1)) // valid first user input
				{
					//Console.WriteLine("input1 = {0}", coordinates[input1]);
					PrintBoard(input1);
					while (true)
					{
						Console.WriteLine("Enter second coordinates: ");
						string input2 = Console.ReadLine().ToUpper();
						Console.WriteLine();
						if (input1 == input2) // the same inputs ex. "A1" and "A1"
						{
							Console.WriteLine("Second coordinate must be different!");
						}
						else if (availabeCoordinates.Contains(input2)) // valid second user input
						{
							//Console.WriteLine("input2 = {0}", coordinates[input2]);
							PrintBoard(input2);
							if (coordinates[input1] == coordinates[input2]) // words match
							{
								//Console.WriteLine("inp1 == inp2");
								availabeCoordinates.Remove(input1);
								availabeCoordinates.Remove(input2);
								if (availabeCoordinates.Count == 0) // no more moves. Player wins. Exit to menu
								{
									Console.WriteLine("Congratulations you win!");
									endgame = true;
								}
								break;
							}
							else // words doesn't match
							{
								boardDic[input1] = "X";
								boardDic[input2] = "X";
								guessChances--;
								if (guessChances <= 0) // Game over and exit to menu
								{
									Console.WriteLine("You lost.");
									endgame = true;
								}
								break;
							}
						}
						else // wrong second input ex. "123", "a11", "xxxxx" etc.
						{
							Console.WriteLine("Wrong coordinates!");
						}
					}
				}
				else // wrong first input ex. "123", "a11", "xxxxx" etc.
				{
					Console.WriteLine("Wrong coordinates!");
				}
				//PrintBoard();
			} while (!endgame);
		}

		private void PrintBoard(int length)
		{
			Console.WriteLine("\n- - - - - - - - - -");
			Console.WriteLine("Level: {0}", difficulty);
			Console.WriteLine("Guess Chances: {0}", guessChances);

			string firstLine = "\n  ";
			string secondLine = "A ";
			string thirdLine = "B ";
			for (int i = 1; i <= length; i++)
			{
				firstLine += i + " ";
				secondLine += "X ";
				thirdLine += "X ";
			}
			Console.WriteLine(firstLine + "\n" + secondLine + "\n" + thirdLine + "\n");

		}
		private void PrintBoard(string coordinate)
		{
			Console.WriteLine("- - - - - - - - - -");
			Console.WriteLine("Level: {0}", difficulty);
			Console.WriteLine("Guess Chances: {0}\n", guessChances);

			boardDic[coordinate] = coordinates[coordinate];

			string first_row = "  1";
			string second_row = "A ";
			string third_row = "B ";

			string[] arr = FormatRows("", "A1", "B1");
			string[] arr2 = FormatRows("2", "A2", "B2");
			string[] arr3 = FormatRows("3", "A3", "B3");
			string[] arr4 = FormatRows("4", "A4", "B4");

			first_row += arr[0] + arr2[0] + arr3[0] + arr4[0];
			second_row += arr[1] + arr2[1] + arr3[1] + arr4[1];
			third_row += arr[2] + arr2[2] + arr3[2] + arr4[2];

			Console.WriteLine(first_row + "\n" + second_row + "\n" + third_row + "\n");
		}

		private string[] FormatRows(string number, string x1, string x2)
		{
			//int diff = boardDic[x1].Length - boardDic[x2].Length;
			if (boardDic[x1].Length > boardDic[x2].Length)
			{
				int diff = boardDic[x1].Length - boardDic[x2].Length;
				string first_row = number + new string(' ', boardDic[x1].Length);
				string second_row = boardDic[x1] + " ";
				string third_row = boardDic[x2] + new string(' ', diff + 1);

				string[] ret = { first_row, second_row, third_row };

				return ret;
			}
			else
			{
				int diff = boardDic[x2].Length - boardDic[x1].Length;
				string first_row = number + new string(' ', boardDic[x2].Length);
				string second_row = boardDic[x1] + new string(' ', diff + 1);
				string third_row = boardDic[x2] + " ";

				string[] ret = { first_row, second_row, third_row };

				return ret;
			}
			//int diff = boardDic[x1].Length > boardDic[x2].Length ? boardDic[x1].Length - boardDic[x2].Length : boardDic[x2].Length - boardDic[x1].Length;
		}

		/// <summary>
		/// Method <c>AssignCoordinates</c> assign words to the field <c>coordinates<c>
		/// </summary>
		/// <param name="length">
		/// sets length of the <c>coordinates</c>.Length must be 4 for easy game or 8 for hard game
		/// </param>
		private void AssignCoordiates(int length)
		{
			Random rnd = new Random();
			words = words.OrderBy(x => rnd.Next()).ToArray();

			int index = 0;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 1; j <= length; j++)
				{
					if (i == 0)
					{
						coordinates.Add("A" + j, words[index]);
					}
					else
					{
						coordinates.Add("B" + j, words[index]);
					}
					index++;
				}
			}

			foreach (var item in coordinates)
			{
				Console.WriteLine($"{item.Key}, {item.Value}");
			}
		}
	}
}
