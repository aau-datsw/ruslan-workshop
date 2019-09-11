\connect matchmaking-db

CREATE TABLE people (
    id          SERIAL       PRIMARY KEY,
    name        TEXT,
    other_name  TEXT
) WITH (OIDS=FALSE);

ALTER TABLE people OWNER TO ya;

INSERT INTO people(name, other_name) VALUES(
    'Anders Brams',
    'The literal god'
);