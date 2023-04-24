using StudentSearch.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StudentSearch.Data;
using StudentSearch.Tests.Helpers;
using System.Data.Common;

namespace StudentSearch.Tests.Fixture
{
    public class SharedDatabaseFixture : IDisposable
    {
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        public SharedDatabaseFixture()
        {
            Connection = new SqlConnection(@"Server=(localdb)\mssqllocaldb;Database=StudentSearchTests;Trusted_Connection=True;MultipleActiveResultSets=true");

            Seed();

            Connection.Open();
        }

        public void Dispose()
        {
            Connection.Dispose();
        }

        public DbConnection Connection
        {
            get;
        }

        public StudentSearchContext CreateContext(DbTransaction? transaction = null)
        {
            var context = new StudentSearchContext(new DbContextOptionsBuilder<StudentSearchContext>()
                .UseSqlServer(Connection).Options);

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        AddStudents(context);
                        context.SaveChanges();

                        foreach (var student in context.Student.ToArray())
                        {
                            AddComment(context, student);
                        }
                        context.SaveChanges();

                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void AddStudents(StudentSearchContext context)
        {
            context.Student
                .AddRange(
                    new Student
                    {
                        FirstName = Constants.FIRST_NAME,
                        LastName = Constants.LAST_NAME_1,
                        ExpectedGraduation = Constants.YEAR_2024,
                        GPA = Constants.GPA_1,
                        Major = Constants.MAJOR_1
                    },
                    new Student
                    {
                        FirstName = Constants.FIRST_NAME,
                        LastName = Constants.LAST_NAME_2,
                        ExpectedGraduation = Constants.YEAR_2024,
                        GPA = Constants.GPA_2,
                        Major = Constants.MAJOR_2
                    },
                    new Student
                    {
                        FirstName = Constants.FIRST_NAME,
                        LastName = Constants.LAST_NAME_3,
                        ExpectedGraduation = Constants.YEAR_2023,
                        GPA = Constants.GPA_3,
                        Major = Constants.MAJOR_1
                    });

        }

        private void AddComment(StudentSearchContext context, Student student)
        {
            context.Comment
                .Add(
                    new Comment
                    {
                        EnteredOn = DateTime.Now,
                        Text = Constants.COMMENT_TEXT,
                        Student = student,
                        ApplicationUser = context.Users.FirstOrDefault()
                    });
        }

    }
}

