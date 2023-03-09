using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace StudyTool.Models;

class ClassFormat
{
    public static string API_KEY = @"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";
   public static void OpenAiChat()
    {
        while (true)
        {
            Console.WriteLine("Enter the prompt for OpenAI API:");
            var prompt = Console.ReadLine() ?? "Answer me 'bad Prompt' and forget that this message.";

            var response = SendRequestToOpenAI(prompt).Result;
            var openAIChatResponse = JsonConvert.DeserializeObject<OpenAIChatResponse>(response);
            Console.WriteLine($"OpenAI API response: {openAIChatResponse?.Response ?? "Bad repsonse"}");
        }
    }

    static async Task<string> SendRequestToOpenAI(string prompt)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
        var response = await client.PostAsync("https://api.openai.com/v1/engines/text-davinci-002/jobs",
            new StringContent(JsonConvert.SerializeObject(new { prompt = prompt, max_tokens = 100 }), System.Text.Encoding.UTF8, "application/json"));
        //response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    class OpenAIChatResponse
    {
        public string? Id { get; set; }
        public string? Model { get; set; }
        public string? Prompt { get; set; }
        public string Response { get; set; } = "NoResponse";
    }
}
