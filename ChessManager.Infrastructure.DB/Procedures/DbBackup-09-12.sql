create table Category
(
    Id        int identity
        constraint Category_PK
            primary key,
    Name      nvarchar(50) not null,
    AgeMax    int          not null,
    AgeMin    int          not null,
    CreatedAt datetime     not null,
    UpdatedAt datetime     not null
)
go

create table Member
(
    Id        int identity
        constraint Member_PK
            primary key,
    Password  nvarchar(250) not null,
    Gender    int           not null,
    Elo       int           not null
        constraint [Elo Range]
            check ([Elo] >= 0 AND [Elo] <= 3200),
    Role      int           not null,
    BirthDate datetime      not null,
    CreatedAt datetime      not null,
    UpdatedAt datetime      not null,
    Pseudo    nvarchar(250) not null
        constraint Member_Pseudo_UK
            unique,
    Email     nvarchar(250) not null
        constraint Member_Email_UK
            unique
)
go

create table Tournament
(
    Id                  int identity
        constraint Tournament_PK
            primary key,
    Name                nvarchar(250) not null,
    Location            nvarchar(250) not null,
    MaxPlayerCount      int           not null
        constraint MaxPlayerLimit
            check ([MaxPlayerCount] >= 2 AND [MaxPlayerCount] <= 32),
    MinPlayerCount      int           not null
        constraint MinPlayerLimit
            check ([MinPlayerCount] >= 2 AND [MinPlayerCount] <= 32),
    MaxEloAllowed       int           not null
        constraint MaxEloRange
            check ([MaxEloAllowed] >= 0 AND [MaxEloAllowed] <= 3200)
        constraint MinEloRange
            check ([MaxEloAllowed] >= 0 AND [MaxEloAllowed] <= 3200),
    MinEloAllowed       int           not null,
    Status              int           not null,
    CurrentRound        int           not null,
    WomenOnly           bit           not null,
    RegistrationEndDate datetime      not null,
    CreatedAt           datetime      not null,
    UpdatedAt           datetime      not null,
    constraint [MaxElo > MinElo]
        check ([MaxEloAllowed] >= [MinEloAllowed]),
    constraint [MaxPlayerCount > MinPlayerCount]
        check ([MaxPlayerCount] >= [MinPlayerCount])
)
go

create table Tournament_Category
(
    Category_id   int not null,
    Tournament_id int not null,
    constraint Tournament_Category_PK
        primary key (Category_id, Tournament_id)
)
go

create table Tournament_Member
(
    Member_id     int not null,
    Tournament_id int not null,
    constraint Tournament_Member_PK
        primary key (Member_id, Tournament_id)
)
go

CREATE PROCEDURE AddCategoryToTournament
    @CategoryId INT,
    @TournamentId INT

AS
BEGIN
    INSERT INTO Tournament_Category (Category_id, Tournament_id) VALUES (@CategoryId, @TournamentId);
END;
go

CREATE PROCEDURE ChangePasswordMember
    @MemberId INT,
    @NewPassword NVARCHAR(255)

AS
BEGIN
    UPDATE Member SET Password = @NewPassword WHERE Id = @MemberId
end
go

CREATE PROCEDURE CreateCategory
    @Name NVARCHAR(255),
    @AgeMax INT,
    @AgeMin INT,
    @CreatedAt INT,
    @UpdatedAt INT

AS

BEGIN
    INSERT INTO Category (Name, AgeMax, AgeMin, CreatedAt, UpdatedAt)
    VALUES (@Name, @AgeMax, @AgeMin, @CreatedAt, @UpdatedAt);
END;
go

-- Create a new member
CREATE PROCEDURE CreateMember
    @Pseudo NVARCHAR(255),
    @Email NVARCHAR(255),
    @Password NVARCHAR(255),
    @Gender CHAR(1),
    @Elo INT,
    @Role NVARCHAR(10),
    @BirthDate DATETIME,
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME
AS
BEGIN
    INSERT INTO Member (Pseudo, Email, Password, Gender, Elo, Role, BirthDate, CreatedAt, UpdatedAt)
    OUTPUT inserted.*
    VALUES (@Pseudo, @Email, @Password, @Gender, @Elo,  @Role, @BirthDate, @CreatedAt, @UpdatedAt);
END;
go

-- Create a new tournament
CREATE PROCEDURE CreateTournament
    @Name NVARCHAR(255),
    @Location NVARCHAR(255),
    @MaxPlayerCount INT,
    @MinPlayerCount INT,
    @MaxEloAllowed INT,
    @MinEloAllowed INT,
    @Status INT,
    @CurrentRound INT,
    @WomenOnly BIT,
    @RegistrationEndDate DATETIME,
    @CreatedAt DATETIME,
    @UpdatedAt DATETIME
AS
BEGIN
    INSERT INTO Tournament (Name, Location, MaxPlayerCount, MinPlayerCount, MaxEloAllowed, MinEloAllowed, Status, CurrentRound, WomenOnly, RegistrationEndDate, CreatedAt, UpdatedAt)
    OUTPUT inserted.*
    VALUES (@Name, @Location, @MaxPlayerCount, @MinPlayerCount, @MaxEloAllowed, @MinEloAllowed, @Status, @CurrentRound, @WomenOnly, @RegistrationEndDate, @CreatedAt, @UpdatedAt);
END;
go

-- Delete a tournament
CREATE PROCEDURE DeleteTournament
@TournamentId INT
AS
BEGIN
    DELETE FROM Tournament WHERE Id = @TournamentId;
END;
go

CREATE PROCEDURE GetAllCategory
AS
BEGIN
    SELECT * FROM Category;
END;
go

-- Get all members
CREATE PROCEDURE GetAllMembers
AS
BEGIN
    SELECT * FROM Member;
END;
go

-- Get all tournaments
CREATE PROCEDURE GetAllTournaments
AS
BEGIN
    SELECT * FROM Tournament;
END;
go

CREATE PROCEDURE GetCategoryById
@CategoryId INT
AS
BEGIN
    SELECT * FROM Category WHERE Id = @CategoryId;
END;
go

-- Get the last 10 modified tournaments
CREATE PROCEDURE GetLastModifiedTournaments
@number INT
AS
BEGIN
    SELECT TOP (@number) * FROM Tournament ORDER BY UpdatedAt DESC;
END;
go

CREATE PROCEDURE GetMemberByEmail
@Email NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Member WHERE Email = @Email;
END;
go

-- Get a member by ID
CREATE PROCEDURE GetMemberById
@MemberId INT
AS
BEGIN
    SELECT * FROM Member WHERE Id = @MemberId;
END;
go

CREATE PROCEDURE GetMemberByPseudo
@Pseudo NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Member WHERE Pseudo = @Pseudo;
END;
go

-- Get a tournament by ID
CREATE PROCEDURE GetTournamentById
@TournamentId INT
AS
BEGIN
    SELECT * FROM Tournament WHERE Id = @TournamentId;
END;
go

CREATE PROCEDURE GetTournamentCategories

@TournamentId INT
AS
BEGIN

    SELECT * FROM Tournament_Category tc
                      JOIN Category c ON tc.Category_id = c.Id
    WHERE tc.Tournament_id = @TournamentId;

end
go

CREATE PROCEDURE GetTournamentMembers
@TournamentId INT
AS
BEGIN
    SELECT * FROM Tournament_Member tm
                      JOIN Member m ON tm.Member_id = m.Id
    WHERE tm.Tournament_id = @TournamentId;
end
go

CREATE PROCEDURE GetTournamentMembersCount

@TournamentId INT
AS
BEGIN
    SELECT COUNT(*) FROM Tournament_Member WHERE Tournament_id = @TournamentId;
END;
go

CREATE PROCEDURE IsPlayerRegisteredTournament
    @MemberId INT,
    @TournamentId INT,
    @IsRegistered BIT OUTPUT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM Tournament_Member
        WHERE Member_id = @MemberId AND Tournament_id = @TournamentId
    )
        BEGIN
            SET @IsRegistered = 1;
        END
    ELSE
        BEGIN
            SET @IsRegistered = 0;
        END
END;
go

CREATE PROCEDURE RegisterMemberToTournament

    @MemberId INT,
    @TournamentId INT

AS
BEGIN
    INSERT INTO Tournament_Member (Member_id, Tournament_id) VALUES (@MemberId, @TournamentId);
END;
go