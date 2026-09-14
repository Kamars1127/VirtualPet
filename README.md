# VirtualPet

VirtualPet 是一個使用 ASP.NET Core 10 開發的虛擬寵物養成 Web Side Project。<br/>
<br/>
專案使用 ASP.NET Core MVC、Razor View、Entity Framework Core、SQL Server 與 ASP.NET Core Identity，實作玩家登入、寵物養成、狀態變化、進化規則、AJAX 即時互動與操作紀錄。<br/>
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
&nbsp;&nbsp;&nbsp;&nbsp;`負責 Pet、User、Value Object、Domain Behavior、進化規則等核心商業邏輯。`

### Application
- 負責 Use Case、DTO、Mapping 與 Repository Interface。

### Infrastructure
- 負責 EF Core、SQL Server、Repository、Identity 與 Migration。

### Web
- 負責 MVC Controller、Razor View、AJAX、API 與 Web UI。

## 寵物系統
### Pet 主要包含：
&nbsp;&nbsp;&nbsp;&nbsp;`Name、Species、Evolution Stage、Level、Experience、Satiety、Happiness、Energy`
### 玩家可以對寵物執行：
