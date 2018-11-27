
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/16/2018 09:46:18
-- Generated from EDMX file: C:\Users\justi\source\repos\Better.Africa\Better.Benefits.Data\BetterDb.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Better Benefits];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Policy_Members]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policy_Member] DROP CONSTRAINT [FK_Policy_Members];
GO
IF OBJECT_ID(N'[dbo].[FK_Policy_Receipts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policy_Receipt] DROP CONSTRAINT [FK_Policy_Receipts];
GO
IF OBJECT_ID(N'[dbo].[FK_PolicyPolicy_Type]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policy] DROP CONSTRAINT [FK_PolicyPolicy_Type];
GO
IF OBJECT_ID(N'[dbo].[FK_PolicyPolicy_Status]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policy] DROP CONSTRAINT [FK_PolicyPolicy_Status];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonPolicy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policy] DROP CONSTRAINT [FK_PersonPolicy];
GO
IF OBJECT_ID(N'[dbo].[FK_Policy_MemberPerson]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policy_Member] DROP CONSTRAINT [FK_Policy_MemberPerson];
GO
IF OBJECT_ID(N'[dbo].[FK_CountryPerson_Identity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Identity] DROP CONSTRAINT [FK_CountryPerson_Identity];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonPerson_Identity]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Identity] DROP CONSTRAINT [FK_PersonPerson_Identity];
GO
IF OBJECT_ID(N'[dbo].[FK_Person_IdentityPerson_IdentityType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Person_Identity] DROP CONSTRAINT [FK_Person_IdentityPerson_IdentityType];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Policy]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policy];
GO
IF OBJECT_ID(N'[dbo].[Policy_Member]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policy_Member];
GO
IF OBJECT_ID(N'[dbo].[Policy_Receipt]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policy_Receipt];
GO
IF OBJECT_ID(N'[dbo].[Person]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person];
GO
IF OBJECT_ID(N'[dbo].[Policy_Type]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policy_Type];
GO
IF OBJECT_ID(N'[dbo].[Policy_Status]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policy_Status];
GO
IF OBJECT_ID(N'[dbo].[Person_Identity]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_Identity];
GO
IF OBJECT_ID(N'[dbo].[Country]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Country];
GO
IF OBJECT_ID(N'[dbo].[Person_IdentityType]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Person_IdentityType];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Policy'
CREATE TABLE [dbo].[Policy] (
    [Code] nvarchar(max)  NOT NULL,
    [TypeNo] tinyint  NOT NULL,
    [StatusNo] tinyint  NOT NULL,
    [OwnerPersonId] uniqueidentifier  NOT NULL,
    [Id] uniqueidentifier  NOT NULL,
    [PolicyId] uniqueidentifier  NOT NULL,
    [Row_Version] int  NOT NULL,
    [Row_Archived] bit  NOT NULL,
    [Row_Deleted] bit  NOT NULL,
    [Row_ModifiedOn] datetime  NOT NULL,
    [Row_ModifiedBy] uniqueidentifier  NOT NULL,
    [Row_DeletedParent] bit  NOT NULL,
    [Row_ArchivedOn] datetime  NULL
);
GO

-- Creating table 'Policy_Member'
CREATE TABLE [dbo].[Policy_Member] (
    [PersonId] uniqueidentifier  NOT NULL,
    [IsDependant] bit  NOT NULL,
    [Id] uniqueidentifier  NOT NULL,
    [PolicyId] uniqueidentifier  NOT NULL,
    [Row_Version] int  NOT NULL,
    [Row_Archived] bit  NOT NULL,
    [Row_Deleted] bit  NOT NULL,
    [Row_ModifiedOn] datetime  NOT NULL,
    [Row_ModifiedBy] uniqueidentifier  NOT NULL,
    [Row_DeletedParent] bit  NOT NULL,
    [Row_ArchivedOn] datetime  NULL
);
GO

-- Creating table 'Policy_Receipt'
CREATE TABLE [dbo].[Policy_Receipt] (
    [SourceCode] nvarchar(50)  NOT NULL,
    [SourceDate] datetime  NOT NULL,
    [AmountExcl] decimal(11,2)  NOT NULL,
    [AmountVat] decimal(11,2)  NOT NULL,
    [Id] uniqueidentifier  NOT NULL,
    [PolicyId] uniqueidentifier  NOT NULL,
    [Row_Version] int  NOT NULL,
    [Row_Archived] bit  NOT NULL,
    [Row_Deleted] bit  NOT NULL,
    [Row_ModifiedOn] datetime  NOT NULL,
    [Row_ModifiedBy] uniqueidentifier  NOT NULL,
    [Row_DeletedParent] bit  NOT NULL,
    [Row_ArchivedOn] datetime  NULL
);
GO

-- Creating table 'Person'
CREATE TABLE [dbo].[Person] (
    [Id] uniqueidentifier  NOT NULL,
    [PersonId] uniqueidentifier  NOT NULL,
    [IsAgent] nvarchar(max)  NOT NULL,
    [IsClient] nvarchar(max)  NOT NULL,
    [IsStaff] nvarchar(max)  NOT NULL,
    [Row_Version] int  NOT NULL,
    [Row_Archived] bit  NOT NULL,
    [Row_Deleted] bit  NOT NULL,
    [Row_ModifiedOn] datetime  NOT NULL,
    [Row_ModifiedBy] uniqueidentifier  NOT NULL,
    [Row_DeletedParent] bit  NOT NULL,
    [Row_ArchivedOn] datetime  NULL,
    [Birthdate] datetime  NULL,
    [Firstname] nvarchar(max)  NOT NULL,
    [Lastname] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Policy_Type'
CREATE TABLE [dbo].[Policy_Type] (
    [No] tinyint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Policy_Status'
CREATE TABLE [dbo].[Policy_Status] (
    [No] tinyint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Person_Identity'
CREATE TABLE [dbo].[Person_Identity] (
    [IdentityTypeNo] tinyint  NOT NULL,
    [CountryCode] nchar(2)  NOT NULL,
    [Number] nvarchar(max)  NOT NULL,
    [Name_First] nvarchar(50)  NULL,
    [Name_Last] nvarchar(50)  NULL,
    [Name_Honor] nvarchar(max)  NOT NULL,
    [Id] uniqueidentifier  NOT NULL,
    [Image] varbinary(max)  NOT NULL,
    [Row_Version] int  NOT NULL,
    [Row_Archived] bit  NOT NULL,
    [Row_Deleted] bit  NOT NULL,
    [Row_ModifiedOn] datetime  NOT NULL,
    [Row_ModifiedBy] uniqueidentifier  NOT NULL,
    [Row_DeletedParent] bit  NOT NULL,
    [Row_ArchivedOn] datetime  NULL,
    [PersonId] uniqueidentifier  NOT NULL,
    [IdentityType_No] tinyint  NOT NULL
);
GO

-- Creating table 'Country'
CREATE TABLE [dbo].[Country] (
    [Code] nchar(2)  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Person_IdentityType'
CREATE TABLE [dbo].[Person_IdentityType] (
    [No] tinyint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Policy'
ALTER TABLE [dbo].[Policy]
ADD CONSTRAINT [PK_Policy]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Policy_Member'
ALTER TABLE [dbo].[Policy_Member]
ADD CONSTRAINT [PK_Policy_Member]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Policy_Receipt'
ALTER TABLE [dbo].[Policy_Receipt]
ADD CONSTRAINT [PK_Policy_Receipt]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Person'
ALTER TABLE [dbo].[Person]
ADD CONSTRAINT [PK_Person]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [No] in table 'Policy_Type'
ALTER TABLE [dbo].[Policy_Type]
ADD CONSTRAINT [PK_Policy_Type]
    PRIMARY KEY CLUSTERED ([No] ASC);
GO

-- Creating primary key on [No] in table 'Policy_Status'
ALTER TABLE [dbo].[Policy_Status]
ADD CONSTRAINT [PK_Policy_Status]
    PRIMARY KEY CLUSTERED ([No] ASC);
GO

-- Creating primary key on [Id] in table 'Person_Identity'
ALTER TABLE [dbo].[Person_Identity]
ADD CONSTRAINT [PK_Person_Identity]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Code] in table 'Country'
ALTER TABLE [dbo].[Country]
ADD CONSTRAINT [PK_Country]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

-- Creating primary key on [No] in table 'Person_IdentityType'
ALTER TABLE [dbo].[Person_IdentityType]
ADD CONSTRAINT [PK_Person_IdentityType]
    PRIMARY KEY CLUSTERED ([No] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [PolicyId] in table 'Policy_Member'
ALTER TABLE [dbo].[Policy_Member]
ADD CONSTRAINT [FK_Policy_Members]
    FOREIGN KEY ([PolicyId])
    REFERENCES [dbo].[Policy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Policy_Members'
CREATE INDEX [IX_FK_Policy_Members]
ON [dbo].[Policy_Member]
    ([PolicyId]);
GO

-- Creating foreign key on [PolicyId] in table 'Policy_Receipt'
ALTER TABLE [dbo].[Policy_Receipt]
ADD CONSTRAINT [FK_Policy_Receipts]
    FOREIGN KEY ([PolicyId])
    REFERENCES [dbo].[Policy]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Policy_Receipts'
CREATE INDEX [IX_FK_Policy_Receipts]
ON [dbo].[Policy_Receipt]
    ([PolicyId]);
GO

-- Creating foreign key on [TypeNo] in table 'Policy'
ALTER TABLE [dbo].[Policy]
ADD CONSTRAINT [FK_PolicyPolicy_Type]
    FOREIGN KEY ([TypeNo])
    REFERENCES [dbo].[Policy_Type]
        ([No])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolicyPolicy_Type'
CREATE INDEX [IX_FK_PolicyPolicy_Type]
ON [dbo].[Policy]
    ([TypeNo]);
GO

-- Creating foreign key on [StatusNo] in table 'Policy'
ALTER TABLE [dbo].[Policy]
ADD CONSTRAINT [FK_PolicyPolicy_Status]
    FOREIGN KEY ([StatusNo])
    REFERENCES [dbo].[Policy_Status]
        ([No])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolicyPolicy_Status'
CREATE INDEX [IX_FK_PolicyPolicy_Status]
ON [dbo].[Policy]
    ([StatusNo]);
GO

-- Creating foreign key on [OwnerPersonId] in table 'Policy'
ALTER TABLE [dbo].[Policy]
ADD CONSTRAINT [FK_PersonPolicy]
    FOREIGN KEY ([OwnerPersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonPolicy'
CREATE INDEX [IX_FK_PersonPolicy]
ON [dbo].[Policy]
    ([OwnerPersonId]);
GO

-- Creating foreign key on [PersonId] in table 'Policy_Member'
ALTER TABLE [dbo].[Policy_Member]
ADD CONSTRAINT [FK_Policy_MemberPerson]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Policy_MemberPerson'
CREATE INDEX [IX_FK_Policy_MemberPerson]
ON [dbo].[Policy_Member]
    ([PersonId]);
GO

-- Creating foreign key on [CountryCode] in table 'Person_Identity'
ALTER TABLE [dbo].[Person_Identity]
ADD CONSTRAINT [FK_CountryPerson_Identity]
    FOREIGN KEY ([CountryCode])
    REFERENCES [dbo].[Country]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CountryPerson_Identity'
CREATE INDEX [IX_FK_CountryPerson_Identity]
ON [dbo].[Person_Identity]
    ([CountryCode]);
GO

-- Creating foreign key on [PersonId] in table 'Person_Identity'
ALTER TABLE [dbo].[Person_Identity]
ADD CONSTRAINT [FK_PersonPerson_Identity]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[Person]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonPerson_Identity'
CREATE INDEX [IX_FK_PersonPerson_Identity]
ON [dbo].[Person_Identity]
    ([PersonId]);
GO

-- Creating foreign key on [IdentityType_No] in table 'Person_Identity'
ALTER TABLE [dbo].[Person_Identity]
ADD CONSTRAINT [FK_Person_IdentityPerson_IdentityType]
    FOREIGN KEY ([IdentityType_No])
    REFERENCES [dbo].[Person_IdentityType]
        ([No])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Person_IdentityPerson_IdentityType'
CREATE INDEX [IX_FK_Person_IdentityPerson_IdentityType]
ON [dbo].[Person_Identity]
    ([IdentityType_No]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------