using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hangman
{
    class Program
    {
        private static void Main(string[] args)
        {
            WelcomeMessage();
            string wordToGuess = GenerateAnswer();
            int numberOfGuesses = 10;
            bool won = false;

            char[] progressChar = new char[wordToGuess.Length];
            for (int i = 0; i < progressChar.Length; i++)
            {
                progressChar[i] = '_';
            }

            // Initiating with space to avoid 'Null' output incase first guess = correct letter
            StringBuilder incorrectGuesses = new StringBuilder(" ");
            List<string> previousGuesses = new List<string>();


            while (!won && numberOfGuesses != 0)
            {
                Console.WriteLine("Hangman ({0}): {1}", wordToGuess.Length, new string(progressChar));
                string userGuess = GetValidatedUserGuess(wordToGuess);

                // player try to guess the word
                if (userGuess.Length == wordToGuess.Length)
                {
                    if (userGuess == wordToGuess)
                    {
                        won = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Bold but wrong!");
                        numberOfGuesses--;
                        AfterGuessSummery(incorrectGuesses, numberOfGuesses, progressChar);
                    }
                }
                else
                {
                    // Following statements:
                    // Player guess a previously guessed letter
                    // Player guess a new & correct letter
                    // Player guess a new & incorrect letter
                    if (previousGuesses.Contains(userGuess))
                    {
                        Console.WriteLine("You've already tried {0}", userGuess);
                        AfterGuessSummery(incorrectGuesses, numberOfGuesses, progressChar);
                    }
                    else if (wordToGuess.Contains(userGuess))
                    {
                        Console.WriteLine("Nice guess!", userGuess);
                        previousGuesses.Add(userGuess);
                        progressChar = FillProgress(progressChar, wordToGuess, userGuess);
                        numberOfGuesses--;
                        AfterGuessSummery(incorrectGuesses, numberOfGuesses, progressChar);
                    }
                    else
                    {
                        Console.WriteLine("'{0}' is not part of the word", userGuess);
                        incorrectGuesses.Append(" " + userGuess);
                        previousGuesses.Add(userGuess);
                        numberOfGuesses--;
                        AfterGuessSummery(incorrectGuesses, numberOfGuesses, progressChar);
                    }
                }
            }
            if (won)
                Console.WriteLine("\nYOU WON !\tAnswer was {0} and it took you {1} guesses", wordToGuess, numberOfGuesses);
            else
                Console.WriteLine("\nYou lost, word to guess was '{0}'", wordToGuess);

            Console.WriteLine("\nPress any key to exit");
            Console.ReadKey();
        }

        // Update representation array so a correct guess make theat letter position(s) visible.
        private static char[] FillProgress(char[] progressChar, string wordToGuess, string userGuess)
        {
            char temp = char.Parse(userGuess);
            foreach ((char value, int i) item in wordToGuess.Select((value, i) => (value, i)))
            {
                char value = item.value;
                int index = item.i;    

               if (value == temp)
                {
                    progressChar[index] = value;
                } 
                continue;
            }
            return progressChar;
        }
        private static string GetValidatedUserGuess(string correctWord)
            {
                bool validGuess = false;
                string userGuess = "";

                do
                {
                    Console.Write("Guess: ");
                    string inputFromUser = Console.ReadLine().ToUpper();

                    if (inputFromUser.All(c => Char.IsLetter(c)) && inputFromUser.Length == 1 || inputFromUser.Length == correctWord.Length)
                    {
                        userGuess = inputFromUser;
                        validGuess = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Not a valid guess (will not count as a guess).");
                    }
                }
                while (!validGuess);

                return userGuess;
            }
        private static string GenerateAnswer()
        {
            string[] listOfWords = System.IO.File.ReadAllText(@"C:\StringTest\words.txt").Split(',');

            Random random = new Random((int)DateTime.Now.Ticks);
            string wordToGuess = listOfWords[random.Next(0, listOfWords.Length)];

            return wordToGuess;
        }
        private static void AfterGuessSummery(StringBuilder incorrectGuesses, int numberOfGuesses, char[] progressChar)
        {
            Console.WriteLine("\nIncorrect letters:{0} - [Guesses left: {1}]", incorrectGuesses, numberOfGuesses);
        }
        // The nerd within me
        private static void WelcomeMessage()
        {
            Console.Clear();
            Console.WriteLine("           __  __");
            Console.WriteLine("    v1.0  / / / /___ _____  ____ _____ ___  ____ _____ ");
            Console.WriteLine("         / /_/ / __ `/ __ \\/ __ `/ __ `__ \\/ __ `/ __ \\");
            Console.WriteLine("        / __  / /_/ / / / / /_/ / / / / / / /_/ / / / /");
            Console.WriteLine("       /_/ /_/\\__,_/_/ /_/\\__, /_/ /_/ /_/\\__,_/_/ /_/");
            Console.WriteLine("                         /____/\n");
            Console.WriteLine(" [+]  You have 10 guesses to guess the correct the word.");
            Console.WriteLine(" [+]  You can guess either a letter or guess the word.");
            Console.WriteLine(" [+]  Unknown letters in the word are displayed as _");
            Console.WriteLine(" [+]  Guessing a letter that occur more than once in the word,");
            Console.WriteLine("      will reveal all of its positions.\n");
            Console.WriteLine("                       Good luck!\n");
            Console.ReadKey();
        }
    }
}

