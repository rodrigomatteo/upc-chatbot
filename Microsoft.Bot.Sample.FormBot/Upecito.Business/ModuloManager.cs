using SimpleInjector;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model;

namespace Upecito.Business
{
    public class ModuloManager
    {
        private Container container;

        public ModuloManager(Container container)
        {
            this.container = container;
        }
    }
}