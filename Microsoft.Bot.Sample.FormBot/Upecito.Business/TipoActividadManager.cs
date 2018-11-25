using SimpleInjector;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model;

namespace Upecito.Business
{
    public class TipoActividadManager
    {
        private Container container;

        public TipoActividadManager(Container container)
        {
            this.container = container;
        }
    }
}