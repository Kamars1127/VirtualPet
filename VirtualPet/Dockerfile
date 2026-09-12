# =========================================================
# ---- Base Stage ----
# 建立最終執行 ASP.NET Core Web App 所需要的 Runtime 環境
# 注意：這裡只有 ASP.NET Core Runtime，沒有 SDK
# =========================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# 設定 Container 內的工作目錄
WORKDIR /app

# 宣告 Container 預計使用 8080 Port
EXPOSE 8080

# 設定 ASP.NET Core 在 Container 內監聽 8080 Port
ENV ASPNETCORE_HTTP_PORTS=8080


# =========================================================
# ---- Build Stage ----
# 使用完整的 .NET 10 SDK 進行套件還原與專案發佈
# =========================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# 建置組態，預設使用 Release
ARG BUILD_CONFIGURATION=Release

# 設定 Build Stage 的工作目錄
WORKDIR /src


# ---------------------------------------------------------
# 先複製各專案的 csproj
#
# 不直接一開始 COPY 全部原始碼的原因：
# Docker 可以針對這一層建立 Layer Cache。
# 如果程式碼有修改，但 csproj / NuGet 套件沒有改變，
# 下次 docker build 時，就可以重複使用 restore 的快取，
# 不必重新下載所有 NuGet 套件。
#
# 這裡只複製「會被 publish 用到」的四個專案，
# 測試專案（Domain.Tests / Web.IntegrationTests）
# 不會進到最終 Image，所以不需要複製。
# ---------------------------------------------------------
COPY ["VirtualPet.Domain/VirtualPet.Domain.csproj", "VirtualPet.Domain/"]
COPY ["VirtualPet.Application/VirtualPet.Application.csproj", "VirtualPet.Application/"]
COPY ["VirtualPet.Infrastructure/VirtualPet.Infrastructure.csproj", "VirtualPet.Infrastructure/"]
COPY ["VirtualPet.Web/VirtualPet.Web.csproj", "VirtualPet.Web/"]


# ---------------------------------------------------------
# 還原 VirtualPet.Web 所需要的 NuGet 套件
#
# 因為 VirtualPet.Web 會透過 ProjectReference
# 參考 Application / Infrastructure，
# 而這些專案又可能參考 Domain，
# 所以前面需要先將各專案 csproj 複製進來。
# ---------------------------------------------------------
RUN dotnet restore "VirtualPet.Web/VirtualPet.Web.csproj"


# ---------------------------------------------------------
# 複製 Solution Root 內其餘所有檔案到 Container
#
# 實際會被複製的內容由 .dockerignore 控制，
# 目前已排除 bin/ obj/ .vs/ 測試專案等不需要的內容，
# 所以這裡不會把整個 repo 都塞進 Image。
# ---------------------------------------------------------
COPY . .


# ---------------------------------------------------------
# 切換到真正要發佈的 ASP.NET Core Web 專案
# ---------------------------------------------------------
WORKDIR "/src/VirtualPet.Web"


# ---------------------------------------------------------
# 發佈 VirtualPet.Web
#
# -c：
#   指定 Build Configuration，例如 Release
#
# -o：
#   指定 publish 結果輸出到 /app/publish
#
# /p:UseAppHost=false：
#   不建立平台專屬 executable，
#   最後使用 dotnet VirtualPet.Web.dll 啟動
# ---------------------------------------------------------
RUN dotnet publish \
    "VirtualPet.Web.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false


# =========================================================
# ---- Final Stage ----
# 建立真正要執行的最終 Image
#
# 直接繼承前面定義好的 base Stage，
# 因此這一層已經包含：
# - ASP.NET Core Runtime
# - WORKDIR /app
# - Port 8080 設定
# =========================================================
FROM base AS final


# ---------------------------------------------------------
# 從 Build Stage 複製 publish 結果
#
# --from=build：
#   指定從前面的 build Stage 取檔案
#
# /app/publish：
#   dotnet publish 的輸出位置
#
# .：
#   代表目前 WORKDIR，也就是 /app（繼承自 base）
# ---------------------------------------------------------
COPY --from=build /app/publish .


# ---------------------------------------------------------
# 使用 .NET Container Image 內建的非 root 使用者執行程式
# 可以降低 Container 以 root 身份執行所產生的安全風險
# ---------------------------------------------------------
USER $APP_UID


# ---------------------------------------------------------
# Container 啟動時執行的指令
#
# 等同於：
# dotnet VirtualPet.Web.dll
# ---------------------------------------------------------
ENTRYPOINT ["dotnet", "VirtualPet.Web.dll"]
