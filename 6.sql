-- Rename Id → CourseID
EXEC sp_rename 'Courses.Id', 'CourseID', 'COLUMN';

-- Rename Name → CourseName
EXEC sp_rename 'Courses.Name', 'CourseName', 'COLUMN';
-- Rename Id → TeacherID
EXEC sp_rename 'Teachers.Id', 'TeacherID', 'COLUMN';

-- Rename Name → TeacherName
EXEC sp_rename 'Teachers.Name', 'TeacherName', 'COLUMN';
