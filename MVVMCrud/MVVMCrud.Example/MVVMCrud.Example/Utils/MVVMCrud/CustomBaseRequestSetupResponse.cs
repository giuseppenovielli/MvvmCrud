using System.Threading.Tasks;
using MVVMCrud.Utils.Request;

namespace MVVMCrud.Example.Utils.MVVMCrud
{
    public class CustomBaseRequestSetupResponse : BaseRequestSetupResponse
    {

        public override Task SetupShowLoading()
        {
            return base.SetupShowLoading();
        }

        public override Task SetupHideLoading()
        {
            return base.SetupHideLoading();
        }
    }

    


}
