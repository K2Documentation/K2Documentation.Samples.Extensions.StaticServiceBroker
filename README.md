# K2Documentation.Samples.Extensions.StaticServiceBroker
Sample code that demonstrates how to build a static (described schema) Service Type

This project contains sample code that demonstrates how to implement a static (described schema) Service Broker, in this case a static broker that exposes Time Zone functions and data. 

For more on static service brokers, please see the topic https://help.k2.com/onlinehelp/k2five/DevRef/current/default.htm#Extend/SmO/StaticBrokers.htm

## Prerequisites
The sample code has the following dependencies: 
* .NET Assemblies (both assemblies are included with K2 client-side tools install and are also included in the project's References folder)
  * SourceCode.SmartObjects.Services.ServiceSDK.dll 

## Getting started
* Use this project to learn how to extend the K2 platform by creating a custom static (described schema) service broker.  
* This specific project exposes time fune data and functions as Service objects, which you can then use to create SmartObjects. 
* This project should compile and run once you have deployed it to yoru K2 enviornment and registsred the service type. 
* Fetch or Pull the appropriate branch of this project for your version of K2. 
* The Master branch is considered the latest, up-to-date version of the sample project. Older versions will be branched. For example, there may be a branch called K2-Five-5.0 that is specific to K2 Five version 5.0. There may be another branch called K2-Five-5.1 that is specific to K2 Five version 5.3. Assume that the master branch is configured for the latest release version of K2 Five. 
* The Visual Studio project contains a folder called "References" where you can find the referenced .NET assemblies or other uncommon assemblies. By default, try to reference these assemblies from your own environment for consistency, but we provide the referenced assemblies as a convenience in case you are not able to locate or use the referenced assemblies in your environment. 
* The Visual Studio project contains a folder called "Resources". This folder contains addiitonal resources that may be required to use the same code, such as K2 deployment packages, sample files, SQL scripts and so on. 
   
## License
This project is licensed under the MIT license, which can be found in LICENSE.

## Notes
 * The sample code is provided as-is without warranty.
 * These sample code projects are not supported by K2 product support. 
 * The sample code is not necessarily comprehensive for all operations, features or functionality. 
 * We only accept code contributions that are compatible with the MIT license (essentially, MIT and Public Domain).
