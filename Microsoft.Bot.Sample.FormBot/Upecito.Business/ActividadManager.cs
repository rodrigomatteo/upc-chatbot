using SimpleInjector;
using System.Collections.Generic;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model.ViewModel;

namespace Upecito.Business
{
    public class ActividadManager: IActividad
    {
        private Container container;

        public ActividadManager(Container container)
        {
            this.container = container;
        }

        public List<ActivitiesByCourseViewModel> GetActivitiesByCourse(int idAlumno)
        {
            var actividadData = container.GetInstance<IActividadData>();
            return actividadData.GetActivitiesByCourse(idAlumno);

        }
    }
}
