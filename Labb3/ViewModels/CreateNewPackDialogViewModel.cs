using Labb3.Command;
using Labb3.Models;
using Labb3.Repositories;
using System.Collections.ObjectModel;
using System.Windows;

namespace Labb3.ViewModels
{

    public class CreateNewPackDialogViewModel : ViewModelBase
    {
        private readonly CategoryRepository _categoryRepository;


        public QuestionPackViewModel NewPack { get; set; } = new QuestionPackViewModel(new QuestionPack("Enter Name"));

        public ObservableCollection<Category> Categories { get; set; }

        public DelegateCommand CreateCommand { get; }
        public DelegateCommand CancelCommand { get; }

        public DelegateCommand AddCategoryCommand { get; }

        public DelegateCommand DeleteCategoryCommand { get; }

        public string? NewCategoryName { get; set; } = "";
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
            
            var context = new MongoDBContext();
            _categoryRepository = new CategoryRepository(context);
            Categories = new ObservableCollection<Category>();
            
            SelectedCategory = Categories.FirstOrDefault();
            Categories.CollectionChanged += (s, e) =>
            {
                Console.WriteLine($"Categories count: {Categories.Count}");
            };
            CreateCommand = new DelegateCommand(Create);
            CancelCommand = new DelegateCommand(Cancel);
            AddCategoryCommand = new DelegateCommand(AddCategory);
            DeleteCategoryCommand = new DelegateCommand(DeleteCategory);
            _ = LoadCategoriesAsync();
            
        }


        private void Create(object? obj)
        {
            if (SelectedCategory != null)
            {
                NewPack.Category = SelectedCategory.Name;
            }

            else
            {
                NewPack.Category = "No Category";
            }

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
            try
            {
                var category = new Category { Name = NewCategoryName };
                await _categoryRepository.AddAsync(category);
                Categories.Add(category);
                SelectedCategory = category;

                NewCategoryName = string.Empty;

                RaisePropertyChanged(nameof(NewCategoryName));
            }
            catch (Exception ex) 
            {
                MessageBox.Show("Error adding category: " + ex.Message);
            }
        }

        private async void DeleteCategory(object? obj)
        {
            if(SelectedCategory == null)
                return;
            try
            {
                await _categoryRepository.DeleteAsync(SelectedCategory);
                Categories.Remove(SelectedCategory);
                SelectedCategory = null;
                RaisePropertyChanged(nameof(SelectedCategory));
            }

            catch(Exception ex)
            {
                MessageBox.Show("Error deleting category: " + ex.Message);
            }
        }

        private async Task LoadCategoriesAsync()
        {
            var context = new MongoDBContext();
            var repo = new CategoryRepository(context);
            var categoriesFromDb = await repo.GetAllAsync();
            MessageBox.Show($"Loaded categories from DB: {categoriesFromDb.Count}");
            
        
            if (categoriesFromDb.Count > 0)
            Categories.Clear();
            foreach (var category in categoriesFromDb)
            {
                Categories.Add(category);
                MessageBox.Show($"Added category: {category.Name}");
            }
        }

        private Category? _selectedCategory;
        public Category? SelectedCategory
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
            DeleteCategoryCommand = new DelegateCommand(DeleteCategory);


        }
    }
}


