using Upecito.Model.ViewModel;

namespace Upecito.Data.Interface
{
    public interface IActividadData
    {
        ActivitiesByCourseViewModel GetActivitiesByCourse(int IdAlumno);
    }
}