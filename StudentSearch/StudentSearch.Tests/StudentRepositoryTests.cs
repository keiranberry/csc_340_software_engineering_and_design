using StudentSearch.Models;
using StudentSearch.Repository;
using StudentSearch.Repository.Interfaces;
using StudentSearch.Tests.Fixture;
using StudentSearch.Tests.Helpers;
using StudentSearch.ViewModels;
using static StudentSearch.Helpers.Enums;


namespace StudentSearch.Tests
{
    [Collection("DisablesParallelExecution")]
    public class StudentRepositoryTests : IClassFixture<SharedDatabaseFixture>
    {
        private readonly SharedDatabaseFixture _fixture;
        private readonly IStudentRepository _repository;

        public StudentRepositoryTests(SharedDatabaseFixture fixture)
        {
            _fixture = fixture;
            _repository = new StudentRepository(_fixture.CreateContext());
        }

        [Fact]
        public async Task Get_Students_FilterBy_Default()
        {
            // Arrange.

            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, string.Empty, SortByParameter.LastNameASC);

            // Assert.
            Assert.Equal(3, students.Count);

            // The number of inspectors should match the number of Students in the list.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName));
        }

        [Fact]
        public async Task Get_Students_FilterBy_None()
        {
            // Arrange.
            var searchString = "nothing";

            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, searchString, SortByParameter.LastNameASC);

            // Assert.
            Assert.Equal(0, students.Count);
        }

        [Fact]
        public async Task Get_Students_SearchBy_FirstName()
        {
            // Arrange.
            var searchString = Constants.FIRST_NAME;

            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, searchString, SortByParameter.LastNameASC);

            // Assert.
            Assert.Equal(3, students.Count);
        }

        [Fact]
        public async Task Get_Students_SearchBy_LastName()
        {
            // Arrange.
            var searchString = Constants.LAST_NAME_1;

            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, searchString, SortByParameter.LastNameASC);

            // Assert.
            Assert.Equal(1, students.Count);
        }

        [Fact]
        public async Task Get_Students_SearchBy_FullName()
        {
            // Arrange.
            var searchString = Constants.FULL_NAME_1;

            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, searchString, SortByParameter.LastNameASC);

            // Assert.
            Assert.Equal(1, students.Count);
        }



        [Fact]
        public async Task Get_Students_SortBy_FirstName_DESC()
        {
            // Arrange.


            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, string.Empty, SortByParameter.FirstNameDESC);

            // Assert.
            Assert.Equal(3, students.Count);

            // The number of inspectors should match the number of Students in the list in the right order.
            // NOTE: The Act sorts by FirstNameDESC which the logic will sort the Last Name Descending in the implementation.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName));
        }

        [Fact]
        public async Task Get_Students_SortBy_LastName_ASC()
        {
            // Arrange.


            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, string.Empty, SortByParameter.LastNameASC);

            // Assert.
            Assert.Equal(3, students.Count);

            // The number of inspectors should match the number of Students in the list in the right order.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName));
        }

        [Fact]
        public async Task Get_Students_SortBy_LastName_DESC()
        {
            // Arrange.


            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, string.Empty, string.Empty, SortByParameter.LastNameDESC);

            // Assert.
            Assert.Equal(3, students.Count);

            // The number of inspectors should match the number of Students in the list in the right order.
            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName));
        }

        [Fact]
        public async Task Get_Students_FilterBy_Grad()
        {
            // Arrange.
            var filterString = Constants.YEAR_2023;

            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, filterString, string.Empty, SortByParameter.LastNameDESC);

            // Assert.
            Assert.Equal(1, students.Count);

        }

        [Fact]
        public async Task Get_Students_FilterBy_Grad2()
        {
            // Arrange.
            var filterString = Constants.YEAR_2024;

            // Act.
            IList<Student> students = await _repository.GetStudents(string.Empty, filterString, string.Empty, SortByParameter.LastNameDESC);

            // Assert.
            Assert.Equal(2, students.Count);

            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_2, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName));

        }

        [Fact]
        public async Task Get_Students_FilterBy_Major()
        {
            // Arrange.
            var filterString = Constants.MAJOR_2;

            // Act.
            IList<Student> students = await _repository.GetStudents(filterString, string.Empty, string.Empty, SortByParameter.LastNameDESC);

            // Assert.
            Assert.Equal(1, students.Count);

        }

        [Fact]
        public async Task Get_Students_FilterBy_Major2()
        {
            // Arrange.
            var filterString = Constants.MAJOR_1;

            // Act.
            IList<Student> students = await _repository.GetStudents(filterString, string.Empty, string.Empty, SortByParameter.LastNameDESC);

            // Assert.
            Assert.Equal(2, students.Count);

            Assert.Collection(students,
                s => Assert.Equal(Constants.LAST_NAME_3, s.LastName),
                s => Assert.Equal(Constants.LAST_NAME_1, s.LastName));
        }

        [Fact]
        public async Task Insert_Student()
        {
            // Arrange.
            StudentViewModel viewModel = new()
            {
                FirstName = Constants.FIRST_NAME_2,
                LastName = Constants.LAST_NAME_4,
                ExpectedGraduation = Constants.YEAR_2025,
                GPA = Constants.GPA_4,
                Major = Constants.MAJOR_3
            };

            // Act.
            Student newStudent = await _repository.InsertStudent(viewModel);
            Student student = await _repository.GetStudentByID(newStudent.Id);

            // Assert.
            Assert.Same(newStudent, student);
            Assert.Equal(student.LastName, viewModel.LastName);
            Assert.Equal(student.ExpectedGraduation, viewModel.ExpectedGraduation);

            // Cleanup.
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Get_Student_ById()
        {
            // Arrange.
            int studentId = 1;

            // Act.
            Student student = await _repository.GetStudentByID(studentId);

            // Assert.
            Assert.Equal(student.LastName, Constants.LAST_NAME_1);
        }

        [Fact]
        public async Task Get_Student_ById_NotFound()
        {
            // Arrange.
            int studentId = -1;

            // Act.
            Student student = await _repository.GetStudentByID(studentId);

            // Assert.
            Assert.Null(student);
        }

        [Fact]
        public async Task Get_Student_ById_After_Insert()
        {
            // Arrange.
            StudentViewModel viewModel = new()
            {
                FirstName = Constants.FIRST_NAME_2,
                LastName = Constants.LAST_NAME_1,
                ExpectedGraduation = Constants.YEAR_2025,
                GPA = Constants.GPA_1,
                Major = Constants.MAJOR_3
            };

            // Act.
            Student newStudent = await _repository.InsertStudent(viewModel);
            Student sudent = await _repository.GetStudentByID(newStudent.Id);

            // Assert.
            Assert.Same(newStudent, sudent);
            Assert.Equal(sudent.LastName, viewModel.LastName);

            // Cleanup.
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Update_Student()
        {
            // Arrange.

            StudentViewModel viewModel = new()
            {
                FirstName = Constants.FIRST_NAME_2,
                LastName = Constants.LAST_NAME_2,
                ExpectedGraduation = Constants.YEAR_2025,
                GPA = Constants.GPA_4,
                Major = Constants.MAJOR_3
            };

            // Act.
            Student newStudent = await _repository.InsertStudent(viewModel);

            viewModel.Id = newStudent.Id;
            viewModel.FirstName = newStudent.FirstName;
            viewModel.LastName = Constants.LAST_NAME_4;
            viewModel.ExpectedGraduation = Constants.YEAR_2025;

            Student student = await _repository.UpdateStudent(viewModel);

            // Assert.
            Assert.IsAssignableFrom<Student>(student);
            Assert.Equal(student.LastName, Constants.LAST_NAME_4);

            // Cleanup.
            await _repository.DeleteStudent(newStudent.Id);
        }

        [Fact]
        public async Task Delete_Student()
        {
            // Arrange.
            StudentViewModel viewModel = new()
            {
                FirstName = Constants.FIRST_NAME,
                LastName = Constants.LAST_NAME_4,
                ExpectedGraduation = Constants.YEAR_2023,
                GPA = Constants.GPA_1,
                Major = Constants.MAJOR_2
            };

            // Act.
            Student newStudent = await _repository.InsertStudent(viewModel);

            int id = newStudent.Id;
            await _repository.DeleteStudent(id);

            Student student = await _repository.GetStudentByID(id);

            // Assert.
            Assert.Null(student);
        }

        [Fact]
        public async Task Insert_Comment()
        {
            // Arrange.
            int studentId = 1;
            DateTime commentEnteredOn = DateTime.Now;
            string commentText = "Good interview.";

            var viewModel = new StudentViewModel();

            viewModel.Id = studentId;
            viewModel.CommentEnteredOn = commentEnteredOn;
            viewModel.CommentEnteredBy = null;
            viewModel.CommentText = commentText;

            // Act.
            Comment comment = await _repository.InsertComment(viewModel);

            // Assert.
            Assert.Equal(commentEnteredOn, comment.EnteredOn);
            Assert.Equal(commentText, comment.Text);

            // Cleanup.
            await _repository.DeleteCommentByID(comment.Id);
        }

    }
}

