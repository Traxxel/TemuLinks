#!/bin/bash

# API publish
dotnet publish /Users/stm/repos/temulinks/src/TemuLinks.WebAPI/TemuLinks.WebAPI.csproj -c Release -r linux-x64 --self-contained false -o /Users/stm/repos/temulinks/publish/webapi

rsync -avz /Users/stm/repos/temulinks/publish/webapi/ root@95.217.0.14:/var/www/temulinks/api/


# WWW publish
dotnet publish /Users/stm/repos/temulinks/src/TemuLinks.WWW/TemuLinks.WWW.csproj -c Release -o /Users/stm/repos/temulinks/publish/www
rsync -avz /Users/stm/repos/temulinks/publish/www/wwwroot/ root@95.217.0.14:/var/www/temulinks/www/