using Labb3.Command;
using Labb3.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using Labb3.Repositories;

namespace Labb3.ViewModels
{
    class MainWindowViewModel: ViewModelBase
    {
        private readonly QuestionPackRepository _repository;
        
        private QuestionPackViewModel _selectedPack;

		public QuestionPackViewModel SelectedPack
		{
			get => _selectedPack;
			set
			{
				_selectedPack = value;
				RaisePropertyChanged();
			}

		}

        public ViewModelBase Model
        {
            get => _model;
            set
            {
                _model = value;
                RaisePropertyChanged();
            }
        }
        private ViewModelBase _model;
        public ObservableCollection<QuestionPackViewModel> Packs { get; } = new();

		private QuestionPackViewModel _activePack;

		public QuestionPackViewModel ActivePack
		{
			get =>_activePack;
			set { 
				_activePack = value;
				RaisePropertyChanged();
				PlayerViewModel.RaisePropertyChanged(nameof(PlayerViewModel.ActivePack));
                ConfigurationViewModel.RaisePropertyChanged(nameof(ConfigurationViewModel.ActivePack));
				}
		}



        public PlayerViewModel? PlayerViewModel { get; set; }
		public ConfigurationViewModel ConfigurationViewModel { get;}

        public DelegateCommand PlayCommand { get; }

       



        public MainWindowViewModel()
        {
            var mongoContext = new MongoDBContext();
            _repository = new QuestionPackRepository(mongoContext);
            PlayerViewModel = new PlayerViewModel(this);
			ConfigurationViewModel = new ConfigurationViewModel(this);
           

            var pack = new QuestionPack("MyQuestionPack");
            ActivePack = new QuestionPackViewModel(pack);
			//ActivePack.Questions.Add(new Question($"Vad heter Sveriges huvudstad?", "Stockholm", "Göteborg", " Malmö", "Helsingborg"));
            Model = ConfigurationViewModel;
            PlayCommand = new DelegateCommand(PlayGame);

            LoadPacksFromDbAsync().Wait();
        }


        private void PlayGame(object? obj)
        {
            Model = PlayerViewModel;
            PlayerViewModel.StartGame();
        }

        public async Task SavePacksToDbAsync()
        {
            if (Packs == null) return;

           foreach (var packVm in Packs)
            {
                await _repository.SavePackAsync(packVm.Model);
            }
        }

        public async Task LoadPacksFromDbAsync()
        {
            var packs = await _repository.GetAllAsync();
            packs.Clear();

            foreach (var pack in packs)
            {
                Packs.Add(new QuestionPackViewModel(pack));
            }

            ActivePack = Packs.FirstOrDefault();
        }
      
    }


}

