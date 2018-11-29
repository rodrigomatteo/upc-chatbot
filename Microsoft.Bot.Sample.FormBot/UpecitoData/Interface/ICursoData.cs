using System.Collections.Generic;
using Upecito.Model.ViewModel;

namespace Upecito.Data.Interface
{
    public interface ICursoData
    {
        List<CourseByModuleViewModel> GetCourseByModuleActive(int idAlumno, string curso);
    }
}
