IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'HospitalManagementSystem')
CREATE DATABASE HospitalManagementSystem;
GO

USE HospitalManagementSystem;
GO

CREATE TABLE Departments (
    DepartmentId INT PRIMARY KEY IDENTITY,
    DepartmentName VARCHAR(50) NOT NULL
);

CREATE TABLE Doctors (
    DoctorId INT PRIMARY KEY IDENTITY,
    DoctorName VARCHAR(100) NOT NULL,
    Specialty VARCHAR(100) NULL,
    DepartmentId INT NULL,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

CREATE TABLE Rooms (
    RoomId INT PRIMARY KEY IDENTITY,
    RoomNumber VARCHAR(10) NOT NULL,
    Occupied BIT NOT NULL DEFAULT 0
);

CREATE TABLE Patients (
    PatientId INT PRIMARY KEY IDENTITY,
    PatientName VARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender CHAR(1) NULL,
    Phone VARCHAR(20) NULL,
    RoomId INT NULL,
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);

CREATE TABLE Treatments (
    TreatmentId INT PRIMARY KEY IDENTITY,
    TreatmentName VARCHAR(100) NOT NULL,
    Description VARCHAR(500) NULL
);

CREATE TABLE Appointments (
    AppointmentId INT PRIMARY KEY IDENTITY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    DateTime DATETIME NOT NULL,
    FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),
    FOREIGN KEY (DoctorId) REFERENCES Doctors(DoctorId)
);

CREATE TABLE PatientTreatments (
    PatientId INT NOT NULL,
    TreatmentId INT NOT NULL,
    ObtainDate DATE NOT NULL,
    EndDate DATE NULL,
    PRIMARY KEY (PatientId, TreatmentId, ObtainDate),
    FOREIGN KEY (PatientId) REFERENCES Patients(PatientId),
    FOREIGN KEY (TreatmentId) REFERENCES Treatments(TreatmentId)
);


INSERT INTO Departments (DepartmentName) VALUES 
('Cardiology'), ('Neurology'), ('Pediatrics'), ('Orthopedics'), ('Oncology');

INSERT INTO Rooms (RoomNumber, Occupied) VALUES 
('101', 1), ('102', 1), ('103', 1), ('104', 1), ('105', 0), ('106', 1);

INSERT INTO Doctors (DoctorName, Specialty, DepartmentId) VALUES 
('Dr. Smith', 'Cardiologist', 1),
('Dr. Johnson', 'Neurosurgeon', 2),
('Dr. Williams', 'Pediatrician', 3),
('Dr. Brown', 'Orthopedic Surgeon', 4),
('Dr. Davis', 'Oncologist', 5);

INSERT INTO Patients (PatientName, DateOfBirth, Gender, Phone, RoomId) VALUES 
('John Doe', '1980-05-15', 'M', '555-0101', 1),
('Jane Smith', '1975-08-22', 'F', '555-0102', 2),
('Robert Johnson', '1992-03-10', 'M', '555-0103', 4);

INSERT INTO Treatments (TreatmentName, Description) VALUES 
('Chemotherapy', 'Cancer treatment using drugs'),
('Physical Therapy', 'Rehabilitation to improve mobility'),
('Angioplasty', 'Procedure to open blocked arteries');

INSERT INTO Appointments (PatientId, DoctorId, DateTime) VALUES 
(1, 1, '2025-04-10 09:00:00'),
(2, 2, '2025-04-10 10:30:00'),
(3, 5, '2025-04-11 14:00:00');

INSERT INTO PatientTreatments (PatientId, TreatmentId, ObtainDate, EndDate) VALUES 
(1, 1, '2025-01-10', '2025-01-15'),
(2, 3, '2025-02-05', '2025-02-05'),
(3, 2, '2025-01-20', '2025-06-20');
