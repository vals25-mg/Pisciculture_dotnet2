-- Vue Nourrissage
create or replace view v_nourrissage as
select 
    n.*,
    a.nom_aliment,
    a.pourcentage_glucide,
    a.pourcentage_proteine,
    a.prix_achat_kg
    from 
        nourrissage n 
join public.aliment a on a.id_aliment = n.id_aliment;

-- Vue nourrissage prix_aliment
create or replace view  v_nourissage_prix_aliment as
select
            row_number() over () id,
            id_aliment,
            nom_aliment,
            date_nourrissage,
            id_dobo,
            poids_aliments,
            prix_achat_kg,
            poids_aliments * prix_achat_kg as prix_total
from
    v_nourrissage;




create or replace view v_croissance_poisson_dobo as
select 
    row_number() over () as id,
    cpd.*,
    pd.id_race
    from 
    croissance_poisson_dobo cpd 
join public.poisson_dobo pd on pd.id_poisson_dobo = cpd.id_poisson_dobo;

