using Labb3.Command;
using Labb3.Models;
using Labb3.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Labb3.ViewModels
{
    class ConfigurationViewModel: ViewModelBase
    {
        private readonly MainWindowViewModel? _mainWindowViewModel;
        private readonly CategoryRepository _categoryRepository;
        public ObservableCollection<Category> Categories { get; } = new();

        public DelegateCommand ChoosePackCommand { get; }

        public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
        {
            this._mainWindowViewModel = mainWindowViewModel;
            var mongoContext = new MongoDBContext();
            _categoryRepository = new CategoryRepository(mongoContext);

        }

        public QuestionPackViewModel? ActivePack
        {
            get => _mainWindowViewModel.ActivePack;
            set
            {
                _mainWindowViewModel.ActivePack = value;
                RaisePropertyChanged();
            }
        }


        private void ChoosePack(object? obj)
        {
            if (obj is QuestionPackViewModel pack)
            {
                ActivePack = pack;
            }
            
        }

    }
}
