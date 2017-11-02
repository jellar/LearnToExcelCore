using AutoMapper;

namespace LearnToExcel.Core.Models.InstructorViewModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Instructor, InstructorIndexData.Instructor>();
            CreateMap<CourseInstructor, InstructorIndexData.CourseInstructor>();
            CreateMap<Course, InstructorIndexData.Course>();
            CreateMap<Enrollment, InstructorIndexData.Enrollment>();
        }
    }
}
