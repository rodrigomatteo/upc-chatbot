using SimpleInjector;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model;

namespace Upecito.Business
{
    public class ActividadManager
    {
        private Container container;

        public ActividadManager(Container container)
        {
            this.container = container;
        }
    }
}
