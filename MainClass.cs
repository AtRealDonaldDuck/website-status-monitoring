using System;
using System.Threading.Tasks;

namespace website_checker
{
    class MainClass
    {
        static void Main(string[] args)
        {
            Uri websiteUrl = GetUrlFromUser();
            bool stopOnResponse = GetStopOnResponseUserPreferenceFromUser();
            int secondsBetweenChecks = GetSecondsBetweenChecksFromUser();

            //var is an implicitly typed variable, meaning the compiler tries to figure out what datatype
            //you want to assign it based off what you initialize it with, in this case it will be type
            //HttpResponse (HttpResponse is a class I created for this program)
            var httpResponse = new HttpResponse();

            Console.WriteLine("Thank you, the words \"WEBSITE RESPONSIVE\" will appear when the website you've entered is up and running and \"no response...\" while the website is down");

            //Task is beyond the scope of this code explanation, just know task is used a lot in asynchronous programming
            //in this case however it is used synchronously, the .Wait() method at the end stops the program from continuing until
            //the method after "=>" is completed (=> is called a lambda operator). if .Wait() wasn't there the
            //program would continue without waiting for httpResponse.RepeatedWebsiteCheckAsync() to finish 
            Task.Run(() => httpResponse.RepeatedWebsiteCheckAsync(websiteUrl, stopOnResponse, secondsBetweenChecks)).Wait();

            Console.WriteLine("PROCESS ENDING");
        }

        private static Uri GetUrlFromUser()
        {
            bool uriCreated = false;
            Uri websiteUrl;
            Console.WriteLine("please enter the url of the website you would like to keep track of: ");
            do
            {
                //Uri.TryCreate() will return true if the uri passed is a valid uri, meaning the uri is written in a way uris
                //are supposed to be written(warning uri.trycreate will not check if the website exists, to find out if a website
                //exists you should look into making HttpRequests with C# which is touched on in HttpResponse.cs).
                //uriKind.absolute just means I want a full url meaning a url that points you directly to a resource,
                //a uriKind.relative means ill be creating a portion of a url not the whole thing.
                //lastly my favourite part of uri.tryCreate, while returning true or false it also returns a Uri in a way
                //out websiteUrl translates to if the uri was succesfully created then assign that uri to websiteUrl, websiteUrl
                //isnt part of the syntax thats just the name of the variable i created at line 33
                switch (Uri.TryCreate((string)Console.ReadLine(), UriKind.Absolute, out websiteUrl))
                {
                    case true:
                        uriCreated = true;
                        break;
                    case false:
                        Console.WriteLine("The Url you entered is incorrect, please make sure you entered a full url");
                        uriCreated = false;
                        break;
                }
            }
            while (uriCreated == false);

            return websiteUrl;
        }

        private static bool GetStopOnResponseUserPreferenceFromUser()
        {
            //the ? at the end of bool means it is a nullable type.
            //some datatypes like string can store null by default but for bool you need to call it bool? to be able to assign
            //null to it
            bool? stopOnResponse = null;
            string userInput;
            Console.WriteLine("Would you like to keep checking on the website after it's responsive? (Y/N)");
            do
            {
                userInput = (string)Console.ReadLine();

                switch (userInput.ToLower())
                {
                    //when using switch case you are able to make one response apply to multiple cases by writing it as is below.
                    //so in this case if userInput was "y" or "yes" stopOnResponse = false; will happen
                    case "y":
                    case "yes":
                        stopOnResponse = false;
                        break;
                    case "n":
                    case "no":
                        stopOnResponse = true;
                        break;
                    default:
                        //using the special character $ before "" turns whatevers inside the "" into an interpolated
                        //string rather than a regular string in an interpolated string you can use {} to make reference
                        // to things for example $"my name is {name}" is the same as writing "my name is " + name
                        //if you are interested another special character you could use with strings is @, when writing
                        //@"" it means that that string will ignore escape characters so @"\n" does not skip a line
                        //furthermore you can use both special characters at the same time like this $@""
                        Console.WriteLine($"you entered {userInput}, please enter Y for yes or N for no");
                        stopOnResponse = null;
                        break;
                }
            }
            while (stopOnResponse == null);
            //since the variable stopOnResponse is a nullable type(nullable types been explained at line 63) in order to convert
            //it back to its non nullable version you must use .value, if stopOnResponse is null when using .value with it
            //you will get an error for trying to assign null to a data type that can not be assigned null
            return stopOnResponse.Value;
        }

        private static int GetSecondsBetweenChecksFromUser()
        {
            int? secondsBetweenChecks = null;

            Console.WriteLine("How much time would you like to wait between checks on the website? (in seconds)");
            do
            {
                try
                {
                    //this might confuse some people as int.Parse() returns a non nullable int, to clarify
                    //you can assign non nullable values to variables of a nullable type, but you can not directly
                    //assign the value of a nullable type variable to a regular type variable that is why you use
                    //.value when assigning a nullable types value to a non nullable type
                    secondsBetweenChecks = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("You entered an invalid number, please enter a whole number in digits");
                }
            }
            while (secondsBetweenChecks == null);

            return secondsBetweenChecks.Value;
        }
    }
}
