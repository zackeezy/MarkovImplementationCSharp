using System;
using System.Collections.Generic;
using System.IO;
using StateMachine = System.Collections.Generic.Dictionary<System.Collections.Generic.List<System.String>, 
	System.Collections.Generic.List<System.String>>;

namespace MarkovChainsImplementation
{
	public class Markov
	{
		List<String> fileNames;

		StateMachine stateMachines;

		int _order;

		public Markov(int order)
		{
			fileNames = DirSearch(Directory.GetCurrentDirectory() + "/Input");
			stateMachines = GenerateStateMachines(order, false);
			_order = order;
		}

		StateMachine GenerateStateMachines(int order, bool trimPunctuation = true)
		{
			StateMachine sm = new StateMachine();

			List<List<String>> files = new List<List<string>>();

			foreach (string fileName in fileNames)
			{
				StreamReader file = new StreamReader(fileName);
				List<String> lines = new List<string>();
				do
				{
					string line = file.ReadLine();
					if (!string.IsNullOrEmpty(line.Trim()))
					{
						lines.Add(line);
					}
				}
				while (file.Peek() != -1);
				files.Add(lines);
			}

			foreach (List<String> file in files)
			{
				List<String> wordsList = new List<string>();

				foreach (String line in file)
				{
					String[] words = line.Split(' ');
					foreach (String word in words)
					{
						String temp = word.Trim();

						if (trimPunctuation)
						{
							while (!char.IsLetter(temp[0]))
							{
								temp = temp.Substring(1);
							}
							while (!char.IsLetter(temp[temp.Length - 1]))
							{
								temp = temp.Substring(1, temp.Length - 1);
							}
						}

						wordsList.Add(temp);
					}
				}
				
				for (int i = 0; i < wordsList.Count - order - 1; i++)
				{
					List<String> key = new List<string>();
					for (int j = 0; j < order; j++)
					{
						key.Add(wordsList[i + j]);
					}
					AddState(key, wordsList[i + order], sm);
				}
			}

			return sm;
		}

		public void OutputStateMachines()
		{
			var keys = stateMachines.Keys;
			foreach (var key in keys)
			{
				String output = "Keys: ";
				foreach (var s in key)
				{
					output += s;
					output += " ";
				}
				output += "Value: ";
				foreach (var s in stateMachines[key])
				{
					output += s;
					output += " ";
				}
				Console.WriteLine(output);
			}
		}

		private List<String> DirSearch(string sDir)
		{
			List<String> files = new List<String>();
			try
			{
				foreach (string f in Directory.GetFiles(sDir))
				{
					files.Add(f);
				}
				foreach (string d in Directory.GetDirectories(sDir))
				{
					files.AddRange(DirSearch(d));
				}
			}
			catch (System.Exception excpt)
			{
				Console.WriteLine(excpt.Message);
			}

			return files;
		}

		private void AddState(List<String> key, String value, StateMachine sm)
		{
			var keys = sm.Keys;
			foreach (List<String> k in keys)
			{
				if (k.TrueForAll(str => key.Contains(str)))
				{
					sm[k].Add(value);
					return;
				}
			}
			sm[key] = new List<string>();
			sm[key].Add(value);
		}
	
		public void GenerateSentence(int words)
		{
			if (words == 0) return;
			Random r = new Random();
			List<List<String>> keys = new List<List<string>>();
			foreach (var key in stateMachines.Keys) keys.Add(key);
			int start = r.Next(keys.Count - 1);

			List<string> beginKey = keys[start];

			beginKey.ForEach(s => Console.Write(s + " "));
			List<String> keyTemp = new List<string>();
			beginKey.ForEach(s => keyTemp.Add(s));
			for (int i = 0; i < words - _order; i++)
			{
				List<String> values = new List<string>();
				foreach (var k in stateMachines.Keys)
				{
					if (k.TrueForAll(s => keyTemp.Contains(s)))
					{
						values = stateMachines[k];
					}
				}
				if (values.Count == 0) {
					Console.Write(Environment.NewLine + "Values list obtained is empty.  Aborted." + Environment.NewLine);
					return; 
				}
				int next = r.Next(values.Count);
				string nextWord = values[next];
				Console.Write(nextWord + " ");
				keyTemp.Add(nextWord);
				keyTemp.RemoveAt(0);
			}
			Console.Write(Environment.NewLine);
		}
	}
}
