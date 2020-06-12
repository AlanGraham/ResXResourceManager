namespace ResXManager.Scripting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.IO;
    using System.Text.RegularExpressions;

    using JetBrains.Annotations;

    using ResXManager.Infrastructure;
    using ResXManager.Model;

    [Export]
    [Export(typeof(ISourceFilesProvider))]
    internal class SourceFilesProvider : ISourceFilesProvider, IFileFilter
    {
        private Regex? _fileExclusionFilter;
        public string? SolutionFolder { get; set; }
        public string? ExclusionFilter { get; set; }

        public IList<ProjectFile> SourceFiles
        {
            get
            {
                var folder = SolutionFolder;
                if (string.IsNullOrEmpty(folder))
                    return Array.Empty<ProjectFile>();

                _fileExclusionFilter = ExclusionFilter.TryCreateRegex();

                return new DirectoryInfo(folder).GetAllSourceFiles(this);
            }
        }

        public void Invalidate()
        {
        }

        public bool IsSourceFile(ProjectFile file)
        {
            return false;
        }

        public bool IncludeFile(ProjectFile file)
        {
            return _fileExclusionFilter?.IsMatch(file.RelativeFilePath) != true;
        }
    }
}