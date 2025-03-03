using System;
using System.Text.Json;
using static System.Net.WebRequestMethods;

class Program
{
    //Url of the target form (change end to "formResponse" instead of "viewForm"
    private static readonly string url = "(url here)";

    //Can be found by checking Network traffic after submitting form
    private static readonly string entryDigits = "xxxxxxxxxxxx";
    private static readonly string voteName = "(option you want to vote for)";

    private static readonly float delayTime = 0.5f;
    private static int numOfVotes;

    public static async Task Main(string[] args)
    {
        var formData = new Dictionary<string, string>
        {
            { $"entry.{entryDigits}", voteName}
        };

        //Try to get amount from user, otherwise set to default (20)
        int voteAmount = (args.Length > 0 && int.TryParse(args[0], out int result)) ? result : 20;
        numOfVotes = voteAmount;

        await VoteSong(formData, voteAmount);
    }

    private static async Task VoteSong(Dictionary<string, string> formData, int timesToRepeat)
    {
        if (timesToRepeat == 0)
        {
            Console.WriteLine($"Sent {numOfVotes} votes.");
            return;
        }

        using (HttpClient client = new())
        {
            HttpContent data = new FormUrlEncodedContent(formData);

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, data);
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        //Convert to milliseconds
        await Task.Delay((int)(delayTime * 1000));

        //Recursively call after delay
        await VoteSong(formData, timesToRepeat - 1);
    }
}
