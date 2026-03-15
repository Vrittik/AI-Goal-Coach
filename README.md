# AI Goal Coach

AI Goal Coach is a system that converts vague goals into structured and actionable **SMART goals** using a Large Language Model (LLM).

The project demonstrates how to build **production-style AI applications** with:

- Structured AI outputs
- Clean architecture
- Observability and telemetry
- Guardrails for unsafe inputs
- Evaluation testing for regression detection
- Model fallback for reliability
- Basic centralized exception handling

-----------------------------
Submitted By - Vrittik Sharma
-----------------------------

---

# Problem Overview

Employees often struggle to translate vague aspirations into actionable plans.

Example vague goal:

```
I want to get better at sales
```

The AI system refines this into a structured goal:

```json
{
  "refined_goal": "Increase sales conversion rate by 15% within the next 90 days by improving prospecting and closing techniques.",
  "key_results": [
    "Complete one advanced sales training course",
    "Increase weekly prospecting calls from 50 to 80",
    "Conduct two mock sales pitches per month with a mentor",
    "Maintain complete CRM documentation for all leads"
  ],
  "confidence_score": 9
}
```

This structure ensures goals follow the **SMART framework**.

---

# Tech Stack

## Backend
- .NET 8 Web API
- Clean architecture
- Dependency Injection
- HttpClientFactory

## Frontend
- React.js (Vite)

## AI Provider
- Google Gemini Flash (Free Tier)

## Testing
- Custom evaluation harness (console project)

---

# System Architecture

The system follows a layered architecture to separate responsibilities.

```
React UI
   ↓
API Controller
   ↓
Domain Logic
   ↓
AI Client
   ↓
HTTP Client Service
   ↓
Gemini API
```

Project structure:

```
AIGoalCoach.Api
AIGoalCoach.Domain
AIGoalCoach.Repository
AIGoalCoach.Models
AIGoalCoach.AIClient
AIGoalCoach.HttpClientServices
AIGoalCoach.Evals
ai-goal-coach-ui
```

Responsibilities:

| Layer | Responsibility |
|------|------|
API | HTTP endpoints |
Domain | Business logic and validation |
AI Client | AI provider integration and fallback routing |
HttpClientService | HTTP transport and latency tracking |
Repository | Data persistence |
Eval Project | AI evaluation harness |

---

# Structured AI Output

The AI model is instructed to return **strict JSON output** to avoid fragile parsing.

Expected schema:

```
{
  refined_goal: string
  key_results: string[]
  confidence_score: integer (1-10)
}
```

This allows reliable deserialization into backend models.

---

# Guardrails

Basic safeguards prevent unsafe inputs from being processed.

Validation includes:

- Empty input detection
- Adversarial input detection
- Confidence score validation

Example adversarial input:

```
DROP TABLE users;
```

Such inputs are rejected or returned with a low confidence score.

---

## Switching AI Models / Providers

The system is designed to support multiple AI providers through configuration and a client abstraction.

All model configurations are defined in `appsettings.json` under the `Clients` section.  
Each provider (Gemini, OpenAI, Anthropic, etc.) can define multiple models with their API key and endpoint.

Example configuration:

```
{
  "Clients": {
    "Gemini": {
      "Gemini_Flash": {
        "ApiKey": "YOUR_API_KEY",
        "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent"
      },
      "Gemini_Flash_V2": {
        "ApiKey": "YOUR_API_KEY",
        "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent"
      }
    },
    "Anthropic": {
      "Claude_Sonnet_3_5": {
        "ApiKey": "YOUR_API_KEY",
        "Url": "https://api.anthropic.com/v1/messages"
      }
    }
  }
}
```

How to Switch Models

To switch from one model to another (for example Gemini → Claude):

1. Add the new model configuration to appsettings.json.

2. Implement a new AI client (for example ClaudeAIClient) that implements the IAIClient interface.

3. Register the client in dependency injection.

4. Update the clientId used by the AI client to point to the desired model.

5. Define Your client in Clients Folder in AIClient Project

6. Define Your client's configuration AI_Goal_Coach.Models.ClientConfig provider to read the values from appsettings and register it in AIClientServiceCollection 

7. Once you inject the dependency in Program.cs, your Client will automatically be called from the domain logic as it depends on the interface

Because the application uses a shared IAIClient abstraction and a centralized HTTP service, switching AI providers requires minimal changes and does not affect the rest of the system.


# Observability

Each AI request logs the following:

- Input prompt
- AI output
- Latency
- Token usage

Example logs:

```
AI Request | Client:Gemini_Flash | Input:I want to improve coding skills

HTTP Call | Client:Gemini_Flash | Latency:842ms | Status:OK

AI Response | Client:Gemini_Flash
PromptTokens:86
CompletionTokens:142
```

Logging responsibilities:

| Component | Responsibility |
|------|------|
AI Client | Input and output logging |
HttpClientService | Latency and HTTP telemetry |
Telemetry Service | Structured logging |

---

# Model Fallback Strategy

To improve reliability, the system implements a **simple model fallback strategy**.

If the primary model fails due to a transient error, timeout, or rate limit, the system automatically retries using a secondary model.

Example flow:

```
Gemini_Flash (primary)
        ↓ failure
Gemini_Flash_V2 (fallback)
```

This ensures that the system can still respond even if one AI model is temporarily unavailable.

The fallback logic is implemented inside the **AI Client layer**, keeping the HTTP transport layer independent of AI model behavior.

---

# Configuration Driven AI Clients

AI providers and models are defined in configuration.

Example configuration:

```json
{
  "Clients": {
    "Gemini": {
      "Gemini_Flash": {
        "ApiKey": "...",
        "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent"
      },
      "Gemini_Flash_V2": {
        "ApiKey": "...",
        "Url": "https://generativelanguage.googleapis.com/v1beta/models/gemini-flash-latest:generateContent"
      }
    }
  }
}
```

# Important Note
Please use your api key when running the code locally for these models (Gemini, Claude or OpenAI)
The private keys are decomissioned if pushed to Github because they are
free tier and aren't publicly accessible

The keys would be stored in secrets manager and read from there instead of storing
in appsettings and pushing to Github

Benefits:

- Easy model switching
- Multi-provider support
- No code changes when adding models
- Enables model fallback strategies

---

# Global Exception Handling

The API includes a simple global exception handler to ensure consistent error responses.

Unhandled exceptions are captured in the middleware pipeline and returned as a JSON response with status code **500**.

Example response:

```json
{
  "message": "Input is not a valid goal."
}
```

This approach:

- keeps controllers clean
- centralizes error handling
- ensures consistent API responses

---

# AI Evaluation Framework

A separate evaluation project verifies AI output quality.

Project:

```
AIGoalCoach.Evals
```

Tests include:

- Valid goal generation
- Output schema validation
- Adversarial input testing

Example test cases:

```
I want to improve my coding skills
Become better at public speaking
I want to learn system design
```

Adversarial test:

```
DROP TABLE users;
```

Example evaluation output:

```
Running AI Eval Tests...

Testing: Improve coding skills
PASS

Testing: Public speaking
PASS

Testing: Learn system design
PASS

Adversarial Test
PASS
```

This helps detect regressions if prompts or models change.

---

# UI

Please find the UI Repo here - https://github.com/Vrittik/AI-Goal-Coach-UI.git

The React interface provides:

1. Goal input textbox
2. AI-powered goal refinement
3. Display of refined goals and key results
4. Ability to save goals
5. View saved goals

---

# Setup Instructions

## Clone Repository

```
git clone <repo-url>
```

---

## Run Backend

```
dotnet run
```

---

## Run Frontend

```
cd ai-goal-coach-ui
npm install
npm run dev
```

---

## Run Evaluation Tests

```
dotnet run --project AIGoalCoach.Evals
```

---

# System Walkthrough (15 Minute Demo)

The system walkthrough is structured into three sections.

## 1. Application Demo (0–5 minutes)

- Showing the UI
- Entering a vague goal
- Clicking **Refine**
- Displaying the structured AI output
- Showing console logs demonstrating:
  - input
  - output
  - latency
  - token usage

## 2. Evaluation Framework (5–10 minutes)

Running the evaluation project:

```
dotnet run --project AIGoalCoach.Evals
```

Demonstrating:

- structured output validation
- adversarial test case
- automated regression testing

## 3. System Design Explanation (10–15 minutes)

Explaining architecture and design decisions:

- layered architecture
- configuration-driven AI clients
- HTTP abstraction layer
- telemetry logging
- model fallback strategy

Discussing how the system could scale and how new models can be integrated without major changes.

---

# Architecture Decisions (ADR)

## Why Gemini Flash?

Reasons:

- Free API access
- Fast response time
- Reliable structured output

Trade-off:

- Smaller context window compared to larger models.

---

## Why Structured JSON Output?

Regex parsing of AI responses is unreliable.

Using structured JSON provides:

- Deterministic output
- Strong schema validation
- Simpler integration

---

## Why a Separate HTTP Layer?

The HTTP layer isolates transport concerns such as:

- retries
- latency measurement
- logging

This allows AI clients to focus only on model integration.

---

## Why a Dedicated Evaluation Project?

AI outputs are inherently non-deterministic.

An evaluation harness helps ensure:

- prompt changes do not break output structure
- model changes do not introduce regressions
- adversarial inputs are handled safely

---

# Scaling Considerations (10,000 Users)

If the system scaled to 10,000 users, improvements would include:

1. Prompt caching
2. Queue-based AI processing
3. Model fallback strategies
4. Persistent storage (Postgres / DynamoDB)
5. Rate limiting for goal refine api
6. Distributed tracing
7. Circuit breakers for AI provider failures

---

# Future Improvements

Potential enhancements include:

- Prompt injection detection
- AI cost monitoring
- distributed tracing
- advanced model routing

---

# Conclusion

This project demonstrates how to build **production-style AI systems** with:

- structured outputs
- clean architecture
- observability
- evaluation testing
- resilient AI integrations.