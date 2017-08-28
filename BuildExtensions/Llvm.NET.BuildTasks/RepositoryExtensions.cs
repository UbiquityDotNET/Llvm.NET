using LibGit2Sharp;

namespace Llvm.NET.BuildTasks
{
    internal static class RepositoryExtensions
    {
        internal static BuildMode GetBuildMode( this Repository repo, bool buildServer, bool pullRequest, string officialBranch)
        {
            RepositoryStatus repoStatus = repo.RetrieveStatus( );
            if( repoStatus.IsDirty || !buildServer )
            {
                return BuildMode.LocalDev;
            }

            if( pullRequest )
            {
                return BuildMode.PullRequest;
            }

            if( string.CompareOrdinal( repo.Head.FriendlyName, officialBranch ) == 0 )
            {
                return BuildMode.OfficialRelease;
            }

            return BuildMode.ContinuousIntegration;
        }
    }
}
