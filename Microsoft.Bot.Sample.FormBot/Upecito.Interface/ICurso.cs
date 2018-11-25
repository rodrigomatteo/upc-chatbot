using Upecito.Model.ViewModel;

namespace Upecito.Interface
{
    interface ICurso
    {
        CourseByModuleViewModel GetCourseByModuleActive(int IdAlumno);
    }
}
