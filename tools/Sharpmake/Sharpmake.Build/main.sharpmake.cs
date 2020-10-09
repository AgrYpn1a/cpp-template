using Sharpmake;
using System.IO;

[Generate]
public class MainProject : Project
{
    private string m_Root = @"[project.SharpmakeCsPath]\..\..\..";
    private const string mc_ProjectName = "#ProjectName";

    public MainProject()
    {
        Name = mc_ProjectName;
        SourceRootPath = Path.Combine(m_Root, "source", mc_ProjectName);

        AddTargets(new Target(
            Platform.win32 | Platform.win64,
            DevEnv.vs2019,
            Optimization.Debug | Optimization.Release
        ));
    }

    [Configure]
    public void ConfigureAll(Project.Configuration config, Target target)
    {
        config.ProjectPath = Path.Combine(m_Root, @"projects", mc_ProjectName);
        config.Options.Add(Options.Vc.Compiler.CppLanguageStandard.CPP17);

        // Additional includes
        //config.IncludePaths.Add(Path.Combine(m_Root, @"dependencies", @"spdlog", @"include"));
        //config.IncludePaths.Add(Path.Combine(m_Root, @"dependencies", @"entt", @"include"));
        //config.IncludePaths.Add(Path.Combine(m_Root, @"dependencies", "SDL", "include"));

        // Add libs
        //config.LibraryPaths.Add(Path.Combine(m_Root, @"dependencies", "SDL", "lib", "x64"));
        //config.LibraryFiles.Add("SDL2.lib");

        //config.TargetCopyFiles.Add(Path.Combine(m_Root, @"dependencies", "SDL", "lib", "x64", "SDL2.dll"));
    }
}

[Generate]
public class MainSolution : Solution
{
    private const string mc_SolutionName = "#SolutionName";
    private string m_Root = Path.Combine(@"[project.SharpmakeCsPath]", @"\..\..\..");

    public MainSolution()
    {
        Name = mc_SolutionName;
        AddTargets(new Target(
            Platform.win32 | Platform.win64,
            DevEnv.vs2019,
            Optimization.Debug | Optimization.Release
        ));
    }

    [Configure]
    public void ConfigureAll(Solution.Configuration config, Target target)
    {
        config.SolutionPath = Path.Combine(m_Root, "projects");
        config.AddProject<MainProject>(target);
    }
}

public static class Main
{
    [Sharpmake.Main]
    public static void SharpmakeMain(Arguments sharpmakeArgs)
    {
        KitsRootPaths.SetUseKitsRootForDevEnv(DevEnv.vs2019, KitsRootEnum.KitsRoot10, Options.Vc.General.WindowsTargetPlatformVersion.Latest);
        sharpmakeArgs.Generate<MainSolution>();
    }
}
