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
		private string[] board;
		private Dictionary<string, string> boardDic;
		private string[] words;
		private readonly string difficulty;
		public GameEngine(string difficulty, string[] words)
		{
			this.difficulty = difficulty;
			this.words = words;

			if (difficulty == "easy")
			{
				boardDic = new Dictionary<string, string>() 
				{
					{ "A1", "X" }, { "A2", "X" }, { "A3", "X" }, { "A4", "X" },
					{ "B1", "X" }, { "B2", "X" }, { "B3", "X" }, { "B4", "X" },
				};
				board = new string[] { "X", "X", "X", "X", "X", "X", "X", "X"};
				availabeCoordinates = new List<string>() { "A1", "A2", "A3", "A4", "B1", "B2", "B3", "B4" };
				//PrintBoard();
				BeginEasyGame();
			}
		}

		private void BeginEasyGame()
		{
			Console.WriteLine("\n  1 2 3 4");
			Console.WriteLine("A X X X X");
			Console.WriteLine("B X X X X");
			AssignCoordiates();
			bool endgame = false;
			do
			{
				Console.WriteLine("Enter first coordinates: ");
				string input1 = Console.ReadLine().ToUpper();
				if (availabeCoordinates.Contains(input1)) // valid first user input
				{
					Console.WriteLine("input1 = {0}", coordinates[input1]);
					PrintBoard(input1);
					while (true)
					{
						Console.WriteLine("Enter second coordinates: ");
						string input2 = Console.ReadLine().ToUpper();
						if (input1 == input2)
						{
							Console.WriteLine("Second coordinates must be different!");
						}
						else if (availabeCoordinates.Contains(input2)) // valid second user input
						{
							Console.WriteLine("input2 = {0}", coordinates[input2]);
							PrintBoard(input2);
							if (coordinates[input1] == coordinates[input2]) // words match
							{
								Console.WriteLine("inp1 == inp2");
								availabeCoordinates.Remove(input1);
								availabeCoordinates.Remove(input2);
								if (availabeCoordinates.Count == 0)
								{
									Console.WriteLine("Congratulations you win!");
									endgame = true;
								}
								break;
							}
							else
							{
								boardDic[input1] = "X";
								boardDic[input2] = "X";
								break;
							}
						}
						else
						{
							Console.WriteLine("Wrong coordinates!");
						}
					}
				}
				else
				{
					Console.WriteLine("Wrong coordinates!");
				}
				//PrintBoard();
			} while (!endgame);
		}

		private void PrintBoard(string coordinate)
		{
			boardDic[coordinate] = coordinates[coordinate];

			string first_row = "  1";
			string second_row = "A ";
			string third_row = "B ";

			string[] arr =  FormatRows("", "A1", "B1");
			string[] arr2 = FormatRows("2", "A2", "B2");
			string[] arr3 = FormatRows("3", "A3", "B3");
			string[] arr4 = FormatRows("4", "A4", "B4");

			first_row += arr[0] + arr2[0] + arr3[0] + arr4[0];
			second_row += arr[1] + arr2[1] + arr3[1] + arr4[1];
			third_row += arr[2] + arr2[2] + arr3[2] + arr4[2];

			Console.WriteLine(first_row + "\n" + second_row + "\n" + third_row);
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
		private void AssignCoordiates()
		{
			Random rnd = new Random();
			words = words.OrderBy(x => rnd.Next()).ToArray();
			if (difficulty == "easy")
			{
				coordinates.Add("A1", words[0]);
				coordinates.Add("A2", words[1]);
				coordinates.Add("A3", words[2]);
				coordinates.Add("A4", words[3]);
				coordinates.Add("B1", words[4]);
				coordinates.Add("B2", words[5]);
				coordinates.Add("B3", words[6]);
				coordinates.Add("B4", words[7]);
			}

			foreach (var item in coordinates)
			{
				Console.WriteLine($"{item.Key}, {item.Value}");
			}
		}
	}
}
