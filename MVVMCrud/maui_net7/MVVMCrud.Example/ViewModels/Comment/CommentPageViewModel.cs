using System.Collections.Generic;
using System.Linq;
using MVVMCrud.Example.Models.Comment;
using MVVMCrud.Example.Models.Post;
using MVVMCrud.Example.ViewModels.Post;
using MVVMCrud.Example.Views.Post;
using MVVMCrud.Models.Base;
using MVVMCrud.Services.Request;
using MVVMCrud.ViewModels.Base;
using MVVMCrud.Views.Base;
using Prism.Navigation;

namespace MVVMCrud.Example.ViewModels.Comment
{
    public class CommentPageViewModel : BaseListPaginationAdvancedHeaderViewModel
        <
        //List
        BaseCellViewModel<CommentItem>, BaseModelItemsRoot<CommentItem>, CommentItem,

        //Header
        PostCellViewModel, BaseModelItemRoot<PostItem>, PostItem
        >
    {
        public CommentPageViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
        }


        #region SetupHeader
        public override string SetupHeaderEndpoint()
        {
            return Constants.Constants.METHOD_POST;
        }

        public override PostCellViewModel SetupHeaderInstanceCell(PostItem item)
        {
            return new PostCellViewModel(item, false);
        }

        public override BaseContentView SetupHeaderView()
        {
            return new PostCell();
        }

        public override string SetupFromPageViewModelName()
        {
            return nameof(PostPageViewModel);
        }
        #endregion

        public override string SetupTitlePage()
        {
            return AppResources.title_page_post_comments;
        }

        public override string SetupEndpoint()
        {
            return string.Format("{0}{1}/comments/", Constants.Constants.METHOD_POST, HeaderID);
        }

        public override List<BaseCellViewModel<CommentItem>> PerformSearchSetup(string newText)
        {
            var newTextLower = newText.ToLower();
            var l = ItemsList.Where(o => o.Item.Name.ToLower().Contains(newTextLower)).ToList();
            return l;
        }

        public override bool SetupIsPaginationEnable()
        {
            return false;
        }
    }
}
