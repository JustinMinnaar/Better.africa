
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 09/26/2018 20:26:18
-- Generated from EDMX file: C:\Users\justi\source\repos\Better.Africa\Better.Benefits.WebApplication1\Data\BenefitsDB.edmx
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

IF OBJECT_ID(N'[dbo].[FK_PolicyMemberPolicyMemberType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PolicyMembers] DROP CONSTRAINT [FK_PolicyMemberPolicyMemberType];
GO
IF OBJECT_ID(N'[dbo].[FK_PolicyStatusPolicy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policies] DROP CONSTRAINT [FK_PolicyStatusPolicy];
GO
IF OBJECT_ID(N'[dbo].[FK_PolicyMemberPolicy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PolicyMembers] DROP CONSTRAINT [FK_PolicyMemberPolicy];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonPolicyMember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PolicyMembers] DROP CONSTRAINT [FK_PersonPolicyMember];
GO
IF OBJECT_ID(N'[dbo].[FK_PersonPolicy]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Policies] DROP CONSTRAINT [FK_PersonPolicy];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[People]', 'U') IS NOT NULL
    DROP TABLE [dbo].[People];
GO
IF OBJECT_ID(N'[dbo].[Policies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Policies];
GO
IF OBJECT_ID(N'[dbo].[PolicyStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PolicyStatus];
GO
IF OBJECT_ID(N'[dbo].[PolicyMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PolicyMembers];
GO
IF OBJECT_ID(N'[dbo].[PolicyCoverTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PolicyCoverTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'People'
CREATE TABLE [dbo].[People] (
    [Id] uniqueidentifier  NOT NULL,
    [Row_RowDateBegin] datetime  NOT NULL,
    [Row_RowVersion] int  NOT NULL,
    [Row_RowDeleted] bit  NOT NULL,
    [Row_RowDeletedParent] bit  NOT NULL,
    [Row_RowValidated] bit  NOT NULL,
    [Row_RowCurrent] bit  NOT NULL,
    [Row_RowEntityId] tinyint  NOT NULL,
    [Row_RowDateEnd] datetime  NULL,
    [NameFirst] nvarchar(max)  NOT NULL,
    [NameLast] nvarchar(max)  NOT NULL,
    [NameTitle] nvarchar(max)  NULL,
    [WorkName] nvarchar(max)  NULL,
    [WorkPhone] nvarchar(max)  NULL,
    [HomePhone] nvarchar(max)  NULL,
    [CellPhone] nvarchar(max)  NULL,
    [IdNumber] nvarchar(13)  NOT NULL,
    [IsAgent] bit  NOT NULL,
    [IsStaff] bit  NOT NULL,
    [IsClient] bit  NOT NULL,
    [ReferrerId] uniqueidentifier  NULL,
    [Birthdate] datetime  NULL,
    [AgentId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'Policies'
CREATE TABLE [dbo].[Policies] (
    [Id] uniqueidentifier  NOT NULL,
    [Row_RowDateBegin] datetime  NOT NULL,
    [Row_RowVersion] int  NOT NULL,
    [Row_RowDeleted] bit  NOT NULL,
    [Row_RowDeletedParent] bit  NOT NULL,
    [Row_RowValidated] bit  NOT NULL,
    [Row_RowCurrent] bit  NOT NULL,
    [Row_RowEntityId] tinyint  NOT NULL,
    [Row_RowDateEnd] datetime  NULL,
    [AgentId] uniqueidentifier  NOT NULL,
    [UnderwriterNumber] nvarchar(max)  NOT NULL,
    [PolicyStatusId] tinyint  NOT NULL,
    [OwnerId] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'PolicyStatus'
CREATE TABLE [dbo].[PolicyStatus] (
    [Id] tinyint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PolicyMembers'
CREATE TABLE [dbo].[PolicyMembers] (
    [PolicyId] uniqueidentifier  NOT NULL,
    [CoverTypeId] tinyint  NOT NULL,
    [PersonId] uniqueidentifier  NOT NULL,
    [Policy_Id] uniqueidentifier  NOT NULL
);
GO

-- Creating table 'PolicyCoverTypes'
CREATE TABLE [dbo].[PolicyCoverTypes] (
    [Id] tinyint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'People'
ALTER TABLE [dbo].[People]
ADD CONSTRAINT [PK_People]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Policies'
ALTER TABLE [dbo].[Policies]
ADD CONSTRAINT [PK_Policies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PolicyStatus'
ALTER TABLE [dbo].[PolicyStatus]
ADD CONSTRAINT [PK_PolicyStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [PolicyId] in table 'PolicyMembers'
ALTER TABLE [dbo].[PolicyMembers]
ADD CONSTRAINT [PK_PolicyMembers]
    PRIMARY KEY CLUSTERED ([PolicyId] ASC);
GO

-- Creating primary key on [Id] in table 'PolicyCoverTypes'
ALTER TABLE [dbo].[PolicyCoverTypes]
ADD CONSTRAINT [PK_PolicyCoverTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CoverTypeId] in table 'PolicyMembers'
ALTER TABLE [dbo].[PolicyMembers]
ADD CONSTRAINT [FK_PolicyMemberPolicyMemberType]
    FOREIGN KEY ([CoverTypeId])
    REFERENCES [dbo].[PolicyCoverTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolicyMemberPolicyMemberType'
CREATE INDEX [IX_FK_PolicyMemberPolicyMemberType]
ON [dbo].[PolicyMembers]
    ([CoverTypeId]);
GO

-- Creating foreign key on [PolicyStatusId] in table 'Policies'
ALTER TABLE [dbo].[Policies]
ADD CONSTRAINT [FK_PolicyStatusPolicy]
    FOREIGN KEY ([PolicyStatusId])
    REFERENCES [dbo].[PolicyStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolicyStatusPolicy'
CREATE INDEX [IX_FK_PolicyStatusPolicy]
ON [dbo].[Policies]
    ([PolicyStatusId]);
GO

-- Creating foreign key on [Policy_Id] in table 'PolicyMembers'
ALTER TABLE [dbo].[PolicyMembers]
ADD CONSTRAINT [FK_PolicyMemberPolicy]
    FOREIGN KEY ([Policy_Id])
    REFERENCES [dbo].[Policies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PolicyMemberPolicy'
CREATE INDEX [IX_FK_PolicyMemberPolicy]
ON [dbo].[PolicyMembers]
    ([Policy_Id]);
GO

-- Creating foreign key on [PersonId] in table 'PolicyMembers'
ALTER TABLE [dbo].[PolicyMembers]
ADD CONSTRAINT [FK_PersonPolicyMember]
    FOREIGN KEY ([PersonId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonPolicyMember'
CREATE INDEX [IX_FK_PersonPolicyMember]
ON [dbo].[PolicyMembers]
    ([PersonId]);
GO

-- Creating foreign key on [OwnerId] in table 'Policies'
ALTER TABLE [dbo].[Policies]
ADD CONSTRAINT [FK_PersonPolicy]
    FOREIGN KEY ([OwnerId])
    REFERENCES [dbo].[People]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PersonPolicy'
CREATE INDEX [IX_FK_PersonPolicy]
ON [dbo].[Policies]
    ([OwnerId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------