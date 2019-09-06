using System;
//I am byron
namespace BigBadBolts_
{
    class Program
    {
        static void Main(string[] args)
        {
            bool exitProgram = false;
            string userInput;

            Console.WriteLine("Welcome to CSCI 473 Assignment 1.");

            while (exitProgram == false)
            {
                userInput = "";

                Console.WriteLine("Please select an option by typing the number and hitting enter. \n");
                Console.WriteLine("1. List All Subreddits ");
                Console.WriteLine("2. List all Posts from All Subreddits");
                Console.WriteLine("3. List All Posts from a Single Subreddit ");
                Console.WriteLine("4. View Comments From a Single Post");
                Console.WriteLine("5. Add Comment to Post ");
                Console.WriteLine("6. Add Reply to Comment");
                Console.WriteLine("7. Create New Post ");
                Console.WriteLine("8. Delete Post");
                Console.WriteLine("9. Quit ");

                userInput = Console.ReadLine();
                Console.Clear();

                //Error check the user input
                if (userInput.Length == 1 || userInput.ToLower() == "quit" || userInput.ToLower() == "exit") // determines if the user enter acceptable criteria
                {
                    if (userInput.ToLower() == "quit" || userInput.ToLower() == "exit")// handle the long word cases
                    {
                        exitProgram = true;
                    }
                    else if( userInput.ToLower() == "q" || userInput.ToLower() == "e")//handles the special single character exceptions
                    {
                        exitProgram = true;
                    }
                    else if(Char.IsDigit(userInput[0]))// Determines the function to call
                    {
                        switch (userInput)
                        {
                            case ("0"):  //Error, reask to enter a new option
                                Console.WriteLine("You have entered something incorrect. Please try again. \n");
                                break;
                            case ("1"):  //List all subreddits
                                break;
                            case ("2"):  //List all posts from all subreddits
                                break;
                            case ("3"):  //List all posts from a single subreddit
                                break;
                            case ("4"):  //View comments of a single post
                                break;
                            case ("5"):  //Add comment to post
                                break;
                            case ("6"):  //Add reply to comment
                                break;
                            case ("7"):  //Create new post
                                break;
                            case ("8"): //Deletes post
                                break;
                            case ("9"): //Quits
                                exitProgram = true;
                                break;
                            default:
                                Console.WriteLine("You have entered something incorrect. Please try again. \n");
                                break;

                        }//end switch
                    }
                    else //entered a single character that is not what we need
                    {
                        Console.WriteLine("You have entered something incorrect. Please try again. \n");
                    }
                }//End inner If
                else //The user entered something wrong, restart the loop.
                {
                    Console.WriteLine("You have entered something incorrect. Please try again. \n");
                }

            }
          
        }
    }
}
