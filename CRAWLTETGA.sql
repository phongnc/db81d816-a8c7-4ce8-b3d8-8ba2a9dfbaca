USE [master]
GO
/****** Object:  Database [CRAWLTETGA]    Script Date: 18/08/2017 4:34:38 PM ******/
CREATE DATABASE [CRAWLTETGA]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CRAWLTETGA', FILENAME = N'D:\_DB\CRAWLTETGA.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CRAWLTETGA_log', FILENAME = N'D:\_DB\CRAWLTETGA_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CRAWLTETGA] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CRAWLTETGA].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CRAWLTETGA] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET ARITHABORT OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CRAWLTETGA] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CRAWLTETGA] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CRAWLTETGA] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CRAWLTETGA] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CRAWLTETGA] SET  MULTI_USER 
GO
ALTER DATABASE [CRAWLTETGA] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CRAWLTETGA] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CRAWLTETGA] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CRAWLTETGA] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [CRAWLTETGA] SET DELAYED_DURABILITY = DISABLED 
GO
USE [CRAWLTETGA]
GO
/****** Object:  Table [dbo].[CrawlDetail]    Script Date: 18/08/2017 4:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrawlDetail](
	[CrawlDetailId] [int] IDENTITY(1,1) NOT NULL,
	[CrawlSiteId] [int] NOT NULL,
	[CrawlDetailUrl] [int] NULL,
	[CrawlDetailTitle] [int] NULL,
	[CrawlDetailDescription] [int] NULL,
	[CrawlDetailImage] [int] NULL,
	[CrawlDetailContent] [int] NULL,
 CONSTRAINT [PK_CrawlDetail] PRIMARY KEY CLUSTERED 
(
	[CrawlDetailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CrawlList]    Script Date: 18/08/2017 4:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrawlList](
	[CrawlListId] [int] IDENTITY(1,1) NOT NULL,
	[CrawlSiteId] [int] NOT NULL,
	[CrawlListItem] [int] NULL,
	[CrawlListPage] [int] NULL,
 CONSTRAINT [PK_CrawlList] PRIMARY KEY CLUSTERED 
(
	[CrawlListId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CrawlLog]    Script Date: 18/08/2017 4:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CrawlLog](
	[CrawlLogId] [int] IDENTITY(1,1) NOT NULL,
	[CrawlLogUrl] [nvarchar](500) NULL,
	[CrawlLogUrlEncode] [varchar](500) NULL,
	[CrawlLogRuleId] [int] NULL,
 CONSTRAINT [PK_CrawlLog] PRIMARY KEY CLUSTERED 
(
	[CrawlLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CrawlRule]    Script Date: 18/08/2017 4:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CrawlRule](
	[CrawlRuleId] [int] IDENTITY(1,1) NOT NULL,
	[CrawlSiteId] [int] NULL,
	[CrawlParentId] [int] NULL,
	[CrawlRuleFor] [nvarchar](500) NULL,
	[CrawlRuleQuery] [nvarchar](500) NULL,
	[CrawlRuleTag] [nvarchar](500) NULL,
	[CrawlRuleClass] [nvarchar](500) NULL,
	[CrawlRuleStart] [nvarchar](500) NULL,
	[CrawlRuleEnd] [nvarchar](500) NULL,
	[CrawlRuleIndex] [int] NULL,
	[CrawlRuleRegex] [nvarchar](500) NULL,
	[CrawlRuleFormat] [nvarchar](500) NULL,
	[CrawlRuleReplace] [nvarchar](500) NULL,
	[CrawlRuleJson] [nvarchar](500) NULL,
	[CrawlRuleNote] [nvarchar](500) NULL,
 CONSTRAINT [PK_CrawlRule] PRIMARY KEY CLUSTERED 
(
	[CrawlRuleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CrawlSite]    Script Date: 18/08/2017 4:34:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CrawlSite](
	[CrawlSiteId] [int] IDENTITY(1,1) NOT NULL,
	[CrawlSiteUrl] [varchar](500) NULL,
	[CrawlSiteTitle] [nvarchar](500) NULL,
	[CrawlSiteNote] [nvarchar](max) NULL,
 CONSTRAINT [PK_CrawlSite] PRIMARY KEY CLUSTERED 
(
	[CrawlSiteId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
USE [master]
GO
ALTER DATABASE [CRAWLTETGA] SET  READ_WRITE 
GO