// *********************************************************************
// Proejct: Lead Creation Topic Listener
// Created: 2019-07-25 15:04:09
// Created By: OPR1113 
// *********************************************************************


// *********************************************************************
// Installation
// *********************************************************************

	1) Move installation files to desntination server
	2) Double click deploy\Install.cmd
	3) Open services console, and double click CRM_Lead_Topic_Listener
	4) Set logon accountto appropriate service account)
		a) DEV: ifb\crmexdev
		b) QAG: ifb\crmexqa
		c) QAQ: ifb\crmexqa
		d) PRD: ifb\crmexprd
	5) Set region specific system environment variable for (CRM_ENV)
		a) DEV: CRM_ENV = DEV
		b) QAG: CRM_ENV = QAG
		c) QAQ: CRM_ENV = QAQ
		d) PRD: CRM_ENV = PRD
	6) Start CRM_Lead_Topic_Listener windows service

	** To uninstall double click deploy\Uninstall.cmd


// *********************************************************************
// Migration
// *********************************************************************

	1) Stop CRM_Lead_Topic_Listener windows service
	2) Backup files in bin\Release\
	3) Move new files into bin\Release\
	4) Restart CRM_Lead_Topic_Listener windows service
	

// *********************************************************************
// Local Debug
// *********************************************************************

	1) Set region specific system environment variable for (CRM_ENV)
		a) DEV: CRM_ENV = DEV
		b) QAG: CRM_ENV = QAG
		c) QAQ: CRM_ENV = QAQ
		d) PRD: CRM_ENV = PRD

	2) There is a pre-processing directive that will allow it to be
	   run directly from visual studio.
		a) You can optionally set the env paremeter in Bootstrap.cs
	