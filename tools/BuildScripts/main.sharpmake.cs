using Sharpmake;
using System;
using System.Collections.Generic;
using System.IO;

[Generate]
public class MainProject : Project
{
    private string m_Root;
    private string m_RootDirectory;
    private const string mc_ProjectName = "#ProjectName";

    public MainProject()
    {
        // Eval paths
        m_Root = Path.Combine(@"[project.SharpmakeCsPath]", @"..\..\");
        m_RootDirectory = Path.Combine(this.SharpmakeCsPath, @"..\..");

        Name = mc_ProjectName;
        SourceRootPath = Path.Combine(m_Root, @"source", mc_ProjectName);

        AdditionalSourceRootPaths.Add(Path.Combine(m_Root, @"data"));

        AddTargets(
            new Target(
                Platform.win64,
                DevEnv.vs2017,
                Optimization.Debug | Optimization.Release
            ),
            new Target(
                Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release
            )
        );
    }

    [Configure]
    public void ConfigureAll(Project.Configuration config, Target target)
    {
        config.ProjectFileName = @"[project.Name].[target.DevEnv]";
        config.ProjectPath = Path.Combine(m_RootDirectory, @"projects", mc_ProjectName);

        config.VcxprojUserFile = new Configuration.VcxprojUserFileSettings();
        config.VcxprojUserFile.LocalDebuggerWorkingDirectory = Path.Combine(m_Root, @"data");

        // Setup additional compiler options
        config.TargetPath = Path.Combine(m_Root, @"bin", config.Platform.ToString());

        config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.v10_0_17763_0);
        config.Options.Add(Options.Vc.Compiler.CppLanguageStandard.CPP17);
        config.Options.Add(Options.Vc.Compiler.RuntimeLibrary.MultiThreadedDebugDLL);
    }

    public override void PostResolve()
    {
        base.PostResolve();
    }
}

[Generate]
public class MainSolution : Solution
{
    private const string mc_SolutionName = "#SolutionName";
    private string m_Root = Path.Combine(@"[solution.SharpmakeCsPath]", @"\..\..");

    public MainSolution()
    {
        Name = mc_SolutionName;

        AddTargets(
            new Target(
                Platform.win64,
                DevEnv.vs2017,
                Optimization.Debug | Optimization.Release
            ),
            new Target(
                Platform.win64,
                DevEnv.vs2019,
                Optimization.Debug | Optimization.Release
            )
        );
    }

    [Configure]
    public void ConfigureAll(Solution.Configuration config, Target target)
    {
        config.SolutionPath = Path.Combine(m_Root, "projects");
        config.SolutionFileName = @"[solution.Name].[target.DevEnv]";
        config.Options.Add(Options.Vc.General.WindowsTargetPlatformVersion.Latest);

        config.AddProject<MainProject>(target);
    }
}

public static class Main
{
    [Sharpmake.Main]
    public static void SharpmakeMain(Arguments sharpmakeArgs)
    {
        //KitsRootPaths.SetUseKitsRootForDevEnv(DevEnv.vs2019, KitsRootEnum.KitsRoot81, Options.Vc.General.WindowsTargetPlatformVersion.Latest);

        sharpmakeArgs.Generate<MainSolution>();
    }
}
