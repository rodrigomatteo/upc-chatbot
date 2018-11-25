using SimpleInjector;
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

        public CourseByModuleViewModel GetCourseByModuleActive(int IdCurso)
        {
            var data = container.GetInstance<ICursoData>();
            return data.GetCourseByModuleActive(IdCurso);
        }
    }
}