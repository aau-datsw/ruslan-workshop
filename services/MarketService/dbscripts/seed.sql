\connect market-db

CREATE TABLE company (
    id          SERIAL PRIMARY KEY,
    name        TEXT,
    price       INTEGER,
    volatility  INTEGER
) WITH (OIDS=FALSE);

ALTER TABLE company OWNER TO ya;