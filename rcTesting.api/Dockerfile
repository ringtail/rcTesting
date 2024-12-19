# 使用構建好的基礎鏡像作為編譯環境
FROM ringtail/netcore:2.2 AS base
# 設置編譯的臨時路徑
WORKDIR /app

# 第一階段：構建代碼包
FROM base as build
# 將編譯工程拷貝過來
COPY *.csproj ./
RUN dotnet restore

# 進行編譯動作
COPY . ./
RUN dotnet publish -c Release -o /out

# 第二階段：將代碼包進行拷貝，實現對外服務
FROM base as production

WORKDIR /app

# 設置service fabric環境
ENV Fabric_InstallPath /opt/microsoft/servicefabric/bin/Fabric/Fabric.Code
ENV LD_LIBRARY_PATH /opt/microsoft/servicefabric/bin/Fabric/Fabric.Code

# 拷貝編譯後的工程
COPY --from=build /out .

# 啟動應用
ENTRYPOINT ["dotnet", "rcTesting.api.dll"]