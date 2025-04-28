
DROP TABLE DDV.Image;
DROP TABLE DDV.UserPlace;
DROP TABLE DDV.Place;

CREATE TABLE DDV.Place 
(
    Id          VARCHAR2(450) NOT NULL,
    Title       VARCHAR2(2000) NOT NULL,
    Lat         BINARY_FLOAT NOT NULL,
    Lon         BINARY_FLOAT NOT NULL,
    CONSTRAINT PK_Place PRIMARY KEY (Id) USING INDEX
);

CREATE TABLE DDV.Image 
(
    Id          VARCHAR2(450) NOT NULL,
    Src         VARCHAR2(2000) NOT NULL,
    Alt         VARCHAR2(2000),
    CONSTRAINT PK_Image PRIMARY KEY (Id) USING INDEX
/*    ,    CONSTRAINT FK_Place FOREIGN KEY (Id) REFERENCES DDV.place(Id) ON DELETE CASCADE  */
);

CREATE TABLE DDV.UserPlace 
(
    UserId      VARCHAR2(450) NOT NULL,
    PlaceId     VARCHAR2(450) NOT NULL,
    AddedOn     DATE DEFAULT SYSDATE,
    CONSTRAINT PK_UserPlace PRIMARY KEY (UserId, PlaceId) USING INDEX,
    CONSTRAINT FK_UP_PLACE FOREIGN KEY (PlaceID) REFERENCES DDV.place(Id)
);

CREATE OR REPLACE VIEW UserPlaces

AS
(
    SELECT up.UserId, p.Id, p.Title, i.Src, i.Alt, p.Lat, p.Lon
    FROM DDV.userplace up, DDV.place p, DDV.image i
    WHERE up.placeid = p.id
    AND   up.placeid = i.id
);

CREATE OR REPLACE VIEW Places
AS
(
    SELECT p.Id, p.Title, i.Src, i.Alt, p.Lat, p.Lon
    FROM DDV.place p, DDV.image i
    WHERE p.id = i.id(+)
);

