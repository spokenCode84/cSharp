CREATE TABLE [dbo].[CRM_Logs](  
    [Id] [int] IDENTITY(1,1) NOT NULL,  
    [Date] [datetime] NOT NULL,  
    [Thread] [varchar](255) NOT NULL,  
    [Level] [varchar](50) NOT NULL,  
    [Logger] [varchar](255) NOT NULL,  
    [Message] [varchar](4000) NOT NULL,
	[UserOpr] [varchar] (255) NULL,
	[Source] [varchar] (255) NULL,
	[Request] [varchar] (8000) NULL,
	[Response] [varchar] (8000) NULL,
	[Stacktrace] [varchar] (4000) NULL,  
    [Exception] [varchar](2000) NULL,  
 CONSTRAINT [PK_CRM_Logs] PRIMARY KEY CLUSTERED   
(  
    [Id] ASC  
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]  
) ON [PRIMARY] 