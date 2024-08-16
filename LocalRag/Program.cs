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



//await LocalRAGBruno.Execute(); 

// Your PHI-3 model location 
var modelPath = @"C:\git\joslat\LocalRag\phi3\Phi-3-mini-4k-instruct-onnx\cpu_and_mobile\cpu-int4-rtn-block-32";
var modelId = "localphi3onnx";

var textModelPath = @"C:\git\joslat\LocalRag\bge-micro-v2\onnx\model.onnx";
var foo = @"C:\git\joslat\LocalRag\bge-micro-v2\vocab.txt";



// Load the model and services
var builder = Kernel.CreateBuilder();
builder.AddOnnxRuntimeGenAIChatCompletion(modelId, modelPath);
//builder.AddOnnxRuntimeGenAIChatCompletion(modelPath);
//builder.AddBertOnnxTextEmbeddingGeneration(modelId, modelPath);
//builder.AddLocalTextEmbeddingGeneration();
builder.AddBertOnnxTextEmbeddingGeneration(textModelPath, foo);
// Build Kernel
var kernel = builder.Build();

// Create services such as chatCompletionService and embeddingGeneration
var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
var embeddingGenerator = kernel.GetRequiredService<ITextEmbeddingGenerationService>();

// Setup a memory store and create a memory out of it
var memoryStore = new VolatileMemoryStore();
var memory = new SemanticTextMemory(memoryStore, embeddingGenerator);

//test adding memory
const string MemoryCollectionName = "fanFacts";
await memory.SaveInformationAsync(MemoryCollectionName, id: "info1", text: "Gisela's favourite super hero is Batman");
await memory.SaveInformationAsync(MemoryCollectionName, id: "info2", text: "The last super hero movie watched by Gisela was Guardians of the Galaxy Vol 3");
await memory.SaveInformationAsync(MemoryCollectionName, id: "info3", text: "Bruno's favourite super hero is Invincible");




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
