# 🎯 AI Resume Scoring API

An AI-powered RESTful API for scoring candidate resumes against job descriptions using OpenAI's GPT model.
Built with **.NET 8**, **Entity Framework Core**, **Azure Blob Storage**, and **JWT Authentication**.

---

## 🚀 Features

✅ Upload, download, and delete candidate resumes (PDF format).  
✅ Secure resume storage on **Azure Blob Storage**.  
✅ Auto-score resumes against job descriptions using **OpenAI GPT API**.  
✅ JWT-based authentication for protected endpoints.  
✅ Interactive API documentation via **Swagger**.

---

## 🗂️ Project Structure

```
ResumeScoringAPI
├── API (Controllers)
├── Application (DTOs & Interfaces)
├── Domain (Entities)
├── Infrastructure (Repositories, Services, Utilities)
├── Persistence (Database)
└── Program.cs
```

---

## 🔐 Authentication

All endpoints (except `/api/auth/login`) require a **JWT Bearer Token**.

Use this demo login:

```
POST /api/auth/login
{
  "username": "admin",
  "password": "password"
}
```

---

## 🌐 API Endpoints

| Method | Endpoint | Description |
|:-----:|:--------------------:|:-------------------------:|
| POST  | `/api/auth/login` | Get JWT Token |
| POST  | `/api/resumes/upload` | Upload a PDF resume |
| GET   | `/api/resumes/download/{fileName}` | Download a resume PDF |
| DELETE| `/api/resumes/delete/{fileName}` | Delete a resume PDF |
| POST  | `/api/resumes/{resumeId}/score/{jobId}` | Score resume against job description |

---

## 🧩 Tech Stack

- **.NET 8 Web API**
- **Entity Framework Core**
- **Azure Blob Storage**
- **OpenAI GPT API**
- **JWT Authentication**
- **Swagger / OpenAPI**

---

## ⚙️ Environment Variables

Configure `appsettings.Development.json` or UserSecrets:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your-SQL-Server-Connection-String"
  },
  "AzureStorage": {
    "ConnectionString": "Your-Azure-Blob-Storage-Connection-String",
    "ContainerName": "your-container-name"
  },
  "OpenAI": {
    "ApiKey": "your-openai-api-key"
  },
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "your-issuer"
  }
}
```

---

## 📝 How Resume Scoring Works

1. Resume uploaded & stored in **Azure Blob Storage**.
2. Text extracted from PDF using **PdfPig**.
3. Sent to **OpenAI GPT API** alongside job description.
4. Score & reasoning saved to the database and returned.

---

## 📄 Example Scoring Response

```json
{
  "ResumeId": 5,
  "JobId": 2,
  "Score": 85.0,
  "Reason": "Strong match in required skills and experience."
}
```

---

## 📌 Setup & Run

```bash
git clone https://github.com/your-username/ResumeScoringAPI.git
cd ResumeScoringAPI
dotnet ef database update
dotnet run
```

Swagger UI will be available at:
```
https://localhost:{port}/swagger
```

---

## 📥 Postman Collection

A ready-to-use Postman Collection is available in this repo:  
`ResumeScoringAPI.postman_collection.json`

---

## ⭐️ License

This project is licensed under the **MIT License**.
