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

-- Get all tournaments
CREATE PROCEDURE GetAllTournaments
AS
BEGIN
    SELECT * FROM Tournament;
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

-- Get the last 10 modified tournaments
CREATE PROCEDURE GetLastModifiedTournaments
@number INT
AS
BEGIN
    SELECT TOP (@number) * FROM Tournament ORDER BY UpdatedAt DESC;
END;
go

--Get the list of members registered to a tournament

CREATE PROCEDURE GetTournamentMembers
    @TournamentId INT
AS
BEGIN 
    SELECT * FROM Tournament_Member tm
    JOIN Member m ON tm.Member_id = m.Id
    WHERE tm.Tournament_id = @TournamentId;
end

--Check if a player is registered to a tournament

CREATE PROCEDURE CheckPlayerTournamentMembership
    @MemberId INT,
    @TournamentId INT,
    @IsMember BIT OUTPUT
AS
BEGIN
    IF EXISTS (
        SELECT 1
        FROM Tournament_Member
        WHERE Member_id = @MemberId AND Tournament_id = @TournamentId
    )
        BEGIN
            SET @IsMember = 1;
        END
    ELSE
        BEGIN
            SET @IsMember = 0;
        END
END;
GO


