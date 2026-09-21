# AI Document Q&A

An AI-powered document question-answering application that allows users to upload documents and ask questions using natural language.

The application is designed to explore **Retrieval-Augmented Generation (RAG)**, document processing, vector search, authentication, and AI-powered question answering.

## 🎯 Project Objective

The goal of this project is to build a complete AI application that can:

* Upload PDF and DOCX documents
* Extract and process document content
* Index documents for efficient retrieval
* Answer user questions using AI
* Provide source citations for generated answers
* Maintain conversation history
* Provide feedback on AI-generated answers
* Provide analytics for administrators

## 🚀 Planned Features

* [ ] User Registration & Login
* [ ] Employee and Admin roles
* [ ] PDF document upload
* [ ] DOCX document upload
* [ ] Document text extraction
* [ ] Document chunking
* [ ] Document indexing
* [ ] Vector search
* [ ] AI-powered Q&A
* [ ] Source citations
* [ ] Conversation history
* [ ] Answer feedback
* [ ] Admin analytics
* [ ] Docker support
* [ ] Azure deployment

## 🏗️ Planned Architecture

```text
User
  ↓
Angular Frontend
  ↓
ASP.NET Core Web API
  ↓
Document Processing
  ↓
Text Chunking
  ↓
Embeddings
  ↓
Vector Database
  ↓
Relevant Context Retrieval
  ↓
LLM
  ↓
Answer + Source Citations
```

## 🛠️ Technology Stack

### Frontend

* Angular
* TypeScript
* HTML
* CSS

### Backend

* ASP.NET Core Web API
* C#
* JWT Authentication

### Database

* PostgreSQL
* pgvector

### AI

* Large Language Model (LLM)
* Embeddings
* Retrieval-Augmented Generation (RAG)

### DevOps

* Git
* Docker
* Azure

## 📂 Project Structure

```text
AI-Document-QA/
│
├── frontend/          # Angular application
├── backend/           # ASP.NET Core Web API
├── database/          # Database scripts
├── documents/         # Document-related resources
├── .gitignore
├── .env.example
└── README.md
```

## 🔄 RAG Workflow

```text
Document Upload
      ↓
Text Extraction
      ↓
Text Chunking
      ↓
Embedding Generation
      ↓
Vector Storage
      ↓
User Question
      ↓
Similarity Search
      ↓
Relevant Context
      ↓
LLM
      ↓
Answer + Source Citation
```

## 📌 Project Status

**Status: 🚧 In Development**

This project is being developed as a hands-on learning and portfolio project focused on modern AI application development.

## 🗺️ Development Roadmap

### Phase 1 — Project Setup

* [x] Repository setup
* [x] README
* [x] Git configuration
* [ ] Frontend setup
* [ ] Backend setup
* [ ] Database setup

### Phase 2 — Authentication

* [ ] User registration
* [ ] Login
* [ ] JWT authentication
* [ ] Role-based authorization

### Phase 3 — Document Management

* [ ] PDF upload
* [ ] DOCX upload
* [ ] Text extraction
* [ ] Document indexing

### Phase 4 — AI / RAG

* [ ] Embeddings
* [ ] Vector database
* [ ] Similarity search
* [ ] AI Q&A
* [ ] Source citations

### Phase 5 — User Experience

* [ ] Conversation history
* [ ] Answer feedback
* [ ] Admin dashboard
* [ ] Analytics

### Phase 6 — Deployment

* [ ] Docker
* [ ] Azure deployment
* [ ] Production configuration
* [ ] Testing

## 🔐 Security

Sensitive information must not be committed to this repository.

Examples:

* API keys
* Passwords
* Database credentials
* JWT secrets
* Access tokens
* Private certificates

Use environment variables or local configuration files for sensitive values.

## 📄 License

This project is currently intended for educational and portfolio purposes.
