using Upecito.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upecito.Data.Interface
{
    public interface ICursoData
    {
        CourseByModuleViewModel GetCourseByModuleActive(int IdAlumno);
    }
}
