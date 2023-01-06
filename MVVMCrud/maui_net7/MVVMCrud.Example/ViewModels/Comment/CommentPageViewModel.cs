﻿using System.Collections.Generic;
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
using MVVMCrud.Example.Views.PostNewEdit;

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
<<<<<<<< HEAD:MVVMCrud/maui_net7/MVVMCrud.Example/ViewModels/Comment/CommentPageViewModel.cs
        public override PostCellViewModel SetupHeaderInstanceCell(PostItem item) => new PostCellViewModel(item, false);

        public override BaseContentView SetupHeaderView() => new PostCell();

========
        public override string SetupHeaderEndpoint() => Constants.Constants.METHOD_POST;

        public override PostCellViewModel SetupHeaderInstanceCell(PostItem item) => new PostCellViewModel(item, false);

        public override BaseContentView SetupHeaderView() => new PostCell();

>>>>>>>> xamarin_forms:MVVMCrud/xamarin_forms_5/MVVMCrud.Example/MVVMCrud.Example/ViewModels/Comment/CommentPageViewModel.cs
        public override async void UpdateEditHeaderItem(NewEditItem<PostItem> editHeaderItem)
        {
            SetupEditItemMessage();
            await GetHeader();
        }
        #endregion

<<<<<<<< HEAD:MVVMCrud/maui_net7/MVVMCrud.Example/ViewModels/Comment/CommentPageViewModel.cs
        public override string SetupEndpoint() => string.Format("{0}{1}/comments/", Constants.Constants.METHOD_POST, HeaderID);

        public override List<BaseCellViewModel<CommentItem>> PerformSearchSetup(string newText) => ItemsList.Where(o => o.Item.Name.ToLower().Contains(newText.ToLower())).ToList();

========
        public override string SetupEndpoint()
        {
            return string.Format("{0}{1}/comments/", Constants.Constants.METHOD_POST, HeaderID);
        }

        public override List<BaseCellViewModel<CommentItem>> PerformSearchSetup(string newText) => ItemsList.Where(o => o.Item.Name.ToLower().Contains(newText.ToLower())).ToList();

>>>>>>>> xamarin_forms:MVVMCrud/xamarin_forms_5/MVVMCrud.Example/MVVMCrud.Example/ViewModels/Comment/CommentPageViewModel.cs
        public override bool SetupIsPaginationEnable() => false;
    }
}