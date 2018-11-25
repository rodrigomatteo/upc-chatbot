using SimpleInjector;
using Upecito.Data.Interface;
using Upecito.Interface;
using Upecito.Model;

namespace Upecito.Business
{
    public class CicloManager
    {
        private Container container;

        public CicloManager(Container container)
        {
            this.container = container;
        }
    }
}