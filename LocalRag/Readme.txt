Source https://arafattehsin.com/ai-copilot-offline-phi3-semantic-kernel/


Issues:
- Cloning from https://huggingface.co/microsoft/Phi-3-mini-4k-instruct-onnx did not fully download the large files...
in HuggingFace large files are hanldled by the GIT LFS component.
So for windows https://git-lfs.github.com/ needs to be installed 

After installation, initialize Git LFS in your cloned repository:
git lfs install
git lfs pull

You can check the status of LFS files:
git lfs ls-files


- The builder.AddLocalTextEmbeddingGeneration(); doesn't seem to fully work... something's strange there...

