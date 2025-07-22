IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [ProfileImageUrl] nvarchar(max) NULL,
    [FullName] nvarchar(max) NULL,
    [BirthDate] datetime2 NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [TestTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [ImageUrl] nvarchar(max) NULL,
    CONSTRAINT [PK_TestTypes] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Quizzes] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(max) NOT NULL,
    [CategoryId] int NULL,
    [TestTypeId] int NULL,
    CONSTRAINT [PK_Quizzes] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Quizzes_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Quizzes_TestTypes_TestTypeId] FOREIGN KEY ([TestTypeId]) REFERENCES [TestTypes] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [Questions] (
    [Id] int NOT NULL IDENTITY,
    [Text] nvarchar(max) NOT NULL,
    [OptionA] nvarchar(max) NOT NULL,
    [OptionB] nvarchar(max) NOT NULL,
    [CorrectOption] nvarchar(max) NOT NULL,
    [QuizId] int NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Questions_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [UserQuizResults] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NULL,
    [QuizId] int NULL,
    [CorrectAnswers] int NOT NULL,
    [TotalQuestions] int NOT NULL,
    [TakenAt] datetime2 NOT NULL,
    CONSTRAINT [PK_UserQuizResults] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserQuizResults_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_UserQuizResults_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [QuizComments] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NULL,
    [QuizId] int NOT NULL,
    [CommentText] nvarchar(max) NOT NULL,
    [CreatedAt] datetime2 NOT NULL,
    [UserQuizResultId] int NULL,
    [ApplicationUserId] nvarchar(450) NULL,
    CONSTRAINT [PK_QuizComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuizComments_AspNetUsers_ApplicationUserId] FOREIGN KEY ([ApplicationUserId]) REFERENCES [AspNetUsers] ([Id]),
    CONSTRAINT [FK_QuizComments_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_QuizComments_Quizzes_QuizId] FOREIGN KEY ([QuizId]) REFERENCES [Quizzes] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_QuizComments_UserQuizResults_UserQuizResultId] FOREIGN KEY ([UserQuizResultId]) REFERENCES [UserQuizResults] ([Id]) ON DELETE SET NULL
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

CREATE INDEX [IX_Questions_QuizId] ON [Questions] ([QuizId]);
GO

CREATE INDEX [IX_QuizComments_ApplicationUserId] ON [QuizComments] ([ApplicationUserId]);
GO

CREATE INDEX [IX_QuizComments_QuizId] ON [QuizComments] ([QuizId]);
GO

CREATE INDEX [IX_QuizComments_UserId] ON [QuizComments] ([UserId]);
GO

CREATE INDEX [IX_QuizComments_UserQuizResultId] ON [QuizComments] ([UserQuizResultId]);
GO

CREATE INDEX [IX_Quizzes_CategoryId] ON [Quizzes] ([CategoryId]);
GO

CREATE INDEX [IX_Quizzes_TestTypeId] ON [Quizzes] ([TestTypeId]);
GO

CREATE INDEX [IX_UserQuizResults_QuizId] ON [UserQuizResults] ([QuizId]);
GO

CREATE INDEX [IX_UserQuizResults_UserId] ON [UserQuizResults] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250709203259_InitialCreate', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [UserAnswers] (
    [Id] int NOT NULL IDENTITY,
    [UserQuizResultId] int NOT NULL,
    [QuestionId] int NOT NULL,
    [SelectedOption] nvarchar(max) NOT NULL,
    [IsCorrect] bit NOT NULL,
    [AnsweredAt] datetime2 NOT NULL,
    [UserQuizResultId1] int NULL,
    CONSTRAINT [PK_UserAnswers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserAnswers_Questions_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserAnswers_UserQuizResults_UserQuizResultId] FOREIGN KEY ([UserQuizResultId]) REFERENCES [UserQuizResults] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserAnswers_UserQuizResults_UserQuizResultId1] FOREIGN KEY ([UserQuizResultId1]) REFERENCES [UserQuizResults] ([Id])
);
GO

CREATE INDEX [IX_UserAnswers_QuestionId] ON [UserAnswers] ([QuestionId]);
GO

CREATE INDEX [IX_UserAnswers_UserQuizResultId] ON [UserAnswers] ([UserQuizResultId]);
GO

CREATE INDEX [IX_UserAnswers_UserQuizResultId1] ON [UserAnswers] ([UserQuizResultId1]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250710080226_AddUserAnswerModel', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [UserAnswers] DROP CONSTRAINT [FK_UserAnswers_UserQuizResults_UserQuizResultId1];
GO

DROP INDEX [IX_UserAnswers_UserQuizResultId1] ON [UserAnswers];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[UserAnswers]') AND [c].[name] = N'UserQuizResultId1');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [UserAnswers] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [UserAnswers] DROP COLUMN [UserQuizResultId1];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250710080606_AddUserAnswer', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Quizzes] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250710095012_AddIsActiveToQuiz', N'7.0.0');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'FullName');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
UPDATE [AspNetUsers] SET [FullName] = N'' WHERE [FullName] IS NULL;
ALTER TABLE [AspNetUsers] ALTER COLUMN [FullName] nvarchar(max) NOT NULL;
ALTER TABLE [AspNetUsers] ADD DEFAULT N'' FOR [FullName];
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AspNetUsers]') AND [c].[name] = N'BirthDate');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var2 + '];');
UPDATE [AspNetUsers] SET [BirthDate] = '0001-01-01T00:00:00.0000000' WHERE [BirthDate] IS NULL;
ALTER TABLE [AspNetUsers] ALTER COLUMN [BirthDate] datetime2 NOT NULL;
ALTER TABLE [AspNetUsers] ADD DEFAULT '0001-01-01T00:00:00.0000000' FOR [BirthDate];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250711205359_UpdateApplicationUser_NullableRemoved', N'7.0.0');
GO

COMMIT;
GO

