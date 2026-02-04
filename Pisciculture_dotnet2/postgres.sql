create database pisciculture;
\c pisciculture


create table dobo(
    id_dobo varchar(30) primary key
);

create table race (
    id_race int primary key ,
    nom_race varchar(50) not null,
    prix_achat_kg double precision not null,
    prix_vente_kg double precision not null
);

create table aliment (
    id_aliment int primary key ,
    nom_aliment varchar(100) not null ,
    pourcentage_proteine double precision not null ,
    pourcentage_glucide double precision not null ,
    prix_achat_kg double precision not null
);

create table croissance_race (
    id_croissance_race serial primary key ,
    id_race int,
    apport_proteine_g double precision,
    apport_glucide_g double precision,
    poids_obtenu_g double precision,
    foreign key (id_race) references race(id_race)
);

create table entre_vague (
    id_entre_vague int primary key ,
    id_dobo varchar(30),
    date_entree date,
    id_race int,
    nombre_poissons int,
    poids_initiale_poisson double precision,
    foreign key (id_dobo) references dobo(id_dobo),
    foreign key (id_race) references race(id_race)
);

create table nourrissage (
    id_nourrissage int primary key ,
    date_heure_nourrissage timestamp,
    id_aliment int,
    id_dobo varchar(30),
    poids_aliments double precision,
    foreign key (id_dobo) references dobo(id_dobo),
    foreign key (id_aliment) references aliment(id_aliment)
);

create table poisson_dobo (
    id_poisson_dobo varchar(20) primary key ,
    id_entre_vague int,
    id_race int,
    poids_initiale_poisson double precision,
    foreign key (id_entre_vague) references entre_vague(id_entre_vague),
    foreign key (id_race) references race(id_race)
);

create table croissance_poisson_dobo (
    id_croissance_poisson_dobo serial primary key ,
    id_poisson_dobo varchar(20),
    poids_recu_kg double precision,
    date_heure_croissance timestamp,
    foreign key (id_poisson_dobo) references poisson_dobo(id_poisson_dobo)
);

-- changement table
CREATE SEQUENCE seq_poisson_dobo
    START 1
    INCREMENT 1;

ALTER TABLE poisson_dobo
    ALTER COLUMN id_poisson_dobo
        SET DEFAULT ('POIS0' || nextval('seq_poisson_dobo'));

alter table race
    add column poids_max double precision;

alter table nourrissage
  drop column date_heure_nourrissage;

alter table nourrissage
    add column date_nourrissage date;

alter table croissance_poisson_dobo
    drop column date_heure_croissance;

alter table croissance_poisson_dobo
    add column date_croissance date;

CREATE SEQUENCE seq_entree_vague
    START 1
    INCREMENT 1;

alter table entre_vague
    alter column id_entre_vague set default nextval('seq_entree_vague');

CREATE SEQUENCE seq_nourrissage
    START 1
    INCREMENT 1;

alter table nourrissage
    alter column id_nourrissage set default nextval('seq_nourrissage');


-- Insertion des données
insert into dobo (id_dobo) values ('DOBO1');
insert into dobo (id_dobo) values ('DOBO2');
insert into dobo (id_dobo) values ('DOBO3');


insert into race (id_race, nom_race, poids_max, prix_achat_kg, prix_vente_kg)
values (1,'Tilapia',5,1000,5000);
insert into race (id_race, nom_race, poids_max, prix_achat_kg, prix_vente_kg)
values (2,'Carpe',4.5,2000,6000);
insert into race (id_race, nom_race, poids_max, prix_achat_kg, prix_vente_kg)
values (3,'Truite',3,1500,4000);

insert into aliment (id_aliment, nom_aliment, pourcentage_proteine, pourcentage_glucide, prix_achat_kg)
VALUES (1,'Tourteau de soja',5.0,14.0,1500);
insert into aliment (id_aliment, nom_aliment, pourcentage_proteine, pourcentage_glucide, prix_achat_kg)
VALUES (2,'Maïs',7.0,20.0,2000);
insert into aliment (id_aliment, nom_aliment, pourcentage_proteine, pourcentage_glucide, prix_achat_kg)
VALUES (3,'Provende',10.0,8.0,5000);

insert into croissance_race (id_race, apport_proteine_g, apport_glucide_g, poids_obtenu_g)
VALUES (1,2.0,4.0,10);
insert into croissance_race (id_race, apport_proteine_g, apport_glucide_g, poids_obtenu_g)
VALUES (2,7.0,3.0,30);
insert into croissance_race (id_race, apport_proteine_g, apport_glucide_g, poids_obtenu_g)
VALUES (3,5.0,4.5,20);


-- Procedure stockée pour insérée poisson dobo automatiquement a chaque vague d'entree
---- Entree dobo -> poissons
CREATE OR REPLACE FUNCTION fn_insert_poissons()
    RETURNS trigger AS
$$
DECLARE
    i INT;
BEGIN
    FOR i IN 1..NEW.nombre_poissons LOOP

            INSERT INTO poisson_dobo (
                id_entre_vague,
                id_race,
                poids_initiale_poisson
            )
            VALUES (
                       NEW.id_entre_vague,
                       NEW.id_race,
                       NEW.poids_initiale_poisson
                   );

        END LOOP;

    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE TRIGGER trg_insert_poissons
    AFTER INSERT ON entre_vague
    FOR EACH ROW
EXECUTE FUNCTION fn_insert_poissons();

---- Nourrissage -> poissons

-- Donnée entrée vague
insert into entre_vague (id_dobo, date_entree, id_race, nombre_poissons, poids_initiale_poisson)
values ('DOBO1','2026-01-12',1,10,0.25);
insert into entre_vague (id_dobo, date_entree, id_race, nombre_poissons, poids_initiale_poisson)
values ('DOBO2','2026-01-12',2,10,0.2);
insert into entre_vague (id_dobo, date_entree, id_race, nombre_poissons, poids_initiale_poisson)
values ('DOBO3','2026-01-02',1,10,0.25);
insert into entre_vague (id_dobo, date_entree, id_race, nombre_poissons, poids_initiale_poisson)
values ('DOBO3','2026-01-02',3,15,0.1);

-- Donnée nourrissage poisson
insert into nourrissage ( id_dobo,date_nourrissage, id_aliment,  poids_aliments)
values ('DOBO1','2026-01-12',1,3);
insert into nourrissage ( id_dobo,date_nourrissage, id_aliment,  poids_aliments)
values ('DOBO2','2026-01-12',1,3);
insert into nourrissage ( id_dobo,date_nourrissage, id_aliment,  poids_aliments)
values ('DOBO3','2026-01-02',1,7);