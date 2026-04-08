# Power Plant API

A REST API that computes a **production plan** for a set of power plants using a **merit-order based heuristic algorithm**.

**.NET 8**  
**ASP.NET Core**  
port **8888**

---

This project solves a simplified version of the **Unit Commitment problem**.  
This implementation uses a **deterministic greedy + adjustment strategy**:  
Not mathematically guaranteed optimal.

---

# Requirements

.NET 8 SDK, Docker 20+

# Clone 

```bash
git clone https://github.com/Matthieu-Jck/PowerPlantApi-CSharp.git
cd PowerPlantApi-CSharp
```

# Build and Run

## Option 1 — .NET CLI

```bash
dotnet run --project PowerPlantApi
```

API available at http://localhost:8888

---

## Option 2 — Docker

```bash
cd PowerPlantApi-CSharp
docker build -t PowerPlantApi-CSharp .
docker run -p 8888:8888 PowerPlantApi-CSharp
```

Detached mode:

```bash
docker run -d -p 8888:8888 --name PowerPlantApi-CSharp PowerPlantApi-CSharp
```

---

# Example Usage

```bash
curl -X POST http://localhost:8888/productionplan \
  -H "Content-Type: application/json" \
  -d @example_payloads/payload3.json
```

---

# Running Tests

```bash
dotnet test
```
