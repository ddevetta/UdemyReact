DECLARE
V_COUNT INTEGER;
BEGIN
SELECT COUNT(TABLE_NAME) INTO V_COUNT from USER_TABLES where TABLE_NAME = '__EFMigrationsHistory';
IF V_COUNT = 0 THEN
Begin
BEGIN
EXECUTE IMMEDIATE 'CREATE TABLE
"__EFMigrationsHistory" (
    "MigrationId" NVARCHAR2(150) NOT NULL,
    "ProductVersion" NVARCHAR2(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
)';
END;

End;

END IF;
EXCEPTION
WHEN OTHERS THEN
    IF(SQLCODE != -942)THEN
        RAISE;
    END IF;
END;
/

DECLARE
    v_Count INTEGER;
BEGIN
SELECT COUNT(*) INTO v_Count FROM "__EFMigrationsHistory" WHERE "MigrationId" = N'20250416154336_InitialCreate';
IF v_Count = 0 THEN

    DECLARE
        USEREXIST INTEGER;
        USER_NOT_EXIST EXCEPTION;
        PRAGMA EXCEPTION_INIT( USER_NOT_EXIST, -01435 );
    BEGIN
    SELECT COUNT(*) INTO USEREXIST FROM ALL_USERS WHERE USERNAME='DDV';
    IF (USEREXIST = 0) THEN
        RAISE USER_NOT_EXIST;
    END IF;
    END;
 END IF;
END;

/

DECLARE
    v_Count INTEGER;
BEGIN
SELECT COUNT(*) INTO v_Count FROM "__EFMigrationsHistory" WHERE "MigrationId" = N'20250416154336_InitialCreate';
IF v_Count = 0 THEN

    BEGIN
    EXECUTE IMMEDIATE 'CREATE TABLE
    "DDV"."UserPlaces" (
        "User" NVARCHAR2(450) NOT NULL,
        "Id" NVARCHAR2(450) NOT NULL,
        "Title" NVARCHAR2(2000) NOT NULL,
        "ImageId" NVARCHAR2(2000),
        "Lat" BINARY_FLOAT NOT NULL,
        "Lon" BINARY_FLOAT NOT NULL,
        CONSTRAINT "PK_UserPlaces" PRIMARY KEY ("User", "Id")
    )';
    END;
 END IF;
END;

/

DECLARE
    v_Count INTEGER;
BEGIN
SELECT COUNT(*) INTO v_Count FROM "__EFMigrationsHistory" WHERE "MigrationId" = N'20250416154336_InitialCreate';
IF v_Count = 0 THEN

    EXECUTE IMMEDIATE '
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES (N''20250416154336_InitialCreate'', N''9.0.4'')
    ';
 END IF;
END;

/
