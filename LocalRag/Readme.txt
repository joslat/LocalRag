Source https://arafattehsin.com/ai-copilot-offline-phi3-semantic-kernel/
Source 2 https://techcommunity.microsoft.com/t5/educator-developer-blog/building-intelligent-applications-with-local-rag-in-net-and-phi/ba-p/4175721

Issue on smartcomponents nuget package (and workaround)
- The builder.AddLocalTextEmbeddingGeneration(); 

https://techcommunity.microsoft.com/t5/educator-developer-blog/building-intelligent-applications-with-local-rag-in-net-and-phi/bc-p/4221062#M2953

BGE-micro-v2 for workaround: https://huggingface.co/TaylorAI/bge-micro-v2


Issues:
- Cloning from https://huggingface.co/microsoft/Phi-3-mini-4k-instruct-onnx did not fully download the large files...
in HuggingFace large files are hanldled by the GIT LFS component.
So for windows https://git-lfs.github.com/ needs to be installed 

After installation, initialize Git LFS in your cloned repository:
git lfs install
git lfs pull

You can check the status of LFS files:
git lfs ls-files
