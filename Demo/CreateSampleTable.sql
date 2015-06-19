-- sample table schema         
CREATE TABLE [dbo].[TableName](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Score] [int] NULL,
	[Winner] [varchar](100) NULL,
	[CreatedOn] [datetime] NULL,
	[IsFinal] [bit] NOT NULL CONSTRAINT [DF_TableName_IsFinal]  DEFAULT ((0)),
 CONSTRAINT [PK_TableName] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
         