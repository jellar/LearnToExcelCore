namespace LearnToExcel.Core.Models
{
    public class CourseInstructor
    {
        public int InstructorId { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Instructor Instructor { get; set; }
    }
}
