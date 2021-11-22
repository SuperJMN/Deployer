using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CSharpFunctionalExtensions;
using Deployer.Wim;
using ReactiveUI;
using Unit = System.Reactive.Unit;

namespace Deployer.Gui.ViewModels.Requirements
{
    internal class WimFileRequirementViewModel : RequirementViewModelBase
    {
        private Maybe<string> wimFilePath;
        private readonly ObservableAsPropertyHelper<IEnumerable<DiskImageMetadata>> images;
        private Maybe<DiskImageMetadata> selectedImage;
        public WimFileRequirement Requirement { get; }

        public WimFileRequirementViewModel(WimFileRequirement requirement, IWindowsImageMetadataReader windowsImageMetadataReader, IFileSystem fileSystem) : base(requirement)
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
                            Extensions = new List<string> {"install.esd;install.wim"},
                            Name = "Windows Images"
                        }
                    }
                };

                var currentApplicationLifetime = (ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime;
                var win=currentApplicationLifetime.MainWindow;
                var files = Maybe.From(await picker.ShowAsync(win));
                return files.Map(paths => paths.First());
            });

            var obs = this
                .WhenAnyValue(x => x.WimFilePath)
                .SelectMany(path => path.ToList())
                .SelectMany(path => LoadImage(windowsImageMetadataReader, fileSystem, path))
                .Select(metadata => metadata.Match(m => m, s => Enumerable.Empty<DiskImageMetadata>()));

            images = obs.ToProperty(this, x => x.Images);

            Browse.Subscribe(s => WimFilePath = s);

            IsValid = this
                .WhenAnyValue(t => t.SelectedImage)
                .Select(maybe => maybe.HasValue);

            this.WhenAnyValue(x => x.Images).Where(r => r is not null).Subscribe(imgs => SelectedImage = imgs.TryFirst());
        }

        public Maybe<DiskImageMetadata> SelectedImage
        {
            get => selectedImage;
            set => this.RaiseAndSetIfChanged(ref selectedImage , value);
        }

        private static IObservable<Result<IList<DiskImageMetadata>>> LoadImage(IWindowsImageMetadataReader windowsImageMetadataReader, IFileSystem fileSystem, string s)
        {
            if (!fileSystem.File.Exists(s))
            {
                return Observable.Return(Result.Failure<IList<DiskImageMetadata>>("The image doesn't exist"));
            }

            return Observable.Using<Result<IList<DiskImageMetadata>>, Stream>(() => fileSystem.File.OpenRead(s),
                stream =>
                {
                    var result = windowsImageMetadataReader.Load(stream).Map(x => x.Images);
                    return Observable.Return(result);
                });
        }

        public IEnumerable<DiskImageMetadata> Images => images.Value;

        public ReactiveCommand<Unit, Maybe<string>> Browse { get; }

        public Maybe<string> WimFilePath
        {
            get => wimFilePath;
            set => this.RaiseAndSetIfChanged(ref wimFilePath, value);
        }

        public override IEnumerable<(string, object)> FilledRequirements => new (string, object)[]
        {
            (Requirement.Key + "Index", SelectedImage.GetValueOrThrow().Index),
            (Requirement.Key + "Path", WimFilePath.GetValueOrThrow()),
        };

        public override IObservable<bool> IsValid { get; }
    }
}