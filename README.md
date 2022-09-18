# MvvmCrud
MvvmCrud helps your Xamarin.Forms app with Prism Library to standardize operations of creating, updating, deletion and display data, from REST API.

NuGet Soon...


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

All fully customizable!

```
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
            var l =  ItemsList.Where(o => o.Item.Title.ToLower().Contains(newTextLower)).ToList();
            return l;
        }

        public override string SetupDetailPageName()
        {
            return nameof(CommentPage);
        }

    }
```


## Docs

Clone this repository and check app's example. 

App's example use the follow endpoint https://jsonplaceholder.typicode.com/ to test functionality.

## Feedback or Requests
Use GitHub [Issues](https://github.com/giuseppenovielli/MvvmCrud/issues) for bug reports and feature requests.

Use GitHub [Discussios](https://github.com/giuseppenovielli/MvvmCrud/discussions) for questions or opinions.


## Disclaimer

This is at the moment an experiment. Use at your own risk.

## Copyright and license
Code released under the [MIT license](https://opensource.org/licenses/MIT).

## Did you like ?
[Buy Me A Coffee](https://www.buymeacoffee.com/giuseppeDev)

