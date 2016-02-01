# GetVSTSBuildUsage
This is a code sample to show how to get information about builds  run under a Visual Studio Team Services (VSTS) account or TFS collection.

Example usage:
GetVSTSBuildUsage.exe [Path to VSTS Account or TFS collection] [Minimum Build Finish Date] [Maximum Build Finish Date]

An example when calling VSTS to get the build usage for a month: 

GetVSTSBuildUsage.exe https://myaccount.visualstudio.com/defaultcollection 1/1/2016 1/31/2016
