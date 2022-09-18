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
            var newTextLower = newText.ToLower();
            return ItemsList.Where((PostCellViewModel arg1, int arg2) =>
            {
                var item = arg1.Item;
                if (
                        !string.IsNullOrWhiteSpace(item.Title)
                        &&
                        item.Title.ToLower().Contains(newTextLower)
                    )
                {
                    return true;
                }
                return false;

            }).ToList();
        }

        public override string SetupDetailPageName()
        {
            return nameof(CommentPage);
        }

    }
}
