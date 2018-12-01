using SimpleInjector;
using System.Collections.Generic;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model.ViewModel;

namespace Upecito.Business
{
    public class CursoManager: ICurso
    {
        private Container container;

        public CursoManager(Container container)
        {
            this.container = container;
        }

        public List<CourseByModuleViewModel> GetCourseByModuleActive(int idAlumno)
        {
            var cursoData = container.GetInstance<ICursoData>();
            return cursoData.GetCourseByModuleActive(idAlumno);
        }
    }
}