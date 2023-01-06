using MVVMCrud.Example.Models.Post;
using MVVMCrud.ViewModels.Base;

namespace MVVMCrud.Example.ViewModels.Post
{
    public class PostCellViewModel : BaseCellViewModel<PostItem>
    {
        public bool ShowCommentIsVisible { get; set; }

        public PostCellViewModel()
        {
            ShowCommentIsVisible = true;
        }

        public PostCellViewModel(
            PostItem item,
            bool showCommentButton = true) : base(item)
        {
            ShowCommentIsVisible = showCommentButton;
        }

    }
}
