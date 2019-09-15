\connect statistics-db

CREATE TABLE person (
    id          SERIAL PRIMARY KEY,
    name        TEXT,
    other_name  TEXT
) WITH (OIDS=FALSE);

ALTER TABLE person OWNER TO ya;

INSERT INTO person(name, other_name) 
VALUES 
    ('Anders Brams', 'The literal god'),
    ('Frederik Spang', 'DuckerKongen'),
    ('Tobias Palludan', 'Danernes Lys');