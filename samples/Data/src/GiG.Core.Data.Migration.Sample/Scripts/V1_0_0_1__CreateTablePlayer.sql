CREATE TABLE player
(
	id				BIGSERIAL PRIMARY KEY,
	resource_id		VARCHAR(36)	NOT NULL,
	name			VARCHAR(100) NOT NULL,
	email			VARCHAR(100) NOT NULL, 
	created_by 		VARCHAR(36),
	created_on		TIMESTAMPTZ	NOT NULL
);

CREATE UNIQUE INDEX player_resource_id_uindex ON player (resource_id);