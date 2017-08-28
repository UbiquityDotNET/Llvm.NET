namespace Llvm.NET.BuildTasks
{
    public interface IPrereleaseVersion
    {
        (int NameIndex, byte Number, byte Fix) Version { get; }

        string ToString( bool useFullPrerelNames );
    }
}
