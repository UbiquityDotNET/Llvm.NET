# GIT OneFlow support scripts
The scripts in this folder are used for release and feature branch management. This
repository follows the [OneFlow](https://www.endoflineblog.com/oneflow-a-git-branching-model-and-workflow#develop-finishing-a-release-branch)
model and work-flow. With one active long term branch 'develop'. The main branch is present
and long term but is not active, it only points to the latest official release (including
preview releases) of the project. This is a convenience to allow getting the latest released
source quickly. Generally speaking, the scripts used here are only for release managers and
are not required (or even an option) for most contributors.

Typical workflow:
1. Modify `BuildVersion.xml` (But do ***NOT*** commit it)
2. Run `Start-Release.ps1`
    1. This will setup and push the various branches used for the release
    2. The build version is NOT committed as the branches can allow CI/PR builds depending
       on the nature of the release changes.
3. Any changes needed for the release are made and pushed (via PR) to the `release/<releas name>`
   branch.
4. Commit, or remodify, `BuildVersion.xml` and push it to the official repo release branch.
    1. This is typically done with git push official to direct push the changes to the
       official repository
5. Run `Publish-Release.ps1` to publish the release and begin the final release process.
    1. This will trigger the automated release build action, which will publish the NuGet
       packages etc... It also creates a merge-back branch for all changes that went into
       the release.
6. Once the release build completes, edit and publish the release in GitHub.
    1. The "release" is created by the automated build as a draft and requires manual
       publication. This is usually editing or selecting the base for auto generated release
       notes.
7. Apply the merge-back PR to the `develop` branch to update the `BuildVersion.xml` at a
   minimum.

---
Step 3 & 4 are normally resolve to simply committing the changes to `BuildVersion.xml` and
pushing that to the official repository. That is, in most cases there are no changes to the
branch other than the bump of version number. That's actually the ideal as it eliminates (
or dramatically reduces the complexity of) conflict resolution to merge the changes into.
