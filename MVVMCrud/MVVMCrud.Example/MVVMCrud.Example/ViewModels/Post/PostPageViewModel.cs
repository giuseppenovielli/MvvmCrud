using System.Collections.Generic;
using System.Linq;
using MVVMCrud.Example.Models.Post;
using MVVMCrud.Example.Views.Comment;
using MVVMCrud.Models.Base;
using MVVMCrud.Services.Request;
using MVVMCrud.ViewModels.Base;
using Prism.Navigation;

namespace MVVMCrud.Example.ViewModels.Post
{
    public class PostPageViewModel : BaseListPaginationAdvancedViewModel
        <PostCellViewModel, BaseModelItemsRoot<PostItem>, PostItem>
    {
        public PostPageViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
        }

        public override string SetupEndpoint()
        {
            return Constants.Constants.METHOD_POST;
        }

        public override List<PostCellViewModel> PerformSearchSetup(string newText)
        {
            return ItemsList.Where(o => o.Item.Title.StartsWith(newText, System.StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        public override string SetupDetailPageName()
        {
            return nameof(CommentPage);
        }

    }
}
