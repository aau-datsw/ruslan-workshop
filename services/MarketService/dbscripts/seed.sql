\connect market-db

CREATE TABLE company (
    id         SERIAL PRIMARY KEY, 
    name       TEXT, 
    price      INTEGER,
    volatility INTEGER
) WITH (OIDS = FALSE);

CREATE TABLE history (
    id        SERIAL PRIMARY KEY,
    price     INTEGER,
    timestamp INTEGER,
    company   INTEGER, 

    FOREIGN KEY company REFERENCES company(id)
) WITH (OIDS = FALSE);

ALTER TABLE company OWNER TO ya;
ALTER TABLE history OWNER TO ya;
