CREATE DATABASE IF NOT EXISTS docker_sh;
USE docker_sh;

CREATE TABLE IF NOT EXISTS roles (
    id_role INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(45) NOT NULL
);

CREATE TABLE IF NOT EXISTS users (
    id_user INT AUTO_INCREMENT PRIMARY KEY,
    login VARCHAR(45) NOT NULL,
    password VARCHAR(45) NOT NULL,
    name VARCHAR(105) NOT NULL,
    email VARCHAR(255),
    phone_number VARCHAR(20),
    id_role INT NOT NULL,
    FOREIGN KEY (id_role) REFERENCES roles(id_role)
);

INSERT IGNORE INTO roles (id_role, name) VALUES 
(1, 'Admin'),
(2, 'User'),
(3, 'Moderator');

INSERT IGNORE INTO users (id_user, login, password, name, email, phone_number, id_role) VALUES 
(1, 'admin', 'admin123', 'Administrator', 'admin@example.com', '+1234567890', 1),
(2, 'user1', 'password123', 'Regular User', 'user1@example.com', '+0987654321', 2);