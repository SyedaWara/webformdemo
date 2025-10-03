-- Course Table
CREATE TABLE Courses (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

-- Teacher Table
CREATE TABLE Teachers (
    Id INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100) NOT NULL
);

-- TeacherCourses (Mapping Table for Many-to-Many)
CREATE TABLE TeacherCourses (
    TeacherId INT FOREIGN KEY REFERENCES Teachers(Id),
    CourseId INT FOREIGN KEY REFERENCES Courses(Id),
    PRIMARY KEY (TeacherId, CourseId)
);

-- Alter Student Table
ALTER TABLE Students
ADD CourseId INT FOREIGN KEY REFERENCES Courses(Id);
