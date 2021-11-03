using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace Deployer.Gui.ViewModels.Requirements
{
    internal class WimFileRequirementViewModel : RequirementViewModelBase
    {
        private Maybe<string> wimFilePath;
        private int wimFileIndex;
        public WimFileRequirement Requirement { get; }

        public WimFileRequirementViewModel(WimFileRequirement requirement) : base(requirement)
        {
            Requirement = requirement;
            Browse = ReactiveCommand.CreateFromTask(async () =>
            {
                var picker = new OpenFileDialog
                {
                    Title = "Select a .WIM or ESD file",
                    Filters = new List<FileDialogFilter>
                    {
                        new()
                        {
                            Extensions = new List<string> {"*.esd;*.wim"},
                            Name = "Windows Images"
                        }
                    }
                };

                var currentApplicationLifetime = (ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;
                var win=currentApplicationLifetime.MainWindow;
                var files = await picker.ShowAsync(win);
                return files.TryFirst();
            });

            WimFileIndex = 1;
            Browse.Subscribe(s => WimFilePath = s);
        }

        public ReactiveCommand<Unit, Maybe<string>> Browse { get; }

        public Maybe<string> WimFilePath
        {
            get => wimFilePath;
            set => this.RaiseAndSetIfChanged(ref wimFilePath, value);
        }

        public int WimFileIndex
        {
            get => wimFileIndex;
            set => this.RaiseAndSetIfChanged(ref wimFileIndex, value);
        }

        public override IEnumerable<(string, object)> FilledRequirements => new (string, object)[]
        {
            (Requirement.Key + "Index", WimFileIndex),
            (Requirement.Key + "Path", WimFilePath),
        };
    }
}