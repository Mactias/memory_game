using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace memory_game
{
	public class GameEngine
	{
		private System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
		private Dictionary<string, string> coordinates = new Dictionary<string, string>();
		private List<string> availabeCoordinates;
		private Dictionary<string, string> boardDict;
		private string[] words;
		private int guessChancesLeft;
		private readonly int guessChances;
		private readonly string difficulty;
		public GameEngine(string difficulty, string[] words)
		{
			stopWatch.Start();

			this.difficulty = difficulty;
			this.words = words;

			if (difficulty == "easy")
			{
				guessChances = 10;
				guessChancesLeft = 10;

				BeginGame(4);
			}
			else
			{
				guessChances = 15;
				guessChancesLeft = 15;

				BeginGame(8);
			}
		}

		/// <summary>
		/// Fill the fields <c>boardDic</c> and <c>availabeCoordinates</c>
		/// </summary>
		/// <param name="length"></param>
		private void InitializeBoard(int length)
		{
			boardDict = new Dictionary<string, string>();
			availabeCoordinates = new List<string>();

			for (int i = 0; i < 2; i++)
			{
				for (int j = 1; j <= length; j++)
				{
					if (i == 0)
					{
						boardDict.Add("A" + j, "X");
						availabeCoordinates.Add("A" + j);
					}
					else
					{
						boardDict.Add("B" + j, "X");
						availabeCoordinates.Add("B" + j);
					}
				}
			}
		}
		private void BeginGame(int length)
		{
			InitializeBoard(length);
			AssignCoordiates(length);

			bool endgame = false;
			do
			{
				Console.Clear();
				PrintBoard("none");
				Console.WriteLine("Enter first coordinates: ");
				string input1 = Console.ReadLine().ToUpper();
				Console.WriteLine();
				if (availabeCoordinates.Contains(input1)) // valid first user input
				{
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
							PrintBoard(input2);
							if (coordinates[input1] == coordinates[input2]) // words match
							{
								availabeCoordinates.Remove(input1);
								availabeCoordinates.Remove(input2);
								if (availabeCoordinates.Count == 0) // no more moves. Player wins. Exit to menu
								{
									stopWatch.Stop();
									Console.Clear();

									Console.WriteLine("Congratulations you win!");
									Console.WriteLine($"You solved the memory game after {guessChances - guessChancesLeft} chances. " 
														+ $"It took you {stopWatch.Elapsed.TotalSeconds} seconds");
									SaveScore();
									PrintScore();
									Console.WriteLine("What do you want to do now?");
									endgame = true;
								}
								break;
							}
							else // words doesn't match
							{
								boardDict[input1] = "X";
								boardDict[input2] = "X";
								guessChancesLeft--;
								if (guessChancesLeft <= 0) // Game over and exit to menu
								{
									PrintScore();
									Console.WriteLine("You lost. What do you want to do now?");
									endgame = true;
								}
								else
								{
									Console.WriteLine("Press any key.");
									Console.ReadKey();
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
					Console.WriteLine("Wrong coordinates! Press any key.");
					Console.ReadKey();
				}
			} while (!endgame);
		}

		/// <summary>
		/// Print actual board to the console.
		/// </summary>
		/// <param name="coordinate">ex. "A1", "B3" or "none"</param>
		private void PrintBoard(string coordinate)
		{
			Console.WriteLine("- - - - - - - - - -");
			Console.WriteLine("Level: {0}", difficulty);
			Console.WriteLine("Guess Chances: {0}\n", guessChancesLeft);

			if (coordinate != "none")
			{
				boardDict[coordinate] = coordinates[coordinate];
			}

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

			if (difficulty == "hard")
			{
				string[] arr5 = FormatRows("5", "A5", "B5");
				string[] arr6 = FormatRows("6", "A6", "B6");
				string[] arr7 = FormatRows("7", "A7", "B7");
				string[] arr8 = FormatRows("8", "A8", "B8");

				first_row += arr5[0] + arr6[0] + arr7[0] + arr8[0];
				second_row += arr5[1] + arr6[1] + arr7[1] + arr8[1];
				third_row += arr5[2] + arr6[2] + arr7[2] + arr8[2];
			}

			Console.WriteLine(first_row + "\n" + second_row + "\n" + third_row + "\n");
		}

		/// <summary>
		/// Format board's rows add spaces to them. All parameters refer to the same column! 
		/// ex. if number is "2" x1 must be "A2" and x2 "B2"
		/// </summary>
		/// <param name="number">First row coordinate</param>
		/// <param name="x1">Second row coordinate ex. "A1"..."A8"</param>
		/// <param name="x2">Second row coordinate ex. "B1"..."B8"</param>
		/// <returns></returns>
		private string[] FormatRows(string number, string x1, string x2)
		{
			if (boardDict[x1].Length > boardDict[x2].Length)
			{
				int diff = boardDict[x1].Length - boardDict[x2].Length;
				string first_row = number + new string(' ', boardDict[x1].Length);
				string second_row = boardDict[x1] + " ";
				string third_row = boardDict[x2] + new string(' ', diff + 1);

				string[] ret = { first_row, second_row, third_row };

				return ret;
			}
			else
			{
				int diff = boardDict[x2].Length - boardDict[x1].Length;
				string first_row = number + new string(' ', boardDict[x2].Length);
				string second_row = boardDict[x1] + new string(' ', diff + 1);
				string third_row = boardDict[x2] + " ";

				string[] ret = { first_row, second_row, third_row };

				return ret;
			}
		}

		/// <summary>
		/// Assigns words to the field <c>coordinates<c>
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
		}

		/// <summary>
		/// If you get a high score then save your score to the file.
		/// </summary>
		private void SaveScore()
		{
			string path = $"high_score_{difficulty}.txt";
			if (!File.Exists(path))
			{
				string score = EnterScore();
				using (StreamWriter sw = File.CreateText(path))
				{
					sw.WriteLine(score);
				}
			}
			else
			{
				List<string> lines = File.ReadAllLines(path).ToList();
				int user_tries = guessChances - guessChancesLeft;
				double user_time = stopWatch.Elapsed.TotalSeconds;
				for (int i = 0; i < lines.Count; i++)
				{
					string[] line = lines[i].Split('|');
					int el_tries = Int32.Parse(line[3]);
					double el_time = Double.Parse(line[2]);
					if (user_tries < el_tries) // compare tries
					{
						string score = EnterScore();
						lines.Insert(i, score);
						if (lines.Count > 10) // if lines > 10 delete last line
						{
							lines.RemoveAt(lines.Count - 1);
						}
						File.WriteAllLines(path, lines);
						return;
					}
					else if (user_tries == el_tries && user_time < el_time) // compare time
					{
						string score = EnterScore();
						lines.Insert(i, score);
						if (lines.Count > 10) // if lines > 10 delete last line
						{
							lines.RemoveAt(lines.Count - 1);
						}
						File.WriteAllLines(path, lines);
						return;
					}
				}
				if (lines.Count < 10) // Add score to last line - if file is not empty && has < 10 lines && score is lower.
				{
					string score = EnterScore();
					lines.Add(score);
					File.WriteAllLines(path, lines);
				}
			}
		}

		/// <summary>
		/// Save your name to the score.
		/// </summary>
		/// <returns>Return your score</returns>
		private string EnterScore()
		{
			Console.WriteLine("\nEnter your name: ");
			string name = Console.ReadLine();
			DateTime today = DateTime.Today;
			string score = $"{name} | {today.ToShortDateString()} | {stopWatch.Elapsed.TotalSeconds} " +
							$"| {guessChances - guessChancesLeft} |";

			return score;
		}

		/// <summary>
		/// Print High Score to the console
		/// </summary>
		private void PrintScore()
		{
			string path = $"high_score_{difficulty}.txt";
			string text = File.ReadAllText(path);
			Console.WriteLine($"- - - High Score - - -\n{text}\n");
		}
	}
}
