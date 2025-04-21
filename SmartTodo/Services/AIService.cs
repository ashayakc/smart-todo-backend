using Azure;
using Azure.AI.OpenAI;

public interface IAIService
{
    Task<string> CategorizeTodo(string title, string? description);
}

public class AIService : IAIService
{
    private readonly OpenAIClient _openAIClient;
    private readonly string _deploymentName; // Add this field

    public AIService(IConfiguration configuration)
    {
        string apiKey = configuration["AzureOpenAI:ApiKey"];
        string endpoint = configuration["AzureOpenAI:Endpoint"];
        _deploymentName = configuration["AzureOpenAI:DeploymentName"]; // Get deployment name

        _openAIClient = new OpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
    }

    public async Task<string> CategorizeTodo(string title, string? description)
    {
        // Construct the prompt
        string prompt = $"""
            Categorize the following todo item into one of the following categories:
            Work, Personal, Home, Finance, Health, Shopping, Social, Travel, Education, Others.

            Todo Title: {title}
            Todo Description: {description}

            Provide only the category name.  Do not include any other text.
            """;

        try
        {
            // Call the Azure OpenAI API
            var response = await _openAIClient.GetChatCompletionsAsync(new ChatCompletionsOptions
            {
                DeploymentName = _deploymentName,
                Messages =
                {
                        new ChatMessage(ChatRole.User, prompt)
                }
                });

            // Extract the category from the response
            string category = response.Value.Choices[0].Message.Content.Trim();
            return category;
        }
        catch (RequestFailedException ex)
        {
            // Handle API errors (very important!)
            Console.WriteLine($"Error calling Azure OpenAI: {ex.Message}");
            return "Others"; // Return a default category on error, or you could throw an exception
        }
    }
}

