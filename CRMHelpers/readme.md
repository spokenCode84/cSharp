# CRM Helpers

This is an external Web API that will be used by CRM for Caching.

Ideas for future usage: 
	processing from a message bus
	external logging
	storing & accessing external data (link to Staging SQL DB)

## Getting Started

Set WebAPI to default Project.  Debug Web API

### Prerequisites

Redis needs to be installed on IIS server running (or localhost).
The Redis windows service also needs to be started and running (in services.msc)

### Debugging

1. Install Redis on localhost
2. Ensure Configurations are correct (currently set to use default)
3. Start Debugging on WebAPI Project

Access Web:
	http://localhost:62371/Cache

Access API:
	http://localhost:62371/api/CRMCache?logicalName=ifb_externalsystemlinkses

### Server Installing

Install Redis for windows service using EXE at: https://github.com/rgl/redis/downloads
	from: https://stackoverflow.com/questions/6476945/how-do-i-run-redis-on-windows

Install .Net Framework 4.7.2: 
	https://support.microsoft.com/en-us/help/4054530/microsoft-net-framework-4-7-2-offline-installer-for-windows

Ensure Redis Service has started.

### Application Installing

Right Click WebAPI Project --> Publish
Publish to Folder, 
Copy to IIS Directory listed under Deployment

# Usage

## CRM Usage

### Javascript Example
`To pull this into CRM:`

1.	`add Javascript library to form: ifb_/ExternalHelpers/CachingHelper.js`          
2.	`Call function on form load: IFBCacheHelper.LoadEntities, Library above`
3.	`Pass Parameters to function: new Array('ifb_externalsystemlinkses', 'ifb_integrationconfigurations')`

`Javascript Example (from ESRForClientAccount.js).  See GetActionLinkConfigs for Old Way, GetActionLinkConfigsFromCache`

`GetActionLinkConfigs = function (currentEntity) {`
    `if (_ActionLinkConfigs != null) return _ActionLinkConfigs;`
    `else {`
        `try {`

​            `var relatedEntityOptionValue;`

​            `switch (currentEntity) {`
​            `case 'account':`
​                `relatedEntityOptionValue = 225090001;`
​                `break;`
​            `case 'policy':`
​                `relatedEntityOptionValue = 225090002;`
​                `break;`
​            `}`

​            `var client = Hsl.WebApi.client;`
​            `var resp = client.query('ifb_externalsystemlinks', {`
​                `filter: ["ifb_relatedentity eq " + relatedEntityOptionValue],`
​                `select: ["ifb_name", "ifb_systemurl", "ifb_replacementfields"],`
​                `orderby: 'ifb_name',`
​            `},`
​            `{`
​                `async: false,`
​                `always: function (resp) {`

​                    `if (resp.value.length > 0) {`
​                        `_ActionLinkConfigs = {};`

​                        `for (var i = 0; i < resp.value.length; i++) {`
​                            `_ActionLinkConfigs[i] = resp.value[i];`
​                        `}`
​                    `}`
​                    `else {`
​                        `//Hsl.Dialog.alert('No link configurations defined, please inform the CRM System Admin Team');`
​                    `}`
​                `}`
​            `});`

​            `return _ActionLinkConfigs;`

​        `} catch(error) {`
​            `//Gracefully exit, return null as it is handled in the SendMessage function.`
​            `return null;`
​        `}`
​    `}`
`} //End Get Action Link Configs`

`GetActionLinkConfigsFromCache = function (currentEntity) {`

​    `if (_ActionLinkConfigs != null)`
​        `return _ActionLinkConfigs;`
​    `else {`

​        `var relatedEntityOptionValue;`
​        `var links = window.top.IFBCacheHelper.CRMEntities['ifb_externalsystemlinkses'];`
​        
​        `switch (currentEntity) {`
​        `case 'account':`
​            `relatedEntityOptionValue = 225090001;`
​            `break;`
​        `case 'policy':`
​            `relatedEntityOptionValue = 225090002;`
​            `break;`
​        `}`

​        `// Filter by Related Entity = Account`
​        `filteredLinks = links.value.filter(function (el) {`

​            `try {`
​                `return el.ifb_relatedentity === relatedEntityOptionValue;`
​            `}`
​            `catch (error) {`
​                `// catch any issue with toLowerCase on a null value, exclude from result set`
​                `return false;`
​            `}`
​        `});`

​        `_ActionLinkConfigs = filteredLinks.sort(dynamicSort('ifb_name'));`

​        `return _ActionLinkConfigs;`
​    `}`
`}`

### API Usage
Website for Caching Admin:
	DEV: https://crmhelpers-dev.infarmbureau.com/Cache 

- Adding CRM Entities
  - GET on https://crmhelpers-dev.infarmbureau.com/api/CRMCache?logicalName=ifb_externalsystemlinkses 
  - Where logicalname = Plural entity name
  - Currently set to refresh 24 hrs.
  - If doesn’t exist, will call CRM to get.
  - Currently pulls all records from an entity
  - Important to update if we want to store more data in this

- Deleting CRM Entities:
  - DELETE on http://localhost:62371/api/CRMCache?key=Entities:ifb_externalsystemlinkses
  - Where key = “Entities:” + Plural logical Name
  - Use Admin Screen
  - Navigate to 2nd column in center of column, is a hyperlink for Delete
  - Click Clear all Keys
  - Following GETS will put back in Cache



## Deployment

1. Back up Server Website
2. Right Click WebAPI --> Publish
3. Select local Folder Profile.
4. Navigate to local Folder
5. Delete from Server Directory
6. Copy & Paste from Local Folder --> Server Directory
7. Set System.Environment variable for logging service (Restart machine to load)
	a. Dev - "d"
	b. QAG - "g"
	c. QAQ - "q"
	d. PRD - "p"

DEV

​	URL: https://crmhelpers-dev.infarmbureau.com/ (HTTPS only)
​	Server: AP-CRM2UTIL-DEV
​	File Path: D:\Websites\crmhelpers
​	Logs: D:\IIS_Logs\W3SVC5
​	U: ifb\crmexdev	
​	P:	eHL7Z4st
​	Admin Screen: https://crmhelpers-dev.infarmbureau.com/Cache
​		
QAG
URL: https://crmhelpers-qag.infarmbureau.com/ (HTTPS only)
Server: AP-CRM2UTILG-QA
File Path: D:\Websites\crmhelpers
Logs: D:\IIS_Logs\W3SVC3

​	ifb\crmexqa	
​	g5xIpA9b
​	
QAQ
URL: https://crmhelpers-qaq.infarmbureau.com/  (HTTPS only)
Server: AP-CRM2UTIL-QA
File Path: D:\Websites\crmhelpers
Logs: D:\IIS_Logs\W3SVC3

​	ifb\crmexqa	
​	g5xIpA9b
​	
PRD
URL: https://crmhelpers.infarmbureau.com/ (HTTPS only)
Server: AP-CRM2UTIL-PRD
File Path: D:\Websites\crmhelpers
Logs: D:\IIS_Logs\W3SVC3

​	ifb\crmexprd	
​	5A2vTmiw

## Built With
.Net 4.7.2 
Redis 2.4.6

## Future Enhancements

- Authentication? 
  - Currently none
  - Authentication to CRM in connection string  as CRM Admin user
- Filtering on CORS (currently set to *, do we want to enable only for any subdomains)
- Will require .Net 4.7.2 & Redis installed on server, redis service running
- There is a newer version of Redis for Windows that we are not using, maybe we want to upgrade here?

## Versioning

## Authors

Aaron Vogt

## License

## Acknowledgments
