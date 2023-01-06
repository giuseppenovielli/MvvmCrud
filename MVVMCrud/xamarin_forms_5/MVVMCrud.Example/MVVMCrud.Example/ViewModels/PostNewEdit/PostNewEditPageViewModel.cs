using MVVMCrud.Example.Models.Post;
using MVVMCrud.Models.Base;
using MVVMCrud.Services.Request;
using MVVMCrud.ViewModels.Base;
using Prism.Navigation;

namespace MVVMCrud.Example.ViewModels.PostNewEdit
{
    public class PostNewEditPageViewModel : BaseItemEditAdvancedViewModel
        <PostItem, PostItem, BaseModelItemRoot<PostItem>>
    {
        public PostNewEditPageViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
        }

        public override void SetupItemBeforeUpdate(PostItem itemInput)
        {
            base.SetupItemBeforeUpdate(itemInput);

            itemInput.UserId = 1;
        }
    }
}
