# MvvmCrud
MvvmCrud helps your Xamarin.Forms/MAUI app with Prism Library to standardize operations of creating, updating, deletion and display data, from REST API.

|                 | Preview                                                                                                                                                                                                                                                                                                                                                | Stable |
|-----------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|--------|
| Xamarin.Forms_5 | [![NuGet version (MvvmCrud.Forms.Prism)](https://img.shields.io/nuget/v/MvvmCrud.Forms.Prism.svg?style=flat-square)](https://www.nuget.org/packages/MvvmCrud.Forms.Prism/) [![NuGet version (MvvmCrud.Forms.Prism)](https://img.shields.io/nuget/dt/MvvmCrud.Forms.Prism.svg?style=flat-square)](https://www.nuget.org/packages/MvvmCrud.Forms.Prism/) |        |
| MAUI_NET7       | [![NuGet version (MvvmCrud.Maui.Prism)](https://img.shields.io/nuget/v/MvvmCrud.Maui.Prism.svg?style=flat-square)](https://www.nuget.org/packages/MvvmCrud.Maui.Prism/) [![NuGet version (MvvmCrud.Maui.Prism)](https://img.shields.io/nuget/dt/MvvmCrud.Maui.Prism.svg?style=flat-square)](https://www.nuget.org/packages/MvvmCrud.Maui.Prism/)       |        |

## How it works

This framework standardize CRUD's operations into ViewModel's file using generic T type, endpoint's requests and fully customizable.

*The target is WRITE LESS CODE, STANDARDIZING THE MOST USED UI CRUD OPERATIONS, IMPROVE PRODUCTIVITY SPEED.*

Only with these following lines of code your page can:
- Display list of data
- Embedded custom message, if endpoint fails
- Embedded pagination's manager
- Embedded Perform searchs
- Embedded deletion cell's content, with automatic detect endpoint and confirm message
- Embedded update cell's content, with automatic: detect endpoint, open page, save, upload and update list's cell
- Embedded open detail's page
- Embedded empty view UI
- Embedded loadig more view UI

All fully customizable!

```
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

        public override List<PostCellViewModel> PerformSearchSetup(string newText) => ItemsList.Where(x => x.Item.Title.Contains(newText.ToLower())).ToList();

        #region DetailPage
        public override string SetupDetailPageName(PostCellViewModel obj) => nameof(CommentPage);

        public override bool IsDetailPageWithHeader(PostCellViewModel obj) => true;
        #endregion

    }
}
```


## Docs

- Clone this repository and check app's example. App's example use the follow endpoint https://jsonplaceholder.typicode.com/ to test functionality.
- Go to [Wiki](https://github.com/giuseppenovielli/MvvmCrud/wiki) page.

## Feedback or Requests
Use GitHub [Issues](https://github.com/giuseppenovielli/MvvmCrud/issues) for bug reports and feature requests.

Use GitHub [Discussios](https://github.com/giuseppenovielli/MvvmCrud/discussions) for questions or opinions.


## Disclaimer

This is at the moment an experiment. Use at your own risk.

## Copyright and license
Code released under the [MIT license](https://opensource.org/licenses/MIT).

## Did you like ?
[Buy Me A Coffee](https://www.buymeacoffee.com/giuseppeDev)

