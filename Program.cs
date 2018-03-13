using System;

namespace MarkovChainsImplementation
{
	class MainClass
	{
		public static void Main(string[] args)
		{

			Console.WriteLine("Input order: ");
			int order = Convert.ToInt32(Console.ReadLine());

			Markov markov = new Markov(order);

			markov.OutputStateMachines();

			int input;
				
			do
			{
				Console.WriteLine("Input word count for generated sentence (input number lower than the selected order to quit): ");

				input = Convert.ToInt32(Console.ReadLine());

				if (input >= order) { 
					markov.GenerateSentence(input); 
				}
			} while (input >= order);
		}
	}
}
