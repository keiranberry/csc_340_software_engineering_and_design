namespace StudentSearch.Helpers
{
    public class Enums
    {
        public enum SortByParameter
        {
            FirstNameASC = 0,
            FirstNameDESC,
            LastNameASC,
            LastNameDESC,
            GraduationDateASC,
            GraduationDateDESC,
            GPAASC,
            GPADESC,
            MajorASC,
            MajorDESC
        }

        public enum OrderByParameter
        {
            Ascending = 0,
            Descending
        }
    }

}
