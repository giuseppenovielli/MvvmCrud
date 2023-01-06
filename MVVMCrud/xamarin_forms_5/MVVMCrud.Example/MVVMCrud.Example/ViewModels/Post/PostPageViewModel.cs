using System.Collections.Generic;
<<<<<<<< HEAD:MVVMCrud/maui_net7/MVVMCrud.Example/ViewModels/Post/PostPageViewModel.cs
using System.Linq;
using MVVMCrud.Example.Models.Post;
using MVVMCrud.Example.Views.Comment;
using MVVMCrud.Models.Base;
using MVVMCrud.Services.Request;
using MVVMCrud.ViewModels.Base;

namespace MVVMCrud.Example.ViewModels.Post
{
    public class PostPageViewModel : BaseListPaginationAdvancedViewModel
        <PostCellViewModel, BaseModelItemsRoot<PostItem>, PostItem>
    {
        public PostPageViewModel(
            INavigationService navigationService,
            IRequestService requestService) : base(navigationService, requestService)
        {
========
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

        public override string SetupEndpoint() => Constants.Constants.METHOD_POST;

        public override async void AddNewItem(PostItem item)
        {
            SetupAddItemMessage();
            await SetupGetItems();
        }

        public override async void UpdateEditItem(NewEditItem<PostItem> newEditItem)
        {
            SetupEditItemMessage();
            await SetupGetItems();
        }

        public override List<PostCellViewModel> PerformSearchSetup(string newText)
        {
            return ItemsList.Where(x => x.Item.Title.Contains(newText.ToLower())).ToList();
>>>>>>>> xamarin_forms:MVVMCrud/xamarin_forms_5/MVVMCrud.Example/MVVMCrud.Example/ViewModels/Post/PostPageViewModel.cs
        }

        #region Header
        public override string SetupDetailPageName(PostCellViewModel obj) => nameof(CommentPage);

<<<<<<<< HEAD:MVVMCrud/maui_net7/MVVMCrud.Example/ViewModels/Post/PostPageViewModel.cs
        public override async void AddNewItem(PostItem item)
        {
            SetupAddItemMessage();
            await SetupGetItems();
        }

        public override async void UpdateEditItem(NewEditItem<PostItem> newEditItem)
        {
            SetupEditItemMessage();
            await SetupGetItems();
        }

        public override List<PostCellViewModel> PerformSearchSetup(string newText) => ItemsList.Where(x => x.Item.Title.Contains(newText.ToLower())).ToList();

        #region DetailPage
        public override string SetupDetailPageName(PostCellViewModel obj) => nameof(CommentPage);

        public override bool IsDetailPageWithHeader(PostCellViewModel obj) => true;
        #endregion

    }
}
========
        public override bool IsDetailPageWithHeader(PostCellViewModel obj) => true;
        #endregion
    }
}
>>>>>>>> xamarin_forms:MVVMCrud/xamarin_forms_5/MVVMCrud.Example/MVVMCrud.Example/ViewModels/Post/PostPageViewModel.cs
