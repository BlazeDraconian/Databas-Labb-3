using Labb3.Command;
using Labb3.Models;
using Labb3.Repositories;
using System.Collections.ObjectModel;

namespace Labb3.ViewModels
{

    public class CreateNewPackDialogViewModel : ViewModelBase
    {

        public QuestionPackViewModel NewPack { get; set; } = new QuestionPackViewModel(new QuestionPack("Enter Name"));

        public ObservableCollection<string> Categories { get; set; }

        public DelegateCommand CreateCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public DelegateCommand AddCategoryCommand { get; }

        public string? NewCategoryName { get; set; }
        public Array Difficulties => Enum.GetValues(typeof(Difficulty));

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get => _dialogResult;
            private set
            {
                if (_dialogResult == value) return;
                _dialogResult = value;
                RaisePropertyChanged(nameof(DialogResult));
            }
        }

        public CreateNewPackDialogViewModel()
        {
            CreateCommand = new DelegateCommand(Create);
            CancelCommand = new DelegateCommand(Cancel);
        }


        private void Create(object? obj)
        {
            NewPack.Category = SelectedCategory;
            DialogResult = true;
        }

        private void Cancel(object? obj)
        {
            DialogResult = false;
        }

        private async void AddCategory(Object? obj)
        {
            if (string.IsNullOrWhiteSpace(NewCategoryName))
                return;

            var context = new MongoDBContext();
            var repo = new CategoryRepository(context);
            await repo.AddAsync(NewCategoryName);
            Categories.Add(NewCategoryName);
            SelectedCategory = NewCategoryName;

            NewCategoryName = "Write category name here";

            RaisePropertyChanged(nameof(NewCategoryName)); 
        }

        private string? _selectedCategory;
        public string? SelectedCategory
        {
            get => _selectedCategory;
            set 
            {
                _selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        public CreateNewPackDialogViewModel(QuestionPackViewModel existingPack)
        {
            NewPack.Name = existingPack.Name;
            NewPack.Difficulty = existingPack.Difficulty;
            NewPack.TimeLimitInSeconds = existingPack.TimeLimitInSeconds;

            CreateCommand = new DelegateCommand((_) =>
            {
                existingPack.Name = NewPack.Name;
                existingPack.Difficulty = NewPack.Difficulty;
                existingPack.TimeLimitInSeconds = NewPack.TimeLimitInSeconds;

                DialogResult = true;
            });

            CancelCommand = new DelegateCommand((_) =>
            {
                DialogResult = false;
            });

            AddCategoryCommand = new DelegateCommand(AddCategory);


        }
    }
}


