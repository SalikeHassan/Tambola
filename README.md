# Tambola Claim Validator
A Restful API for managing Tambola game claim validation.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Setup](#setup)
- [Running the API](#running-the-api)
- [Running Unit Tests](#running-unit-tests)
- [API Endpoint](#api-endpoint)
- [Postman Collection](#postman-collection)
- [CURL Command for API Requests](#curl-command-for-api-requests)

## Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- IDE (Visual Studio 2022+, VS Code)
- [Postman](https://www.postman.com/) or `curl` for API testing
- Git

## Setup
1. Clone Repository
  ```bash
  git clone https://github.com/SalikeHassan/Tambola.git
  ```
2. Restore dependencies
 ```bash
 cd Tambola
 dotnet restore Tambola.sln
 ```
## Running the API
   ```bash
    cd Tambola.Api
    dotnet run
   ```

## Running Unit Tests
   ```bash
   cd Tambola.Api.Test
   dotnet test
   ```
## API Endpoints
### Validate Claim
**Method:** `POST`  
**Endpoint:** `api/v1.0/claim`

## Postman Collection
You can import the Postman collection by following the instructions in the official documentation:  
 [Importing an API in Postman](https://learning.postman.com/docs/designing-and-developing-your-api/importing-an-api/)

### Postman Collection File
You can find the Postman collection JSON file in the repository:  
[Tambola Postman Collection](https://github.com/SalikeHassan/Tambola/blob/main/Tambola.postman_collection.json)

## CURL Command for API Requests
#### Valid Claim
```bash
curl -X POST "http://localhost:5004/api/v1.0/claim" \
     -H "Content-Type: application/json" \
     -d '{
          "PlayerId": "f3f3f1e0-7e47-4f6c-9a51-71e3f379b862",
          "TicketNumbers": [
              [4, 16, null, null, 48, null, 63, 76, null],
              [7, null, 23, 38, null, 52, null, null, 80],
              [9, null, 25, null, null, 56, 64, null, 83]
          ],
          "AnnouncedNumbers": [4, 16, 48, 63, 76],
          "ClaimType": "TopLine"
      }'
```
#### Invalid Claim
```bash
curl -X POST "http://localhost:5004/api/v1.0/claim" \
     -H "Content-Type: application/json" \
     -d '{
          "PlayerId": "68bb9212-7c0a-4782-bb83-f2370c8377e4",
          "TicketNumbers": [
              [4, 16, null, null, 48, null, 63, 76, null],
              [7, null, 23, 38, null, 52, null, null, 80],
              [9, null, 25, null, null, 56, 64, null, 83]
          ],
          "AnnouncedNumbers": [4, 16, 63, 76],
          "ClaimType": "TopLine"
      }'
```
