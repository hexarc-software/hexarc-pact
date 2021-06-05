#!/bin/sh
source .env
PACKAGE="./nupkg/Hexarc.Pact.AspNetCore.1.8.1.nupkg"

if [ -z $NUGET_API_KEY ]; then
  echo "No NUGET_API_KEY found in .env file";
else
  echo "Current NUGET_API_KEY =$NUGET_API_KEY";
  read -p "Do you want to publish: $PACKAGE? Yes[Y]/No[N]" yn
  case $yn in
    [Yy]* )
      echo "Publishing";
      dotnet nuget push $PACKAGE -k $NUGET_API_KEY -s https://api.nuget.org/v3/index.json
      break;;
    [Nn]* )
      echo "Canceled";
      break;;
    * )
      echo "Invalid options";
      break;;
  esac
fi;
