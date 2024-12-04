-- Create a new member
CREATE PROCEDURE CreateMember
    @Pseudo NVARCHAR(255),
    @Email NVARCHAR(255),
    @Password NVARCHAR(255),
    @Gender CHAR(1),
    @Role NVARCHAR(10),
    @BirthDate DATE
AS
BEGIN
    INSERT INTO Member (Pseudo, Email, Password, Gender, Role, BirthDate)
    VALUES (@Pseudo, @Email, @Password, @Gender, @Role, @BirthDate);
END;
go

-- Delete a member
CREATE PROCEDURE DeleteMember
@MemberId INT
AS
BEGIN
    DELETE FROM Member WHERE Id = @MemberId;
END;
go

-- Get all members
CREATE PROCEDURE GetAllMembers
AS
BEGIN
    SELECT * FROM Member;
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

-- Update a member
CREATE PROCEDURE UpdateMember
    @MemberId INT,
    @Pseudo NVARCHAR(255),
    @Email NVARCHAR(255),
    @Password NVARCHAR(255),
    @Gender CHAR(1),
    @Role NVARCHAR(10),
    @BirthDate DATE
AS
BEGIN
    UPDATE Member
    SET Pseudo = @Pseudo,
        Email = @Email,
        Password = @Password,
        Gender = @Gender,
        Role = @Role,
        BirthDate = @BirthDate
    WHERE Id = @MemberId;
END;
go
