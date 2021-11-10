using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Iridio;
using Iridio.Preprocessing;
using Iridio.Runtime;
using IFileSystem = System.IO.Abstractions.IFileSystem;

namespace Deployer
{
    public class DeployerCore
    {
        private readonly ISourceCodeCompiler compiler;
        private readonly IInterpreter interpreter;
        private readonly IFileSystem fileFileSystem;
        private readonly IPreprocessor preprocessor;

        public DeployerCore(IInterpreter interpreter, IFileSystem fileFileSystem, IPreprocessor preprocessor, ISourceCodeCompiler sourceCodeCompiler)
        {
            compiler = sourceCodeCompiler;
            this.preprocessor = preprocessor;
            this.interpreter = interpreter;
            this.fileFileSystem = fileFileSystem;
        }

        public async Task<Result<ExecutionSummary, IridioError>> Run(string path, IDictionary<string, object> initialState)
        {
            using (new DirectorySwitch(fileFileSystem, fileFileSystem.Path.GetDirectoryName(path)))
            {
                return await BuildCore(fileFileSystem.Path.GetFileName(path)).Bind(artifact => RunArtifact(artifact, initialState));
            }
        }

        public Result<BuildArtifact, IridioError> Check(string path)
        {
            return BuildCore(path);
        }

        public IObservable<string> Messages => interpreter.Messages;

        private Result<BuildArtifact, IridioError> BuildCore(string path)
        {
            var sourceCode = preprocessor.Process(path);
            return compiler.Compile(sourceCode)
                .Map(script => new BuildArtifact(sourceCode, script))
                .MapError(compilerError => (IridioError)new IridioCompileError(compilerError, sourceCode));
        }

        private async Task<Result<ExecutionSummary, IridioError>> RunArtifact(BuildArtifact artifact, IDictionary<string, object> initialState)
        {
            var run = await interpreter.Run(artifact.Script, initialState);
            return run
                .MapError(error => (IridioError)new IridioRuntimeError(error, artifact.Source));
        }
    }
}