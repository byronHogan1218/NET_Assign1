using System;
using System.Collections.Generic;
/**
 * NOTES TO MY PARTNER: i wrote a helper function for profanity checking. not sure if you need it but to call it you say
 * vulgarityChecker("SOME STRING");  //will return true if profanity is found
 */
namespace BigBadBolts_
{
    class Program
    {
        static public List<string> BANNED_WORDS = new List<string>()
        {
            "fudge","shoot","baddie","butthead"
        };


        /**
         * This is the class definition for the Post class. 
         * Created by Byron. 
         */
        public class Post
        {
            private readonly uint postId;
            private string title;
            private readonly uint authorID;
            private string postContent;
            private readonly uint subHome;
            private uint upVotes;
            private uint downVotes;
            private uint weight;
            private readonly DateTime timeStamp;
            private SortedSet<Comment> postComments;



            public uint Score
            {
                get { return upVotes - downVotes; }
            }

            public uint PostRating
            {
                get
                {
                    if (weight == 0)
                    {
                      return Score; 
                    }
                    else if (weight == 1)
                    {
                        double returnValue = (double)Score * .66;
                        return (uint)returnValue;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }

            public string PostContent
            {
                get { return postContent; }
                set
                {
                    if (value.Length >= 1 && value.Length <= 1000)
                    {
                        try
                        {
                            if (vulgarityChecker(value))//If true, found profanity
                            {
                                throw new FoulLanguageException();
                            }
                            else //did not find profanity
                            {
                                postContent = value;
                            }
                        }
                        catch (FoulLanguageException fle)
                        {
                            Console.WriteLine(fle.ToString());
                            return;//THIS MIGHT TAKE US BACK TO THE MAIN LOOP.HAVE TO TEST
                        }
                    }
                }
            }

            public string Title //This is my property for a post title
            {
                get { return title; }
                set
                {
                    if (value.Length >= 1 && value.Length <= 100)
                    {
                        try
                        {
                            if (vulgarityChecker(value))//If true, found profanity
                            {
                                //TODO: IMPLEMENT THE FOUL LANGUAGE EXECPTION
                            }
                            else //did not find profanity
                            {
                                title = value;
                            }
                        }
                        catch (FoulLanguageException fle)
                        {
                            Console.WriteLine(fle.ToString());
                            return; //BE SUSPIPCIOUS HERE
                        }
                    }
                }
            }//end title property


        }//End post class

        /**
         * This is the definition for the Comment class
         * Created by Byron Hogan
         */
         public class Comment
        {
            private readonly uint commentID;
            private readonly uint authorID;
            private string content;
            private readonly uint parentID;
            private uint upVotes;
            private uint downVotes;
            private readonly DateTime timeStamp;
            private SortedSet<Comment> commentReplies;
            private uint indentLevel;





        }

        /**
         * This is the definition of the foul language exception
         * 
         * returns:  A string indicationg that a FLE occured
         */
         public class FoulLanguageException : Exception
        {
            public override string ToString()
            {
                return "You seem to have included a bad word in your message. Please avoid doing this in the future";
            }
        }

        /**
         * This is a helper function that can be called to check for profanity.
         * Parameters: This takes a string and will check the entire thing for profanity somewhere in it
         * Returns:   This will return true if profanity has been found.
         */
        static public bool vulgarityChecker(string s)  
        {
            foreach (string badWord in BANNED_WORDS) // Check each word in the bad words if they appear in the passed string
            {
                if (s.ToLower().Contains(badWord)){ //Found a bad word!
                    return true;
                }
            }
            // The string is clean of bad words,return false
            return false;
        }

        /**
         * This is the main function of the program. It runs a loop that will quit out when the user enters 
         * the correct option to quit. It mainly functions to call other functions to do the rest of the program.
         */
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
                    else if (userInput.ToLower() == "q" || userInput.ToLower() == "e")//handles the special single character exceptions
                    {
                        exitProgram = true;
                    }
                    else if (Char.IsDigit(userInput[0]))// Determines the function to call
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
