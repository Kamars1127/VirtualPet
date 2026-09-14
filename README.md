# VirtualPet

VirtualPet 是一個使用 ASP.NET Core 10 開發的虛擬寵物養成 Web Side Project。<br/>
<br/>
專案使用 ASP.NET Core MVC、Razor View、Entity Framework Core、SQL Server 與 ASP.NET Core Identity，實作玩家登入、寵物養成、狀態變化、進化規則、AJAX 即時互動與操作紀錄。<br/><br/>
本專案主要用來練習 ASP.NET Core Web Application 的完整開發流程，以及分層架構、Domain Model、Repository、Testing 與 Docker。


## 技術
-  C# / .NET 10
-  ASP.NET Core MVC
-  Razor View
-  JavaScript / AJAX
-  ASP.NET Core Identity
-  Entity Framework Core
-  SQL Server
-  xUnit
-  Docker / Docker Compose

## 主要功能
-  玩家註冊、登入與登出
-  建立與管理寵物
-  Feed / Play / Rest / Training
-  寵物經驗值與進化系統
-  Satiety / Happiness / Energy 狀態
-  寵物狀態隨時間變化
-  寵物操作歷史紀錄
-  AJAX 即時更新畫面
-  REST API
-  Global Exception Handling
-  Unit Test / Integration Test
-  Docker 容器化


## 專案架構
```text
VirtualPet/
├── src/
│   ├── VirtualPet.Domain/
│   ├── VirtualPet.Application/
│   ├── VirtualPet.Infrastructure/
│   └── VirtualPet.Web/
└── tests/
    ├── VirtualPet.Domain.Tests/
    └── VirtualPet.Web.IntegrationTests/
```
### Domain
- 負責 Pet、User、Value Object、Domain Behavior、進化規則等核心商業邏輯。

### Application
- 負責 Use Case、DTO、Mapping 與 Repository Interface。

### Infrastructure
- 負責 EF Core、SQL Server、Repository、Identity 與 Migration。

### Web
- 負責 MVC Controller、Razor View、AJAX、API 與 Web UI。

## 寵物系統
### Pet 主要包含：
- Name、Species、Evolution Stage、Level、Experience、Satiety、Happiness、Energy

### 玩家可以對寵物執行：
- Feed Play Rest Training。
- 操作後會更新寵物狀態、記錄 History，並檢查是否符合進化條件。

## 時間系統
- 寵物狀態會依照離線時間自動變化。
- 系統透過 LastStatusUpdateAt 計算經過時間，再更新：Satiety、Happiness、Energy。
- 因此不需要持續執行 Timer 或 Background Job。

## AJAX
- 寵物 Detail 頁面使用 JavaScript Fetch API。
- 玩家操作 Feed、Play、Rest 或 Training 時，不需要重新載入整個頁面即可更新寵物狀態。
- User Action ➝ AJAX ➝ Controller ➝ Application Service ➝ Domain ➝ JSON ➝ Update UI。

## Database
- 使用：SQL Server + Entity Framework Core。
- 並透過 EF Core Migration 管理 Database Schema。

## Testing
- 專案包含：VirtualPet.Domain.Tests、VirtualPet.Web.IntegrationTests
- 使用 xUnit 測試 Domain Logic，以及 ASP.NET Core Web Application 的基本整合行為。

## Docker
- 專案提供：Dockerfile、docker-compose.yml。
- 可同時建立：ASP.NET Core Web + SQL Server。
- 執行：`docker compose up --build、。
- 預設網站：http://localhost:8080。

## 開發目的
過去主要開發經驗以 C# Windows Desktop Application 為主，<br/>
因此透過此專案進一步實作 ASP.NET Core Web 開發，並練習：
- MVC、Domain Design、Database、Authentication、AJAX、REST API、Testing、Docker。
<br/><br/>
將既有的 C# 與應用程式開發經驗延伸至 Web Application。
