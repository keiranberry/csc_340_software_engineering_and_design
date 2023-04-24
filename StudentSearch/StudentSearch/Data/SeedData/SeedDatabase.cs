using Microsoft.EntityFrameworkCore;
using StudentSearch.Models;
using System.Diagnostics;
using System.Reflection;

namespace StudentSearch.Data.SeedData
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using var context = new StudentSearchContext(serviceProvider.GetRequiredService<DbContextOptions<StudentSearchContext>>());

            // Look for any Students.
            // NOTE:  Not robust enough yet.
            if (context.Student.Any())
            {
                return;
            }

            // Reference:
            // https://stackoverflow.com/questions/3314140/how-to-read-embedded-resource-text-file
            // 
            //

            var assembly = Assembly.GetExecutingAssembly();

            // NOTE:
            // Use the following to get the exact resource name
            // to be assigned to the resourceName variable below.
            //
            //string[] resourceNames = assembly.GetManifestResourceNames();

            string resourceName = "StudentSearch.Data.SeedData.MOCK_DATA.csv";
            string line;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName)!)
            using (StreamReader reader = new StreamReader(stream))
            {
                // Eat the header row.
                reader.ReadLine();
                while ((line = reader.ReadLine()!) != null)
                {
                    // Writes to the Output Window.
                    Debug.WriteLine(line);

                    // Logic to parse the line, separate by comma(s), and assign fields
                    // to the Student model.

                    string[] values = line.Split(",");

                    context.Student.Add(
                        new Student
                        {
                            FirstName = values[1],
                            LastName = values[2],
                            ExpectedGraduation = values[3],
                            GPA = decimal.Parse(values[4]),
                            Major = values[5]
                        }
                    );
                }
            }

            context.SaveChanges();
        }
    }
}