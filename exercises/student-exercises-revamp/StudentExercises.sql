DROP TABLE IF EXISTS StudentExercise;
DROP TABLE IF EXISTS Exercise;
DROP TABLE IF EXISTS Student;
DROP TABLE IF EXISTS Instructor;
DROP TABLE IF EXISTS Cohort;

--1. Create tables from each entity in the Student Exercises ERD.
CREATE TABLE Cohort (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
);

CREATE TABLE Student (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	SlackHandle VARCHAR(50) NOT NULL,
	CohortId INTEGER NOT NULL,
	CONSTRAINT FK_StudentCohort FOREIGN KEY (CohortId)
	REFERENCES Cohort(Id)
);

CREATE TABLE Instructor (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	FirstName VARCHAR(50) NOT NULL,
	LastName VARCHAR(50) NOT NULL,
	SlackHandle VARCHAR(50) NOT NULL,
	CohortId INTEGER NOT NULL,
	CONSTRAINT FK_InstructorCohort FOREIGN KEY (CohortId)
	REFERENCES Cohort(Id)
);

CREATE TABLE Exercise (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	[Language] VARCHAR(50) NOT NULL
);

CREATE TABLE StudentExercise (
	Id INTEGER NOT NULL PRIMARY KEY IDENTITY,
	StudentId Integer,
	ExerciseId Integer,
	CONSTRAINT FK_SEStudent FOREIGN KEY (StudentId)
	REFERENCES Student(Id),
	CONSTRAINT FK_SEExercise FOREIGN KEY (ExerciseId)
	REFERENCES Exercise(Id)
);

--2. Populate each table with data. You should have 2-3 cohorts, 5-10 students, 4-8 instructors, 2-5 exercises and each student should be assigned 1-2 exercises.

INSERT INTO Cohort
VALUES ('Cohort 29'), ('Cohort 30'), ('Cohort E8');

INSERT INTO Student (FirstName, LastName, SlackHandle, CohortId)
VALUES ('Cole', 'Bryant', 'colebryant', 1), ('Asia', 'Carter', 'asiacart', 1), ('Hunter', 'Metts', 'hunterboy12', 1), ('Paul', 'McCartney', 'pauliem', 2),
('John', 'Lennon', 'johnlenn', 2), ('George', 'Harrison', 'georgeharr', 2), ('Ringo', 'Starr', 'worstbeatlesdrummer', 2), ('Maggie', 'Leavell', 'maggiel', 3);

INSERT INTO Instructor (FirstName, LastName, SlackHandle, CohortId)
VALUES ('Andy', 'Collins', 'acollins', 1), ('Leah', 'Hoefling', 'leahhoef', 1), ('Yoko', 'Ono', '5thbeatle', 2), ('John', 'Wark', 'johnw', 3);

INSERT INTO Exercise ([Name], [Language])
VALUES ('Kennel', 'React'), ('Bangazon', 'C#'), ('Front-End Capstone', 'React'), ('Nutshell', 'JavaScript'), ('Welcome to Nashville', 'JavaScript'), ('Superhero', 'CSS');

INSERT INTO StudentExercise(StudentId, ExerciseId)
VALUES (1, 1), (1, 2), (2, 1), (2, 2), (3, 3), (3, 4), (4, 3), (4, 4), (5, 5), (5, 6), (6, 5), (6, 6), (7, 1), (7, 4), (8, 2), (8, 4);