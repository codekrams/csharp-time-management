create database personal;

create table mitarbeiter(
maid primary key auto_increment integer,
nachname varchar(100),
vprname varchar(100),
gebdat date,
tagesarbeitszeit integer,
urlaubsanspruch integer,
bild varchar(200),
bewertung integer
);

create table fehlgrund(
fehlid primary key auto_increment integer,
bezeichnung varchar(100)
);

create table einsatz(
eid primary key auto_increment integer,
maid integer,
datum date,
einsatzVon_Zeit time,
einsatzBis_Zeit time,
foreign key (maid) references mitarbeiter (maid)
);

create table fehlzeit(
fzid primary key auto_increment integer,
maid integer,
von_Datum date,
bis_Datum date,
fid integer,
fehltage integer,
foreign key (maid) references mitarbeiter (maid)
foreign key (fid) references fehlgrund (fehlid)
);