using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace website_checker
{
    class HttpResponse
    {
        public async Task RepeatedWebsiteCheckAsync(Uri websiteUrl, bool StopOnResponse, int secondsBetweenChecks)
        {
            bool exit = false;
            while (exit == false)
            {
                //await can only be used in methods that are specified to be async like this one,
                //await is used in asynchronous programming, basically await tells the program to stop executing lines of code
                //till the line thats being told to be awaited has finished executing, in this case it is used because we are
                //making a web request which depends on the speed of the internet connection which means the program
                //must wait otherwise it will return an error because the program is trying to use values that have not
                //been recieved yet
                if (await Task.Run(() => CheckIfWebsiteIsUpAsync(websiteUrl)))
                {
                    Console.WriteLine("WEBSITE RESPONSIVE");
                    if (StopOnResponse)
                    {
                        exit = true;
                    }
                }
                else
                {
                    Console.WriteLine("no response...");
                }

                if (exit != true)
                {
                    //secondsBetweenChecks  is multiplied because thread.Sleep() works with milliseconds not seconds
                    Thread.Sleep(secondsBetweenChecks * 1000);
                }
            }
        }

        //this is interesting and very useful to understand not with just type Task but multiple other types like List<>
        //you write a datatype between the <>, this is to specify which datatype you want to work with
        //so in this case writing Task<bool> will create a Task of Type bool, furthermore if you just write Task
        //the compiler will assume you want to create a Task of type void 
        private async Task<bool> CheckIfWebsiteIsUpAsync(Uri websiteUrl)
        {
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)await HttpWebRequest.Create(websiteUrl).GetResponseAsync();
            }
            catch (Exception e)
            {
                return false;
            }

            //this may look complicated but it is a lot simpler than it looks, in this case ? is a ternary operator
            //the ternary operator takes one condition before the ? and outputs if its true or false respectively beside the :
            //I will give an example, assuming I want string greeting to "Good Morning" if bool sunIsUp is true, and
            //"Good Night" if sunIsUp is false, then i will write string greeting = sunIsUp ? "Good Morning" : "Good Night";

            //another way of explaining it is to look at the ternary operator like so condition ? value to return if the
            //condition is true : value to return if the condition is false;
            return response.StatusCode == HttpStatusCode.OK ? true : false;
        }
    }
}
