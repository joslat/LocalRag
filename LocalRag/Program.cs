#pragma warning disable SKEXP0070
#pragma warning disable SKEXP0050
#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010

// Create a chat completion service
using LocalRag;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Embeddings;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Plugins.Memory;
using SmartComponents.LocalEmbeddings.SemanticKernel;

// Your PHI-3 model location 
var modelPath = @"C:\github\Phi-3-mini-4k-instruct-onnx\cpu_and_mobile\cpu-int4-rtn-block-32";
var modelId = "localphi3onnx";

// Load the model and services
var builder = Kernel.CreateBuilder();
builder.AddOnnxRuntimeGenAIChatCompletion(modelId, modelPath);
// builder.AddOnnxRuntimeGenAIChatCompletion(modelPath);
builder.AddLocalTextEmbeddingGeneration();

// Build Kernel
var kernel = builder.Build();

// Create services such as chatCompletionService and embeddingGeneration
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var embeddingGenerator = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

// Setup a memory store and create a memory out of it
var memoryStore = new VolatileMemoryStore();
var memory = new SemanticTextMemory(memoryStore, embeddingGenerator);

// Loading it for Save, Recall and other methods
kernel.ImportPluginFromObject(new TextMemoryPlugin(memory));

// Populate the memory with some interesting facts
string collectionName = "TheLevelOrg";
MemoryHelper.PopulateInterestingFacts(memory, collectionName);

Console.ForegroundColor = ConsoleColor.Cyan;
Console.WriteLine("""
  _                     _   _____            _____ 
 | |                   | | |  __ \     /\   / ____|
 | |     ___   ___ __ _| | | |__) |   /  \ | |  __ 
 | |    / _ \ / __/ _` | | |  _  /   / /\ \| | |_ |
 | |___| (_) | (_| (_| | | | | \ \  / ____ \ |__| |
 |______\___/ \___\__,_|_| |_|  \_\/_/    \_\_____|         
                                   by Arafat Tehsin              
""");


// Start the conversation
while (true)
{
    // Get user input
    Console.ForegroundColor = ConsoleColor.White;
    Console.Write("User > ");
    var question = Console.ReadLine()!;

    // Enable auto function calling
    OpenAIPromptExecutionSettings executionSettings = new()
    {
        ToolCallBehavior = ToolCallBehavior.EnableKernelFunctions,
        MaxTokens = 200
    };

    // Invoke the kernel with the user input
    var response = kernel.InvokePromptStreamingAsync(
        promptTemplate: @"Question: {{$input}}
        Answer the question using the memory content: {{Recall}}",
        arguments: new KernelArguments(executionSettings)
        {
            { "input", question },
            { "collection", collectionName }
        }
        );

    Console.ForegroundColor = ConsoleColor.Green;
    Console.Write("\nAssistant > ");

    string combinedResponse = string.Empty;
    await foreach (var message in response)
    {
        //Write the response to the console
        Console.Write(message);
        combinedResponse += message;
    }

    Console.WriteLine();
}
