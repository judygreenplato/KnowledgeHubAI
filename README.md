# 📚 KnowledgeHubAI

KnowledgeHubAI is an AI-powered knowledge management system built with ASP.NET Core, Clean Architecture, React, and OpenAI. It allows users to upload PDF documents, extract text, generate embeddings, perform semantic search, and answer questions using Retrieval-Augmented Generation (RAG).

---

## 🚀 Features

### Authentication & Authorization
- User Registration
- User Login
- JWT Authentication
- Refresh Token Authentication
- Role-based Authorization (Admin & User)

### Article Management
- Create Article
- Update Article
- Delete Article
- List Articles
- Pagination
- AutoMapper
- FluentValidation

### Document Management
- Upload PDF Documents
- Store Uploaded Files
- Extract PDF Text (PdfPig)
- Store Extracted Text
- Chunk Documents
- Generate OpenAI Embeddings
- Store Embeddings in Database

### AI Features
- OpenAI Embeddings
- Semantic Search 
- Retrieval-Augmented Generation (RAG) 

---

# 🏗 Architecture

This project follows **Clean Architecture** principles.

```
KnowledgeHubAI
│
├── KnowledgeHub.API
│
├── KnowledgeHub.Application
│
├── KnowledgeHub.Domain
│
└── KnowledgeHub.Infrastructure
```

The architecture separates:

- Presentation Layer
- Application Layer
- Domain Layer
- Infrastructure Layer

which improves maintainability, scalability, and testability.

---

# 🛠 Technologies Used

### Backend

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQLite
- JWT Authentication
- AutoMapper
- FluentValidation

### AI

- OpenAI API
- text-embedding-3-small
- Retrieval-Augmented Generation (RAG)

### PDF Processing

- PdfPig

### Frontend

- React
- React Router
- Axios

### Tools

- Visual Studio 2022
- Swagger
- Git
- GitHub

---

# 📂 Project Structure

```
src/

KnowledgeHub.API

KnowledgeHub.Application

KnowledgeHub.Domain

KnowledgeHub.Infrastructure
```

---

# 📄 RAG Pipeline

```
Upload PDF
        │
        ▼
Extract Text
        │
        ▼
Chunk Document
        │
        ▼
Generate Embeddings
        │
        ▼
Store Embeddings
        │
        ▼
Semantic Search
        │
        ▼
Retrieve Relevant Chunks
        │
        ▼
OpenAI
        │
        ▼
AI Generated Answer
```

---

# 🔒 Security

- JWT Authentication
- Refresh Tokens
- Role-based Authorization
- Password Hashing
- Protected Endpoints

---

# 🗄 Database

Current database:

- SQLite

Main Tables

- Users
- Articles
- Documents
- DocumentChunks
- DocumentEmbeddings
- RefreshTokens

---

# ⚙ Getting Started

## Clone Repository

```bash
git clone https://github.com/judygreenplato/KnowledgeHubAI.git
```

---

## Install Packages

```bash
dotnet restore
```

---

## Apply Database

```bash
dotnet ef database update
```

---

## Configure OpenAI

Use ASP.NET Core User Secrets:

```bash
dotnet user-secrets init

dotnet user-secrets set "OpenAI:ApiKey" "YOUR_API_KEY"
```

---

## Run

```bash
dotnet run
```

Swagger:

```
https://localhost:xxxx/swagger
```



# Future Improvements

- Semantic Search
- Ask AI Chat Interface
- Redis Caching
- RabbitMQ
- Docker
- Azure Blob Storage
- Azure Deployment
- CI/CD
- Unit Testing
- Integration Testing
- Vector Database
- OCR Support
- OpenTelemetry
- Kubernetes


---

# Author

Judy Green C A

Master's Student – Software Engineering

.NET Developer

Sweden 🇸🇪

---

