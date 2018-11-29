using System.Collections.Generic;
using Upecito.Model.ViewModel;

namespace Upecito.Interface
{
    interface ICurso
    {
        List<CourseByModuleViewModel> GetCourseByModuleActive(int idAlumno, string curso);
    }
}
