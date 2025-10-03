ALTER TABLE StudentCourses
DROP COLUMN TeacherID;

ALTER TABLE StudentCourses
ADD TeacherID INT NULL;

ALTER TABLE StudentCourses
ADD CONSTRAINT FK_StudentCourses_Teachers FOREIGN KEY (TeacherID) REFERENCES Teachers(TeacherID);
