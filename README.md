# XRayAnalyzer - No Installation Required!

XRayAnalyzer is software for X-ray fluorescence spectrometry.

<div align='center'>
    <a href='https://scottplot.net'><img src='https://user-images.githubusercontent.com/25822781/168403944-b216ccab-8c6c-484b-bb7b-ec76967f956d.png'></a>
</div>

Links to part of github repository of part of project:

- [WPF part of project](https://github.com/irelevant25/XRayAnalyzer/tree/XRayAnalyzer-0.1/XRayAnalyzer) UI is implemented in WPF using .NET 6.0.
- [python part of project](https://github.com/irelevant25/XRayAnalyzer/tree/XRayAnalyzer-0.1/XRayAnalyzer/Scripts) Algorithms for signal processing and qualitative and quantitative analysis are implemented in python.
- [datasets for XRayAnalyzer](https://github.com/irelevant25/XRayAnalyzer/tree/XRayAnalyzer-0.1/XRayAnalyzer/Datasets) Algorithms using data of X-Rays.

Check out [releases](https://github.com/irelevant25/XRayAnalyzer/releases) for downloads!

# XRayAnalyzer - Development

To be able to run the project you have to install a portable local version of python. For this purpose, there is a [powershell script](https://github.com/irelevant25/XRayAnalyzer/blob/XRayAnalyzer-0.1/XRayAnalyzer/InstallPythonWithLibs.ps1) what will do everything what you need.

To run the script open powershell script exactly where is its location and run command:

    .\InstallPythonWithLibs.ps1

If this error occured:

    .\InstallPythonWithLibs.ps1 : File ...\InstallPythonWithLibs.ps1 cannot be loaded
    . The file ...\InstallPythonWithLibs.ps1 is not digitally signed. You cannot run
    this script on the current system. For more information about running scripts and setting execution policy, see about_E
    xecution_Policies at https:/go.microsoft.com/fwlink/?LinkID=135170.
    At line:1 char:1
    + .\InstallPythonWithLibs.ps1
    + ~~~~~~~~~~~~~~~~~~~~~~~~~~~
        + CategoryInfo          : SecurityError: (:) [], PSSecurityException
        + FullyQualifiedErrorId : UnauthorizedAccess

All you have to do is run Set-ExecutionPolicy and change the Execution Policy setting:

    Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

This command sets the execution policy to bypass for only the current PowerShell session after the window is closed, the next PowerShell session will open running with the default execution policy. “Bypass” means nothing is blocked and no warnings, prompts, or messages will be displayed.