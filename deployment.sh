#!/usr/bin/env bash

echo "*** Root version ***"  
echo "*** adding zip ... ***"  
sudo apt-get update
sudo apt-get install zip gzip tar

echo "*** changing directoriy for command ... ***"  
cd Avt.Web.Backend
echo "*** restoring packages ... ***"  
dotnet restore
echo "*** building ... ***"  
dotnet build -c Release
echo "*** publishing ... ***"  
dotnet publish --no-restore -c Release

echo "*** Creating suffix for file name ... ***"  

tail=< /dev/urandom tr -dc _A-Z-a-z-0-9 | head -c${5:-16};

echo "*** Zipping the artifact ... ***"  

zip -r "$backend_$tail.zip" bin/Release/netcoreapp2.1/publish/

echo "*** All Finished :) ... ***"  

