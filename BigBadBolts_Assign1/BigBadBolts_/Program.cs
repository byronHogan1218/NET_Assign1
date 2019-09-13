/*******************************************************************
*                                                                  *
*  CSCI 473-1/504-1       Assignment 1                Fall   2019  *
*                                                                  *
*                                                                  *
*  Program Name:  Reddit                                           *
*                                                                  *
*  Programmer:    Byron Hogan,                                     *
*                 Margaret Higginbotham, z1793581                  *
*                                                                  *
*******************************************************************/
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
        static public SortedSet<Comment> myCommentsReplies = new SortedSet<Comment>();

        static public SortedSet<Subreddit> mySubReddits = new SortedSet<Subreddit>();
        static public SortedSet<User> myUsers = new SortedSet<User>();




        /* The User class
 * By Margaret
 */
        public class User : IComparable
        {
            private readonly uint id;
            private readonly string name;
            private int postScore;
            private int commonScore;

            //public versions
            public uint Id
            {
                get { return id; }
            }

            public string Name
            {
                get { return name; }
                set
                {
                    if (value.Length >= 5 && value.Length <= 21) ; //Added a semicolon
                        //name = value;//This isnt working properly
                }
            }

            public int PostScore
            {
                get { return postScore; }
                set { postScore = value; }
            }

            public int CommonScore
            {
                get { return commonScore; }
                set { commonScore = value; }
            }

            // default constructor for User
            public User()
            {
                id = (uint)myUsers.Count + 1;
                name = "";
                postScore = 0;
                commonScore = 0;
            }

            // constructor for User when all properties are set
            public User(uint conId, string conName, int conPost, int conCommon)
            {
                id = conId;
                name = conName;
                postScore = conPost;
                commonScore = conCommon;
            }

            //Used to create new user
            public User(string conName)
            {
                id = (uint)myUsers.Count + 1;
                name = conName;
                postScore = 0;
                commonScore = 0;
            }

            public int CompareTo(object alpha)
            {
                if (alpha == null)
                    throw new ArgumentNullException();

                User rightOp = alpha as User;

                if (rightOp != null)
                    return Name.CompareTo(rightOp.Name);
                else
                    throw new ArgumentException("[User]: CompareTo argument is not a Name");
            }
            //need to finish toString
            public override string ToString()
            {//wasn't sure how to format toString
                return "WORDS"; // Console.WriteLine();  THIS NEEDS TO BE FIXED
            }

        }

        /* Subreddit class
         * by Margaret
         */
        public class Subreddit : IComparable
        {
            private readonly uint id;
            private string name;
            private uint members;
            private uint active;
            private SortedSet<Post> subPosts;

            //public versions
            public uint Id
            {
                get { return id; }
                set { }// id = value; } added the '}' this needs to be fixed
            }
            //i need to do unique id

            public SortedSet<Post> SubPosts
            {
                get
                {
                    return subPosts; 
                }
            }

            public string Title
            {
                get { return name; }
            }

            public string Name
            {
                get { return name; }
                set
                {
                    if (value.Length >= 3 && value.Length <= 21)
                        name = value;
                }
            }

            public uint Members
            {
                get { return members; }
                set { members = value; }
            }

            public uint Active
            {
                get { return active; }
                set { active = value; }
            }

            //defult constructor
            public Subreddit()
            {
                id = (uint)mySubReddits.Count + 1;
                name = "";
                members = 0;
                active = 0;
                subPosts = myPosts;
            }

            //constructor to create new subreddit
            public Subreddit(string conName)
            {
                id = (uint)mySubReddits.Count + 1;//A somewhat unique identifier
                name = conName;
                members = 0;
                active = 0;
                subPosts = myPosts;
            }

            //constructor for input file
            public Subreddit(uint conId, string conName, uint conMembers, uint conActive)
            {
                id = conId;
                name = conName;
                members = conMembers;
                active = conActive;
                subPosts = myPosts;
            }

            public int CompareTo(Object alpha)
            {
                if (alpha == null)
                    throw new ArgumentNullException();

                Subreddit rightOp = alpha as Subreddit;

                if (rightOp != null)
                    return Name.CompareTo(rightOp.Name);
                else
                    throw new ArgumentException("[Subreddit]: CompareTo argument is not a name");
            }

            public override string ToString()
            {
                return '\t' + "<" + this.id + "> " + this.name + " -- (" + this.active + "/" + this.members + ")";
            }

        }


        //Subreddit IEnumerable
        public class SubredditEnum : IEnumerable
        {
            private Subreddit[] _subreddit;

            public SubredditEnum(Subreddit[] subArray)
            {
                _subreddit = new Subreddit[_subreddit.Length];

                for (int i = 0; i < subArray.Length; i++)
                {
                    _subreddit[i] = subArray[i];
                }

            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return (IEnumerator)GetEnumerator();
            }

            public SubEnum GetEnumerator()
            {
                return new SubEnum(_subreddit);
            }

        }

        //Subreddit IEnumerator
        public class SubEnum : IEnumerator
        {
            public Subreddit[] _subreddit;

            int position = -1;

            public SubEnum(Subreddit[] list)
            {
                _subreddit = list;
            }

            public bool MoveNext()
            {
                position++;
                return (position < _subreddit.Length);
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

            public Subreddit Current
            {
                get
                {
                    try
                    {
                        return _subreddit[position];
                    }

                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }

                }

            }

        }


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
                postID = (uint)myPosts.Count + 1;
                title = "";
                authorID = 0;
                postContent = "";
                subHome = 0;
                upVotes = 0;
                downVotes = 0;
                weight = 0;
                timeStamp = DateTime.Now;
                postComments = myComments;
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
                postComments = myComments;
            }
            public Post(string _title, uint _authorID, string _postContent, uint _subHome)
            {
                postID = (uint)myPosts.Count + 1;
                title = _title;
                authorID = _authorID;
                PostContent = _postContent;
                subHome = _subHome;
                upVotes = 1;
                downVotes = 0;
                weight = 0;
                timeStamp = DateTime.Now;
                postComments = myComments;
            }
            ////////////////END CONSTREUCTOR ZONE///////////////////////////////////////////

            public uint PostAuthorId
            {
                get { return authorID; }
            }

   

            public SortedSet<Comment> PostComments
            {
                get
                {
                    return postComments;
                }
            }

            public uint PostID
            {
                get { return postID; }
            }


            public uint Score
            {
                get { return upVotes - downVotes; }
            }

            public uint SubId
            {
                get { return subHome;  }
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

            public override string ToString()
            {
                string authorName = "";
                foreach(User item in myUsers)
                {
                    if (item.Id == this.authorID)
                        authorName = item.Name;
                }
                if (authorName.Length == 0)
                    authorName = this.authorID.ToString();

                string commentsOnPost = "\n";


                foreach(Comment postComments in myComments)
                {
                    if( postComments.CommentID == this.PostID)
                    {
                        commentsOnPost = commentsOnPost + postComments.ToString() + '\n';
                    }
                }

                return "\t<" + this.PostID + "> [" + this.subHome + "] (" + this.Score + ") " + this.Title + " " + this.postContent + " - " + authorName + "|" + this.timeStamp.ToString() + "|" + commentsOnPost;
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

            public uint CommentID
            {
                get { return commentID;  }
            }
            public uint ParentID
            {
                get { return parentID; }
            }
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
                commentID = (uint)myComments.Count + 1;
                authorID = 0;
                content = "";
                parentID = 0;
                upVotes = 0;
                downVotes = 0;
                timeStamp = DateTime.Now;
                commentReplies = myComments;
                indentLevel = 0;
                foreach (Comment tabs in myComments)
                {
                    if (tabs.CommentID == this.parentID)
                    {
                        indentLevel = tabs.indentLevel + 1;
                    }
                }  
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
                commentReplies = myComments;
                indentLevel = 0;
                foreach (Comment tabs in myComments)
                {
                    if (tabs.CommentID == this.parentID)
                    {
                        indentLevel = tabs.indentLevel + 1;
                    }
                }

            }
            public Comment(string _content, uint _authorID, uint _parentID)
            {
                commentID = (uint)myComments.Count + 1;
                content = _content;
                authorID = _authorID;
                parentID = _parentID;
                upVotes = 1;
                downVotes = 0;
                commentReplies = myComments;
                indentLevel = 0;
                foreach (Comment tabs in myComments)
                {
                    if (tabs.CommentID == this.parentID)
                    {
                        indentLevel = tabs.indentLevel + 1;
                    }
                }
            }
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

            public override string ToString()
            {
                string authorName = "";
                foreach (User item in myUsers)
                {
                    if (item.Id == this.authorID)
                        authorName = item.Name;
                }
                if (authorName.Length == 0)
                    authorName = this.authorID.ToString();
                string replies = "\n";
                string replyAuthorName = "";
                foreach (Comment reply in this.commentReplies)
                {
                    for (int i = 0; i < this.indentLevel; ++i)
                    {
                        replies = replies + '\t'; 
                    }
                    foreach (User item in myUsers)
                    {
                        if (item.Id == reply.authorID)
                            replyAuthorName = item.Name;
                    }
                    if (replyAuthorName.Length == 0)
                        replyAuthorName = reply.authorID.ToString();
                    replies = replies + "\t<" + reply.CommentID + "> (" + reply.Score + ") " + reply.content + " - " + replyAuthorName + "|" + reply.timeStamp.ToString() + "|";
                    replies = replies + '\n';
                }
                replies = ""; //NOT WORKING
                return "\t<" + this.CommentID + "> (" + this.Score + ") " + this.content + " - " + authorName + "|" + this.timeStamp.ToString() + "|" + replies;
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
         *             mySubreddits - Sorted set of Subreddit objects
         *             myUsers - Sorted set of user objects
         */
         static public void getFileInput(SortedSet<Post> myPosts, SortedSet<Comment> myComments, SortedSet<Subreddit> mySubreddits, SortedSet<User> myUsers)
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

            //This will read the the subreddit file and build the objects from them
            using (StreamReader inFile = new StreamReader("..//..//..//subreddits.txt"))
            {
                currentLine = inFile.ReadLine(); //prime the read
                while (currentLine != null)
                {
                    tokens = currentLine.Split('\t');
                 
                    Subreddit newSub = new Subreddit(//build the subreddit
                      UInt32.Parse(tokens[0]),//id
                      tokens[1],//name
                      UInt32.Parse(tokens[2]), //Members
                      UInt32.Parse(tokens[3]) //Active
                    );

                    mySubReddits.Add(newSub);

                    currentLine = inFile.ReadLine(); //get the next line
                }
            }

            //This will get the user information and store it in the user object
            using (StreamReader inFile = new StreamReader("..//..//..//users.txt"))
            {
                currentLine = inFile.ReadLine(); //prime the read
                while (currentLine != null)
                {
                    tokens = currentLine.Split('\t');

                    User newUser = new User(//build the user
                      UInt32.Parse(tokens[0]),//id
                      tokens[1],//name
                      Int32.Parse(tokens[2]), //postScore
                      Int32.Parse(tokens[3]) //commentScore
                    );

                    myUsers.Add(newUser);
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
            bool found = false;
            string userInput;

            //Read the input files here to build the objects
            getFileInput( myPosts, myComments, mySubReddits, myUsers);

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
                Console.WriteLine(userInput);

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
                                foreach (Subreddit currentReddit in mySubReddits)
                                {
                                    Console.WriteLine("Name -- (Active Members / Total Members)");
                                    Console.WriteLine(currentReddit.ToString());
                                    Console.WriteLine("");//Blank Line
                                }
                                break;
                            case ("2"):  //List all posts from all subreddits
                                Console.WriteLine("<ID> [Subreddit] (Score) Title + PostContent - PosterName |TimeStamp|");
                                Console.WriteLine("");//Blank Line
                                foreach (Subreddit currentReddit in mySubReddits)
                                {
                                    foreach (Post redditPost in myPosts) //GET NULL ERROR HERE
                                    {
                                        if(redditPost.SubId == currentReddit.Id)
                                        {
                                            Console.WriteLine(redditPost.ToString()); // Might need to override the tostring method for this
                                            Console.WriteLine("");//Blank Line
                                        }
                                    }
                                }
                                break;
                            case ("3"):  //List all posts from a single subreddit
                                string subbredditIDToView;
                                found = false;
                                Console.Write("Please enter the ID of the Subbreddit you wish to view: ");
                                subbredditIDToView = Console.ReadLine();
                                Console.WriteLine("");//blank line
                                foreach (Subreddit subreddit in mySubReddits) //Search for the subbreddit to view
                                {
                                    if (subreddit.Id == UInt32.Parse(subbredditIDToView))//Found the subreddit to view
                                    {
                                        found = true;
                                        Console.WriteLine("<ID> [Subreddit] (Score) Title + PostContent - PosterName |TimeStamp|");

                                        foreach (Post subPost in myPosts)
                                        {
                                            if (subPost.SubId == subreddit.Id)
                                            {
                                                Console.WriteLine("");//blank line
                                                Console.WriteLine(subPost.ToString());//This might need to overriden
                                                Console.WriteLine("");//blank line
                                            }
                                        }
                                        break;
                                    }
                                }
                                //We did not find the SubReddit_ID
                                if (!found)
                                    Console.WriteLine("The Subbreddit ID entered was not found.");
                                break;
                            case ("4"):  //View comments of a single post
                                string postIDToView;
                                found = false;
                                Console.Write("Please enter the ID of the Post you wish to view: ");
                                postIDToView = Console.ReadLine();
                                Console.WriteLine("");//blank line
                                foreach (Post post in myPosts) //Search for the post to view
                                {
                                    if (post.PostID.ToString() == postIDToView)//Found the post to view
                                    {
                                        found = true;
                                        foreach (Comment commentOfPost in myComments)
                                        {
                                            if (commentOfPost.ParentID == post.PostID)
                                            {
                                                Console.WriteLine("");//blank line
                                                Console.WriteLine(commentOfPost.ToString());//This might need to overriden
                                                Console.WriteLine("");//blank line
                                            }
                                        }
                                        break;
                                    }
                                }

                                //We did not find the Post_ID
                                if (!found)
                                    Console.WriteLine("The Post ID entered was not found.");

                                break;
                            case ("5"):  //Add comment to post
                                string postIDtoComment;
                                string comment = "";
                                found = false;
                                Console.Write("Please enter the ID of the post you wish to add a comment to: ");
                                postIDtoComment = Console.ReadLine();
                                Console.WriteLine("");//blank line
                                foreach (Post post in myPosts) //Search for the post to comment to
                                {
                                    if (post.PostID == UInt32.Parse(postIDtoComment))//Found the post to add a comment
                                    {
                                        found = true;
                                        Console.WriteLine("Please enter a Comment: ");
                                        try { 
                                        comment = Console.ReadLine();
                                            if (vulgarityChecker(comment))
                                            {
                                                throw new FoulLanguageException();
                                            }
                                        }
                                        catch (FoulLanguageException fle)
                                        {
                                            Console.WriteLine(fle.ToString());
                                            break;
                                        }
                                        
                                        Comment commentToAdd = new Comment(
                                            comment, //content
                                            0001, //authorID //THIS IS ROGNESS USER
                                            post.PostID //parentID
                                            );
                                        Console.WriteLine(comment + post.PostID.ToString());
                                        if (!myComments.Add(commentToAdd))//;
                                            Console.WriteLine("");
                                        else
                                            commentToAdd = null;
                                        Console.WriteLine("");//blank line
                                        Console.WriteLine("Comment was added successfully to post : " + postIDtoComment);
                                        Console.WriteLine("");//blank line
                                        break;
                                    }
                                }
                                //We did not find the postID
                                if (!found)
                                    Console.WriteLine("The postID entered was not found.");
                                break;
                            case ("6"):  //Add reply to comment
                                string commentIDToReplyTo;
                                string replyContent;
                                found = false;
                                Console.Write("Please enter the ID of the comment you wish to reply to: ");
                                commentIDToReplyTo = Console.ReadLine();
                                Console.WriteLine("");//blank line
                                foreach (Comment commentToReply in myComments) //Search for the post to comment to
                                {
                                    if (commentToReply.CommentID.ToString() == commentIDToReplyTo)//Found the comment to reply to
                                    {
                                        found = true;
                                        Console.WriteLine("Please enter your reply: ");
                                        try
                                        {
                                            replyContent = Console.ReadLine();
                                            if (vulgarityChecker(replyContent))
                                            {
                                                throw new FoulLanguageException();
                                            }
                                        }
                                        catch (FoulLanguageException fle)
                                        {
                                            Console.WriteLine(fle.ToString());
                                            break;
                                        }
                                        Comment replyToAdd = new Comment(
                                            replyContent, //content
                                            0001, //authorID //THIS IS ROGNESS USER
                                            commentToReply.CommentID //parentID
                                            );
                                        myComments.Add(replyToAdd);
                                        Console.WriteLine("");//blank line
                                        Console.WriteLine("Reply was added successfully to comment with ID : " + commentIDToReplyTo);
                                        Console.WriteLine("");//blank line
                                        break;
                                    }
                                }
                                //We did not find the comment
                                if (!found)
                                    Console.WriteLine("The commentID entered was not found.");
                                break;
                            case ("7"):  //Create new post
                                found = false;
                                string newPostTitle;
                                string newPostContent;
                                string newPostSubbreddit;
                                Console.WriteLine("Please enter the name of the Subbreddit you want to create a post to: ");
                                newPostSubbreddit = Console.ReadLine();
                                Console.WriteLine("");//blank line
                                foreach (Subreddit subredditGettingPost in mySubReddits) //Search for the subreddit to post to
                                {
                                    if (subredditGettingPost.Title.ToString() == newPostSubbreddit)
                                    { 
                                        found = true;
                                        Console.WriteLine("Enter the title of your new Post: ");
                                        try
                                        {
                                            newPostTitle = Console.ReadLine();
                                            Console.WriteLine("");//blank line
                                            if (vulgarityChecker(newPostTitle))
                                            {
                                                throw new FoulLanguageException();
                                            }
                                        }
                                        catch (FoulLanguageException fle)
                                        {
                                            Console.WriteLine(fle.ToString());
                                            break;
                                        }
                                        Console.WriteLine("Please enter any content you would like to add: ");
                                        try
                                        {
                                            newPostContent = Console.ReadLine();
                                            Console.WriteLine("");//blank line
                                            if (vulgarityChecker(newPostContent))
                                            {
                                                throw new FoulLanguageException();
                                            }
                                        }
                                        catch (FoulLanguageException fle)
                                        {
                                            Console.WriteLine(fle.ToString());
                                            break;
                                        }
                                        //Appropriate post was completed,add it to the SortedSet
                                        Post postToAdd = new Post( 
                                            newPostTitle, //content
                                            0001, //authorID //THIS IS ROGNESS USER
                                            newPostContent, //post content
                                            subredditGettingPost.Id //Subbreddit home
                                            );
                                        myPosts.Add(postToAdd);
                                        Console.WriteLine("");//blank line
                                        Console.WriteLine("Post was added successfully to subbreddit : " + subredditGettingPost.Title.ToString());
                                        Console.WriteLine("");//blank line
                                        break;
                                    }
                                }
                                //We did not find the Subbreddit to add a post to
                                if (!found)
                                    Console.WriteLine("The Subreddit name entered was not found.");
                                break;
                            case ("8"): //Deletes post
                                string postIDtoDelete;
                                found = false;
                                Console.Write("Please enter the ID of the post you wish to delete: ");
                                postIDtoDelete = Console.ReadLine();
                                Console.WriteLine("");//blank line
                                foreach (Post post in myPosts) //Search for the post to comment to
                                {
                                    if (post.PostID.ToString() == postIDtoDelete)//Found the post to delete
                                    {
                                        found = true;
                                        if (post.PostAuthorId == 0001) //This is user rognesses id,its faked we are logged in as him. This would need to change in the future to User.GET_ID() (UNWRITTEN)
                                        {      //Can delete the post because it was written by the USER
                                            if (myPosts.Remove(post)) //Idk if this works
                                                Console.WriteLine("The post was succesfully deleted");
                                            else
                                                Console.WriteLine("Tried to delete, but something went wrong.");
                                            break;
                                        }
                                        else //Post is not written by the logged on user aka user 0001 rogness
                                        {
                                            Console.WriteLine("Sorry but you are not allowed to delete other user's Posts.");
                                            break;
                                        }
                                    }
                                }
                                //We did not find the postID
                                if (!found)
                                    Console.WriteLine("The postID entered was not found.");
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
            Console.Clear();
        }
    }
}
