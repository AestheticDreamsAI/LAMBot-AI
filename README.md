# LAMBot-AI

This project implements a simple Language Action Model (LAM) chatbot using C# and ML.NET. The chatbot can recognize user intents from natural language inputs and perform specific actions, such as launching applications, based on the recognized intents.

## Features

- **Intent Recognition**: Uses machine learning to classify user inputs into predefined intents.
- **Action Execution**: Executes specific actions, such as launching programs, based on the recognized intents.
- **Response Generation**: Provides predefined responses for each recognized intent.
- **Model Persistence**: Saves the trained model to a file and loads it for future use, avoiding the need to retrain.

Combined with OpenAI's ChatGPT or other large language models (LLMs), this can become a powerful A.I. capable of controlling and launching programs via commands.

## Getting Started

### Prerequisites

- .NET SDK
- Visual Studio or any other C# IDE

### Installation

1. **Clone the repository:**

    ```sh
    git clone https://github.com/yourusername/LAMBot-AI.git
    cd LAMBot-AI
    ```

2. **Install the required NuGet packages:**

    ```sh
    dotnet add package Microsoft.ML
    dotnet add package Microsoft.ML.DataView
    dotnet add package Newtonsoft.Json
    ```

### Usage

1. **Prepare the `intents.json` file:**

    Create a file named `intents.json` in the project data directory with the following content:

    ```json
    {
      "intents": [
        {
          "tag": "greeting",
          "patterns": [
            "Hi",
            "Hey",
            "How are you",
            "Is anyone there?",
            "Hello",
            "Good day"
          ],
          "responses": [
            "Hey :-)",
            "Hello, thanks for visiting",
            "Hi there, what can I do for you?",
            "Hi there, how can I help?"
          ]
        },
        {
          "tag": "goodbye",
          "patterns": [
            "Bye",
            "See you later",
            "Goodbye",
            "Have a nice day"
          ],
          "responses": [
            "Goodbye!",
            "See you later!",
            "Have a great day!",
            "Bye! Come back soon."
          ]
        },
        {
          "tag": "start_program",
          "patterns": [
            "Open Notepad",
            "Start Notepad",
            "Launch Notepad",
            "Run Notepad"
          ],
          "responses": [
            "Starting Notepad",
            "Opening Notepad",
            "Launching Notepad"
          ],
          "actions": [
            "notepad.exe"
          ]
        },
        {
          "tag": "start_browser",
          "patterns": [
            "Open browser",
            "Start browser",
            "Launch browser",
            "Run browser"
          ],
          "responses": [
            "Starting browser",
            "Opening browser",
            "Launching browser"
          ],
          "actions": [
            "C:\\Program Files\\Mozilla Firefox\\firefox.exe"
          ]
        }
      ]
    }
    ```

2. **Run the application:**

    ```sh
    dotnet run
    ```

3. **Interact with the chatbot:**

    - Type `Hi` or `Hello` to receive a greeting.
    - Type `Open Notepad` to launch Notepad.
    - Type `Open browser` to launch your default browser.
    - Type `exit` to quit the chatbot.

## Project Structure

- **Program.cs**: Main program file that contains the logic for training, saving, loading the model, and handling user interactions.
- **intents.json**: JSON file containing the intents, patterns, responses, and actions.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgements

- [ML.NET](https://dotnet.microsoft.com/apps/machinelearning-ai/ml-dotnet) - Machine Learning framework for .NET
- [Newtonsoft.Json](https://www.newtonsoft.com/json) - JSON framework for .NET
