delete from nourrissage;
delete from croissance_poisson_dobo;
delete from poisson_dobo;
delete from entre_vague;
delete from croissance_race;
delete from aliment;

drop view v_nourissage_prix_aliment;

drop view v_nourrissage;

alter table croissance_race
    drop column apport_glucide_g;

alter table croissance_race
    drop column apport_proteine_g;

alter table aliment
    drop column pourcentage_proteine;

alter table aliment
    drop column pourcentage_glucide;
    
create table nutriment (
    id_nutriment int primary key,
    nom_nutriment varchar(50)
);

create table nutriment_aliment(
    id_nutriment_aliment serial primary key ,
    id_aliment int,
    id_nutriment int,
    pourcentage_nutriment double precision,
    foreign key (id_aliment) references aliment(id_aliment),
    foreign key (id_nutriment) references nutriment(id_nutriment)
);


CREATE SEQUENCE seq_aliment
    START 4
    INCREMENT 1;

ALTER TABLE aliment
    ALTER COLUMN id_aliment
        SET DEFAULT (nextval('seq_aliment'));

CREATE SEQUENCE seq_nutriment
    START 4
    INCREMENT 1;

ALTER TABLE nutriment
    ALTER COLUMN id_nutriment
        SET DEFAULT (nextval('seq_nutriment'));

CREATE SEQUENCE seq_race
    START 4
    INCREMENT 1;

ALTER TABLE race
    ALTER COLUMN id_race
        SET DEFAULT (nextval('seq_race'));

alter table croissance_race
    add column id_nutriment int;

alter table croissance_race
    add foreign key (id_nutriment) references nutriment(id_nutriment);

alter table croissance_race
    add column poids_nutriment_g double precision default 0;

insert into nutriment (id_nutriment, nom_nutriment)
values (1,'Protéine'),
(2,'Glucide'),
(3,'Lipide');

insert into aliment (id_aliment, nom_aliment, prix_achat_kg)
VALUES (1,'Tourteau de soja',1500);
insert into aliment (id_aliment, nom_aliment, prix_achat_kg)
VALUES (2,'Maïs',2000);
insert into aliment (id_aliment, nom_aliment, prix_achat_kg)
VALUES (3,'Provende',5000);

-- Tourteau de soja
insert into nutriment_aliment (id_aliment, id_nutriment, pourcentage_nutriment)
values (1,1,20),
(1,2,5),
(1,3,15);

-- Maïs
insert into nutriment_aliment (id_aliment, id_nutriment, pourcentage_nutriment)
values (2,2,10),
       (2,2,25),
       (2,3,15);

-- Provende
insert into nutriment_aliment (id_aliment, id_nutriment, pourcentage_nutriment)
values (3,1,12),
       (3,2,5),
       (3,3,2);

-- Croissance race
insert into croissance_race (id_race, poids_obtenu_g, id_nutriment, poids_nutriment_g) 
VALUES (1,0,1,10),
       (1,0,2,20),
       (1,0,3,5);

insert into croissance_race (id_race, poids_obtenu_g, id_nutriment, poids_nutriment_g)
VALUES (2,0,1,10),
       (2,0,2,2),
       (2,0,3,7);

insert into croissance_race (id_race, poids_obtenu_g, id_nutriment, poids_nutriment_g)
VALUES (3,0,1,5),
       (3,0,2,10),
       (3,0,3,15);





