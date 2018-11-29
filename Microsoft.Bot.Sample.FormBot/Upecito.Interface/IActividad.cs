using System.Collections.Generic;
using Upecito.Model.ViewModel;

namespace Upecito.Interface
{
    public interface IActividad
    {
        List<ActivitiesByCourseViewModel> GetActivitiesByCourse(int idAlumno);
    }
}