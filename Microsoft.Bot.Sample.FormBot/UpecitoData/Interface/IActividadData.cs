using System.Collections.Generic;
using Upecito.Model.ViewModel;

namespace Upecito.Data.Interface
{
    public interface IActividadData
    {
        List<ActivitiesByCourseViewModel> GetActivitiesByCourse(int IdAlumno);
    }
}