CREATE TABLE Roles (
    role_id SERIAL PRIMARY KEY,
    role_name VARCHAR(50) NOT NULL
);

INSERT INTO Roles(role_id, role_name)	
	VALUES (1, 'Admin'), (2, 'Teacher'), (3, 'Student');

CREATE TABLE Users (
    user_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    role_id INT,
    FOREIGN KEY (role_id) REFERENCES Roles(role_id)
);




CREATE TABLE Roles (
    role_name VARCHAR(50) PRIMARY KEY
);

INSERT INTO Roles(role_name)	
	VALUES ('Admin', 'Teacher', 'Student');

CREATE TABLE Users (
    user_id SERIAL PRIMARY KEY,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    role_name VARCHAR(50),
    FOREIGN KEY (role_name) REFERENCES Roles(role_name)
);