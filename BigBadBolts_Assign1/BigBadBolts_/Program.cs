using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
/**
 * NOTES TO MY PARTNER: i wrote a helper function for profanity checking. not sure if you need it but to call it you say
 * vulgarityChecker("SOME STRING");  //will return true if profanity is found, thats the goal anyways.
 * 
 * Need to add your reads to the file reader function and the main class call, i have posts and comments
 */
namespace BigBadBolts_
{
    class Program
    {
        static public List<string> BANNED_WORDS = new List<string>()
        {
            "fudge","shoot","baddie","butthead"
        };
        static public SortedSet<Post> myPosts = new SortedSet<Post>();
        static public SortedSet<Comment> myComments = new SortedSet<Comment>();


        /**
         * This is the class definition for the Post class. 
         * Created by Byron. 
         */
        public class Post : IComparable
        {
            private readonly uint postID;
            private string title;
            private readonly uint authorID;
            private string postContent;
            private readonly uint subHome;
            private uint upVotes;
            private uint downVotes;
            private uint weight;
            private readonly DateTime timeStamp;
            private SortedSet<Comment> postComments;

            /////////CONSTRUCTOR ZONE////////////////////////////////////////////////////////
            public Post() //DEFAULT CONSTRUCTOR....may need some tweaks
            {
                postID = 0;
                title = "";
                authorID = 0;
                postContent = "";
                subHome = 0;
                upVotes = 0;
                downVotes = 0;
                weight = 0;
                timeStamp = DateTime.Now;
                postComments = null;
            }
            //This is used to create a new post
            public Post(uint _postID, uint _authorID, string _title, string _postContent, uint _subHome, uint _upVotes, uint _downVotes, uint _weight, DateTime _timeStamp)
            {
                postID = _postID;
                title = _title;
                authorID = _authorID;
                postContent = _postContent;
                subHome = _subHome;
                upVotes = _upVotes;
                downVotes = _downVotes;
                weight = _weight;
                timeStamp = _timeStamp;
               // postComments = _postComments as SortedSet<Comment>;
            }
            public Post(string _title, uint _authorID, string _postContent, uint _subHome)
            {
                postID = 0;
                title = _title;
                authorID = _authorID;
                PostContent = _postContent;
                subHome = _subHome;
                upVotes = 1;
                downVotes = 0;
                weight = 0;
                timeStamp = DateTime.Now;
                postComments = null;
            }
            ////////////////END CONSTREUCTOR ZONE///////////////////////////////////////////

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
                                throw new FoulLanguageException();
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

            public int CompareTo(Object aplha)
            {
                if (aplha == null)
                    throw new ArgumentNullException();

                Post rightOp = aplha as Post;

                if (rightOp != null)
                {
                    return PostRating.CompareTo(rightOp.PostRating); //This might have to be switched around
                }
                else
                {
                    throw new ArgumentException("[Post]:CompareTo argument is not a Post Object.");
                }
            }


        }//End post class

        /** Collection of Post objects. This class
        * implements IEnumerable so that it can be used
        * with ForEach syntax. 
        */
        public class Posts : IEnumerable
        {
            private Post[] _post;
            public Posts(Post[] pArray)
            {
                _post = new Post[pArray.Length];

                for (int i = 0; i < pArray.Length; i++)
                {
                    _post[i] = pArray[i];
                }
            }

            // Implementation for the GetEnumerator method.
            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }

            public PostEnum GetEnumerator()
            {
                return new PostEnum(_post);
            }
        }

        // When you implement IEnumerable, you must also implement IEnumerator.
        public class PostEnum : IEnumerator
        {
            public Post[] _post;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            int position = -1;

            public PostEnum(Post[] list)
            {
                _post = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _post.Length);
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public Post Current
            {
                get
                {
                    try
                    {
                        return _post[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }


        /**
         * This is the definition for the Comment class
         * Created by Byron Hogan
         */
        public class Comment : IComparable
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

            public string Content
            {
                get { return this.content; }
            }

            public uint Score
            {
                get { return upVotes - downVotes; }
            }

            /////////CONSTRUCTOR ZONE////////////////////////////////////////////////////////
            public Comment() //DEFAULT CONSTRUCTOR....may need some tweaks
            {
                commentID = 0;
                authorID = 0;
                content = "";
                parentID = 0;
                upVotes = 0;
                downVotes = 0;
                timeStamp = DateTime.Now;
                commentReplies = null;
                indentLevel = 0;
            }
            //This is used to create a new post from file.
            public Comment(uint _commentID, uint _authorID, string _content, uint _parentID, uint _upVotes, uint _downVotes, DateTime _timeStamp)
            {
                commentID = _commentID;
                content = _content;
                authorID = _authorID;
                parentID = _parentID;
                upVotes = _upVotes;
                downVotes = _downVotes;
                timeStamp = _timeStamp;
            }
            /*Change to work for comments
            Comment(string _title, uint _authorID, string _postContent, uint _subHome)
            {
                postID = 0;
                title = _title;
                authorID = _authorID;
                PostContent = _postContent;
                subHome = _subHome;
                upVotes = 1;
                downVotes = 0;
                weight = 0;
                timeStamp = DateTime.Now;
                postComments = null;
            } */
            ////////////////END CONSTREUCTOR ZONE///////////////////////////////////////////


   

        public int CompareTo(Object aplha)
            {
                if (aplha == null)
                    throw new ArgumentNullException();

                Comment rightOp = aplha as Comment;

                if (rightOp != null)
                {
                    return Score.CompareTo(rightOp.Score); //This might have to be switched around
                }
                else
                {
                    throw new ArgumentException("[Comment]:CompareTo argument is not a Comment Object.");
                }
            }

        }//End comment class

        /** Collection of Comment objects. This class
         * implements IEnumerable so that it can be used
         * with ForEach syntax. 
         */
        public class Comments : IEnumerable
        {
            private Comment[] _comment;
            public Comments(Comment[] cArray)
            {
                _comment = new Comment[cArray.Length];

                for (int i = 0; i < cArray.Length; i++)
                {
                    _comment[i] = cArray[i];
                }
            }

            // Implementation for the GetEnumerator method.
            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }

            public CommentEnum GetEnumerator()
            {
                return new CommentEnum(_comment);
            }
        }

        // When you implement IEnumerable, you must also implement IEnumerator.
        public class CommentEnum : IEnumerator
        {
            public Comment[] _comment;

            // Enumerators are positioned before the first element
            // until the first MoveNext() call.
            int position = -1;

            public CommentEnum(Comment[] list)
            {
                _comment = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _comment.Length);
            }

            public void Reset()
            {
                position = -1;
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public Comment Current
            {
                get
                {
                    try
                    {
                        return _comment[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
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
         * This function gets and reads input from files provided to us. 
         * Parameters: myPosts- a SortedSet of post objects to fill with post info
         *             myComments - a SortedSet of Comment objects to fill with comment info
         */
         static public void getFileInput(SortedSet<Post> myPosts, SortedSet<Comment> myComments)
        {
            string currentLine;
            string[] tokens;

            //This will read the post file and build the objects from there
            using (StreamReader inFile = new StreamReader("..//..//..//posts.txt"))
            {
                currentLine = inFile.ReadLine(); //prime the read
                while (currentLine != null)
                {
                    tokens = currentLine.Split('\t');

                    string dateString = tokens[8] + '-' + tokens[9] + '-' + tokens[10] + ' ' + tokens[11] + ':' + tokens[12] + ':' + tokens[13];
                    DateTime temp;
                    if (DateTime.TryParse(dateString, out temp))//Makes sure the date converted successfully
                    {
                        Post postToAdd = new Post(//build the post to add
                            UInt32.Parse(tokens[0]),//postId
                            UInt32.Parse(tokens[1]),//authorID
                            tokens[2],//title
                            tokens[3],//postContent
                            UInt32.Parse(tokens[4]),//subHome
                            UInt32.Parse(tokens[5]),//upvotes
                            UInt32.Parse(tokens[6]),//downVotes
                            UInt32.Parse(tokens[7]),//weight
                            temp//dateTime
                            );

                        myPosts.Add(postToAdd);
                    }
                    else //We failed to conver the date
                    {
                        Console.WriteLine("We didn't conver the date properly! QUIT (Handle this better)");
                        return;
                    }
                    currentLine = inFile.ReadLine(); //get the next line
                }
            }

            //This will read the comment file and build the objects from there
            using (StreamReader inFile = new StreamReader("..//..//..//comments.txt"))
            {
                currentLine = inFile.ReadLine(); //prime the read
                while (currentLine != null)
                {
                    tokens = currentLine.Split('\t');

                    string dateString = tokens[6] + '-' + tokens[7] + '-' + tokens[8] + ' ' + tokens[9] + ':' + tokens[10] + ':' + tokens[11];
                    DateTime temp;
                    if (DateTime.TryParse(dateString, out temp))//Make sure the date converted successfully
                    {
                        Comment commentToAdd = new Comment(//build the comment to add
                            UInt32.Parse(tokens[0]),//commentId
                            UInt32.Parse(tokens[1]),//authorID
                            tokens[2],//content
                            UInt32.Parse(tokens[3]),//parentID
                            UInt32.Parse(tokens[4]),//upvotes
                            UInt32.Parse(tokens[5]),//downVotes
                            temp//dateTime
                            );

                        myComments.Add(commentToAdd);
                    }
                    else
                    {
                        Console.WriteLine("We didn't convert the date properly! QUIT (Handle this better)");
                        return;
                    }
                    currentLine = inFile.ReadLine(); //get the next line
                }
            }

        }

        /**
         * This is the main function of the program. It runs a loop that will quit out when the user enters 
         * the correct option to quit. It mainly functions to call other functions to do the rest of the program.
         */
        static void Main(string[] args) //Need to implement reading in input files
        {
            bool exitProgram = false;
            string userInput;


            //Read the input files here to build the objects
            getFileInput( myPosts, myComments);

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
                                foreach(Post test in myPosts)
                                {
                                    Console.WriteLine(test.Title.ToString());
                                }
                                Console.WriteLine(" ");
                                foreach (Comment test in myComments)
                                {
                                    Console.WriteLine(test.Content.ToString());
                                }
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
