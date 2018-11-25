using Upecito.Model.ViewModel;

namespace Upecito.Interface
{
    public interface IActividad
    {
        ActivitiesByCourseViewModel GetActivitiesByCourse(int IdAlumno);
    }
}