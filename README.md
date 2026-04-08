# Power Plant API

A REST API that computes a **production plan** for a set of power plants using a **merit-order based heuristic algorithm**.

**.NET 8**  
**ASP.NET Core**  
port **8888**

---

This project solves a simplified version of the **Unit Commitment problem**.  
This implementation uses a **deterministic greedy + adjustment strateg# Power Plant API

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

---

# Build and Run

## Option 1 — .NET CLI

```bash
git clone https://github.com/Matthieu-Jck/PowerPlantApi-CSharp.git
cd PowerPlantApi-CSharp

dotnet run --project PowerPlantApi
```

API available at http://localhost:8888

---

## Option 2 — Docker

```bash
docker build -t PowerPlant-Api .
docker run -p 8888:8888 PowerPlant-Api
```

Detached mode:

```bash
docker run -d -p 8888:8888 --name PowerPlant-Api PowerPlant-Api
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
```y**:  
Not mathematically guaranteed optimal.

---

# Requirements

.NET 8 SDK, Docker 20+

---

# Build and Run

## Option 1 — .NET CLI

```bash
git clone https://github.com/YOUR_USERNAME/PowerPlant-Api.git
cd PowerPlant-Api

dotnet run --project PowerPlantApi
```

API available at http://localhost:8888

---

## Option 2 — Docker

```bash
docker build -t PowerPlant-Api .
docker run -p 8888:8888 PowerPlant-Api
```

Detached mode:

```bash
docker run -d -p 8888:8888 --name PowerPlant-Api PowerPlant-Api
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
