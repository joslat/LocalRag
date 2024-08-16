# LocalRag
Trying out LocalRag's approach from Arafat Tehsin
https://arafattehsin.com/ai-copilot-offline-phi3-semantic-kernel/

Also, Bruno's approach using only the smartcomponents with a local phi3 in lm studio
https://techcommunity.microsoft.com/t5/educator-developer-blog/building-intelligent-applications-with-local-rag-in-net-and-phi/ba-p/4175721

Note there is an Issue on the smartcomponents Semantic Kernel nuget package
The issue happens on the local text embedded added by this instruction:
- The builder.AddLocalTextEmbeddingGeneration(); 

Here is the issue and the workaround:
https://techcommunity.microsoft.com/t5/educator-developer-blog/building-intelligent-applications-with-local-rag-in-net-and-phi/bc-p/4221062#M2953
BGE-micro-v2 for workaround: https://huggingface.co/TaylorAI/bge-micro-v2

Have fun!!