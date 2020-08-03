#!/bin/bash

# Copy deploy folder contents
mkdir ./_publish
cp ./deploca-package.yml ./_publish/

# Publish Api Project
cd ./src/Knowledgebase.Api
dotnet publish -c Release -o ../../_publish/ui-and-api --self-contained false
cd ..
cd ..

# Publish UI
cd ./src/Knowledgebase.UI
npm install
npm run build
npm run export
mv ./dist ../../_publish/ui-and-api/wwwroot
cd ..
cd ..

echo Publish finished successfully
